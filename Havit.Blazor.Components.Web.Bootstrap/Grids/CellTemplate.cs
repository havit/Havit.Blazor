namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Cell template.
	/// </summary>
	public class CellTemplate
	{
		public static CellTemplate Empty = new CellTemplate();

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
		public static CellTemplate Create(RenderFragment template, string cssClass = null)
		{
			if ((template == RenderFragmentBuilder.Empty()) && String.IsNullOrEmpty(cssClass))
			{
				return Empty;
			}

			return new CellTemplate
			{
				Template = template,
				CssClass = cssClass
			};
		}
	}
}
