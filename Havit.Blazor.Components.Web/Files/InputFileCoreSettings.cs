namespace Havit.Blazor.Components.Web;

/// <summary>
/// Defaults for the <see cref="HxInputFileCore"/>.
/// </summary>
public record InputFileCoreSettings
{
	/// <summary>
	/// The maximum file size in bytes.
	/// When exceeded, the <see cref="HxInputFileCore.OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
	/// </summary>
	public long? MaxFileSize { get; set; }

	/// <summary>
	/// Maximum number of concurrent uploads.
	/// </summary>
	public int? MaxParallelUploads { get; set; }
}
