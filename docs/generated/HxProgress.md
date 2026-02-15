# HxProgress

Bootstrap 5 Progress component. A wrapping component for the `HxProgressBar`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Animated | `bool` | If `true`, the stripes are animated right to left using CSS3 animations. The stripes are automatically switched on. |
| CssClass | `string` | Additional CSS classes for the progress. |
| Height | `int` | Height of all inner progress bars. The default value is `15px`. |
| ChildContent | `RenderFragment` | Content consisting of one or multiple `HxProgressBar` components. |
| MaxValue | `float` | Highest possible value. The default value is `100`. |
| MinValue | `float` | Lowest possible value. The default value is `0`. |
| Striped | `bool` | If `true`, applies a stripe via CSS gradient over the background color of the progress bar. |

## Available demo samples

- HxProgress_Demo.razor
- HxProgress_Demo_AnimatedStripes.razor
- HxProgress_Demo_Backgrounds.razor
- HxProgress_Demo_Height.razor
- HxProgress_Demo_Interactive.razor
- HxProgress_Demo_Labels.razor
- HxProgress_Demo_MultipleBars.razor
- HxProgress_Demo_Scales.razor
- HxProgress_Demo_SettingParameters.razor
- HxProgress_Demo_Striped.razor

