using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxTreeViewItem<TValue> : ComponentBase
	{
		[Parameter] public ValueWrapper<TValue> Value { get; set; }

		private void OnItemClicked()
		{
			Value.IsExpanded = !Value.IsExpanded;
			Value.IsSelected = true;
		}
	}
}