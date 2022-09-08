namespace Havit.Blazor.Components.Web;

/// <summary>
/// Arguments for <see cref="HxInputFileCore.OnUploadCompleted"/> event.
/// </summary>
public class UploadCompletedEventArgs
{
	/// <summary>
	/// Total count of files uploaded.
	/// </summary>
	public int FileCount { get; set; }

	/// <summary>
	/// Total size of files uploaded.
	/// </summary>
	public long TotalSize { get; set; }

	/// <summary>
	/// Files uploaded (event arguments of individual OnFileUploded events).
	/// </summary>
	public IReadOnlyCollection<FileUploadedEventArgs> FilesUploaded { get; internal set; }
}
