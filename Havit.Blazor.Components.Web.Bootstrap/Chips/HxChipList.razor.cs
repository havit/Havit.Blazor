using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxChipList
	{
		[Parameter]	public IEnumerable<ChipItem> Chips { get; set; }

		[Parameter] public EventCallback<ChipItem> OnChipRemoveClick { get; set; }

		public async Task HandleRemoveClick(ChipItem chipItemToRemove)
		{
			await OnChipRemoveClick.InvokeAsync(chipItemToRemove);
		}
	}
}