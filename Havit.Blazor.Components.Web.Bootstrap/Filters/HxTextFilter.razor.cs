using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxTextFilter : HxFilterBase
	{
		[Parameter]
		public string Value { get; set; }

		[Parameter]
		public EventCallback<string> ValueChanged { get; set; }

		[Parameter]
		public Expression<Func<string>> ValueSelector { get; set; }

		protected async Task HandleValueChanged(string value)
		{
			Value = value;
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
