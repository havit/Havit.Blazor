namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents a cell template.
/// </summary>
public class GridCellTemplate
{
	public static GridCellTemplate Empty = new GridCellTemplate();

	/// <summary>
	/// Gets or sets the template used to render the cell.
	/// </summary>
	public RenderFragment Template { get; init; }

	/// <summary>
	/// Gets or sets the CSS class of the cell.
	/// </summary>
	public string CssClass { get; init; }

	/// <summary>
	/// Creates a new instance of the GridCellTemplate class.
	/// </summary>
	public static GridCellTemplate Create(RenderFragment template, string cssClass = null)
	{
		if ((template == RenderFragmentBuilder.Empty()) && String.IsNullOrEmpty(cssClass))
		{
			return Empty;
		}

		return new GridCellTemplate
		{
			Template = template,
			CssClass = cssClass
		};
	}
}
