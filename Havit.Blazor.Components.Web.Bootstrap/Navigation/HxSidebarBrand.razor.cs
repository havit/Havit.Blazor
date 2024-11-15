namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Brand for the <see cref="HxSidebar.HeaderTemplate"/>.
/// </summary>
public partial class HxSidebarBrand
{
	/// <summary>
	/// The long name of the brand.
	/// </summary>
	[Parameter] public string BrandName { get; set; }

	/// <summary>
	/// The logo of the brand.
	/// </summary>
	[Parameter] public RenderFragment<SidebarBrandLogoTemplateContext> LogoTemplate { get; set; }

	/// <summary>
	/// The short name of the brand.
	/// </summary>
	[Parameter] public string BrandNameShort { get; set; }

	/// <summary>
	/// The <see cref="HxSidebar"/> containing the <see cref="HxSidebarBrand"/>.
	/// </summary>
	[CascadingParameter] protected HxSidebar ParentSidebar { get; set; }

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(ParentSidebar is not null, $"{nameof(HxSidebarBrand)} has to be placed inside {nameof(HxSidebar)}.");
	}

	private string GetResponsiveCssClass(string cssClassPattern)
	{
		return ParentSidebar.ResponsiveBreakpoint switch
		{
			SidebarResponsiveBreakpoint.None => cssClassPattern.Replace("-??-", "-"), // !!! Simplified for the use case of this component.
			SidebarResponsiveBreakpoint.Small => cssClassPattern.Replace("??", "sm"),
			SidebarResponsiveBreakpoint.Medium => cssClassPattern.Replace("??", "md"),
			SidebarResponsiveBreakpoint.Large => cssClassPattern.Replace("??", "lg"),
			SidebarResponsiveBreakpoint.ExtraLarge => cssClassPattern.Replace("??", "xl"),
			SidebarResponsiveBreakpoint.Xxl => cssClassPattern.Replace("??", "xxl"),
			_ => throw new InvalidOperationException($"Unknown nameof(ResponsiveBreakpoint) value {ParentSidebar.ResponsiveBreakpoint}")
		};
	}
}
