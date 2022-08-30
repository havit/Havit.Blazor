namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Switch input.<br/>
/// (Replaces the former <see cref="HxInputSwitch"/> component which is now obsolete.)<br/>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSwitch">https://havit.blazor.eu/components/HxSwitch</see>
/// </summary>
public class HxSwitch : HxCheckbox
{
	/// <inheritdoc cref="HxCheckbox.CoreFormElementCssClass" />
	private protected override string CoreFormElementCssClass => "form-check form-switch";
}