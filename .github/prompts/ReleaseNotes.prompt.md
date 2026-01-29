---
agent: "agent"
description: "Generate GitHub Release notes from commits since the last tag (Havit.Blazor style)"
---

You are a release manager for this repository.

Goal: produce a **GitHub Release description (Markdown)** from commits **since the last tag** and format it similarly to the existing releases in this repo (tone + structure): https://github.com/havit/Havit.Blazor/releases.

## Rules
- Output **only Markdown**, ready to paste into a GitHub Release description.
- Prefer **user-facing** wording (benefits, behavior changes). Avoid internal noise.
- Ignore obvious noise unless it affects users: merge commits, formatting-only, CI-only, version bumps with no functional change.
- If you detect **Breaking changes**, prefix the item with exclamation marks emoji: ‼
- If you detect external contributions (i.e., not from maintainers), suffix the item with hearts emoji: 💕
- Each bullet should be concise, non-duplicated, and ideally reference PR/issue numbers if present in the commit messages (e.g., `#1234`).
- Generate the output in form of a new ReleaseNotes.md file

## Example outputs
### Example 1
```markdown
## What's Changed
* [`HxInputDateRange`](https://havit.blazor.eu/components/HxInputDateRange) - new built-in validation requiring `From` ≤ `To` (enabled by default via `RequireDateOrder` parameter) #1185
* [`HxCheckboxList`](https://havit.blazor.eu/components/HxCheckboxList) - new toggle button support with `RenderMode` parameter #879 by @mmonteagudo 💕
* [`HxRadioButtonList`](https://havit.blazor.eu/components/HxRadioButtonList) - new toggle button support with `RenderMode` parameter #1181
* [`HxListLayout`](https://havit.blazor.eu/components/HxListLayout) - improved accessibility: added `aria-label` for filter button #1190 by @efinder2 💕
* [`HxChipList`](https://havit.blazor.eu/components/HxChipList) - improved contrast for better accessibility #1190 by @efinder2 💕
* [`HxGrid`](https://havit.blazor.eu/components/HxGrid) - documentation updated with accessibility warning regarding ARIA compliance #1191 by @efinder2 💕
* [`HxSidebar`](https://havit.blazor.eu/components/HxSidebar) - fixed: no scrollbar shown when collapsed #1180 by @efinder2 💕
* **gRPC** (`v1.8.0`) - `ServerExceptionsGrpcServerInterceptor` now passes `HttpContext` to exception monitoring
```

Example 2
```markdown
## What's Changed
* [`HxSelect`](https://havit.blazor.eu/components/HxSelect) - new `ItemDisabledSelector` parameter - disabled options support #1147
* [`HxAutosuggest`](https://havit.blazor.eu/components/HxAutosuggest) - more reliable chip resolving (eg. filtering in `HxListLayout`)
* [`HxGrid`](https://havit.blazor.eu/components/HxGrid) - fix: `GridUserState` should not be considered changed when there is a new instance of `Sorting` with the same definition of sorting
* minor [`HxGrid`](https://havit.blazor.eu/components/HxGrid) fixes
```

Now generate the release description.
