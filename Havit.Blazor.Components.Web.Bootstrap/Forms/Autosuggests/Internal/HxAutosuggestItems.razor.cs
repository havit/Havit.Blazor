using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestItems<TItemType>
	{
		[Parameter] public List<TItemType> Items { get; set; }

		[Parameter] public EventCallback<TItemType> OnItemClick { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		private async Task HandleItemClick(TItemType value)
		{
			await OnItemClick.InvokeAsync(value);
		}
	}
}
