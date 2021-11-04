using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxTreeViewItem<TValue> : ComponentBase
	{
		[Parameter] public ValueWrapper<TValue> Value { get; set; }

		private string PaddingLeft { get; set; }

		protected override void OnInitialized()
		{
			if (Value == null)
			{
				return;
			}

			PaddingLeft = $"{Value.Level * 15}px";
			Value.PropertyChanged += (_, _) => StateHasChanged();
		}

		private void OnItemClicked()
		{
			Value.IsExpanded = !Value.IsExpanded;
			Value.IsSelected = true;
		}
	}
}