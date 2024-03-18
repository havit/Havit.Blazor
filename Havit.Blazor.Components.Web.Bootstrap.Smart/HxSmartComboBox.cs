using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Smart;

/// <summary>
/// Smart ComboBox is an AI upgrade to the traditional combo box.<br />
/// Traditional combo boxes suggest values only based on exact substring matches.
/// Smart ComboBox upgrades this by suggesting semantic matches
/// (i.e., options with the most closely related meanings).
/// This is much more helpful for users who don't know/remember
/// the exact predefined string they are looking for.<br />
/// <code>HxSmartComboBox</code> derives from <see href="https://github.com/dotnet-smartcomponents/smartcomponents/blob/main/docs/smart-combobox.md">SmartComboBox</see>,
/// a component created by the Microsoft Blazor team.
/// It extends the original component with Bootstrap styling and Hx-component features.
/// </summary>
public class HxSmartComboBox : HxInputBaseWithInputGroups<string>, IInputWithPlaceholder, IInputWithSize, IInputWithLabelType
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputFile"/> and derived components.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputText">https://havit.blazor.eu/components/HxInputText</see>.
	/// </summary>
	public static InputTextSettings Defaults { get; set; }

	static HxSmartComboBox()
	{
		Defaults = new InputTextSettings()
		{
			InputSize = Bootstrap.InputSize.Regular,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override InputTextSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputText.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputTextSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputTextSettings GetSettings() => Settings;

	/// <summary>
	/// API endpoint that Smart ComboBox will call to get suggestions.
	/// </summary>
	[Parameter, EditorRequired] public string Url { get; set; }

	/// <summary>
	/// Maximum number of suggestions offered. Default is <code>10</code>.
	/// </summary>
	[Parameter] public int MaxSuggestions { get; set; } = 10;

	/// <summary>
	/// Minimal similarity coefficient for an item to be offered. Default is <code>0.5</code>.
	/// </summary>
	[Parameter] public float SimilarityThreshold { get; set; } = 0.5f;

	/// <summary>
	/// The maximum number of characters (UTF-16 code units) that the user can enter.<br />
	/// If the parameter value isn't specified, the <see cref="System.ComponentModel.DataAnnotations.MaxLengthAttribute"/> of the <c>Value</c> is checked and used.<br />
	/// If not specified either, the user can enter an unlimited number of characters.
	/// </summary>
	[Parameter] public int? MaxLength { get; set; }

	/// <summary>
	/// Hint to browsers as to the type of virtual keyboard configuration to use when editing.<br/>
	/// The default is <c>null</c> (not set).
	/// </summary>
	[Parameter] public InputMode? InputMode { get; set; }
	protected InputMode? InputModeEffective => InputMode ?? GetSettings()?.InputMode ?? GetDefaults()?.InputMode;

	/// <summary>
	/// Placeholder for the input.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxInputNumber) + " has to be set.");
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <inheritdoc cref="Bootstrap.LabelType" />
	[Parameter] public LabelType? LabelType { get; set; }

	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		RenderWithAutoCreatedEditContextAsCascadingValue(builder, 0, BuildRenderInputCore);
	}

	protected virtual void BuildRenderInputCore(RenderTreeBuilder builder)
	{
		LabelType labelTypeEffective = (this as IInputWithLabelType).LabelTypeEffective;

		builder.OpenComponent<SmartComboBox>(1);

		builder.AddAttribute(100, nameof(Value), Value);
		builder.AddAttribute(101, nameof(ValueChanged), EventCallback.Factory.Create<string>(this, value => CurrentValue = value));
		builder.AddAttribute(102, nameof(ValueExpression), ValueExpression);
		builder.AddAttribute(103, nameof(Url), Url);
		builder.AddAttribute(104, nameof(MaxSuggestions), MaxSuggestions);
		builder.AddAttribute(105, nameof(SimilarityThreshold), SimilarityThreshold);

		builder.AddAttribute(200, "id", InputId);
		builder.AddAttribute(201, "class", CssClassHelper.Combine("smartcombobox", GetInputCssClassToRender()));
		builder.AddAttribute(202, "disabled", EnabledEffective ? (bool?)null : true);
		builder.AddAttribute(204, "placeholder", (labelTypeEffective == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating) ? "placeholder" : Placeholder);

		int? maxLengthEffective = MaxLength ?? GetValueAttribute<MaxLengthAttribute>()?.Length;
		if (maxLengthEffective > 0) // [MaxLength] attribute has a default value of -1
		{
			builder.AddAttribute(300, "maxlength", maxLengthEffective);
		}

		builder.AddMultipleAttributes(300, AdditionalAttributes);

		builder.CloseComponent();
	}

	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out string result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}
}
