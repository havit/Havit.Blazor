# PasswordStrengthSettings

Settings for `HxPasswordStrength`.

## Properties

| Name | Type | Description |
|------|------|-------------|
| CssClass | `string` | Additional CSS class(es) for the strength meter. |
| FairText | `string` | Text feedback message for the `PasswordStrengthLevel.Fair` level. |
| GoodText | `string` | Text feedback message for the `PasswordStrengthLevel.Good` level. |
| MinLength | `int?` | Minimum password length required for the first strength point. |
| ShowText | `bool?` | When `true`, a text feedback element is rendered below the meter. |
| StrongText | `string` | Text feedback message for the `PasswordStrengthLevel.Strong` level. |
| Thresholds | `PasswordStrengthThresholds` | Score boundaries for the strength levels. |
| Variant | `PasswordStrengthVariant?` | Visualization variant of the strength meter (segmented meter or a single progress bar). |
| WeakText | `string` | Text feedback message for the `PasswordStrengthLevel.Weak` level. |
| Weights | `PasswordStrengthWeights` | Point values for the individual criteria of the scoring algorithm. |

