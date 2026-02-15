# HxInputFileCore

Raw component extending `InputFile` with direct upload.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Accept | `string` | Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow. MDN Web Docs - HTML attribute: accept. |
| AdditionalAttributes | `IDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| Enabled | `bool` | Make the item appear disabled by setting to `false`. The default is `true`. |
| Id | `string` | The input element id. |
| MaxFileSize | `long?` | The maximum file size in bytes. When exceeded, the `OnFileUploaded` returns `413-RequestEntityTooLarge` as `FileUploadedEventArgs.ResponseStatus`. The default is `long.MaxValue` (unlimited). |
| MaxParallelUploads | `int?` | The maximum number of concurrent uploads. The default is `6`. |
| Multiple | `bool` | Single `false` or multiple `true` files upload. |
| Settings | `InputFileCoreSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| UploadHttpMethod | `string` | HTTP Method (verb) used for file upload. The default is `POST`. |
| UploadUrl | `string` | URL of the server endpoint receiving the files. |

## Properties

| Name | Type | Description |
|------|------|-------------|
| Element | `ElementReference?` |  |
| FileCount | `int` | The last known count of associated files. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnFileUploaded | `EventCallbackFileUploadedEventArgs>` | Raised after a file is uploaded (for every single file separately). |
| OnChange | `EventCallback<InputFileChangeEventArgs>` |  |
| OnProgress | `EventCallbackUploadProgressEventArgs>` | Raised during running file upload (the frequency depends on browser implementation). |
| OnUploadCompleted | `EventCallbackUploadCompletedEventArgs>` | Raised when all files are uploaded (after all `OnFileUploaded` events). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| GetFilesAsync() | `Task<FileInfo<>>` | Gets list of files chosen. |
| ResetAsync() | `Task` | Clears associated input element and resets component to initial state. |
| StartUploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `Task` | Initiates the upload (does not wait for upload completion). Use OnUploadCompleted event. |
| UploadAsync(string accessToken, string antiforgeryToken, string antiforgeryHeaderName) | `TaskUploadCompletedEventArgs>` | Uploads the file(s). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `InputFileCoreSettings` | Application-wide defaults for the `HxInputFileCore`. |

