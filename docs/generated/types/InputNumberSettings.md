# InputNumberSettings

Settings for the `HxInputNumber` and derived components.

## Properties

| Name | Type | Description |
|------|------|-------------|
| InputMode | `InputMode?` | A hint to browsers regarding the type of virtual keyboard configuration to use when editing. |
| InputSize | `InputSize?` | The size of the input. |
| LabelType | `LabelType?` | The label type. |
| SelectOnFocus | `bool?` | Determines whether all the content within the input field is automatically selected when it receives focus. |
| SmartKeyboard | `bool?` | When enabled, the input may provide an optimized keyboard experience for numeric entry. Currently, this means whenever a minus key is pressed, the sign of the number is toggled. |
| SmartPaste | `bool?` | When enabled, pasted values are normalized to contain only valid numeric characters. |
| Type | `InputType?` | Allows switching between textual and numeric input types. Only `InputType.Text` (default) and `InputType.Number` are supported. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. |

