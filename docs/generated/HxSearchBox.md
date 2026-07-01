# HxSearchBox

A search input component with automatic suggestions, initial dropdown template, and support for free-text queries.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AllowTextQuery | `bool` | Indicates whether text-query mode is enabled (accepts free text in addition to suggested items). Default is `true`. |
| ClearIcon | `IconBase` | Icon of the input, displayed when text is entered, allowing the user to clear the text. |
| CssClass | `string` | Additional CSS classes for the dropdown. |
| DataProvider | `SearchBoxDataProviderDelegate<TItem>` | Method (delegate) that provides data for the suggestions. |
| DefaultContentTemplate | `RenderFragment` | Rendered when no input is entered (i.e. initial state). |
| Delay | `int?` | Debounce delay in milliseconds. Default is `300` ms. |
| DropdownOffset | `ValueTuple<int, int>` | Offset between the dropdown and the input. |
| Enabled | `bool` | Allows you to disable the input. The default is `true`. |
| InputCssClass | `string` | Additional CSS classes for the search box input. |
| InputGroupCssClass | `string` | Custom CSS class to render with the input-group span. |
| InputGroupEndTemplate | `RenderFragment` | Input-group at the end of the input. Hides the search icon when used! |
| InputGroupEndText | `string` | Input-group at the end of the input. Hides the search icon when used! |
| InputGroupStartTemplate | `RenderFragment` | Input-group at the beginning of the input. |
| InputGroupStartText | `string` | Input-group at the beginning of the input. |
| InputSize | `InputSize?` | Input size of the input field. |
| ItemCssClass | `string` | Additional CSS classes for the items in the dropdown menu. |
| ItemIconSelector | `Func<TItem, IconBase>` | Selector to display the icon from the data item. |
| ItemSelectionBehavior | `SearchBoxItemSelectionBehavior?` | Behavior when the item is selected. |
| ItemSubtitleSelector | `Func<TItem, string>` | Selector to display the item subtitle from the data item. |
| ItemTemplate | `RenderFragment<TItem>` | Template for the item content. |
| ItemTitleSelector | `Func<TItem, string>` | Selector to display the item title from the data item. |
| Label | `string` | Label of the input field. |
| LabelType | `LabelType?` | Label type. |
| MinimumLength | `int?` | Minimum length to call the data provider (display any results). Default is `2`. |
| NotFoundTemplate | `RenderFragment` | Rendered when the `DataProvider` doesn't return any data. |
| Placeholder | `string` | Placeholder text for the search input. |
| SearchIcon | `IconBase` | Icon of the input when no text is written. |
| SearchIconPlacement | `SearchBoxSearchIconPlacement?` | Placement of the search icon. Default is `SearchBoxSearchIconPlacement.End`. |
| Settings | `SearchBoxSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Spellcheck | `bool?` | Defines whether the input may be checked for spelling errors. Default is `false`. |
| TextQuery | `string` | Text written by the user (input text). |
| TextQueryChanged | `EventCallback<string>` |  |
| TextQueryItemTemplate | `RenderFragment<string>` | Template for the text-query item content (requires `AllowTextQuery`="true"). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnHiding | `EventCallbackDropdownHidingEventArgs>` | Fired immediately when the 'hide' method of the dropdown is called. To prevent hiding, set `DropdownHidingEventArgs.Cancel` to `true`. |
| OnItemSelected | `EventCallback<TItem>` | Occurs when any of the suggested items (other than plain text-query) is selected. |
| OnTextQueryTriggered | `EventCallback<string>` | Raised when the enter key is pressed or when the text-query item is selected in the dropdown menu. (Does not trigger when `AllowTextQuery` is `false`.) |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| FocusAsync() | `Task` | Gives focus to the input element. |
| HideDropdownAsync() | `Task` | Hides the dropdown menu. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `SearchBoxSettings` | Application-wide defaults for the `HxSearchBox` and derived components. |

## Available demo samples

- HxSearchBox_Demo.razor
- HxSearchBox_Demo_DisableTextQuery.razor
- HxSearchBox_Demo_InitialSuggestions.razor
- HxSearchBox_Demo_TextQueryOnly.razor

