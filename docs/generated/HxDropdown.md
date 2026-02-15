# HxDropdown

Bootstrap 5 Dropdown generic component. For buttons with dropdowns, use the more specific `HxDropdownButtonGroup`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `div` element. |
| AutoClose | `DropdownAutoClose` | By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (`DropdownAutoClose.True`). You can use the AutoClose parameter to change this behavior of the dropdown. See https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior for more information. |
| CssClass | `string` | Any additional CSS class to apply. |
| Direction | `DropdownDirection` | The direction in which the dropdown is opened. |
| ChildContent | `RenderFragment` | Content of the component. |

## Available demo samples

- HxDropdown_Demo_AutoClose.razor
- HxDropdown_Demo_Basic.razor
- HxDropdown_Demo_CustomContent.razor
- HxDropdown_Demo_Dark.razor
- HxDropdown_Demo_Directions.razor
- HxDropdown_Demo_DropdownOffsetAndReference.razor
- HxDropdown_Demo_DropdownReference.razor
- HxDropdown_Demo_HeaderDisabledActive.razor
- HxDropdown_Demo_Icons.razor
- HxDropdown_Demo_MenuAlignment.razor

