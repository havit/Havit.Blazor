namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Switch input.<br/>
/// (Replaces the former <c>HxInputSwitch</c> component which was dropped in v4.0.0.)<br/>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSwitch">https://havit.blazor.eu/components/HxSwitch</see>
/// </summary>
public class HxSwitch : HxCheckbox
{
	/// <inheritdoc cref="HxCheckbox.AdditionalFormElementCssClass" />
	private protected override string AdditionalFormElementCssClass => "form-switch";
}