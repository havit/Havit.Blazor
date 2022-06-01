namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Column for displaying the <see cref="HxContextMenu"/> in the <see cref="HxGrid"/>.
	/// </summary>
	public class HxContextMenuGridColumn<TItem> : HxGridColumnBase<TItem>
	{
		/// <summary>
		/// The order (display index) of the column.
		/// Columns are displayed in the order of this property.
		/// Columns with the same value are displayed in the order of appereance in the code (when the columns are not conditionaly displayed using @if).
		/// </summary>
		/// <exception cref="ArgumentException">Value is <c>Int32.MinValue</c> or <c>Int32.MaxValue</c>.</exception>
		[Parameter]
		public int Order
		{
			get => order;
			set
			{
				// This is to ensure MultiSelectGridColumn is displayed always as the first column.
				// MultiSelectGridColumn uses Int32.MinValue and we do not want to enable column to have same value.
				Contract.Requires<ArgumentException>(value != Int32.MinValue);

				order = value;
			}
		}
		private int order = 0;

		/// <summary>
		/// Context menu template.
		/// </summary>
		[Parameter] public RenderFragment<TItem> ChildContent { get; set; }

		/// <inheritdoc />
		protected override GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context) => GridCellTemplate.Empty;

		/// <inheritdoc />
		protected override GridCellTemplate GetItemCellTemplate(TItem item) => GridCellTemplate.Create(ChildContent(item));

		/// <inheritdoc />
		protected override GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context) => GridCellTemplate.Empty;

		/// <inheritdoc />
		protected override GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context) => GridCellTemplate.Empty;

		/// <inheritdoc />
		protected override IEnumerable<SortingItem<TItem>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItem>>();
		}

		/// <inheritdoc />
		protected override int GetColumnOrder() => order;
	}
}
