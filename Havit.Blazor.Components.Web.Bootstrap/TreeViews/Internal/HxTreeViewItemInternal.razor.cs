using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxTreeViewItemInternal<TItem> : ComponentBase
	{
		[Parameter] public TItem Item { get; set; }
		[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }

		[Parameter] public bool IsExpanded { get; set; }

		[Parameter] public Func<TItem, string> TitleSelector { get; set; }
		[Parameter] public Func<TItem, IconBase> IconSelector { get; set; }
		[Parameter] public Func<TItem, IEnumerable<TItem>> ChildrenSelector { get; set; }
		[Parameter] public int Level { get; set; }

		[CascadingParameter] protected HxTreeView<TItem> TreeViewContainer { get; set; }

		private async Task HandleItemClicked()
		{
			this.IsExpanded = !this.IsExpanded;

			await this.OnItemSelected.InvokeAsync(this.Item);
		}
	}
}