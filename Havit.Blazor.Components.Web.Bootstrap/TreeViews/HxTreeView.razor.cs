using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.TreeViews
{
	/// <summary>
	/// Component to display hierarchy data structure. Roadmap: virtualization, asynchronous data loading, multi-column, templates.
	/// </summary>
	/// <typeparam name="TValue">Type of tree data item.</typeparam>
	public partial class HxTreeView<TValue> : ComponentBase
	{
		private ValueWrapper<TValue>[] wrappers;
		private ValueWrapper<TValue> selectedWrapper;

		/// <summary>
		/// Collection of hierarchy data to display
		/// </summary>
		[Parameter] public ICollection<TValue> Items { get; set; }

		/// <summary>
		/// Selector to display content from data item
		/// </summary>
		[Parameter] public Func<TValue, string> Content { get; set; }

		/// <summary>
		/// Selector to display badge from data item. Can be null
		/// </summary>
		[Parameter] public Func<TValue, string> Badge { get; set; }

		/// <summary>
		/// Selector to display badge color from data item. Can be null
		/// </summary>
		[Parameter] public Func<TValue, ThemeColor> BadgeColor { get; set; }

		/// <summary>
		/// Selector to display icon from data item. Can be null
		/// </summary>
		[Parameter] public Func<TValue, string> Icon { get; set; }

		/// <summary>
		/// Selector to display children collection for current data item. Children collection should have same type as current item.
		/// </summary>
		[Parameter] public Func<TValue, IEnumerable<TValue>> Children { get; set; }

		/// <summary>
		/// Callback which runs when any item in tree selected.
		/// </summary>
		[Parameter] public EventCallback<TValue> OnItemSelected { get; set; }

		protected override void OnInitialized()
		{
			RebuildWrappers();
		}

		private void RebuildWrappers()
		{
			if (Items == null || Content == null)
			{
				return;
			}

			wrappers = new ValueWrapper<TValue>[Items.Count];
			wrappers = Items.Select(p => new ValueWrapper<TValue>(p, 0, Content, Badge, BadgeColor, Icon, Children, InternalOnItemSelected)).ToArray();
		}

		private void InternalOnItemSelected(ValueWrapper<TValue> wrapper)
		{
			if (selectedWrapper != null)
			{
				selectedWrapper.IsSelected = false;
			}

			selectedWrapper = wrapper;
			selectedWrapper.IsSelected = true;

			if (OnItemSelected.HasDelegate)
			{
				OnItemSelected.InvokeAsync(wrapper.Value);
			}
		}
	}
}