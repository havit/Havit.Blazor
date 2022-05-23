namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Switch input.
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputSwitch">https://havit.blazor.eu/components/HxInputSwitch</see>
	/// </summary>
	public class HxInputSwitch : HxInputCheckbox
	{
		/// <inheritdoc cref="HxInputCheckbox.CoreCssClass" />
		private protected override string CoreCssClass => "form-check form-switch position-relative";
	}
}
