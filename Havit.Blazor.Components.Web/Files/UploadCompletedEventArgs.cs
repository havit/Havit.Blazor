namespace Havit.Blazor.Components.Web;

/// <summary>
/// Arguments for <see cref="HxInputFileCore.OnUploadCompleted"/> event.
/// </summary>
public class UploadCompletedEventArgs
{
	/// <summary>
	/// Total number of files uploaded.
	/// </summary>
	public int FileCount { get; set; }

	/// <summary>
	/// Total size of files uploaded.
	/// </summary>
	public long TotalSize { get; set; }

	/// <summary>
	/// Files uploaded (event arguments of individual OnFileUploaded events).
	/// </summary>
	public IReadOnlyCollection<FileUploadedEventArgs> FilesUploaded { get; internal set; }
}
