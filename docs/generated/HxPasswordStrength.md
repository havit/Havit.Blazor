# HxPasswordStrength

Bootstrap Password strength meter (new in Bootstrap 6). Provides real-time visual feedback on password strength. Attaches the Bootstrap Strength plugin to a password input—either a password input placed in `HxPasswordStrength.ChildContent` or any input targeted with `HxPasswordStrength.InputSelector`. The password is evaluated in JavaScript on every keystroke (no server round-trips, the password itself never leaves the browser); strength level changes are surfaced to .NET via `HxPasswordStrength.OnStrengthChanged`. This is not a form input component—it does not participate in `EditForm` validation and does not interfere with validation classes of the associated input.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the strength meter element. |
| ChildContent | `RenderFragment` | Content rendered above the strength meter, typically containing the password input the meter should evaluate (e.g. `HxInputText` with `Type="InputType.Password"`). When no `InputSelector` is set, the meter automatically attaches to the first `input[type="password"]` found in the content. |
| CssClass | `string` | Additional CSS class(es) for the strength meter. |
| FairText | `string` | Text feedback message for the `PasswordStrengthLevel.Fair` level. The default is a localized `Fair`. |
| GoodText | `string` | Text feedback message for the `PasswordStrengthLevel.Good` level. The default is a localized `Good`. |
| InputSelector | `string` | CSS selector of the password input to evaluate (the `input` option of the underlying plugin). Use when the password input cannot be placed in `ChildContent` (the meter then attaches to the input anywhere in the document). |
| MinLength | `int?` | Minimum password length required for the first strength point (the `minLength` option of the underlying plugin). The default is `8` (underlying plugin default). |
| Settings | `PasswordStrengthSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowText | `bool?` | When `true`, a text feedback element (`strength-text`) displaying the strength level message is rendered below the meter. The default is `false`. |
| Strength | `PasswordStrengthLevel?` | Strength level for a static (CSS-only) display. When set, the meter renders the level directly (filled segments, `data-bs-strength` attribute, text feedback) and no JavaScript evaluation takes place (`OnStrengthChanged` is never raised). |
| StrongText | `string` | Text feedback message for the `PasswordStrengthLevel.Strong` level. The default is a localized `Strong`. |
| Thresholds | `PasswordStrengthThresholds` | Score boundaries for the strength levels (the `thresholds` option of the underlying plugin). When not set, the underlying plugin defaults are used (weak ≤ 2, fair ≤ 4, good ≤ 6, strong > 6). |
| Variant | `PasswordStrengthVariant?` | Visualization variant of the strength meter. The default is `PasswordStrengthVariant.Segments` (a four-segment meter); use `PasswordStrengthVariant.ProgressBar` for a single growing bar. |
| WeakText | `string` | Text feedback message for the `PasswordStrengthLevel.Weak` level. The default is a localized `Weak`. |
| Weights | `PasswordStrengthWeights` | Point values for the individual criteria of the scoring algorithm (the `weights` option of the underlying plugin). Set a weight to `0` to disable the corresponding criterion. When not set, the underlying plugin defaults are used (all criteria worth `1` point). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnStrengthChanged | `EventCallbackPasswordStrengthChangedEventArgs>` | Raised when the strength level of the evaluated password changes (the `strengthChange.bs.strength` event of the underlying plugin). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| EvaluateAsync() | `Task` | Manually triggers the password evaluation. Useful when the input value is changed programmatically (no `input` event is raised in such a case). |
| GetStrengthAsync() | `Task<Nullable<PasswordStrengthLevel>>` | Gets the current strength level of the evaluated password (`null` when the password is empty). For a static display (`HxPasswordStrength.Strength` set), returns the `HxPasswordStrength.Strength` value. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `PasswordStrengthSettings` | Application-wide defaults for `HxPasswordStrength` and derived components. |

## Available demo samples

- HxPasswordStrength_Demo.razor
- HxPasswordStrength_Demo_Criteria.razor
- HxPasswordStrength_Demo_InputSelector.razor
- HxPasswordStrength_Demo_ProgressBar.razor
- HxPasswordStrength_Demo_Static.razor
- HxPasswordStrength_Demo_StrengthChanged.razor
- HxPasswordStrength_Demo_TextFeedback.razor

