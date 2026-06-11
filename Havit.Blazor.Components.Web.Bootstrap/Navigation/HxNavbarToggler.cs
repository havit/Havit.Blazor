namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/navbar/#toggler">navbar-toggler</see> component.
/// The toggler icon is rendered via a CSS <c>mask-image</c> tinted with <c>currentcolor</c> (no separate light/dark icons needed in Bootstrap 6).
/// In Bootstrap 6 the toggler opens the navbar's responsive content as a <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Drawer</see>
/// (see <see cref="HxNavbarCollapse"/>) via the Bootstrap data API — the v5 Collapse plugin is no longer used for navbars.
/// </summary>
public class HxNavbarToggler : ComponentBase
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	/// <summary>
	/// Selector of the drawer to toggle. The default value is derived from the parent navbar (matches the default <see cref="HxNavbarCollapse"/> ID).
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

	protected string DrawerTargetEffective => DrawerTarget ?? ("#" + NavbarContainer?.GetDefaultCollapseId());

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarToggler)} requires the parent {nameof(HxNavbar)}.");
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(100, "button");
		builder.AddAttribute(101, "type", "button");
		builder.AddAttribute(102, "class", CssClassHelper.Combine("btn-icon navbar-toggler", CssClass));
		builder.AddAttribute(103, "data-bs-toggle", "drawer");
		builder.AddAttribute(104, "data-bs-target", DrawerTargetEffective);
		builder.AddAttribute(105, "aria-controls", DrawerTargetEffective.TrimStart('#'));
		builder.AddAttribute(106, "aria-expanded", "false");
		builder.AddAttribute(107, "aria-label", "Toggle navigation");
		builder.AddMultipleAttributes(108, AdditionalAttributes);

		if (ChildContent is not null)
		{
			builder.AddContent(110, ChildContent);
		}
		else
		{
			builder.AddMarkupContent(111, "<span class=\"navbar-toggler-icon\" aria-hidden=\"true\"></span>");
		}

		builder.CloseElement(); // button
	}
}
