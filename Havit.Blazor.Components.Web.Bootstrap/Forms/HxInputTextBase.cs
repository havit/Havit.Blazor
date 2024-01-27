using System.ComponentModel.DataAnnotations;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text-based (string) input base class.
/// </summary>
public abstract class HxInputTextBase : HxInputBaseWithInputGroups<string>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
{
	/// <summary>
	/// Return <see cref="HxInputText"/> defaults.
	/// Enables not sharing defaults in descendants with base classes.
	/// Enables having multiple descendants that differ in the default values.
	/// </summary>
	protected override abstract InputTextSettings GetDefaults();

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
	/// Gets or sets the behavior when the model is updated from the input.
	/// </summary>
	[Parameter] public BindEvent BindEvent { get; set; } = BindEvent.OnChange;

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

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, GetElementName());
		BuildRenderInput_AddCommonAttributes(builder, GetTypeAttributeValue());

		int? maxLengthEffective = MaxLength ?? GetValueAttribute<MaxLengthAttribute>()?.Length;
		if (maxLengthEffective > 0) // [MaxLength] attribute has a default value of -1
		{
			builder.AddAttribute(1000, "maxlength", maxLengthEffective);
		}

		builder.AddAttribute(1002, "value", CurrentValueAsString);
		builder.AddAttribute(1003, BindEvent.ToEventName(), EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
#if NET8_0_OR_GREATER
		builder.SetUpdatesAttributeName("value");
		if (!String.IsNullOrEmpty(NameAttributeValue))
		{
			builder.AddAttribute(1004, "name", NameAttributeValue);
		}
#endif

		if (InputModeEffective is not null)
		{
			builder.AddAttribute(1005, "inputmode", InputModeEffective.Value.ToString("f").ToLower());
		}
		builder.AddEventStopPropagationAttribute(1006, "onclick", true);
		builder.AddElementReferenceCapture(1007, elementReference => InputElement = elementReference);

		builder.CloseElement();
	}

	/// <summary>
	/// Returns the element name to render.
	/// </summary>
	private protected abstract string GetElementName();

	/// <summary>
	/// Returns the type attribute value.
	/// </summary>
	private protected abstract string GetTypeAttributeValue();

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
	{
		result = value;
		validationErrorMessage = null;
		return true;
	}
}
