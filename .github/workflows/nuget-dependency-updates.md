---
emoji: 📦
description: Weekly NuGet dependency updates — patch versions auto-applied, minor/major versions researched and code-adapted, then opened as a pull request
on:
  schedule: weekly on monday
  workflow_dispatch:
permissions:
  contents: read
tools:
  github:
    mode: gh-proxy
    toolsets: [context, repos]
strict: true
steps:
  - name: Set up .NET
    uses: actions/setup-dotnet@v5
    with:
      dotnet-version: |
        8.0.x
        9.0.x
        10.0.x
  - name: Collect and classify outdated NuGet packages
    run: |
      mkdir -p /tmp/gh-aw/data
      dotnet restore Havit.Blazor.slnx --verbosity quiet
      dotnet list Havit.Blazor.slnx package --outdated --format json \
        > /tmp/gh-aw/data/outdated-raw.json 2>/dev/null \
        || echo '{"version":1,"projects":[]}' > /tmp/gh-aw/data/outdated-raw.json
      jq -r '
        .projects[]?.frameworks[]?.topLevelPackages[]? |
        select(.latestVersion != null and .latestVersion != .resolvedVersion) |
        "\(.id)|\(.resolvedVersion)|\(.latestVersion)"
      ' /tmp/gh-aw/data/outdated-raw.json | sort -u > /tmp/gh-aw/data/pkg-list.txt
      echo '[]' > /tmp/gh-aw/data/patch.json
      echo '[]' > /tmp/gh-aw/data/minor.json
      echo '[]' > /tmp/gh-aw/data/major.json
      while IFS='|' read -r pid cur latest; do
        cur_maj=$(echo "$cur" | awk -F'[.+-]' '{print $1+0}')
        cur_min=$(echo "$cur" | awk -F'[.+-]' '{print $2+0}')
        new_maj=$(echo "$latest" | awk -F'[.+-]' '{print $1+0}')
        new_min=$(echo "$latest" | awk -F'[.+-]' '{print $2+0}')
        entry=$(jq -cn --arg id "$pid" --arg c "$cur" --arg l "$latest" '{id:$id,current:$c,latest:$l}')
        if [ "$new_maj" -gt "$cur_maj" ]; then
          target=major
        elif [ "$new_min" -gt "$cur_min" ]; then
          target=minor
        else
          target=patch
        fi
        jq --argjson e "$entry" '. + [$e]' /tmp/gh-aw/data/${target}.json \
          > /tmp/gh-aw/data/${target}.tmp && mv /tmp/gh-aw/data/${target}.tmp /tmp/gh-aw/data/${target}.json
      done < /tmp/gh-aw/data/pkg-list.txt
      jq -n \
        --slurpfile p /tmp/gh-aw/data/patch.json \
        --slurpfile m /tmp/gh-aw/data/minor.json \
        --slurpfile M /tmp/gh-aw/data/major.json \
        '{patch:$p[0],minor:$m[0],major:$M[0]}' > /tmp/gh-aw/data/outdated.json
      echo "=== Package update summary ==="
      jq -r '"  [patch] " + .id + ": " + .current + " -> " + .latest' /tmp/gh-aw/data/patch.json
      jq -r '"  [minor] " + .id + ": " + .current + " -> " + .latest' /tmp/gh-aw/data/minor.json
      jq -r '"  [major] " + .id + ": " + .current + " -> " + .latest' /tmp/gh-aw/data/major.json
    env:
      GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
safe-outputs:
  create-pull-request:
    title-prefix: "[deps] "
    labels: [dependencies]
    draft: false
    preserve-branch-name: true
    allowed-files:
      - "Directory.Packages.props"
      - "**/*.cs"
      - "**/*.razor"
      - "**/*.csproj"
network:
  allowed:
    - defaults
    - dotnet
    - github
---
# NuGet Dependency Updater

## Context

This repository is a .NET Blazor component library (Havit.Blazor) targeting net8.0, net9.0, and net10.0.
NuGet packages are centrally managed in `Directory.Packages.props` (Central Package Management).

Some packages share version properties defined at the top of `Directory.Packages.props`:
- `$(AspNetCoreVersion8)` — used for net8.0 conditional entries
- `$(AspNetCoreVersion9)` — used for net9.0 conditional entries
- `$(AspNetCoreVersion10)` — used for net10.0 (and default) entries

When updating packages that use one of these properties, update the **property value** in the `<PropertyGroup>`,
not the individual `<PackageVersion>` entries (which reference the property via `$(AspNetCoreVersionX)`).

## Task

Read `/tmp/gh-aw/data/outdated.json` which contains three arrays: `patch`, `minor`, and `major`.
Each entry has `id` (package name), `current` version, and `latest` version.

If all three arrays are empty, call `noop` with reason "All NuGet packages are up to date."

### Step A — Patch Updates

For every package in the `patch` array:
1. Open `Directory.Packages.props` and find the matching `<PackageVersion Include="<id>" Version="..." />` entry.
   - If the version is a property reference (e.g. `$(AspNetCoreVersion10)`), update the corresponding property value.
   - Otherwise update the `Version` attribute directly.
2. Change the version string to the `latest` value.

Apply all patch updates together in one editing pass.

### Step B — Minor and Major Updates

For every package in the `minor` and `major` arrays:

1. **Fetch package metadata** from NuGet:
   ```
   curl -s "https://api.nuget.org/v3/registration5-semver2/$(echo <package-id> | tr '[:upper:]' '[:lower:]')/index.json"
   ```
   Extract the `projectUrl` and any `releaseNotes` fields.

2. **Fetch the changelog or release notes**:
   - If `projectUrl` points to a GitHub repository, try:
     - `gh api repos/<owner>/<repo>/releases` to list releases between `current` and `latest`
     - `gh api repos/<owner>/<repo>/contents/CHANGELOG.md` or `CHANGELOG` or `RELEASES.md`
   - Parse and summarize: breaking changes, renamed APIs, removed members, behavioral changes.

3. **Apply code changes** to adapt to the new API:
   - Use `grep -r --include="*.cs" --include="*.razor" -l "<pattern>"` to find affected files.
   - Use `bash` to read and modify affected source files via the `edit` tool.
   - If a breaking change cannot be safely automated (e.g. complex architectural change), leave a TODO comment
     in the relevant file(s) and note it in the PR body.

4. **Update the version** in `Directory.Packages.props` (same as Step A).

### Step C — Create Pull Request

After all updates are applied, create a pull request with:
- **Branch**: `deps/nuget-updates-YYYYMMDD` (replace with today's date)
- **Target**: `main`
- **Title**: `chore: NuGet dependency updates`

**PR body** must include:

```
## NuGet Dependency Updates

### Summary

| Package | Current | Latest | Type |
|---|---|---|---|
| <package> | <current> | <latest> | patch / minor / major |
...

### Minor & Major Changes

#### <PackageName> (current → latest)
<Summary of breaking changes and migration steps applied>

*(If no code changes needed: "No breaking changes found.")*
```

## Safe Outputs

- Use `create-pull-request` with all modified files once all updates are applied.
- Call `noop` with reason "All NuGet packages are up to date." when no updates exist.
