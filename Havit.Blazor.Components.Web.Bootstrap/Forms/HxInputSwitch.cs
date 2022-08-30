namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Switch input.
	/// Obsolete, use <see cref="HxSwitch"/> instead.
	/// </summary>
	[Obsolete("Use HxSwitch instead. The former Label parameter is now Text.")]
	public class HxInputSwitch : HxInputCheckbox
	{
		/// <inheritdoc cref="HxInputCheckbox.CoreCssClass" />
		private protected override string CoreCssClass => "form-check form-switch position-relative";
	}
}
