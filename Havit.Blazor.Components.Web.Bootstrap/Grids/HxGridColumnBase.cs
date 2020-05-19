using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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

		RenderFragment IHxGridColumn<TItemType>.GetHeaderTemplate() => this.GetHeaderTemplate();
		RenderFragment IHxGridColumn<TItemType>.GetItemTemplate(TItemType item) => this.GetItemTemplate(item);
		RenderFragment IHxGridColumn<TItemType>.GetFooterTemplate() => this.GetFooterTemplate();

		protected abstract RenderFragment GetHeaderTemplate();

		protected abstract RenderFragment GetItemTemplate(TItemType item);

		protected abstract RenderFragment GetFooterTemplate();

		public virtual void Dispose()
		{
			ColumnsRegistration.Unregister(this);
		}
	}
}
