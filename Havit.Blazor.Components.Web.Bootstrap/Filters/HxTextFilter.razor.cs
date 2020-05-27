using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxTextFilter
	{
		protected async Task TextChanged(ChangeEventArgs eventArgs)
		{			
			Value = (string)eventArgs.Value;
			await ValueChanged.InvokeAsync(Value);
		}

		//protected virtual async Task RemoveChip()
		//{
		//	Value = null; // nebo nějaký default?
		//				  // TODO: OnChange?
		//	await ValueChanged.InvokeAsync(Value);
		//}
	}
}
