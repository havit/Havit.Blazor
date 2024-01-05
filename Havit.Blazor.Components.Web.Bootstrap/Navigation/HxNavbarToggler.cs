namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap 5 <see href="https://getbootstrap.com/docs/5.3/components/navbar/#toggler">navbar-toggler</see> component.
/// Derived from <see cref="HxCollapseToggleButton"/>.
/// </summary>
public class HxNavbarToggler : HxCollapseToggleButton
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		await base.SetParametersAsync(parameters);

		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarToggler)} requires the parent {nameof(HxNavbar)}.");

		CollapseTarget = parameters.GetValueOrDefault(nameof(CollapseTarget), "#" + NavbarContainer.GetDefaultCollapseId());
		Color = parameters.GetValueOrDefault(nameof(Color), ThemeColor.None);
		ChildContent = parameters.GetValueOrDefault<RenderFragment>(nameof(ChildContent), GetDefaultChildContent);
	}

	protected override string CoreCssClass => CssClassHelper.Combine(base.CoreCssClass, "navbar-toggler");

	protected virtual void GetDefaultChildContent(RenderTreeBuilder builder)
	{
		builder.AddMarkupContent(1, "<span class=\"navbar-toggler-icon\"></span>");
	}
}
