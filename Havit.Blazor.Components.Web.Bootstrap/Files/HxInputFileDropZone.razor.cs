using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Ready-made UX for drag&amp;drop file upload.
/// For custom drag&amp;drop UX, use <see cref="HxInputFileCore"/> and <see href="https://github.com/havit/Havit.Blazor/blob/728567c9c83a0b4ab7fe2e031bf1ff378f1b1ce7/Havit.Blazor.Components.Web.Bootstrap/Files/HxInputFileDropZone.razor.css#L20-L26">a little bit of HTML/CSS</see>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputFileDropZone">https://havit.blazor.eu/components/HxInputFileDropZone</see>
/// </summary>
public partial class HxInputFileDropZone : ICascadeEnabledComponent
{
	private const int FirstFileNamesMaxCount = 3; // Might be converted to a parameter if needed.

	/// <summary>
	/// URL of the server endpoint receiving the files.
	/// </summary>
	[Parameter] public string UploadUrl { get; set; }

	/// <summary>
	/// HTTP Method (verb) used for file upload. The default is <c>POST</c>.
	/// </summary>
	[Parameter] public string UploadHttpMethod { get; set; } = "POST";

	/// <summary>
	/// Gets or sets the event callback that will be invoked when the collection of selected files changes.
	/// </summary>
	[Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnChange"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnChangeAsync(InputFileChangeEventArgs args) => OnChange.InvokeAsync(args);

	/// <summary>
	/// Raised during running file upload (the frequency depends on browser implementation).
	/// </summary>
	[Parameter] public EventCallback<UploadProgressEventArgs> OnProgress { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnProgress"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnProgressAsync(UploadProgressEventArgs args) => OnProgress.InvokeAsync(args);

	/// <summary>
	/// Raised after a file is uploaded (for every single file separately).
	/// </summary>
	[Parameter] public EventCallback<FileUploadedEventArgs> OnFileUploaded { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnFileUploaded"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnFileUploadedAsync(FileUploadedEventArgs args) => OnFileUploaded.InvokeAsync(args);

	/// <summary>
	/// Raised when all files are uploaded (after all <see cref="OnFileUploaded"/> events).
	/// </summary>
	[Parameter] public EventCallback<UploadCompletedEventArgs> OnUploadCompleted { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnUploadCompleted"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnUploadCompletedAsync(UploadCompletedEventArgs args) => OnUploadCompleted.InvokeAsync(args);

	/// <summary>
	/// Single <c>false</c> or multiple <c>true</c> files upload.
	/// </summary>
	[Parameter] public bool Multiple { get; set; }

	/// <summary>
	/// The maximum file size in bytes.
	/// When exceeded, the <see cref="OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
	/// Default is <c>null</c> (unlimited).
	/// </summary>
	[Parameter] public long? MaxFileSize { get; set; }

	/// <summary>
	/// Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow.
	/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">MDN Web Docs - HTML attribute: accept</see>.
	/// </summary>
	[Parameter] public string Accept { get; set; }

	/// <summary>
	/// Content to render when no files are picked. Default content is displayed when not set.
	/// </summary>
	[Parameter] public RenderFragment NoFilesTemplate { get; set; }

	/// <summary>
	/// Custom CSS class to render with the wrapping div.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with the input element.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	/// <inheritdoc cref="Web.FormState" />
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	[Inject] protected IStringLocalizer<HxInputFileDropZone> Localizer { get; set; }

	private int _fileCount = 0;
	private List<string> _firstFileNames = new List<string>();
	private protected HxInputFileCore _hxInputFileCoreComponentReference;

	/// <summary>
	/// Gets the list of chosen files.
	/// </summary>
	public Task<FileInfo[]> GetFilesAsync() => _hxInputFileCoreComponentReference.GetFilesAsync();

	/// <summary>
	/// Clears the associated input-file element and resets the component to its initial state.
	/// </summary>
	public Task ResetAsync() => _hxInputFileCoreComponentReference.ResetAsync();

	/// <summary>
	/// Starts the upload.
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	/// <param name="antiforgeryToken">Antiforgery Token to be used for upload</param>
	/// <param name="antiforgeryHeaderName">The name of the antiforgery header to be used for upload. Default is "RequestVerificationToken".</param>
	/// <remarks>
	/// We do not want to make the Havit.Blazor library dependent on WebAssembly libraries (IAccessTokenProvider and such). Therefore, the accessToken here...
	/// </remarks>
	public Task StartUploadAsync(string accessToken = null, string antiforgeryToken = null, string antiforgeryHeaderName = "RequestVerificationToken")
		=> _hxInputFileCoreComponentReference?.StartUploadAsync(accessToken, antiforgeryToken, antiforgeryHeaderName);

	/// <summary>
	/// Uploads the file(s).
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	/// <param name="antiforgeryToken">Antiforgery Token to be used for upload</param>
	/// <param name="antiforgeryHeaderName">The name of the antiforgery header to be used for upload. Default is "RequestVerificationToken".</param>
	public Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null, string antiforgeryToken = null, string antiforgeryHeaderName = "RequestVerificationToken")
		=> _hxInputFileCoreComponentReference?.UploadAsync(accessToken, antiforgeryToken, antiforgeryHeaderName);

	protected Task HandleOnChange(InputFileChangeEventArgs args)
	{
		_fileCount = args.FileCount;
		_firstFileNames = args.GetMultipleFiles(int.MaxValue).Take(FirstFileNamesMaxCount).Select(f => f.Name).ToList();
		StateHasChanged();

		return InvokeOnChangeAsync(args);
	}

	protected string GetFilesPickedText()
	{
		if (_fileCount == 1)
		{
			return Localizer["SingleFileSelected", _firstFileNames[0]];
		}
		else
		{
			string result = Localizer["MultipleFilesSelected", _fileCount, String.Join(", ", _firstFileNames)];
			if (_fileCount > _firstFileNames.Count)
			{
				result = result + " " + Localizer["MoreFiles", _fileCount - _firstFileNames.Count];
			}
			return result;
		}
	}

	protected bool EnabledEffective => CascadeEnabledComponent.EnabledEffective(this);
}
