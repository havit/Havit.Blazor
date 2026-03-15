# HxChipList

Presents a list of chips as badges. Usually used to present filter criteria gathered by `HxFilterForm`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CssClass | `string` | Additional CSS class. |
| ChipBadgeSettings | `BadgeSettings` | Settings for the `HxBadge` used to render chips. |
| Chips | `IEnumerableChipItem>` | Chips to be presented. |
| ResetButtonTemplate | `RenderFragment` | Template for the reset button. If used, the `ResetButtonText` is ignored and the `OnResetClick` callback is not triggered (you are expected to wire the reset logic on your own). |
| ResetButtonText | `string` | Text of the reset button. |
| Settings | `ChipListSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowResetButton | `bool?` | Enables or disables the reset button. The default is `false` (can be changed with `HxChipList.Defaults.ShowResetButton`). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnChipRemoveClick | `EventCallbackChipItem>` | Called when the chip remove button is clicked. |
| OnResetClick | `EventCallbackChipItem>` | Called when the reset button is clicked (when using the ready-made reset button, not the `ResetButtonTemplate` where you are expected to wire the event on your own). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ChipListSettings` | Application-wide defaults for the `HxChipList` component. |

## Available demo samples

- HxChipList_Demo.razor
- HxChipList_Demo_Reset.razor

