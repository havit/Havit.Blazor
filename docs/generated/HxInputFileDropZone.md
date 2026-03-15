# HxInputFileDropZone

Ready-made UX for drag&drop file upload. For custom drag&drop UX, use `HxInputFileCore` and a little bit of HTML/CSS.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Accept | `string` | Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow. MDN Web Docs - HTML attribute: accept. |
| CssClass | `string` | Custom CSS class to render with the wrapping div. |
| Enabled | `bool?` | When `null` (default), the Enabled value is received from cascading `FormState`. When value is `false`, input is rendered as disabled. To set multiple controls as disabled use `HxFormState`. |
| InputCssClass | `string` | Custom CSS class to render with the input element. |
| MaxFileSize | `long?` | The maximum file size in bytes. When exceeded, the `OnFileUploaded` returns `413-RequestEntityTooLarge` as `FileUploadedEventArgs.ResponseStatus`. Default is `null` (unlimited). |
| Multiple | `bool` | Single `false` or multiple `true` files upload. |
| NoFilesTemplate | `RenderFragment` | Content to render when no files are picked. Default content is displayed when not set. |
| UploadHttpMethod | `string` | HTTP Method (verb) used for file upload. The default is `POST`. |
| UploadUrl | `string` | URL of the server endpoint receiving the files. |

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
| GetFilesAsync() | `Task<FileInfo<>>` | Gets the list of chosen files. |
| ResetAsync() | `Task` | Clears the associated input-file element and resets the component to its initial state. |
| StartUploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `Task` | Starts the upload. |
| UploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `TaskUploadCompletedEventArgs>` | Uploads the file(s). |

## Available demo samples

- HxInputFileDropZone_Demo_Multiple.razor
- HxInputFileDropZone_Demo_NoFilesTemplate.razor
- HxInputFileDropZone_Demo_Single.razor

