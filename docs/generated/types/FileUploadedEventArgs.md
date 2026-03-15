# FileUploadedEventArgs

Arguments for `HxInputFileCore.OnFileUploaded` event.

## Properties

| Name | Type | Description |
|------|------|-------------|
| ContentType | `string` | MIME type of the file provided by the browser. |
| FileIndex | `int` | Index of the uploaded file. |
| LastModified | `DateTimeOffset` | Last modification date of the file provided by the browser. |
| OriginalFileName | `string` | Name of the file provided by the browser. |
| ResponseStatus | `HttpStatusCode` | Response status received from the UploadUrl endpoint. |
| ResponseText | `string` | Response body received from the UploadUrl endpoint. |
| Size | `long` | Size of the file provided by the browser. |

