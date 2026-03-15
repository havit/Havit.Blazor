# InputFileCoreSettings

Defaults for the `HxInputFileCore`.

## Properties

| Name | Type | Description |
|------|------|-------------|
| MaxFileSize | `long?` | The maximum file size in bytes. When exceeded, the `HxInputFileCore.OnFileUploaded` returns `413-RequestEntityTooLarge` as `FileUploadedEventArgs.ResponseStatus`. |
| MaxParallelUploads | `int?` | Maximum number of concurrent uploads. |
| UploadHttpMethod | `string` | HTTP Method (verb) used for file upload. The default is `POST`. |

