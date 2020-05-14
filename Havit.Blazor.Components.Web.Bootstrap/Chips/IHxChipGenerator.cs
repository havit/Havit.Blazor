using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Chips
{
	public interface IHxChipGenerator
	{
		IEnumerable<ChipItem> GetChips();
	}
}