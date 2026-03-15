# HxRedirectTo

Rendering a `HxRedirectTo` will navigate to a new location. Can be used in `AuthorizeRouteView`, `Router` and similar components to redirect to a login page, error page, or similar.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| ForceLoad | `bool` | If `true`, bypasses client-side routing and forces the browser to load the new page from the server, regardless of whether the URI would normally be handled by the client-side router. Default is `false`. |
| Uri | `string` | URI to navigate to. |

## Available demo samples

- HxRedirectTo_Demo_BasicUsage.razor

