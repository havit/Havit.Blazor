using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/otp-input/">Bootstrap OTP input</see> component (new in Bootstrap 6).<br />
/// A single accessible input rendered as separate digit slots for one-time passwords, PIN codes, and verification codes,
/// with in-place digit editing, paste support, and browser autofill (<c>autocomplete="one-time-code"</c>).
/// The <c>Value</c> is bound as a <see cref="string"/> (the complete code).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxOtpInput">https://havit.blazor.eu/components/HxOtpInput</see>
/// </summary>
public class HxOtpInput : HxInputBase<string>, IAsyncDisposable
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxOtpInput"/> component.
	/// </summary>
	public static OtpInputSettings Defaults { get; set; }

	static HxOtpInput()
	{
		Defaults = new OtpInputSettings()
		{
			Length = 6, // Bootstrap OtpInput default
			Mask = false,
			Connected = false
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override OtpInputSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public OtpInputSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected override OtpInputSettings GetSettings() => Settings;

	/// <summary>
	/// The number of digit slots to render (the maximum length of the code). The default is <c>6</c>.
	/// </summary>
	[Parameter] public int? Length { get; set; }
	protected int LengthEffective => Length ?? GetSettings()?.Length ?? GetDefaults().Length ?? throw new InvalidOperationException(nameof(Length) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the slots display a dot (<c>•</c>) instead of the entered characters.
	/// The input stays a regular text field, so the numeric keyboard and autofill keep working.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Mask { get; set; }
	protected bool MaskEffective => Mask ?? GetSettings()?.Mask ?? GetDefaults().Mask ?? throw new InvalidOperationException(nameof(Mask) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the slots are visually connected into a single cohesive field with shared borders (Bootstrap <c>otp-connected</c> class).
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Connected { get; set; }
	protected bool ConnectedEffective => Connected ?? GetSettings()?.Connected ?? GetDefaults().Connected ?? throw new InvalidOperationException(nameof(Connected) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When set, the slots are split into groups of this size with a visual <see cref="Separator"/> between the groups (e.g. <c>123-456</c>).
	/// The separator is purely visual, it does not affect the <c>Value</c>.
	/// The default is <c>null</c> (no groups).
	/// </summary>
	[Parameter] public int? GroupSize { get; set; }

	/// <summary>
	/// The separator text rendered between the groups of slots (<see cref="GroupSize"/>). The default is <c>–</c> (en dash).
	/// </summary>
	[Parameter] public string Separator { get; set; } = "–";

	/// <summary>
	/// Raised when all slots are filled (Bootstrap <c>complete.bs.otpInput</c> event).
	/// Receives the complete code as a parameter.
	/// </summary>
	[Parameter] public EventCallback<string> OnCompleted { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnCompleted"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnCompletedAsync(string value) => OnCompleted.InvokeAsync(value);

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
	/// <remarks>
	/// The <c>otp-rendered</c> class is added by the JavaScript plugin. Once the plugin is initialized, we render it as well,
	/// otherwise any change of the class attribute (e.g. validation state) would remove it from the DOM.
	/// </remarks>
	private protected override string CoreInputCssClass => CssClassHelper.Combine("otp", ConnectedEffective ? "otp-connected" : null, _jsInitialized ? "otp-rendered" : null);

	private ElementReference _otpInputElement;
	private DotNetObjectReference<HxOtpInput> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _jsInitialized;
	private int _jsInitializedLength;
	private bool _jsInitializedMask;
	private int? _jsInitializedGroupSize;
	private string _jsInitializedSeparator;
	private string _jsValue; // value as known by the JavaScript plugin (kept in sync via JS interop)
	private string _renderedValue; // value rendered to the value attribute (kept stable while JS owns the input value)
	private bool _disposed;

	public HxOtpInput()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		Contract.Requires<InvalidOperationException>(LengthEffective > 0, $"[{GetType().Name}] The {nameof(Length)} parameter value has to be a positive number.");
		Contract.Requires<InvalidOperationException>((GroupSize is null) || (GroupSize > 0), $"[{GetType().Name}] The {nameof(GroupSize)} parameter value has to be a positive number.");
	}

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		if ((CurrentValueAsString ?? String.Empty) != (_jsValue ?? String.Empty))
		{
			// the value was changed programmatically (or this is the initial render)
			// otherwise we keep the rendered value attributes stable - the JavaScript plugin owns the input values
			_renderedValue = CurrentValueAsString ?? String.Empty;
		}

		builder.OpenElement(100, "div");
		builder.AddMultipleAttributes(101, AdditionalAttributes);
		builder.AddAttribute(102, "class", GetInputCssClassToRender());
		builder.AddElementReferenceCapture(103, elementReference => _otpInputElement = elementReference);

		// A single input backs the whole control, the Bootstrap plugin renders the visual slots (div.otp-slots) next to it.
		// The attributes are the same the Bootstrap plugin sets on the input (rendered by Blazor to keep the render tree consistent with the DOM).
		builder.OpenElement(200, "input");
		builder.AddAttribute(201, "type", "text");
		builder.AddAttribute(202, "class", "otp-input");
		builder.AddAttribute(203, "maxlength", LengthEffective);
		builder.AddAttribute(204, "inputmode", "numeric");
		builder.AddAttribute(205, "pattern", "[0-9]*");
		builder.AddAttribute(206, "autocomplete", "one-time-code");
		builder.AddAttribute(207, "id", InputId);

		string ariaDescribedBy = String.Join(" ", HintId, ValidationMessageId).Trim();
		if (!String.IsNullOrEmpty(ariaDescribedBy))
		{
			builder.AddAttribute(208, "aria-describedby", ariaDescribedBy);
		}

		builder.AddAttribute(209, "disabled", !EnabledEffective);
		builder.AddAttribute(210, "value", _renderedValue);
		builder.CloseElement(); // input

		builder.CloseElement(); // div.otp
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (_disposed)
		{
			return;
		}

		if (!_jsInitialized
			|| (_jsInitializedLength != LengthEffective)
			|| (_jsInitializedMask != MaskEffective)
			|| (_jsInitializedGroupSize != GroupSize)
			|| (_jsInitializedSeparator != Separator))
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			_jsInitialized = true;
			_jsInitializedLength = LengthEffective;
			_jsInitializedMask = MaskEffective;
			_jsInitializedGroupSize = GroupSize;
			_jsInitializedSeparator = Separator;
			_jsValue = CurrentValueAsString ?? String.Empty;

			var options = new
			{
				length = LengthEffective,
				mask = MaskEffective,
				groups = (GroupSize is null) ? null : new[] { GroupSize.Value }, // Bootstrap repeats the last group size
				separator = Separator ?? String.Empty // Bootstrap rejects null (the option has to be a string)
			};
			await _jsModule.InvokeVoidAsync("initialize", _otpInputElement, _dotnetObjectReference, options, _jsValue);
		}
		else if ((CurrentValueAsString ?? String.Empty) != (_jsValue ?? String.Empty))
		{
			// propagate the programmatic value change to the JavaScript plugin
			_jsValue = CurrentValueAsString ?? String.Empty;
			await _jsModule.InvokeVoidAsync("setValue", _otpInputElement, _jsValue);
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxOtpInput));
	}

	/// <summary>
	/// Receives notification from JavaScript when the value changes (Bootstrap <c>input.bs.otpInput</c> event).
	/// </summary>
	/// <remarks>
	/// The method is intended for JS interop, do not call it directly.
	/// </remarks>
	[JSInvokable("HxOtpInput_HandleJsInput")]
	public Task HandleJsInput(string value)
	{
		_jsValue = value ?? String.Empty;
		CurrentValueAsString = value;

		return Task.CompletedTask;
	}

	/// <summary>
	/// Receives notification from JavaScript when all slots are filled (Bootstrap <c>complete.bs.otpInput</c> event).
	/// </summary>
	/// <remarks>
	/// The method is intended for JS interop, do not call it directly.
	/// </remarks>
	[JSInvokable("HxOtpInput_HandleJsComplete")]
	public async Task HandleJsComplete(string value)
	{
		_jsValue = value ?? String.Empty;
		CurrentValueAsString = value;

		await InvokeOnCompletedAsync(value);
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
	{
		result = value;
		validationErrorMessage = null;
		return true;
	}

	/// <summary>
	/// Focuses the component (the first empty slot, or the last slot when all slots are filled).
	/// </summary>
	public async ValueTask FocusAsync()
	{
		if (!_jsInitialized)
		{
			throw new InvalidOperationException($"[{GetType().Name}] Cannot focus. The component reference is not available (the component has not been rendered yet).");
		}
		await _jsModule.InvokeVoidAsync("focus", _otpInputElement);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				if (_jsInitialized)
				{
					await _jsModule.InvokeVoidAsync("dispose", _otpInputElement);
				}
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			catch (TaskCanceledException)
			{
				// NOOP
			}

			_jsModule = null;
		}

		_dotnetObjectReference?.Dispose();
	}
}
