---
name: release-notes
description: Generate a GitHub Release description (Markdown) from commits since the last tag, matching the tone and structure of the repository's existing releases. Use when the user asks for release notes, a release description, a changelog since the last tag, or "what's changed" for the next version.
---

# Release Notes Generator

Act as the release manager for the current repository. Produce a **GitHub Release description in Markdown**, ready to copy-paste, covering everything merged since the last release tag.

## 1. Determine the range

```bash
git fetch --tags
git describe --tags --abbrev=0          # last tag
git log <last-tag>..HEAD --no-merges --pretty=format:"%h %an %s"
```

- If the user names a tag / version / date range, use that instead.
- If no tags exist, fall back to the first commit (`git log --no-merges`) and say so.
- For a release of a single package in a multi-package repo, filter by path: `git log <last-tag>..HEAD -- <path>`.

## 2. Learn the repository's release style

Before writing, sample the existing releases so the output matches the established tone, heading structure, bullet format, and link conventions:

```bash
gh release list --limit 5
gh release view <tag>
```

If `gh` is unavailable, fall back to `CHANGELOG.md` / release notes files in the repo. Mirror whatever conventions you find (heading text such as `## What's Changed`, component/module prefixes, links to docs, emoji usage). The rules below are defaults — repository conventions win.

## 3. Enrich the entries

- Extract PR and issue numbers from commit messages / merge commits (`#1234`) and keep them in the bullets.
- Identify **external contributions** (authors who are not maintainers of the repo). Use `gh pr view <number> --json author,authorAssociation` or `gh api repos/{owner}/{repo}/collaborators` when available; otherwise infer from the commit author list and ask the user if uncertain.
- Detect **breaking changes**: `!` in conventional-commit type, `BREAKING CHANGE:` footers, removed/renamed public API, changed defaults.

## 4. Write the notes

Rules:

- Output the result in a **fenced code block** so it can be copy-pasted into the GitHub Release description.
- Use **user-facing wording** — describe the benefit or behavior change, not the internal implementation.
- One concise bullet per change; merge duplicates and follow-up fix commits into a single item.
- Group by component / module / package when the repo has more than one, using the repo's existing prefix style.
- Omit noise unless it affects users: merge commits, formatting-only changes, CI/build tweaks, dependency and version bumps without functional impact, test-only changes.
- Prefix breaking changes with ‼ (the repository may use a different marker — follow its precedent).
- Suffix external contributions with `by @author 💕` (again, follow repo precedent).
- Keep any "Full Changelog" / compare link the repository normally appends.

## Output shape (default)

````markdown
## What's Changed
* `ComponentA` - new `SomeParameter` for X #1234
* ‼ `ComponentB` - `OldParameter` removed, use `NewParameter` instead #1240
* `ComponentC` - fixed: incorrect rendering when Y #1236 by @contributor 💕
* **SubPackage** (`v1.8.0`) - short description of the change
* minor accessibility fixes
````

## Notes

- If commit messages are too terse to describe user impact, open the referenced PRs (`gh pr view <number>`) rather than guessing.
- State briefly (outside the code block) which tag range was used, so the user can verify the scope.
