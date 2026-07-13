namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Brand/logo slot for <see cref="HxNavbar"/>, rendered as Bootstrap's <see href="https://getbootstrap.com/docs/5.3/components/navbar/#brand">navbar-brand</see>.
/// </summary>
public partial class HxNavbarBrand
{
	/// <summary>
	/// The navigation target. Default is <c>"/"</c>.
	/// </summary>
	[Parameter] public string Href { get; set; } = "/";


	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Content of the navbar-brand.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }
}
