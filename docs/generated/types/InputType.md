# InputType

Enum for HTML input types.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| Text | 0 | A single-line text field. Line-breaks are automatically removed from the input value. |
| Email | 1 | A field for editing an email address. It looks like a `InputType.Text` input, but has validation parameters and a relevant keyboard in supporting browsers and devices with dynamic keyboards. |
| Password | 2 | A single-line text field whose value is obscured. It will alert the user if the site is not secure. |
| Search | 3 | A single-line text field for entering search strings. Line-breaks are automatically removed from the input value. It may include a delete icon in supporting browsers that can be used to clear the field. It displays a search icon instead of the enter key on some devices with dynamic keypads. |
| Tel | 4 | A control for entering a telephone number. It displays a telephone keypad on some devices with dynamic keypads. |
| Url | 5 | A field for entering a URL. It looks like a `InputType.Text` input, but has validation parameters and a relevant keyboard in supporting browsers and devices with dynamic keyboards. |
| Number | 6 | A control for entering a number. Displays a numeric keypad in some devices with dynamic keypads. |

