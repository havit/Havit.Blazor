namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/navbar/#toggler">navbar-toggler</see> component. The toggler icon is rendered via a CSS <c>mask-image</c> tinted with <c>currentcolor</c> (no separate light/dark icons needed in Bootstrap 6).
/// Derived from <see cref="HxCollapseToggleButton"/>.
/// </summary>
public class HxNavbarToggler : HxCollapseToggleButton
{
	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		parameters.SetParameterProperties(this);

		Contract.Requires<InvalidOperationException>(NavbarContainer is not null, $"{nameof(HxNavbarToggler)} requires the parent {nameof(HxNavbar)}.");

		CollapseTarget = parameters.GetValueOrDefault(nameof(CollapseTarget), "#" + NavbarContainer.GetDefaultCollapseId());
		Color = parameters.GetValueOrDefault(nameof(Color), ThemeColor.None);
		ChildContent = parameters.GetValueOrDefault<RenderFragment>(nameof(ChildContent), GetDefaultChildContent);

		await base.SetParametersAsync(ParameterView.Empty);
	}

	protected override string CoreCssClass => CssClassHelper.Combine(base.CoreCssClass, "navbar-toggler");

	protected virtual void GetDefaultChildContent(RenderTreeBuilder builder)
	{
		builder.AddMarkupContent(1, "<span class=\"navbar-toggler-icon\"></span>");
	}
}
