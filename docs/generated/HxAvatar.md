# HxAvatar

Bootstrap Avatar component. Represents a user or entity with an image or initials.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| ChildContent | `RenderFragment` | Content of the avatar, usually initials (e.g. `AB`). Can also be used for fully custom content. |
| Color | `ThemeColor?` | Theme color of the avatar (background and contrasting content color), usually combined with initials. The default is `ThemeColor.None` (the default avatar background). |
| CssClass | `string` | Any additional CSS class to apply. |
| ImageAlt | `string` | Alternate text for the avatar image (`ImageSrc`). |
| ImageSrc | `string` | URL of the avatar image. Renders an `img.avatar-img` element inside the avatar. |
| Settings | `AvatarSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Size | `AvatarSize?` | Size of the avatar. The default is `AvatarSize.Regular`. |
| Status | `AvatarStatus` | Status indicator of the avatar. The default is `AvatarStatus.None` (no indicator rendered). |
| StatusLabel | `string` | Accessible label of the status indicator (rendered as `aria-label`). The default is the English name of the `Status` (e.g. `Online`). |
| Subtle | `bool?` | When `true`, the avatar uses the subtle variant of the theme color (`avatar-subtle`), composed with `Color`. The default is `false`. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `AvatarSettings` | Application-wide defaults for `HxAvatar` and derived components. |

## Available demo samples

- HxAvatar_Demo_Image.razor
- HxAvatar_Demo_Initials.razor
- HxAvatar_Demo_Sizes.razor
- HxAvatar_Demo_Stack.razor
- HxAvatar_Demo_StackCount.razor
- HxAvatar_Demo_StackSizes.razor
- HxAvatar_Demo_Status.razor
- HxAvatar_Demo_StatusSizes.razor
- HxAvatar_Demo_Subtle.razor

