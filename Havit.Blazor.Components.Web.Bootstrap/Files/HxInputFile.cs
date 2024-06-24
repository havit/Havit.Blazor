using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Wraps <see cref="HxInputFileCore"/> as a Bootstrap form control (including <c>Label</c> etc.)<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputFile">https://havit.blazor.eu/components/HxInputFile</see>
/// </summary>
public partial class HxInputFile : ComponentBase, ICascadeEnabledComponent, IFormValueComponent, IInputWithSize
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputFile"/> and derived components.
	/// </summary>
	public static InputFileSettings Defaults { get; set; }

	static HxInputFile()
	{
		Defaults = new InputFileSettings()
		{
			InputSize = Bootstrap.InputSize.Regular,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual InputFileSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputFileSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual InputFileSettings GetSettings() => Settings;


	/// <summary>
	/// URL of the server endpoint receiving the files.
	/// </summary>
	[Parameter] public string UploadUrl { get; set; }

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
	/// Maximum number of concurrent uploads. The default is <c>6</c> (from <see cref="HxInputFileCore"/>).
	/// </summary>
	[Parameter] public int? MaxParallelUploads { get; set; }

	/// <summary>
	/// Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow.
	/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">MDN Web Docs - HTML attribute: accept</see>.
	/// </summary>
	[Parameter] public string Accept { get; set; }

	/// <summary>
	/// The maximum file size in bytes.
	/// When exceeded, the <see cref="OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
	/// The default is <c>null</c> (unlimited).
	/// </summary>
	[Parameter] public long? MaxFileSize { get; set; }

	#region IFormValueComponent public properties
	/// <summary>
	/// Label to render before input (or after input for Checkbox).		
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <summary>
	/// Label to render before input (or after input for Checkbox).
	/// </summary>
	[Parameter] public RenderFragment LabelTemplate { get; set; }

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	[Parameter] public string Hint { get; set; }

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	[Parameter] public RenderFragment HintTemplate { get; set; }

	/// <summary>
	/// Custom CSS class to render with the wrapping div.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with the label.
	/// </summary>
	[Parameter] public string LabelCssClass { get; set; }
	#endregion

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxInputFile) + " has to be set.");
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <summary>
	/// Custom CSS class to render with the input element.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	/// <inheritdoc cref="Web.FormState" />
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	/// <summary>
	/// Last known count of associated files.
	/// </summary>
	public int FileCount => _hxInputFileCoreComponentReference.FileCount;

	/// <summary>
	/// ID if the input element. Autogenerated when used with label.
	/// </summary>
	protected string InputId { get; private set; } = "hx" + Guid.NewGuid().ToString("N");

	/// <summary>
	/// CSS class to be rendered with the input element.
	/// </summary>
	private protected virtual string CoreInputCssClass => "form-control";

	string IFormValueComponent.LabelFor => InputId;
	string IFormValueComponent.CoreCssClass => "";
	string IFormValueComponent.CoreLabelCssClass => "form-label";
	string IFormValueComponent.CoreHintCssClass => "form-text";

	private protected HxInputFileCore _hxInputFileCoreComponentReference;

	/// <summary>
	/// Gets list of files chosen.
	/// </summary>
	public Task<FileInfo[]> GetFilesAsync() => _hxInputFileCoreComponentReference.GetFilesAsync();

	/// <summary>
	/// Clears associated input-file element and resets component to initial state.
	/// </summary>
	public Task ResetAsync() => _hxInputFileCoreComponentReference.ResetAsync();

	/// <summary>
	/// Starts the upload.
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	/// <remarks>
	/// We do not want to make the Havit.Blazor library dependent on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
	/// </remarks>
	public Task StartUploadAsync(string accessToken = null) => _hxInputFileCoreComponentReference?.StartUploadAsync(accessToken);

	/// <summary>
	/// Uploads the file(s).
	/// </summary>
	/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
	public Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null) => _hxInputFileCoreComponentReference?.UploadAsync(accessToken);

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenRegion(0);
		base.BuildRenderTree(builder);
		builder.CloseRegion();

		FormValueRenderer.Current.Render(1, builder, this);
	}

	void IFormValueComponent.RenderValue(RenderTreeBuilder builder)
	{
		builder.OpenComponent<HxInputFileCore>(1);
		builder.AddAttribute(1001, nameof(HxInputFileCore.Id), InputId);
		builder.AddAttribute(1002, nameof(HxInputFileCore.UploadUrl), UploadUrl);
		builder.AddAttribute(1003, nameof(HxInputFileCore.Multiple), Multiple);
		builder.AddAttribute(1004, nameof(HxInputFileCore.OnChange), EventCallback.Factory.Create<InputFileChangeEventArgs>(this, InvokeOnChangeAsync));
		builder.AddAttribute(1005, nameof(HxInputFileCore.OnProgress), EventCallback.Factory.Create<UploadProgressEventArgs>(this, InvokeOnProgressAsync));
		builder.AddAttribute(1006, nameof(HxInputFileCore.OnFileUploaded), EventCallback.Factory.Create<FileUploadedEventArgs>(this, InvokeOnFileUploadedAsync));
		builder.AddAttribute(1007, nameof(HxInputFileCore.OnUploadCompleted), EventCallback.Factory.Create<UploadCompletedEventArgs>(this, InvokeOnUploadCompletedAsync));
		builder.AddAttribute(1008, nameof(HxInputFileCore.Accept), Accept);
		builder.AddAttribute(1009, nameof(HxInputFileCore.MaxFileSize), MaxFileSize);
		builder.AddAttribute(1010, nameof(HxInputFileCore.MaxParallelUploads), MaxParallelUploads);
		builder.AddAttribute(1011, "class", CssClassHelper.Combine(CoreInputCssClass, InputCssClass, (this is IInputWithSize inputWithSize) ? inputWithSize.GetInputSizeCssClass() : null));
		builder.AddAttribute(1012, nameof(HxInputFileCore.Enabled), CascadeEnabledComponent.EnabledEffective(this));
		builder.AddComponentReferenceCapture(1013, r => _hxInputFileCoreComponentReference = (HxInputFileCore)r);
		builder.CloseComponent();
	}
}
