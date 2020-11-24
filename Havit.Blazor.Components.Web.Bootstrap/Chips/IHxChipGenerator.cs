using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public interface IHxChipGenerator
	{
		IEnumerable<ChipItem> GetChips();
	}
}