using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxTextFilter
	{
		[Parameter]
		public string Value { get; set; }

		[Parameter]
		public EventCallback<string> ValueChanged { get; set; }

		[Parameter]
		public Expression<Func<string>> ValueExpression { get; set; }

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
