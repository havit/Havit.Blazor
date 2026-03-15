# UploadProgressEventArgs

Arguments for `HxInputFileCore.OnProgress` event.

## Properties

| Name | Type | Description |
|------|------|-------------|
| FileIndex | `int` | Index of the uploaded file. |
| OriginalFileName | `string` | Name of the file provided by the browser. |
| UploadedBytes | `long` | Number of bytes uploaded. |
| UploadSize | `long` | Upload request size (slightly larger than the file itself). |

