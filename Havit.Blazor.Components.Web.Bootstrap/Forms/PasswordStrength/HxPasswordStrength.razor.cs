using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/password-strength/">Bootstrap Password strength</see> meter (new in Bootstrap 6).<br />
/// Provides real-time visual feedback on password strength. Attaches the Bootstrap Strength plugin to a password input&#8212;either a password input
/// placed in <see cref="ChildContent"/> or any input targeted with <see cref="InputSelector"/>. The password is evaluated in JavaScript on every keystroke
/// (no server round-trips, the password itself never leaves the browser); strength level changes are surfaced to .NET via <see cref="OnStrengthChanged"/>.<br />
/// This is not a form input component&#8212;it does not participate in <c>EditForm</c> validation and does not interfere with validation classes of the associated input.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPasswordStrength">https://havit.blazor.eu/components/HxPasswordStrength</see>
/// </summary>
public partial class HxPasswordStrength : IAsyncDisposable
{
	/// <summary>
	/// Number of segments of the segmented meter (corresponds to the number of strength levels).
	/// </summary>
	internal const int SegmentCount = 4;

	/// <summary>
	/// Application-wide defaults for <see cref="HxPasswordStrength"/> and derived components.
	/// </summary>
	public static PasswordStrengthSettings Defaults { get; set; }

	static HxPasswordStrength()
	{
		Defaults = new PasswordStrengthSettings();
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual PasswordStrengthSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public PasswordStrengthSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual PasswordStrengthSettings GetSettings() => Settings;

	/// <summary>
	/// Content rendered above the strength meter, typically containing the password input the meter should evaluate
	/// (e.g. <c>HxInputText</c> with <c>Type="InputType.Password"</c>).
	/// When no <see cref="InputSelector"/> is set, the meter automatically attaches to the first <c>input[type="password"]</c> found in the content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// CSS selector of the password input to evaluate (the <c>input</c> option of the underlying plugin).
	/// Use when the password input cannot be placed in <see cref="ChildContent"/> (the meter then attaches to the input anywhere in the document).
	/// </summary>
	[Parameter] public string InputSelector { get; set; }

	/// <summary>
	/// Visualization variant of the strength meter.
	/// The default is <see cref="PasswordStrengthVariant.Segments"/> (a four-segment meter); use <see cref="PasswordStrengthVariant.ProgressBar"/> for a single growing bar.
	/// </summary>
	[Parameter] public PasswordStrengthVariant? Variant { get; set; }
	protected PasswordStrengthVariant VariantEffective => Variant ?? GetSettings()?.Variant ?? GetDefaults().Variant ?? PasswordStrengthVariant.Segments;

	/// <summary>
	/// Minimum password length required for the first strength point (the <c>minLength</c> option of the underlying plugin).
	/// The default is <c>8</c> (underlying plugin default).
	/// </summary>
	[Parameter] public int? MinLength { get; set; }
	protected int? MinLengthEffective => MinLength ?? GetSettings()?.MinLength ?? GetDefaults().MinLength;

	/// <summary>
	/// Point values for the individual criteria of the scoring algorithm (the <c>weights</c> option of the underlying plugin).
	/// Set a weight to <c>0</c> to disable the corresponding criterion. When not set, the underlying plugin defaults are used (all criteria worth <c>1</c> point).
	/// </summary>
	[Parameter] public PasswordStrengthWeights Weights { get; set; }
	protected PasswordStrengthWeights WeightsEffective => Weights ?? GetSettings()?.Weights ?? GetDefaults().Weights;

	/// <summary>
	/// Score boundaries for the strength levels (the <c>thresholds</c> option of the underlying plugin).
	/// When not set, the underlying plugin defaults are used (weak &#8804; 2, fair &#8804; 4, good &#8804; 6, strong &gt; 6).
	/// </summary>
	[Parameter] public PasswordStrengthThresholds Thresholds { get; set; }
	protected PasswordStrengthThresholds ThresholdsEffective => Thresholds ?? GetSettings()?.Thresholds ?? GetDefaults().Thresholds;

	/// <summary>
	/// When <c>true</c>, a text feedback element (<c>strength-text</c>) displaying the strength level message is rendered below the meter.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? ShowText { get; set; }
	protected bool ShowTextEffective => ShowText ?? GetSettings()?.ShowText ?? GetDefaults().ShowText ?? false;

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Weak"/> level. The default is a localized <c>Weak</c>.
	/// </summary>
	[Parameter] public string WeakText { get; set; }
	protected string WeakTextEffective => WeakText ?? GetSettings()?.WeakText ?? GetDefaults().WeakText ?? Localizer["Weak"];

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Fair"/> level. The default is a localized <c>Fair</c>.
	/// </summary>
	[Parameter] public string FairText { get; set; }
	protected string FairTextEffective => FairText ?? GetSettings()?.FairText ?? GetDefaults().FairText ?? Localizer["Fair"];

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Good"/> level. The default is a localized <c>Good</c>.
	/// </summary>
	[Parameter] public string GoodText { get; set; }
	protected string GoodTextEffective => GoodText ?? GetSettings()?.GoodText ?? GetDefaults().GoodText ?? Localizer["Good"];

	/// <summary>
	/// Text feedback message for the <see cref="PasswordStrengthLevel.Strong"/> level. The default is a localized <c>Strong</c>.
	/// </summary>
	[Parameter] public string StrongText { get; set; }
	protected string StrongTextEffective => StrongText ?? GetSettings()?.StrongText ?? GetDefaults().StrongText ?? Localizer["Strong"];

	/// <summary>
	/// Strength level for a static (CSS-only) display.
	/// When set, the meter renders the level directly (filled segments, <c>data-bs-strength</c> attribute, text feedback)
	/// and no JavaScript evaluation takes place (<see cref="OnStrengthChanged"/> is never raised).
	/// </summary>
	[Parameter] public PasswordStrengthLevel? Strength { get; set; }

	/// <summary>
	/// Raised when the strength level of the evaluated password changes (the <c>strengthChange.bs.strength</c> event of the underlying plugin).
	/// </summary>
	[Parameter] public EventCallback<PasswordStrengthChangedEventArgs> OnStrengthChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnStrengthChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnStrengthChangedAsync(PasswordStrengthChangedEventArgs args) => OnStrengthChanged.InvokeAsync(args);

	/// <summary>
	/// Additional CSS class(es) for the strength meter.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto the strength meter element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }
	[Inject] protected IStringLocalizer<HxPasswordStrength> Localizer { get; set; }

	private ElementReference _elementReference;
	private DotNetObjectReference<HxPasswordStrength> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _initialized;
	private bool _disposed;

	public HxPasswordStrength()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && (Strength is null))
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _elementReference, _dotnetObjectReference, GetJsOptions());
			_initialized = true;
		}
	}

	/// <summary>
	/// Builds the options for the underlying Bootstrap Strength plugin.
	/// Only options explicitly set are included (the plugin merges its own defaults shallowly;
	/// <see cref="PasswordStrengthWeights"/> and <see cref="PasswordStrengthThresholds"/> are therefore always passed complete).
	/// </summary>
	private Dictionary<string, object> GetJsOptions()
	{
		var options = new Dictionary<string, object>();

		if (!String.IsNullOrEmpty(InputSelector))
		{
			options["input"] = InputSelector;
		}

		if (MinLengthEffective is not null)
		{
			options["minLength"] = MinLengthEffective.Value;
		}

		options["messages"] = new Dictionary<string, string>
		{
			["weak"] = WeakTextEffective,
			["fair"] = FairTextEffective,
			["good"] = GoodTextEffective,
			["strong"] = StrongTextEffective
		};

		if (WeightsEffective is not null)
		{
			options["weights"] = new Dictionary<string, int>
			{
				["minLength"] = WeightsEffective.MinLength,
				["extraLength"] = WeightsEffective.ExtraLength,
				["lowercase"] = WeightsEffective.Lowercase,
				["uppercase"] = WeightsEffective.Uppercase,
				["numbers"] = WeightsEffective.Numbers,
				["special"] = WeightsEffective.Special,
				["multipleSpecial"] = WeightsEffective.MultipleSpecial,
				["longPassword"] = WeightsEffective.LongPassword
			};
		}

		if (ThresholdsEffective is not null)
		{
			options["thresholds"] = new int[] { ThresholdsEffective.Weak, ThresholdsEffective.Fair, ThresholdsEffective.Good };
		}

		return options;
	}

	/// <summary>
	/// Receives notification from JavaScript when the strength level changes.
	/// </summary>
	[JSInvokable("HxPasswordStrength_HandleStrengthChanged")]
	public async Task HandleStrengthChanged(string strength, int score)
	{
		await InvokeOnStrengthChangedAsync(new PasswordStrengthChangedEventArgs
		{
			Strength = ParseStrengthLevel(strength),
			Score = score
		});
	}

	/// <summary>
	/// Gets the current strength level of the evaluated password (<c>null</c> when the password is empty).
	/// For a static display (<see cref="Strength"/> set), returns the <see cref="Strength"/> value.
	/// </summary>
	public async Task<PasswordStrengthLevel?> GetStrengthAsync()
	{
		if (Strength is not null)
		{
			return Strength;
		}

		if (!_initialized)
		{
			return null;
		}

		await EnsureJsModuleAsync();
		string strength = await _jsModule.InvokeAsync<string>("getStrength", _elementReference);
		return ParseStrengthLevel(strength);
	}

	/// <summary>
	/// Manually triggers the password evaluation.
	/// Useful when the input value is changed programmatically (no <c>input</c> event is raised in such a case).
	/// </summary>
	public async Task EvaluateAsync()
	{
		if (!_initialized)
		{
			return;
		}

		await EnsureJsModuleAsync();
		await _jsModule.InvokeVoidAsync("evaluate", _elementReference);
	}

	private static PasswordStrengthLevel? ParseStrengthLevel(string strength)
	{
		return strength switch
		{
			"weak" => PasswordStrengthLevel.Weak,
			"fair" => PasswordStrengthLevel.Fair,
			"good" => PasswordStrengthLevel.Good,
			"strong" => PasswordStrengthLevel.Strong,
			_ => null
		};
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxPasswordStrength));
	}

	protected virtual string GetMeterCssClass()
	{
		return CssClassHelper.Combine(
			VariantEffective switch
			{
				PasswordStrengthVariant.Segments => "strength",
				PasswordStrengthVariant.ProgressBar => "strength-bar",
				_ => throw new InvalidOperationException($"Unknown {nameof(PasswordStrengthVariant)} value {VariantEffective}.")
			},
			CssClassEffective);
	}

	private string GetSegmentCssClass(int segmentIndex)
	{
		return CssClassHelper.Combine(
			"strength-segment",
			((Strength is not null) && (segmentIndex <= (int)Strength.Value)) ? "active" : null);
	}

	/// <summary>
	/// Returns the <c>data-bs-strength</c> attribute value for the static display (<c>null</c> in the dynamic mode, the attribute is managed by the plugin).
	/// </summary>
	private string GetStaticStrengthAttributeValue()
	{
		return Strength switch
		{
			null => null,
			PasswordStrengthLevel.Weak => "weak",
			PasswordStrengthLevel.Fair => "fair",
			PasswordStrengthLevel.Good => "good",
			PasswordStrengthLevel.Strong => "strong",
			_ => throw new InvalidOperationException($"Unknown {nameof(PasswordStrengthLevel)} value {Strength}.")
		};
	}

	private string GetStaticTextValue()
	{
		return Strength switch
		{
			null => null,
			PasswordStrengthLevel.Weak => WeakTextEffective,
			PasswordStrengthLevel.Fair => FairTextEffective,
			PasswordStrengthLevel.Good => GoodTextEffective,
			PasswordStrengthLevel.Strong => StrongTextEffective,
			_ => throw new InvalidOperationException($"Unknown {nameof(PasswordStrengthLevel)} value {Strength}.")
		};
	}

	/// <summary>
	/// Returns the text feedback color for the static display (the plugin sets the <c>--strength-color</c> custom property the same way in the dynamic mode).
	/// </summary>
	private string GetStaticTextStyle()
	{
		return Strength switch
		{
			null => null,
			PasswordStrengthLevel.Weak => "--strength-color: var(--danger-text)",
			PasswordStrengthLevel.Fair => "--strength-color: var(--warning-text)",
			PasswordStrengthLevel.Good => "--strength-color: var(--info-text)",
			PasswordStrengthLevel.Strong => "--strength-color: var(--success-text)",
			_ => throw new InvalidOperationException($"Unknown {nameof(PasswordStrengthLevel)} value {Strength}.")
		};
	}

	/// <inheritdoc />
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
				if (_initialized)
				{
					await _jsModule.InvokeVoidAsync("dispose", _elementReference);
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
		}

		_dotnetObjectReference?.Dispose();
	}
}
