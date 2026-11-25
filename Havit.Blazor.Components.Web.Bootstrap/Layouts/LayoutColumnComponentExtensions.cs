namespace Havit.Blazor.Components.Web.Bootstrap;

public static class LayoutColumnComponentExtensions
{
	/// <summary>
	/// Returns CSS classes representing the column layout of the component.
	/// </summary>
	public static string GetColumnsCssClasses(this ILayoutColumnComponent columnComponent)
	{
		return CssClassHelper.Combine(
			GetColumnCssClass(columnComponent.Columns, String.Empty),
			GetColumnCssClass(columnComponent.ColumnsSmallUp, "sm"),
			GetColumnCssClass(columnComponent.ColumnsMediumUp, "md"),
			GetColumnCssClass(columnComponent.ColumnsLargeUp, "lg"),
			GetColumnCssClass(columnComponent.ColumnsExtraLargeUp, "xl"),
			GetColumnCssClass(columnComponent.ColumnsXxlUp, "2xl"));
	}

	private static string GetColumnCssClass(string value, string breakpoint)
	{
		// Bootstrap 6 uses prefix syntax for responsive grid classes (col-md-6 -> md:col-6).
		string prefix = String.IsNullOrWhiteSpace(breakpoint) ? String.Empty : breakpoint + ":";
		return value switch
		{
			null => null,
			"" => null,
			"1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9" or "10" or "11" or "12" => prefix + "col-" + value,
			"auto" => prefix + "col-auto",
			"true" => prefix + "col",
			_ => throw new ArgumentException($"Unknown column value {value}")
		};
	}
}
