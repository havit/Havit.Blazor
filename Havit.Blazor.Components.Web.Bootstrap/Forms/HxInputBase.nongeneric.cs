namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for the <see cref="HxInputBase{TValue}"/> component.
/// </summary>
public sealed class HxInputBase
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputBase{TValue}"/> and derived components.
	/// </summary>
	public static InputsSettings Defaults { get; set; }

	static HxInputBase()
	{
		Defaults = new InputsSettings()
		{
			ValidationMessageMode = ValidationMessageMode.Floating
		};
	}
}