namespace Havit.Blazor.Components.Web.Bootstrap;

public static class SidebarResponsiveBreakpointExtensions
{
	/// <summary>
	/// Returns a breakpoint marker CSS class (e.g. <c>prefix-md</c>) consumed by the component's scoped CSS media queries.
	/// The sidebar's responsive behavior is implemented in scoped CSS (not responsive utilities) so it works against
	/// the unmodified base Bootstrap 6 build, where utilities live in @layer utilities and cannot override
	/// unlayered styles such as the component isolation CSS or the (unlayered) .collapse rules.
	/// </summary>
	public static string GetMarkerCssClass(this SidebarResponsiveBreakpoint breakpoint, string prefix)
	{
		return breakpoint switch
		{
			SidebarResponsiveBreakpoint.None => prefix + "-none",
			SidebarResponsiveBreakpoint.Small => prefix + "-sm",
			SidebarResponsiveBreakpoint.Medium => prefix + "-md",
			SidebarResponsiveBreakpoint.Large => prefix + "-lg",
			SidebarResponsiveBreakpoint.ExtraLarge => prefix + "-xl",
			SidebarResponsiveBreakpoint.Xxl => prefix + "-2xl",
			_ => throw new InvalidOperationException($"Unknown {nameof(SidebarResponsiveBreakpoint)} value {breakpoint}.")
		};
	}

	public static string GetCssClass(this SidebarResponsiveBreakpoint breakpoint, string cssClassPattern)
	{
		// Bootstrap 6 uses prefix syntax for responsive utilities (d-md-none -> md:d-none).
		// The "-??-" placeholder in the pattern marks the former v5 infix position; the breakpoint is now emitted as a prefix.
		string baseClass = cssClassPattern.Replace("-??-", "-"); // !!! Simplified for the use case of this component.
		return breakpoint switch
		{
			SidebarResponsiveBreakpoint.None => baseClass,
			SidebarResponsiveBreakpoint.Small => "sm:" + baseClass,
			SidebarResponsiveBreakpoint.Medium => "md:" + baseClass,
			SidebarResponsiveBreakpoint.Large => "lg:" + baseClass,
			SidebarResponsiveBreakpoint.ExtraLarge => "xl:" + baseClass,
			SidebarResponsiveBreakpoint.Xxl => "2xl:" + baseClass,
			_ => throw new InvalidOperationException($"Unknown {nameof(SidebarResponsiveBreakpoint)} value {breakpoint}.")
		};
	}
}
