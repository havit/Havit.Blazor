using System.Net;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Arguments for <see cref="HxInputFileCore.OnFileUploaded"/> event.
/// </summary>
public class FileUploadedEventArgs
{
	/// <summary>
	/// Index of the file uploaded.
	/// </summary>
	public int FileIndex { get; internal set; }

	/// <summary>
	/// Name of the file provided by the browser.
	/// </summary>
	public string OriginalFileName { get; set; }

	/// <summary>
	/// File MIME type provided by the browser.
	/// </summary>
	public string ContentType { get; set; }

	/// <summary>
	/// File provided by the browser.
	/// </summary>
	public long Size { get; set; }

	/// <summary>
	/// Response body received from the UploadUrl endpoint.
	/// </summary>
	public string ResponseText { get; set; }

	/// <summary>
	/// Response status received from the UploadUrl endpoint.
	/// </summary>
	public HttpStatusCode ResponseStatus { get; set; }

	/// <summary>
	/// File last modification provided by the browser.
	/// </summary>
	public DateTimeOffset LastModified { get; set; }
}
