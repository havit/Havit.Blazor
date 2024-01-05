namespace Havit.Blazor.Components.Web;

/// <summary>
/// Arguments for <see cref="HxInputFileCore.OnProgress"/> event.
/// </summary>
public class UploadProgressEventArgs
{
	/// <summary>
	/// Index of the uploaded file.
	/// </summary>
	public int FileIndex { get; internal set; }

	/// <summary>
	/// Name of the file provided by the browser.
	/// </summary>
	public string OriginalFileName { get; set; }

	/// <summary>
	/// Number of bytes uploaded.
	/// </summary>
	public long UploadedBytes { get; set; }

	/// <summary>
	/// Upload request size (slightly larger than the file itself).
	/// </summary>
	public long UploadSize { get; set; }
}
