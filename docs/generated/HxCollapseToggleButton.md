# HxCollapseToggleButton

Bootstrap Collapse toggle (in the form of a button) which triggers the `HxCollapse` to toggle. Derived from `HxButton` (including `HxButton.Defaults` inheritance).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `<button>` element. |
| AriaLabel | `string` | Defines the `aria-label` of the button. Should be used to provide an accessible name for icon-only buttons. |
| CollapseTarget | `string` | Target selector of the toggle. Use `#id` to reference a single `HxCollapse` or `.class` for multiple `HxCollapse`s. |
| Color | `ThemeColor?` | Bootstrap button style - theme color. The default is taken from `Defaults` (`ThemeColor.None` if not customized). |
| CssClass | `string` | Custom CSS class to render with the `<button />`. When using `Tooltip` you might want to use `TooltipWrapperCssClass` instead of `CssClass` to get the desired result. |
| EditContext | `EditContext` | Associated `EditContext`. |
| Enabled | `bool?` | When `null` (default), the Enabled value is received from cascading `FormState`. When value is `false`, input is rendered as disabled. To set multiple controls as disabled use `HxFormState`. |
| FormId | `string` | Specifies the form the button belongs to. |
| ChildContent | `RenderFragment` | Button content. |
| Icon | `IconBase` | Icon to render into the button. |
| IconCssClass | `string` | CSS class to be rendered with the button icon. |
| IconPlacement | `ButtonIconPlacement?` | Position of the icon within the button. The default is `ButtonIconPlacement.Start` (configurable through `Defaults`). |
| OnClickPreventDefault | `bool` | Prevents the default action for the onclick event. Default is `false`. |
| OnClickStopPropagation | `bool` | Stops onClick-event propagation. Default is `true`. |
| Outline | `bool?` | Bootstrap "outline" button style. |
| Settings | `ButtonSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| SingleClickProtection | `bool` | Sets `false` if you want to allow multiple `OnClick` handlers in parallel. Default is `true`. |
| Size | `ButtonSize?` | Button size. The default is `ButtonSize.Regular`. |
| Spinner | `bool?` | Sets the state of the embedded `HxSpinner`. Leave `null` if you want automated spinner when any of the `OnClick` handlers is running. You can set an explicit `false` constant to disable (override) the spinner automation. |
| Text | `string` | Text of the button. |
| Tooltip | `string` | Tooltip text. If set, a `span` wrapper will be rendered around the `<button />`. For most scenarios, you will then use `TooltipWrapperCssClass` instead of `CssClass`. |
| TooltipContainer | `string` | Appends the tooltip to a specific element. Default is `body`. |
| TooltipCssClass | `string` | Custom CSS class to render with the tooltip. |
| TooltipPlacement | `TooltipPlacement?` | Tooltip placement. |
| TooltipSettings | `TooltipSettings` | Tooltip settings (overrides `ButtonSettings.TooltipSettings`, overridden by individual Tooltip* parameters). |
| TooltipWrapperCssClass | `string` | Custom CSS class to render with the tooltip `span` wrapper of the `<button />`. If set, the `span` wrapper will be rendered no matter whether the `Tooltip` text is set or not. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClick | `EventCallback<MouseEventArgs>` | Raised after the button is clicked. |
| OnInvalidClick | `EventCallback<MouseEventArgs>` | Raised after the button is clicked and EditContext validation fails. |
| OnValidClick | `EventCallback<MouseEventArgs>` | Raised after the button is clicked and EditContext validation succeeds. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ButtonSettings` | Application-wide defaults for `HxButton` and derived components. |

