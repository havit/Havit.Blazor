# HxScrollspy

Bootstrap Scrollspy that automatically highlights the navigation link matching the section currently scrolled into view.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| TargetId **[REQUIRED]** | `string` | ID of the `HxNav` or list-group with scrollspy navigation. |
| ChildContent | `RenderFragment` | Content to be spied on. Elements with IDs are required (corresponding IDs to be used in `HxNavLink.Href`). |
| CssClass | `string` | Scrollspy additional CSS class. Added to the main div (.hx-scrollspy). |
| RootMargin | `string` | Advanced override for the `IntersectionObserver`rootMargin (e.g. `0px 0px -40%`). When set, it takes precedence over `TopMargin` and is passed straight to the observer. Default is `null` (the activation line is derived from `TopMargin`). |
| SmoothScroll | `bool` | Enables smooth scrolling of the container (CSS `scroll-behavior: smooth`) when the user clicks a `HxScrollspyNavLink`. Respects the `prefers-reduced-motion` user preference. Default is `false`. |
| Threshold | `IReadOnlyList<double>` | `IntersectionObserver` threshold values (between `0` and `1`) used when calculating the scroll position. Default is `null` (Bootstrap default `[0]` is used). |
| TopMargin | `string` | Position of the activation line, measured from the top of the scroll container. A section becomes active once its top scrolls above this line. Accepts a percentage (e.g. `12%`) or pixels (e.g. `96px`, to sit below a sticky header). Ignored when `RootMargin` is set. Default is `null` (Bootstrap default `12%` is used). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| RefreshAsync() | `Task` | When using scrollspy in conjunction with adding or removing elements from the DOM (e.g. asynchronous data load), you’ll need to refresh the scrollspy explicitly. |

## Available demo samples

- HxScrollspy_Demo_CustomNavigationContent.razor
- HxScrollspy_Demo_DynamicContent.razor
- HxScrollspy_Demo_HxNav.razor
- HxScrollspy_Demo_ListGroup.razor
- HxScrollspy_Demo_Navbar.razor
- HxScrollspy_Demo_Nested.razor
- HxScrollspy_Demo_SimpleAnchors.razor

