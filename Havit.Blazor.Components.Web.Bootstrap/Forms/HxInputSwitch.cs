namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Switch input.
	/// </summary>
	public class HxInputSwitch : HxInputCheckbox
	{
		/// <inheritdoc cref="HxInputCheckbox.CoreCssClass" />
		private protected override string CoreCssClass => "form-check form-switch position-relative";
	}
}
