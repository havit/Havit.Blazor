namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxSwitch"/>.
/// </summary>
public record SwitchSettings : InputSettings
{
	/// <summary>
	/// [Obsolete] No effect. Since Bootstrap 6, every switch renders the <c>switch</c> attribute (native haptic feedback in mobile Safari, iOS 17.4+).
	/// </summary>
	[Obsolete("No effect. Since Bootstrap 6, every switch renders the switch attribute (native haptic feedback).")]
	public bool? Native { get; set; }

	/// <summary>
	/// Theme color of the switch (renders the <c>theme-*</c> class on the <c>.switch</c> wrapper).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Size of the switch (<c>switch-sm</c>/<c>switch-lg</c>).
	/// </summary>
	public InputSize? InputSize { get; set; }
}
