using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Column displaying the context menu in the <see cref="HxGrid"/>.
	/// </summary>
	public class ContextMenuGridColumn<TItem> : HxGridColumnBase<TItem>
	{
		/// <summary>
		/// Context menu template.
		/// </summary>
		[Parameter] public RenderFragment<TItem> ContextMenu { get; set; }

		/// <inheritdoc />
		protected override CellTemplate GetHeaderCellTemplate() => CellTemplate.Empty;

		/// <inheritdoc />
		protected override CellTemplate GetItemCellTemplate(TItem item) => CellTemplate.Create(ContextMenu(item));

		/// <inheritdoc />
		protected override CellTemplate GetItemPlaceholderCellTemplate(PlaceholderContext context) => CellTemplate.Empty;

		/// <inheritdoc />
		protected override CellTemplate GetFooterCellTemplate() => CellTemplate.Empty;

		/// <inheritdoc />
		protected override IEnumerable<SortingItem<TItem>> GetSorting()
		{
			return Enumerable.Empty<SortingItem<TItem>>();
		}

		/// <inheritdoc />
		protected override int GetColumnOrder() => Int32.MaxValue;
	}
}
