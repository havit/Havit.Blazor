# PasswordStrengthChangedEventArgs

Arguments for the `HxPasswordStrength.OnStrengthChanged` event.

## Properties

| Name | Type | Description |
|------|------|-------------|
| Score | `int` | Score computed by the scoring algorithm. |
| Strength | `PasswordStrengthLevel?` | Current strength level of the password (`null` when the password is empty). |

