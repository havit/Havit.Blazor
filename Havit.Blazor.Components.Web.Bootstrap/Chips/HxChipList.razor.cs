using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxChipList
	{
		[Parameter]	public IEnumerable<ChipItem> Chips { get; set; }
	}
}