# HxAutosuggest

Component for single item selection with dynamic suggestions (based on typed characters).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| ClearIcon | `IconBase` | Icon displayed in the input on the selection clear button when an item is selected. |
| CssClass | `string` | The custom CSS class to render with the wrapping div. |
| DataProvider | `AutosuggestDataProviderDelegate<TItem>` | Method (delegate) that provides data for the suggestions. |
| Delay | `int?` | The debounce delay in milliseconds. Default is 300 ms. |
| DisplayName | `string` | Gets or sets the display name for this field. This value is used when generating error messages when the input value fails to parse correctly. |
| EmptyTemplate | `RenderFragment` | Template to display when items are empty. |
| Enabled | `bool?` | When `null` (default), the `Enabled` value is received from the cascading . When the value is `false`, the input is rendered as disabled. To set multiple controls as disabled, use . |
| GenerateChip | `bool` | When `true`, ` is used to generate chip item(s). The default is true`. |
| Hint | `string` | The hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | The hint to render after the input as form-text. |
| ChipTemplate | `RenderFragment` | The chip template. |
| InputCssClass | `string` | The custom CSS class to render with the input element. |
| InputGroupEndTemplate | `RenderFragment` | The input-group at the end of the input. Hides the search icon when used! |
| InputGroupEndText | `string` | The input-group at the end of the input. Hides the search icon when used! |
| InputGroupStartTemplate | `RenderFragment` | The input-group at the beginning of the input. |
| InputGroupStartText | `string` | The input-group at the beginning of the input. |
| InputSize | `InputSize?` | The size of the input. |
| ItemFromValueResolver | `Func<TValue, Task<TItem>>` | Returns the corresponding item for the (selected) value. |
| ItemTemplate | `RenderFragment<TItem>` | Template to display an item. When not set, `TextSelector` is used. |
| Label | `string` | The label text. |
| LabelCssClass | `string` | The custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | The label content. |
| LabelType | `LabelType?` | Label type. |
| MinimumLength | `int?` | The minimal number of characters to start suggesting. |
| Placeholder | `string` | A short hint displayed in the input field before the user enters a value. |
| SearchIcon | `IconBase` | Icon displayed in the input when no item is selected. |
| Settings | `AutosuggestSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Spellcheck | `bool?` | Defines whether the input may be checked for spelling errors. Default is `false`. |
| TextSelector | `Func<TItem, string>` | Selects the text to display from an item. When not set, `ToString()` is used. |
| ValidationMessageMode | `ValidationMessageMode?` | Specifies how the validation message should be displayed. The default is `ValidationMessageMode.Regular`, you can override the application-wide default for all inputs in . |
| Value | `TValue` | Value of the input. This should be used with two-way binding. |
| ValueExpression | `Expression<Func<TValue>>` | An expression that identifies the bound value. |
| ValueChanged | `EventCallback<TValue>` | A callback that updates the bound value. |
| ValueSelector | `Func<TItem, TValue>` | Selects a value from an item. Not required when `TValue` is the same as `TItem`. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `ValueTask` |  |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `AutosuggestSettings` | Application-wide defaults for the `HxAutosuggest` and derived components. |

## Available demo samples

- HxAutosuggest_Demo_Basic.razor
- HxAutosuggest_Demo_InitialSuggestions.razor

