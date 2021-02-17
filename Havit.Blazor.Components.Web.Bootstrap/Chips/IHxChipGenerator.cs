using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public interface IHxChipGenerator
	{
		Task<ChipItem[]> GetChipsAsync();
	}
}