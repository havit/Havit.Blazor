namespace Havit.Blazor.Components.Web.Bootstrap;

public static class SidebarResponsiveBreakpointExtensions
{
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
