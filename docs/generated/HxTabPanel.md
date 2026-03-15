# HxTabPanel

Container for `HxTab`s for easier implementation of tabbed UI. Encapsulates `HxNav` (`NavVariant.Tabs` variant as default) and `HxNavLink`s so you don't have to bother with them explicitly.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| ActiveTabId | `string` | ID of the active tab (@bindable). |
| ActiveTabIdChanged | `EventCallback<string>` | Raised when the ID of the active tab changes. |
| CardSettings | `CardSettings` | Card settings for the wrapping card. Applies only if `Variant` is set to `TabPanelVariant.Card`. |
| CssClass | `string` | Additional CSS class. |
| ChildContent | `RenderFragment` | Tabs. |
| InitialActiveTabId | `string` | ID of the tab which should be active at the very beginning. We are considering deprecating this parameter. Please use `ActiveTabId` instead (`@bind-ActiveTabId`). |
| NavVariant | `NavVariant` | The visual variant of the nav items. Default is `NavVariant.Tabs`. |
| RenderMode | `TabPanelRenderMode` | Determines whether the content of all tabs is always rendered or only if the tab is active. Default is `TabPanelRenderMode.AllTabs`. |
| Variant | `TabPanelVariant` | Set to `TabPanelVariant.Card` if you want to wrap the tab panel in a card with the tab navigation in the header. |

## Available demo samples

- HxTabPanel_Demo_ActiveTabId.razor
- HxTabPanel_Demo_BasicUsage.razor
- HxTabPanel_Demo_CardMode.razor
- HxTabPanel_Demo_NavVariantPills.razor
- HxTabPanel_Demo_NavVariantStandard.razor
- HxTabPanel_Demo_NavVariantTabs.razor
- HxTabPanel_Demo_RenderMode.razor

