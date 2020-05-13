using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public interface IHxChipGenerator
	{
		IEnumerable<Chip> GetChips();
	}
}