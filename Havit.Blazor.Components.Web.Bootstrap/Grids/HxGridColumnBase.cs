using Havit.Collections;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public abstract class HxGridColumnBase<TItemType> : ComponentBase, IHxGridColumn<TItemType>, IDisposable
	{
		[CascadingParameter(Name = HxGrid<TItemType>.ColumnsRegistrationCascadingValueName)]
		protected CollectionRegistration<IHxGridColumn<TItemType>> ColumnsRegistration { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();
			Contract.Requires(ColumnsRegistration != null, $"Grid column invalid usage. Must be used in a {typeof(HxGrid<TItemType>).FullName}.");
			ColumnsRegistration.Register(this);
		}

		CellTemplate IHxGridColumn<TItemType>.GetHeaderCellTemplate() => this.GetHeaderCellTemplate();
		CellTemplate IHxGridColumn<TItemType>.GetItemCellTemplate(TItemType item) => this.GetItemCellTemplate(item);
		CellTemplate IHxGridColumn<TItemType>.GetFooterCellTemplate() => this.GetFooterCellTemplate();
		IEnumerable<SortingItem<TItemType>> IHxGridColumn<TItemType>.GetSorting() => this.GetSorting();

		protected abstract IEnumerable<SortingItem<TItemType>> GetSorting();

		protected abstract CellTemplate GetHeaderCellTemplate();

		protected abstract CellTemplate GetItemCellTemplate(TItemType item);

		protected abstract CellTemplate GetFooterCellTemplate();

		public virtual void Dispose()
		{
			ColumnsRegistration.Unregister(this);
		}
	}
}
