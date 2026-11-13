using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Input for entering tags.
/// Does not allow duplicate tags.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputTags">https://havit.blazor.eu/components/HxInputTags</see>
/// </summary>
public class HxInputTags : HxInputBase<List<string>>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputTags"/>.
	/// </summary>
	public static InputTagsSettings Defaults { get; set; }

	static HxInputTags()
	{
		Defaults = new InputTagsSettings()
		{
			SuggestMinimumLength = 2,
			SuggestDelay = 300,
			Delimiters = new() { ',', ';', ' ' },
			ShowAddButton = false,
			Color = ThemeColor.None,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override InputTagsSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputTagsSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputTagsSettings GetSettings() => Settings;


	/// <summary>
	/// Indicates whether you are restricted to suggested items only (<c>false</c>).
	/// Default is <c>true</c> (you can type your own tags).
	/// </summary>
	[Parameter] public bool AllowCustomTags { get; set; } = true;

	/// <summary>
	/// Set to a method providing data for tag suggestions.
	/// </summary>
	[Parameter] public InputTagsDataProviderDelegate DataProvider { get; set; }

	/// <summary>
	/// The minimum number of characters to start suggesting. The default is <c>2</c>.
	/// </summary>
	[Parameter] public int? SuggestMinimumLength { get; set; }
	protected int SuggestMinimumLengthEffective => SuggestMinimumLength ?? GetSettings()?.SuggestMinimumLength ?? GetDefaults().SuggestMinimumLength ?? throw new InvalidOperationException(nameof(SuggestMinimumLength) + " default for " + nameof(HxInputTags) + " has to be set.");

	/// <summary>
	/// The debounce delay in milliseconds. The default is <c>300 ms</c>.
	/// </summary>
	[Parameter] public int? SuggestDelay { get; set; }
	protected int SuggestDelayEffective => SuggestDelay ?? GetSettings()?.SuggestDelay ?? GetDefaults().SuggestDelay ?? throw new InvalidOperationException(nameof(SuggestDelay) + " default for " + nameof(HxInputTags) + " has to be set.");

	/// <summary>
	/// Characters that divide the current input into separate tags when typed. The default is comma, semicolon, and space.
	/// </summary>
	[Parameter] public List<char> Delimiters { get; set; }
	protected List<char> DelimitersEffective => Delimiters ?? GetSettings()?.Delimiters ?? GetDefaults().Delimiters ?? throw new InvalidOperationException(nameof(Delimiters) + " default for " + nameof(HxInputTags) + " has to be set.");

	/// <summary>
	/// Indicates whether the add icon (+) should be displayed. The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? ShowAddButton { get; set; }
	protected bool ShowAddButtonEffective => ShowAddButton ?? GetSettings()?.ShowAddButton ?? GetDefaults().ShowAddButton ?? throw new InvalidOperationException(nameof(ShowAddButton) + " default for " + nameof(HxInputTags) + " has to be set.");

	/// <summary>
	/// The optional text for the add button. Displayed only when there are no tags (the <c>Value</c> is empty). The default is <c>null</c> (none).
	/// </summary>
	[Parameter] public string AddButtonText { get; set; }

	/// <summary>
	/// Indicates whether a "naked" variant should be displayed (no border). The default is <c>false</c>. Consider enabling <see cref="ShowAddButton"/> when using <c>Naked</c>.
	/// </summary>
	[Parameter] public bool Naked { get; set; } = false;

	/// <summary>
	/// A short hint displayed in the input field before the user enters a value.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Defines whether the input for new tag may be checked for spelling errors.
	/// </summary>
	[Parameter] public bool? Spellcheck { get; set; }
	protected bool? SpellcheckEffective => Spellcheck ?? GetSettings()?.Spellcheck ?? GetDefaults()?.Spellcheck;

	/// <summary>
	/// The theme color of the tag chips (applied as a Bootstrap <c>theme-*</c> class).
	/// The default is <see cref="ThemeColor.None"/> (the native grayscale chip appearance).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
#pragma warning disable CS0618 // TagBadgeSettings is obsolete, we still honor its Color and CssClass for backward compatibility
	protected ThemeColor ColorEffective => Color
		?? TagBadgeSettings?.Color
		?? GetSettings()?.Color
		?? GetSettings()?.TagBadgeSettings?.Color
		?? GetDefaults().Color
		?? GetDefaults().TagBadgeSettings?.Color
		?? ThemeColor.None;

	/// <summary>
	/// The CSS class(es) rendered with the individual tag chips (theme color + legacy <see cref="TagBadgeSettings"/> CssClass).
	/// </summary>
	protected string TagCssClassEffective => CssClassHelper.Combine(
		ColorEffective.ToThemeCss(),
		TagBadgeSettings?.CssClass ?? GetSettings()?.TagBadgeSettings?.CssClass ?? GetDefaults().TagBadgeSettings?.CssClass);
#pragma warning restore CS0618

	/// <summary>
	/// The settings for the <see cref="HxBadge"/> previously used to render tags.
	/// </summary>
	/// <remarks>
	/// Tags are no longer rendered as badges since Bootstrap 6. Only the <see cref="BadgeSettings.Color"/> (mapped to a <c>theme-*</c> class)
	/// and <see cref="BadgeSettings.CssClass"/> values are honored.
	/// </remarks>
	[Obsolete("Tags are no longer rendered as badges since Bootstrap 6 (the native Chip component is used). Use the Color parameter (mapped to a theme-* class) instead. Only the Color and CssClass values of these settings are honored.")]
	[Parameter] public BadgeSettings TagBadgeSettings { get; set; }

	/// <inheritdoc cref="Bootstrap.LabelType" />
	/// <remarks>
	/// <see cref="Bootstrap.LabelType.Floating"/> is not supported since Bootstrap 6. The component renders with the
	/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/chips/">chip-input</see> pattern,
	/// where the wrapper owns the visual chrome, while Bootstrap 6 floating labels require the input itself to be the <c>.form-control</c>.
	/// </remarks>
	[Parameter] public LabelType? LabelType { get; set; }
	protected LabelType LabelTypeEffective => LabelType ?? GetSettings()?.LabelType ?? GetDefaults()?.LabelType ?? HxSetup.Defaults.LabelType;
	LabelType IInputWithLabelType.LabelTypeEffective => LabelTypeEffective;

	/// <summary>
	/// The size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize; InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;


	private protected override string CoreInputCssClass => "form-ghost";
	private protected override string CoreCssClass => "hx-input-tags position-relative";

	/// <summary>
	/// The custom CSS class to render with the input-group span.
	/// </summary>
	[Parameter] public string InputGroupCssClass { get; set; }

	/// <summary>
	/// The input-group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// The input-group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// The input-group at the end of the input.
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// The input-group at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	private HxInputTagsInternal _hxInputTagsInternalComponent;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		if (LabelTypeEffective == Bootstrap.LabelType.Floating)
		{
			throw new InvalidOperationException("LabelType.Floating is not supported on HxInputTags in Bootstrap 6 — the chip-input wrapper owns the visual chrome and cannot host a floating label. Use LabelType.Regular.");
		}

		base.OnParametersSet();
	}

	/// <inheritdoc />
	protected override string GetInputCssClassToRender()
	{
		// The input size CSS class (form-control-sm/form-control-lg) is not rendered, the Bootstrap 6 chip-input wrapper has no size variants.
		string validationCssClass = IsValueInvalid() ? InvalidCssClass : null;
		return CssClassHelper.Combine(CoreInputCssClass, InputCssClass, validationCssClass);
	}

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenComponent<HxInputTagsInternal>(1);
		builder.AddAttribute(1000, nameof(HxInputTagsInternal.Value), Value);
		builder.AddAttribute(1001, nameof(HxInputTagsInternal.ValueChanged), EventCallback.Factory.Create<List<string>>(this, HandleValueChanged));
		builder.AddAttribute(1002, nameof(HxInputTagsInternal.DataProvider), DataProvider);
		builder.AddAttribute(1005, nameof(HxInputTagsInternal.SuggestMinimumLengthEffective), SuggestMinimumLengthEffective);
		builder.AddAttribute(1006, nameof(HxInputTagsInternal.SuggestDelayEffective), SuggestDelayEffective);
		builder.AddAttribute(1007, nameof(HxInputTagsInternal.InputId), InputId);
		builder.AddAttribute(1008, nameof(HxInputTagsInternal.InputCssClass), GetInputCssClassToRender()); // form-ghost (+ is-invalid) goes to the inner input, the chip-input wrapper reflects the states via :has()
		builder.AddAttribute(1009, nameof(HxInputTagsInternal.EnabledEffective), EnabledEffective);
		builder.AddAttribute(1011, nameof(HxInputTagsInternal.Placeholder), Placeholder);
		builder.AddAttribute(1013, nameof(HxInputTagsInternal.AllowCustomTags), AllowCustomTags);
		builder.AddAttribute(1014, nameof(HxInputTagsInternal.DelimitersEffective), DelimitersEffective);
		builder.AddAttribute(1015, nameof(HxInputTagsInternal.InputSizeEffective), ((IInputWithSize)this).InputSizeEffective);
		builder.AddAttribute(1016, nameof(HxInputTagsInternal.ShowAddButtonEffective), ShowAddButtonEffective);
		builder.AddAttribute(1017, nameof(HxInputTagsInternal.Naked), Naked);
		builder.AddAttribute(1018, nameof(HxInputTagsInternal.CssClass), CssClass);
		builder.AddAttribute(1019, nameof(HxInputTagsInternal.AddButtonText), AddButtonText);
		builder.AddAttribute(1020, nameof(HxInputTagsInternal.TagCssClass), TagCssClassEffective);
		builder.AddAttribute(1021, nameof(HxInputTagsInternal.InputGroupStartText), InputGroupStartText);
		builder.AddAttribute(1022, nameof(HxInputTagsInternal.InputGroupEndText), InputGroupEndText);
		builder.AddAttribute(1023, nameof(HxInputTagsInternal.InputGroupStartTemplate), InputGroupStartTemplate);
		builder.AddAttribute(1024, nameof(HxInputTagsInternal.InputGroupEndTemplate), InputGroupEndTemplate);
		builder.AddAttribute(1025, nameof(HxInputTagsInternal.InputGroupCssClass), InputGroupCssClass);
		builder.AddAttribute(1026, nameof(HxInputTagsInternal.SpellcheckEffective), SpellcheckEffective);

		builder.AddMultipleAttributes(1090, AdditionalAttributes);

		builder.AddComponentReferenceCapture(1100, component => _hxInputTagsInternalComponent = (HxInputTagsInternal)component);
		builder.CloseComponent();
	}

	private void HandleValueChanged(List<string> newValue)
	{
		CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out List<string> result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	public async ValueTask FocusAsync()
	{
		if (_hxInputTagsInternalComponent == null)
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The component reference is not available. You are most likely calling the method too early. The first render must complete before calling this method.");
		}
		await _hxInputTagsInternalComponent.FocusAsync();
	}

	/// <inheritdoc />
	protected override void RenderChipGenerator(RenderTreeBuilder builder)
	{
		if (Value?.Any() ?? false)
		{
			base.RenderChipGenerator(builder);
		}
	}

	/// <inheritdoc />
	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, String.Join(", ", Value));
	}

	/// <inheritdoc />
	protected override List<string> GetChipRemoveValue()
	{
		return new List<string>();
	}
}
