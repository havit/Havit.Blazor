namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Divider for the <see cref="HxMenu"/>. Renders an <c>hr.menu-divider</c> element.
/// </summary>
public partial class HxMenuDivider
{
	/// <summary>
	/// Additional CSS class for the underlying <c>hr</c> element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
}
