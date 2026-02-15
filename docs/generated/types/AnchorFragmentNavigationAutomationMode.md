# AnchorFragmentNavigationAutomationMode

Automation mode for `HxAnchorFragmentNavigation`.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| Full | 0 | Scrolls to the anchor on `firstRender` and whenever the location changes (`LocationChanged`). With `HxScrollspy`, this mode is suitable only for static pages (where the size/offset of individual sections remains the same from the very beginning). Use `AnchorFragmentNavigationAutomationMode.SamePage` or `AnchorFragmentNavigationAutomationMode.Manual` when the page contents load asynchronously and the layout changes. |
| Manual | 1 | Explicit calls to `HxAnchorFragmentNavigation.ScrollToCurrentUriFragmentAsync` or `HxAnchorFragmentNavigation.ScrollToAnchorAsync` are needed. Use this mode with `HxScrollspy` when you need scrollspy to call `HxScrollspy.RefreshAsync` to recalculate the target offsets before scrolling (scrollspy does not work properly on scrolled content). |
| SamePage | 2 | Same as `AnchorFragmentNavigationAutomationMode.Manual`, but scrolls to the anchor on `LocationChanged` if the page remains the same (just the #fragment portion changed). Works for most scenarios when you refresh the `HxScrollspy` after data load and then you just need to navigate over the page. |

