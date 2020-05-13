using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxChipList
	{
		[Parameter]
		public IEnumerable<Chip> Chips { get; set; }
	}
}