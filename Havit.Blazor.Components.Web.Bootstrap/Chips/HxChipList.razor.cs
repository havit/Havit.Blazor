using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Chips
{
	public partial class HxChipList
	{
		[Parameter]
		public IEnumerable<ChipItem> Chips { get; set; }
	}
}