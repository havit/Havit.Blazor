# HxListGroup

Bootstrap 5 List group component. List groups are a flexible and powerful component for displaying a series of content.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `HxListGroup` component. |
| CssClass | `string` | Additional CSS class. |
| Flush | `bool` | If set to `true`, removes borders and rounded corners to render list group items edge-to-edge. |
| Horizontal | `ListGroupHorizontal` | Changes the layout of the list group items from vertical to horizontal. Cannot be combined with `Flush`. |
| ChildContent | `RenderFragment` | Content of the list group component. |
| Numbered | `bool` | Set to `true` to opt into numbered list group items. The list group changes from an unordered list to an ordered list. |

## Available demo samples

- HxListGroup_Demo.razor
- HxListGroup_Demo_ActiveAndDisabled.razor
- HxListGroup_Demo_Badges.razor
- HxListGroup_Demo_Buttons.razor
- HxListGroup_Demo_Colors.razor
- HxListGroup_Demo_CustomContent.razor
- HxListGroup_Demo_Flush.razor
- HxListGroup_Demo_Horizontal.razor
- HxListGroup_Demo_Links.razor
- HxListGroup_Demo_Numbered.razor
- HxListGroup_Demo_NumberedCustomContent.razor

