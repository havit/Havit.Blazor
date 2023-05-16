using System.Collections.Concurrent;
using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Raw component extending <see cref="InputFile"/> with direct upload.
/// </summary>
public class HxInputFileCore : InputFile, IAsyncDisposable
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputFileCore"/>.
	/// </summary>
	public static InputFileCoreSettings Defaults { get; }

	static HxInputFileCore()
	{
		Defaults = new InputFileCoreSettings()
		{
			MaxFileSize = long.MaxValue,
			MaxParallelUploads = 6
		};
	}

	/// <summary>
	/// Returns component defaults.
	/// Enables overriding defaults in descandants (use separate set of defaults).
	/// </summary>
	protected virtual InputFileCoreSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
	/// </summary>
	[Parameter] public InputFileCoreSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
	/// </remarks>
	protected virtual InputFileCoreSettings GetSettings() => this.Settings;


	/// <summary>
	/// URL of the server endpoint receiving the files.
	/// </summary>
	[Parameter] public string UploadUrl { get; set; }

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
	/// Make the item appear disabled by setting to <c>false</c>.
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool Enabled { get; set; } = true;

	/// <summary>
	/// Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow.
	/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">MDN Web Docs - HTML attribute: accept</see>.
	/// </summary>
	[Parameter] public string Accept { get; set; }

	/// <summary>
	/// The maximum files size in bytes.
	/// When exceeded, the <see cref="OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
	/// Default is <c>long.MaxValue</c> (unlimited).
	/// </summary>
	[Parameter] public long? MaxFileSize { get; set; }
	protected long MaxFileSizeEffective => this.MaxFileSize ?? this.GetSettings()?.MaxFileSize ?? GetDefaults().MaxFileSize ?? throw new InvalidOperationException(nameof(MaxFileSize) + " default for " + nameof(HxInputFileCore) + " has to be set.");

	/// <summary>
	/// Maximum number of concurrent uploads. Default is <c>6</c>.
	/// </summary>
	[Parameter] public int? MaxParallelUploads { get; set; }
	protected int MaxParallelUploadsEffective => this.MaxParallelUploads ?? this.GetSettings()?.MaxParallelUploads ?? this.GetDefaults().MaxParallelUploads ?? throw new InvalidOperationException(nameof(MaxParallelUploads) + " default for " + nameof(HxInputFileCore) + " has to be set.");

	/// <summary>
	/// Input element id.
	/// </summary>
	[Parameter] public string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// Last known count of associated files.
	/// </summary>
	public int FileCount { get; private set; }

	private DotNetObjectReference<HxInputFileCore> dotnetObjectReference;
	private IJSObjectReference jsModule;
	private TaskCompletionSource<UploadCompletedEventArgs> uploadCompletedTaskCompletionSource;
	private ConcurrentBag<FileUploadedEventArgs> filesUploaded;
	private bool disposed;


	public HxInputFileCore()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		// Temporary hack as base implementation of InputFile does not expose ElementReference (vNext: https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/InputFile.cs)
		AdditionalAttributes ??= new Dictionary<string, object>();
		AdditionalAttributes["id"] = this.Id;

		AdditionalAttributes["multiple"] = this.Multiple;
		AdditionalAttributes["accept"] = this.Accept;
		AdditionalAttributes["disabled"] = !this.Enabled;
	}

	/// <summary>
	/// Initiates the upload (does not wait for upload completion). Use OnUploadCompleted event.
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	/// <remarks>
	/// We do not want to make the Havit.Blazor library dependant on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
	/// </remarks>
	public async Task StartUploadAsync(string accessToken = null)
	{
		Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(UploadUrl), nameof(UploadUrl) + " has to be set.");

		await EnsureJsModuleAsync();
		filesUploaded = new ConcurrentBag<FileUploadedEventArgs>();

		if (disposed)
		{
			return;
		}
		await jsModule.InvokeVoidAsync("upload", Id, dotnetObjectReference, UploadUrl, accessToken, MaxFileSizeEffective == long.MaxValue ? null : MaxFileSizeEffective, MaxParallelUploadsEffective);
	}

	/// <summary>
	/// Uploads the file(s).
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	public async Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null)
	{
		uploadCompletedTaskCompletionSource = new TaskCompletionSource<UploadCompletedEventArgs>();

		await StartUploadAsync(accessToken);

		return await uploadCompletedTaskCompletionSource.Task;
	}

	/// <summary>
	/// Gets list of files chosen.
	/// </summary>
	public async Task<FileInfo[]> GetFilesAsync()
	{
		await EnsureJsModuleAsync();
		return await jsModule.InvokeAsync<FileInfo[]>("getFiles", Id);
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorWebModuleAsync(nameof(HxInputFileCore));
	}

	/// <summary>
	/// Clears associated input element and resets component to initial state.
	/// </summary>
	public async Task ResetAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("reset", Id);
	}

	/// <summary>
	/// Receive upload progress notification from underlying javascript.
	/// </summary>
	[JSInvokable("HxInputFileCore_HandleUploadProgress")]
	public async Task HandleUploadProgress(int fileIndex, string fileName, long loaded, long total)
	{
		var uploadProgress = new UploadProgressEventArgs()
		{
			FileIndex = fileIndex,
			OriginalFileName = fileName,
			UploadedBytes = loaded,
			UploadSize = total
		};
		await InvokeOnProgressAsync(uploadProgress);
	}

	/// <summary>
	/// Receive upload finished notification from underlying javascript.
	/// </summary>
	[JSInvokable("HxInputFileCore_HandleFileUploaded")]
	public async Task HandleFileUploaded(int fileIndex, string fileName, long fileSize, string fileType, long fileLastModified, int responseStatus, string responseText)
	{
		var fileUploaded = new FileUploadedEventArgs()
		{
			FileIndex = fileIndex,
			OriginalFileName = fileName,
			ContentType = fileType,
			Size = fileSize,
			LastModified = DateTimeOffset.FromUnixTimeMilliseconds(fileLastModified),
			ResponseStatus = (HttpStatusCode)responseStatus,
			ResponseText = responseText,
		};
		filesUploaded?.Add(fileUploaded);
		await InvokeOnFileUploadedAsync(fileUploaded);
	}

	/// <summary>
	/// Receive upload finished notification from underlying javascript.
	/// </summary>
	[JSInvokable("HxInputFileCore_HandleUploadCompleted")]
	public async Task HandleUploadCompleted(int fileCount, long totalSize)
	{
		var uploadCompleted = new UploadCompletedEventArgs()
		{
			FilesUploaded = filesUploaded,
			FileCount = fileCount,
			TotalSize = totalSize
		};
		filesUploaded = null;
		uploadCompletedTaskCompletionSource?.TrySetResult(uploadCompleted);
		await InvokeOnUploadCompletedAsync(uploadCompleted);
	}


	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		disposed = true;

		// Microsoft violates the pattern - there is no protected virtual voud Dispose(bool) method and the IDisposable implementation is explicit.
		((IDisposable)this).Dispose();

		if (jsModule != null)
		{
			try
			{
				await jsModule.InvokeVoidAsync("dispose", Id);
				await jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}

		dotnetObjectReference.Dispose();
	}
}