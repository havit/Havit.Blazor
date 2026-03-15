# UploadCompletedEventArgs

Arguments for `HxInputFileCore.OnUploadCompleted` event.

## Properties

| Name | Type | Description |
|------|------|-------------|
| FileCount | `int` | Total number of files uploaded. |
| FilesUploaded | `IReadOnlyCollectionFileUploadedEventArgs>` | Files uploaded (event arguments of individual OnFileUploaded events). |
| TotalSize | `long` | Total size of files uploaded. |

