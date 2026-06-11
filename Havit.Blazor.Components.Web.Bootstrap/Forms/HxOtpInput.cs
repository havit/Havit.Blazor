using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/otp-input/">Bootstrap OTP input</see> component (new in Bootstrap 6).<br />
/// Connected input fields for one-time passwords, PIN codes, and verification codes
/// with automatic focus advancement, backspace navigation, paste support, and browser autofill (<c>autocomplete="one-time-code"</c>).
/// The <c>Value</c> is bound as a <see cref="string"/> (the concatenated code).<br />
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
	/// The number of inputs (digits) to render. The default is <c>6</c>.
	/// </summary>
	[Parameter] public int? Length { get; set; }
	protected int LengthEffective => Length ?? GetSettings()?.Length ?? GetDefaults().Length ?? throw new InvalidOperationException(nameof(Length) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the inputs render as <c>type="password"</c> fields to hide the entered values.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Mask { get; set; }
	protected bool MaskEffective => Mask ?? GetSettings()?.Mask ?? GetDefaults().Mask ?? throw new InvalidOperationException(nameof(Mask) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the inputs are visually connected into a single cohesive field (Bootstrap <c>input-group</c> styles).
	/// When used together with <see cref="GroupSize"/>, each group of inputs is wrapped in a nested <c>input-group</c>.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Connected { get; set; }
	protected bool ConnectedEffective => Connected ?? GetSettings()?.Connected ?? GetDefaults().Connected ?? throw new InvalidOperationException(nameof(Connected) + " default for " + nameof(HxOtpInput) + " has to be set.");

	/// <summary>
	/// When set, the inputs are split into groups of this size with a visual <see cref="Separator"/> between the groups (e.g. <c>123-456</c>).
	/// The separator is purely visual, it does not affect the <c>Value</c>.
	/// The default is <c>null</c> (no groups).
	/// </summary>
	[Parameter] public int? GroupSize { get; set; }

	/// <summary>
	/// The separator text rendered between the groups of inputs (<see cref="GroupSize"/>). The default is <c>–</c> (en dash).
	/// </summary>
	[Parameter] public string Separator { get; set; } = "–";

	/// <summary>
	/// Raised when all inputs are filled (Bootstrap <c>complete.bs.otp-input</c> event).
	/// Receives the complete code as a parameter.
	/// </summary>
	[Parameter] public EventCallback<string> OnCompleted { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnCompleted"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnCompletedAsync(string value) => OnCompleted.InvokeAsync(value);

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <inheritdoc cref="HxInputBase{TValue}.CoreInputCssClass" />
	private protected override string CoreInputCssClass => CssClassHelper.Combine("otp", (ConnectedEffective && (GroupSize is null)) ? "input-group" : null);

	private ElementReference _otpInputElement;
	private DotNetObjectReference<HxOtpInput> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _jsInitialized;
	private int _jsInitializedLength;
	private bool _jsInitializedMask;
	private string _jsValue; // value as known by the JavaScript plugin (kept in sync via JS interop)
	private string _renderedValue; // value whose characters are rendered to the value attributes (kept stable while JS owns the input values)
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

		int lengthEffective = LengthEffective;

		builder.OpenElement(100, "div");
		builder.AddMultipleAttributes(101, AdditionalAttributes);
		builder.AddAttribute(102, "class", GetInputCssClassToRender());

		string ariaDescribedBy = String.Join(" ", HintId, ValidationMessageId).Trim();
		if (!String.IsNullOrEmpty(ariaDescribedBy))
		{
			builder.AddAttribute(103, "aria-describedby", ariaDescribedBy);
		}
		builder.AddElementReferenceCapture(104, elementReference => _otpInputElement = elementReference);

		if (GroupSize is null)
		{
			for (int index = 0; index < lengthEffective; index++)
			{
				builder.OpenRegion(200);
				BuildRenderInput_RenderDigitInput(builder, index);
				builder.CloseRegion();
			}
		}
		else
		{
			int groupSize = GroupSize.Value;
			for (int groupStartIndex = 0; groupStartIndex < lengthEffective; groupStartIndex += groupSize)
			{
				builder.OpenRegion(300);

				if (groupStartIndex > 0)
				{
					builder.OpenElement(301, "span");
					builder.AddAttribute(302, "class", "otp-separator");
					builder.AddContent(303, Separator);
					builder.CloseElement(); // span.otp-separator
				}

				if (ConnectedEffective)
				{
					builder.OpenElement(304, "div");
					builder.AddAttribute(305, "class", "input-group");
				}

				for (int index = groupStartIndex; index < Math.Min(groupStartIndex + groupSize, lengthEffective); index++)
				{
					builder.OpenRegion(306);
					BuildRenderInput_RenderDigitInput(builder, index);
					builder.CloseRegion();
				}

				if (ConnectedEffective)
				{
					builder.CloseElement(); // div.input-group
				}

				builder.CloseRegion();
			}
		}

		builder.CloseElement(); // div.otp
	}

	/// <summary>
	/// Renders a single digit input.
	/// The attributes follow the Bootstrap OTP input accessibility guidance
	/// (the same attributes the Bootstrap plugin sets on the inputs, rendered by Blazor to keep the render tree consistent with the DOM).
	/// </summary>
	private void BuildRenderInput_RenderDigitInput(RenderTreeBuilder builder, int index)
	{
		builder.OpenElement(1, "input");
		builder.AddAttribute(2, "type", MaskEffective ? "password" : "text");
		builder.AddAttribute(3, "class", "form-control");
		builder.AddAttribute(4, "maxlength", "1");
		builder.AddAttribute(5, "inputmode", "numeric");
		builder.AddAttribute(6, "pattern", "\\d*");
		builder.AddAttribute(7, "autocomplete", (index == 0) ? "one-time-code" : "off"); // first input gets autocomplete for browser OTP autofill
		builder.AddAttribute(8, "aria-label", $"Digit {index + 1}");
		if ((index == 0) && !String.IsNullOrEmpty(InputId))
		{
			builder.AddAttribute(9, "id", InputId);
		}
		builder.AddAttribute(10, "disabled", !EnabledEffective);
		if (index < _renderedValue?.Length)
		{
			builder.AddAttribute(11, "value", _renderedValue[index].ToString());
		}
		builder.CloseElement(); // input
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (_disposed)
		{
			return;
		}

		if (!_jsInitialized || (_jsInitializedLength != LengthEffective) || (_jsInitializedMask != MaskEffective))
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			_jsInitialized = true;
			_jsInitializedLength = LengthEffective;
			_jsInitializedMask = MaskEffective;
			_jsValue = CurrentValueAsString ?? String.Empty;

			await _jsModule.InvokeVoidAsync("initialize", _otpInputElement, _dotnetObjectReference, MaskEffective, _jsValue);
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
	/// Receives notification from JavaScript when the value of any of the inputs changes (Bootstrap <c>input.bs.otp-input</c> event).
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
	/// Receives notification from JavaScript when all inputs are filled (Bootstrap <c>complete.bs.otp-input</c> event).
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
	/// Focuses the component (the first empty input, or the last input when all inputs are filled).
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
