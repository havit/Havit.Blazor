# HxInputTags

Input for entering tags. Does not allow duplicate tags.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AddButtonText | `string` | The optional text for the add button. Displayed only when there are no tags (the `Value` is empty). The default is `null` (none). |
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| AllowCustomTags | `bool` | Indicates whether you are restricted to suggested items only (`false`). Default is `true` (you can type your own tags). |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DataProvider | `InputTagsDataProviderDelegate` | Set to a method providing data for tag suggestions. |
| Delimiters | `List<char>` | Characters that divide the current input into separate tags when typed. The default is comma, semicolon, and space. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading `FormState`. When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use HxFormState. |
| GenerateChip | `bool` | When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupCssClass | `string` | The custom CSS class to render with the input-group span. |
| InputGroupEndTemplate | `RenderFragment` | The input-group at the end of the input. |
| InputGroupEndText | `string` | The input-group at the end of the input. |
| InputGroupStartTemplate | `RenderFragment` | The input-group at the beginning of the input. |
| InputGroupStartText | `string` | The input-group at the beginning of the input. |
| InputSize | `InputSize?` | The size of the input. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| Naked | `bool` | Indicates whether a "naked" variant should be displayed (no border). The default is `false`. Consider enabling `ShowAddButton` when using `Naked`. |
| Placeholder | `string` | A short hint displayed in the input field before the user enters a value. |
| Settings | `InputTagsSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowAddButton | `bool?` | Indicates whether the add icon (+) should be displayed. The default is `false`. |
| Spellcheck | `bool?` | Defines whether the input for new tag may be checked for spelling errors. |
| SuggestDelay | `int?` | The debounce delay in milliseconds. The default is `300 ms`. |
| SuggestMinimumLength | `int?` | The minimum number of characters to start suggesting. The default is `2`. |
| TagBadgeSettings | `BadgeSettings` | The settings for the `HxBadge` used to render tags. The default is `Color="ThemeColor.Light`" and `TextColor="ThemeColor.Dark`". |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in `Defaults`. |
| Value | `List<string>` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<List<string>>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<List<string>>` | A callback that updates the bound value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` |  |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputTagsSettings` | Application-wide defaults for the `HxInputTags`. |

## Available demo samples

- HxInputTags_Demo_AllowCustomTags.razor
- HxInputTags_Demo_Basic.razor
- HxInputTags_Demo_Colors.razor
- HxInputTags_Demo_Disabled.razor
- HxInputTags_Demo_InputGroupTemplates.razor
- HxInputTags_Demo_InputGroups.razor
- HxInputTags_Demo_Naked.razor
- HxInputTags_Demo_Naked_AddButtonText.razor
- HxInputTags_Demo_Naked_InputSize.razor
- HxInputTags_Demo_NoSuggestions.razor
- HxInputTags_Demo_StaticSuggestions.razor

