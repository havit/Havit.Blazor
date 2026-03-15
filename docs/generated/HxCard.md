# HxCard

Bootstrap card component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| BodyCssClass | `string` | Additional CSS class for the body. |
| BodyTemplate | `RenderFragment` | The body content. |
| CssClass | `string` | Additional CSS classes for the card container. |
| FooterCssClass | `string` | Additional CSS class for the footer. |
| FooterTemplate | `RenderFragment` | The footer content. |
| HeaderCssClass | `string` | Additional CSS class for the header. |
| HeaderTemplate | `RenderFragment` | The header content. |
| ChildContent | `RenderFragment` | The generic card content (outside `.card-body`). |
| ImageAlt | `string` | The value of the image's `alt` attribute. |
| ImageCssClass | `string` | Additional CSS class for the image. |
| ImageHeight | `int?` | The value of the image's `height` attribute. |
| ImagePlacement | `CardImagePlacement` | Placement of the image. The default is `CardImagePlacement.Top`. |
| ImageSrc | `string` | Image to be placed in the card. For the image position, see `ImagePlacement`. |
| ImageWidth | `int?` | The value of the image's `width` attribute. |
| Settings | `CardSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `CardSettings` | Application-wide defaults for `HxCard` and derived components. |

## Available demo samples

- HxCard_Demo_Basic.razor
- HxCard_Demo_Body.razor
- HxCard_Demo_Header.razor
- HxCard_Demo_HeaderFooter.razor
- HxCard_Demo_ImageBottom.razor
- HxCard_Demo_ImageOverlay.razor
- HxCard_Demo_ImageTop.razor
- HxCard_Demo_ListGroup.razor
- HxCard_Demo_NavigationPills.razor
- HxCard_Demo_NavigationTabs.razor
- HxCard_Demo_TitlesTextLinks.razor
- HxCard_Demo_Variants.razor

