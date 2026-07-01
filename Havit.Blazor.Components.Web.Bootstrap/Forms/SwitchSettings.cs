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
}