# HxInputFile

Wraps `HxInputFileCore` as a Bootstrap form control (including `Label` etc.)

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Accept | `string` | Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow. MDN Web Docs - HTML attribute: accept. |
| CssClass | `string` | Custom CSS class to render with the wrapping div. |
| Enabled | `bool?` | When `null` (default), the Enabled value is received from cascading `FormState`. When value is `false`, input is rendered as disabled. To set multiple controls as disabled use `HxFormState`. |
| Hint | `string` | Hint to render after input as form-text. |
| HintTemplate | `RenderFragment` | Hint to render after input as form-text. |
| InputCssClass | `string` | Custom CSS class to render with the input element. |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | Label to render before input (or after input for Checkbox). |
| LabelCssClass | `string` | Custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | Label to render before input (or after input for Checkbox). |
| MaxFileSize | `long?` | The maximum file size in bytes. When exceeded, the `OnFileUploaded` returns `413-RequestEntityTooLarge` as `FileUploadedEventArgs.ResponseStatus`. The default is `null` (unlimited). |
| MaxParallelUploads | `int?` | Maximum number of concurrent uploads. The default is `6` (from `HxInputFileCore`). |
| Multiple | `bool` | Single `false` or multiple `true` files upload. |
| Settings | `InputFileSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| UploadHttpMethod | `string` | HTTP Method (verb) used for file upload. The default is `POST`. |
| UploadUrl | `string` | URL of the server endpoint receiving the files. |

## Properties

| Name | Type | Description |
|------|------|-------------|
| FileCount | `int` | Last known count of associated files. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnFileUploaded | `EventCallbackFileUploadedEventArgs>` | Raised after a file is uploaded (for every single file separately). |
| OnChange | `EventCallback<InputFileChangeEventArgs>` | Gets or sets the event callback that will be invoked when the collection of selected files changes. |
| OnProgress | `EventCallbackUploadProgressEventArgs>` | Raised during running file upload (the frequency depends on browser implementation). |
| OnUploadCompleted | `EventCallbackUploadCompletedEventArgs>` | Raised when all files are uploaded (after all `OnFileUploaded` events). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| GetFilesAsync() | `Task<FileInfo<>>` | Gets list of files chosen. |
| ResetAsync() | `Task` | Clears associated input-file element and resets component to initial state. |
| StartUploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `Task` | Starts the upload. |
| UploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `TaskUploadCompletedEventArgs>` | Uploads the file(s). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputFileSettings` | Application-wide defaults for the `HxInputFile` and derived components. |

## Available demo samples

- HxInputFile_ComplexDemo.razor
- HxInputFile_Demo_Limits.razor
- HxInputFile_SimpleDemo.razor

