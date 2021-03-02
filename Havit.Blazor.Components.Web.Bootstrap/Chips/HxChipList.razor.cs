using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxChipList
	{
		[Parameter] public IEnumerable<ChipItem> Chips { get; set; }

		[Parameter] public EventCallback<ChipItem> OnChipRemoveClick { get; set; }

		public async Task HandleRemoveClick(ChipItem chipItemToRemove)
		{
			await OnChipRemoveClick.InvokeAsync(chipItemToRemove);
		}
	}
}