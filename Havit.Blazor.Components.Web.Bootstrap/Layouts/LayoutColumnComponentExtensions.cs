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
			GetColumnCssClass(columnComponent.ColumnsXxlUp, "xxl"));
	}

	private static string GetColumnCssClass(string value, string infix)
	{
		if (!String.IsNullOrWhiteSpace(infix))
		{
			infix = "-" + infix;
		}
		return value switch
		{
			null => null,
			"" => null,
			"1" or "2" or "3" or "4" or "5" or "6" or "7" or "8" or "9" or "10" or "11" or "12" => "col" + infix + "-" + value,
			"auto" => "col" + infix + "-auto",
			"true" => "col" + infix,
			_ => throw new ArgumentException($"Unknown column value {value}")
		};
	}
}
