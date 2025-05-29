namespace Havit.Blazor.Components.Web.Bootstrap;

public static class SidebarResponsiveBreakpointExtensions
{
	public static string GetCssClass(this SidebarResponsiveBreakpoint breakpoint, string cssClassPattern)
	{
		return breakpoint switch
		{
			SidebarResponsiveBreakpoint.None => cssClassPattern.Replace("-??-", "-"), // !!! Simplified for the use case of this component.
			SidebarResponsiveBreakpoint.Small => cssClassPattern.Replace("??", "sm"),
			SidebarResponsiveBreakpoint.Medium => cssClassPattern.Replace("??", "md"),
			SidebarResponsiveBreakpoint.Large => cssClassPattern.Replace("??", "lg"),
			SidebarResponsiveBreakpoint.ExtraLarge => cssClassPattern.Replace("??", "xl"),
			SidebarResponsiveBreakpoint.Xxl => cssClassPattern.Replace("??", "xxl"),
			_ => throw new InvalidOperationException($"Unknown {nameof(SidebarResponsiveBreakpoint)} value {breakpoint}.")
		};
	}
}
