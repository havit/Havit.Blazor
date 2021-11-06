using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Component to display hierarchy data structure.
	/// </summary>
	/// <remarks>
	///  Roadmap: virtualization, asynchronous data loading, multi-column, templates.
	/// </remarks>
	/// <typeparam name="TValue">Type of tree data item.</typeparam>
	public partial class HxTreeView<TValue> : ComponentBase
	{
		private ValueWrapper<TValue>[] wrappers;
		private ValueWrapper<TValue> selectedWrapper;

		/// <summary>
		/// Collection of hierarchy data to display.
		/// </summary>
		[Parameter] public ICollection<TValue> Items { get; set; }

		/// <summary>
		/// Selector to display content from data item.
		/// </summary>
		[Parameter] public Func<TValue, string> ContentSelector { get; set; }

		/// <summary>
		/// Selector to display badge from data item.
		/// </summary>
		[Parameter] public Func<TValue, string> BadgeSelector { get; set; }

		/// <summary>
		/// Selector to display badge color from data item.
		/// </summary>
		[Parameter] public Func<TValue, ThemeColor> BadgeColorSelector { get; set; }

		/// <summary>
		/// Selector to display icon from data item.
		/// </summary>
		[Parameter] public Func<TValue, string> IconSelector { get; set; }

		/// <summary>
		/// Selector to display children collection for current data item. Children collection should have same type as current item.
		/// </summary>
		[Parameter] public Func<TValue, IEnumerable<TValue>> ChildrenSelector { get; set; }

		/// <summary>
		/// Additional CSS class to be applied.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Raised when an item in the tree is selected.
		/// </summary>
		[Parameter] public EventCallback<TValue> OnItemSelected { get; set; }

		protected override void OnInitialized()
		{
			RebuildWrappers();
		}

		private void RebuildWrappers()
		{
			if ((Items == null) || (ContentSelector == null))
			{
				return;
			}

			wrappers = new ValueWrapper<TValue>[Items.Count];
			wrappers = Items.Select(p => new ValueWrapper<TValue>(value: p, level: 0, ContentSelector, BadgeSelector, BadgeColorSelector, IconSelector, ChildrenSelector, InternalOnItemSelected)).ToArray();
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