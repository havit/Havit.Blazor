using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Nebude nakonec přesunuto do Havit.Blazor.Components.Web?
	public partial class HxDropDownList<TItemType>
	{
		[Parameter] public string NullText { get; set; }
		[Parameter] public bool Nullable { get; set; } = false;
		[Parameter] public IEnumerable<TItemType> Items { get; set; }
		[Parameter] public TItemType SelectedItem { get; set; }
		[Parameter] public EventCallback<TItemType> SelectedItemChanged { get; set; }
		[Parameter] public Func<TItemType, string> Text { get; set; } // TODO: Pojmenování?
																	  // TODO: Pojmenování?
		[Parameter] public Func<TItemType, IComparable> Sort { get; set; } // TODO: Neumíme zřetězení výrazů pro řazení, v takovém případě buď umělou vlastnost s IComparable nebo seřadit předem.
		[Parameter] public bool AutoSort { get; set; } = true;

		protected List<TItemType> itemsToRender;

		// TODO!
		protected async Task SelectedItemChangedX(ChangeEventArgs eventArgs)
		{
			int index = int.Parse((string)eventArgs.Value);
			TItemType newSelectedItem = (index == -1)
				? default(TItemType)
				: itemsToRender[int.Parse((string)eventArgs.Value)];

			SelectedItem = newSelectedItem;
			await SelectedItemChanged.InvokeAsync(newSelectedItem);
		}

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			itemsToRender = Items.ToList() ?? new List<TItemType>(); // TODO: AutoSort?
			if ((SelectedItem != null) && !itemsToRender.Contains(SelectedItem))
			{
				itemsToRender.Add(SelectedItem);
			}

			if (AutoSort && (itemsToRender.Count > 1))
			{
				if (Sort != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Sort).ToList();
				}
				else if (Text != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Text).ToList();
				}
			}
		}

	}
}
