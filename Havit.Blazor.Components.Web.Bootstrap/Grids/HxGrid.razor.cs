using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public partial class HxGrid<TItemType>
	{
		public const string ColumnsRegistrationCascadingValueName = "ColumnsRegistration";

		[Parameter]
		public IEnumerable<TItemType> Data { get; set; }

		[Parameter]
		public bool AllowSelection { get; set; } // TODO: OnClickBehavior nebo tak něco?

		[Parameter]
		public bool AllowSorting { get; set; } // TODO

		[Parameter]
		public RenderFragment Columns { get; set; }

		[Parameter]
		public RenderFragment<TItemType> ContextMenu { get; set; }

		[Parameter]
		public TItemType SelectedDataItem { get; set; } // TODO

		[Parameter]
		public EventCallback<TItemType> SelectedDataItemChanged { get; set; } 

		private List<IHxGridColumn<TItemType>> columnsList;
		protected CollectionRegistration<IHxGridColumn<TItemType>> columnsListRegistration; // protected: The field 'HxGrid<TItemType>.columnsListRegistration' is never used

		public HxGrid()
		{
			columnsList = new List<IHxGridColumn<TItemType>>();
			columnsListRegistration = new CollectionRegistration<IHxGridColumn<TItemType>>(columnsList);
		}

		/// <summary>
		/// Vrací sloupce k vyrenderování.
		/// Hlavním účelem je doplnění sloupce pro menu k uživatelsky definovaným sloupcům
		/// </summary>		
		protected List<IHxGridColumn<TItemType>> ColumnsToRender()
		{
			var result = new List<IHxGridColumn<TItemType>>(columnsList);
			if (ContextMenu != null)
			{
				result.Add(new ContextMenuGridColumn<TItemType>(ContextMenu));
			}
			return result;
		}

	}
}
