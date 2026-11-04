namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/navbar/#toggler">navbar-toggler</see> component.
/// The toggler icon is rendered via a CSS <c>mask-image</c> tinted with <c>currentcolor</c> (no separate light/dark icons needed in Bootstrap 6).
/// In Bootstrap 6 the toggler opens the navbar's responsive content as a <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Drawer</see>
/// (see <see cref="HxNavbarDrawer"/>) via the Bootstrap data API — the v5 Collapse plugin is no longer used for navbars.
/// </summary>
public partial class HxNavbarToggler : ComponentBase
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	/// <summary>
	/// Selector of the drawer to toggle. The default value is derived from the parent navbar (matches the default <see cref="HxNavbarDrawer"/> ID).
	/// </summary>
	[Parameter] public string DrawerTarget { get; set; }

	/// <summary>
	/// Content of the toggler button. The default renders the <c>navbar-toggler-icon</c>.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the toggler button.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the toggler button.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected string DrawerTargetEffective => DrawerTarget ?? ("#" + NavbarContainer?.GetDefaultDrawerId());

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarToggler)} requires the parent {nameof(HxNavbar)}.");
	}
}
