# HxDropdownButtonGroup

Bootstrap 5 Dropdown component for dropdown buttons. For generic dropdowns, use `HxDropdown`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `div` element. |
| AutoClose | `DropdownAutoClose` | By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (`DropdownAutoClose.True`). You can use the AutoClose parameter to change this behavior of the dropdown. See https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior for more information. |
| CssClass | `string` | Any additional CSS class to apply. |
| Direction | `DropdownDirection` | The direction in which the dropdown is opened. |
| ChildContent | `RenderFragment` | Content of the component. |
| Split | `bool` | Set `true` to create a split dropdown (using a `btn-group`). |

## Available demo samples

- HxDropdownButtonGroup_Demo_Basic.razor
- HxDropdownButtonGroup_Demo_Colors.razor
- HxDropdownButtonGroup_Demo_Sizing.razor
- HxDropdownButtonGroup_Demo_Split.razor

