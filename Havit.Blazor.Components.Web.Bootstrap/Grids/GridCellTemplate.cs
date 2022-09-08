namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Cell template.
/// </summary>
public class GridCellTemplate
{
	public static GridCellTemplate Empty = new GridCellTemplate();

	/// <summary>
	/// Template to render cell.
	/// </summary>
	public RenderFragment Template { get; init; }

	/// <summary>
	/// Css class of the cell.
	/// </summary>
	public string CssClass { get; init; }

	/// <summary>
	/// Constructor.
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
