namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Container for the responsive content of the <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/navbar/">navbar</see>.
/// Bootstrap 6 replaced the v5 <c>.navbar-collapse</c> (Collapse plugin) with a <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Drawer</see>:
/// below the navbar's expand breakpoint the content opens as a drawer (toggled by <see cref="HxNavbarToggler"/> via the Bootstrap data API),
/// at or above the breakpoint the drawer markup is flattened into inline navbar content by the Bootstrap CSS.
/// </summary>
public class HxNavbarCollapse : ComponentBase
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	/// <summary>
	/// The drawer element ID. The default value is derived from the parent navbar (matches the default <see cref="HxNavbarToggler"/> target).
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Title rendered in the drawer header (visible only below the navbar's expand breakpoint).
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Placement of the drawer when opened below the expand breakpoint. The default is <see cref="DrawerPlacement.End"/>.
	/// </summary>
	[Parameter] public DrawerPlacement Placement { get; set; } = DrawerPlacement.End;

	/// <summary>
	/// Additional CSS class(es) for the drawer element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the drawer element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected string IdEffective => Id ?? NavbarContainer?.GetDefaultCollapseId();

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarCollapse)} requires the parent {nameof(HxNavbar)}.");
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(100, "dialog");
		builder.AddAttribute(101, "class", CssClassHelper.Combine("drawer", GetPlacementCssClass(), CssClass));
		builder.AddAttribute(102, "id", IdEffective);
		builder.AddAttribute(103, "tabindex", "-1");
		if (Title is not null)
		{
			builder.AddAttribute(104, "aria-labelledby", IdEffective + "-label");
		}
		builder.AddMultipleAttributes(105, AdditionalAttributes);

		builder.OpenElement(110, "div");
		builder.AddAttribute(111, "class", "drawer-header");
		if (Title is not null)
		{
			builder.OpenElement(112, "h5");
			builder.AddAttribute(113, "class", "drawer-title");
			builder.AddAttribute(114, "id", IdEffective + "-label");
			builder.AddContent(115, Title);
			builder.CloseElement(); // h5
		}
		builder.OpenElement(116, "button");
		builder.AddAttribute(117, "type", "button");
		builder.AddAttribute(118, "class", "btn-close");
		builder.AddAttribute(119, "data-bs-dismiss", "drawer");
		builder.AddAttribute(120, "aria-label", "Close");
		builder.CloseElement(); // button
		builder.CloseElement(); // div.drawer-header

		builder.OpenElement(130, "div");
		builder.AddAttribute(131, "class", "drawer-body");
		builder.AddContent(132, ChildContent);
		builder.CloseElement(); // div.drawer-body

		builder.CloseElement(); // dialog
	}

	private string GetPlacementCssClass()
	{
		return Placement switch
		{
			DrawerPlacement.Start => "drawer-start",
			DrawerPlacement.End => "drawer-end",
			DrawerPlacement.Top => "drawer-top",
			DrawerPlacement.Bottom => "drawer-bottom",
			_ => throw new InvalidOperationException($"Unknown {nameof(DrawerPlacement)} value {Placement}.")
		};
	}
}
