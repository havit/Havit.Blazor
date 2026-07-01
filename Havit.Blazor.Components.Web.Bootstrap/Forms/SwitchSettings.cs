namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxSwitch"/>.
/// </summary>
public record SwitchSettings : InputSettings
{
	/// <summary>
	/// To render switch as a native switch.
	/// </summary>
	public bool? Native { get; set; }

	/// <summary>
	/// Theme color of the switch (renders the <c>theme-*</c> class on the <c>.switch</c> wrapper).
	/// </summary>
	public ThemeColor? Color { get; set; }
}
