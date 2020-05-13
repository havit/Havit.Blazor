using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxCustomFilter
	{
		[Parameter]
		public RenderFragment FilterTemplate { get; set; }

		[Parameter]
		public RenderFragment ChipTemplate { get; set; }

		[Parameter]
		public EventCallback OnRemoveChip { get; set; } // TODO: Jak vyčistit filtr? Clear?

		//public override IEnumerable<FilterChip> GetChips()
		//{
		//	yield return new FilterChip
		//	{
		//		Chip = ChipTemplate,
		//		RemoveCallback = RemoveChip
		//	};
		//}

		protected async Task RemoveChip()
		{
			await OnRemoveChip.InvokeAsync(null);
		}

		//public override RenderFragment GetFilter() => FilterTemplate;
	}
}
