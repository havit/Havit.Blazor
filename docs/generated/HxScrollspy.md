# HxScrollspy

Bootstrap Scrollspy component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| TargetId **[REQUIRED]** | `string` | ID of the `HxNav` or list-group with scrollspy navigation. |
| CssClass | `string` | Scrollspy additional CSS class. Added to the main div (.hx-scrollspy). |
| ChildContent | `RenderFragment` | Content to be spied on. Elements with IDs are required (corresponding IDs to be used in `HxNavLink.Href`). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| RefreshAsync() | `Task` | When using scrollspy in conjunction with adding or removing elements from the DOM (e.g. asynchronous data load), you’ll need to refresh the scrollspy explicitly. |

## Available demo samples

- HxScrollspy_Demo_CustomNavigationContent.razor
- HxScrollspy_Demo_DynamicContent.razor
- HxScrollspy_Demo_HxNav.razor
- HxScrollspy_Demo_ListGroup.razor

