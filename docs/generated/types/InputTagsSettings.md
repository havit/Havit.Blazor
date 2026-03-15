# InputTagsSettings

Settings for the `HxInputTags` component.

## Properties

| Name | Type | Description |
|------|------|-------------|
| Delimiters | `List<char>` | Characters that, when typed, divide the current input into separate tags. |
| InputSize | `InputSize?` | Input size. |
| LabelType | `LabelType?` | The label type. |
| ShowAddButton | `bool?` | Indicates whether the add-icon (+) should be displayed. |
| Spellcheck | `bool?` | Defines whether the input for new tag may be checked for spelling errors. |
| SuggestDelay | `int?` | Debounce delay in milliseconds. |
| SuggestMinimumLength | `int?` | Minimum number of characters to start suggesting. |
| TagBadgeSettings | `BadgeSettings` | Settings for the `HxBadge` used to render tags. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. |

