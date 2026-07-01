# HxAvatarStack

Bootstrap Avatar stack component. Groups multiple `HxAvatar` components with an overlapping effect. Avatars are rendered in reverse order, so the first avatar appears on top. To show a count of additional users, add an initials avatar (e.g. `<HxAvatar Color="ThemeColor.Secondary">+5</HxAvatar>`) as the last item.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| ChildContent | `RenderFragment` | Content of the avatar stack (put your `HxAvatar`s here). |
| CssClass | `string` | Any additional CSS class to apply. |
| Settings | `AvatarStackSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Size | `AvatarSize?` | Size of the avatars in the stack (a shorthand for setting the size of all contained avatars). The default is `AvatarSize.Regular`. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `AvatarStackSettings` | Application-wide defaults for `HxAvatarStack` and derived components. |

