using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for the <see cref="HxSidebar"/>.
/// </summary>
public partial class HxSidebarFooter
{
	/// <summary>
	/// Item text.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Item icon (optional).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Item navigation target.
	/// </summary>
	[Parameter] public string Href { get; set; }

	/// <summary>
	/// URL matching behavior for the underlying <see cref="NavLink"/>.
	/// The default value is <see cref="NavLinkMatch.Prefix"/>.
	/// </summary>
	[Parameter] public NavLinkMatch? Match { get; set; } = NavLinkMatch.Prefix;

	/// <summary>
	/// Allows you to disable the item with <c>false</c>.
	/// The default value is <c>true</c>.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Sub-items (not intended to be used for any other purpose).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }
}
