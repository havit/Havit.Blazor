namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text input (also password, search, etc.)
/// </summary>
public class HxInputText : HxInputTextBase
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputFile"/> and derived components.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputText">https://havit.blazor.eu/components/HxInputText</see>.
	/// </summary>
	public static InputTextSettings Defaults { get; set; }

	static HxInputText()
	{
		Defaults = new InputTextSettings()
		{
			InputSize = Bootstrap.InputSize.Regular,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override InputTextSettings GetDefaults() => Defaults;

	/// <summary>
	/// Input type.
	/// </summary>
	[Parameter] public InputType Type { get; set; } = InputType.Text;

	/// <inheritdoc />
	private protected override string GetElementName() => "input";

	/// <inheritdoc />
	private protected override string GetTypeAttributeValue() => Type.ToString().ToLower();
}
