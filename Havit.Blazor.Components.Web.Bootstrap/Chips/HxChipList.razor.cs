using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Presents a list of chips as badges.<br/>
	/// Usualy being used to present filter-criteria gathered by <see cref="HxFilterForm{TModel}"/>.
	/// </summary>
	public partial class HxChipList
	{
		/// <summary>
		/// Chips to be presented.
		/// </summary>
		[Parameter] public IEnumerable<ChipItem> Chips { get; set; }

		/// <summary>
		/// Called when chip remove button is clicked.
		/// </summary>
		[Parameter] public EventCallback<ChipItem> OnChipRemoveClick { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnChipRemoveClick"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnChipRemoveClickAsync(ChipItem chipRemoved) => OnChipRemoveClick.InvokeAsync(chipRemoved);

		private async Task HandleRemoveClick(ChipItem chipItemToRemove)
		{
			await InvokeOnChipRemoveClickAsync(chipItemToRemove);
		}
	}
}