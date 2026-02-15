# HxValidationMessage

Displays a list of validation messages for a specified field within a cascaded `HxValidationMessage.EditContext`. Reimplementation of Blazor `ValidationMessage` as Bootstrap 5 validation. Used by `HxInputBase` and derived components.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| EditContext | `EditContext` | EditContext. For exceptional use where EditContext is not used as a CascadingParameter. |
| For | `Expression<Func<TValue>>` | Specifies the field for which validation messages should be displayed. Mutual exclusive with `ForFieldIdentifier` and `ForFieldIdentifiers`. |
| ForFieldIdentifier | `FieldIdentifier?` | Specifies the field for which validation messages should be displayed. Mutual exclusive with `For` and `ForFieldIdentifiers`. |
| ForFieldIdentifiers | `FieldIdentifier<>` | Specifies the field for which validation messages should be displayed. Mutual exclusive with `For` and `ForFieldIdentifier`. |
| ForFieldName | `string` | [Obsolete] Use `ForFieldIdentifier` instead. |
| ForFieldNames | `string<>` | [Obsolete] Use `ForFieldIdentifiers` instead. |
| Id | `string` | Container element ID. |
| Mode | `ValidationMessageMode` | Specifies how the validation message should be displayed. |

## Available demo samples

- HxValidationMessage_Demo.razor
- HxValidationMessage_Demo_Mode.razor

