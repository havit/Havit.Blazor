using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Collections;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Grid column base class.
	/// </summary>
	public abstract class HxGridColumnBase<TItem> : ComponentBase, IHxGridColumn<TItem>, IDisposable
	{
		/// <summary>
		/// Cascading parameter to register column to the grid.
		/// </summary>
		[CascadingParameter(Name = HxGrid<TItem>.ColumnsRegistrationCascadingValueName)] protected CollectionRegistration<IHxGridColumn<TItem>> ColumnsRegistration { get; set; }

		RenderedEventHandler IRenderNotificationComponent.Rendered { get; set; }

		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();

			Contract.Requires(ColumnsRegistration != null, $"Grid column invalid usage. Must be used in a {typeof(HxGrid<TItem>).FullName}.");
			ColumnsRegistration.Register(this);
		}

		/// <inheritdoc />
		CellTemplate IHxGridColumn<TItem>.GetHeaderCellTemplate() => this.GetHeaderCellTemplate();

		/// <inheritdoc />
		CellTemplate IHxGridColumn<TItem>.GetItemCellTemplate(TItem item) => this.GetItemCellTemplate(item);

		/// <inheritdoc />
		CellTemplate IHxGridColumn<TItem>.GetFooterCellTemplate() => this.GetFooterCellTemplate();

		/// <inheritdoc />
		SortingItem<TItem>[] IHxGridColumn<TItem>.GetSorting() => this.GetSorting().ToArray();

		/// <summary>
		/// Returns header cell template.
		/// </summary>
		protected abstract CellTemplate GetHeaderCellTemplate();

		/// <summary>
		/// Returns data cell template for the specific item.
		/// </summary>
		protected abstract CellTemplate GetItemCellTemplate(TItem item);

		/// <summary>
		/// Returns footer cell template.
		/// </summary>
		protected abstract CellTemplate GetFooterCellTemplate();

		/// <summary>
		/// Returns column sorting.
		/// </summary>
		protected abstract IEnumerable<SortingItem<TItem>> GetSorting();

		/// <inheritdoc />
		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);

			((IRenderNotificationComponent)this).Rendered?.Invoke(this, firstRender);
		}

		/// <inheritdoc />
		public virtual void Dispose()
		{
			ColumnsRegistration.Unregister(this);
		}
	}
}
