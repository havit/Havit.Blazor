namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Text input (including password, search, etc.)
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
			SelectOnFocus = false
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override InputTextSettings GetDefaults() => Defaults;

	/// <summary>
	/// Input type.
	/// </summary>
	[Parameter] public InputType Type { get; set; } = InputType.Text;

	protected override void OnParametersSet()
	{
		if ((Type != InputType.Text)
			&& (Type != InputType.Email)
			&& (Type != InputType.Tel)
			&& (Type != InputType.Search)
			&& (Type != InputType.Password)
			&& (Type != InputType.Url))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unsupported {nameof(Type)} parameter value {Type}.");
		}

		base.OnParametersSet();
	}

	/// <inheritdoc />
	private protected override string GetElementName() => "input";

	/// <inheritdoc />
	private protected override string GetTypeAttributeValue() => Type.ToString().ToLowerInvariant();
}
