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

		/// <summary>
		/// Gets or sets an expression that identifies the bound value.
		/// </summary>
		/// <remarks>
		/// The ValueExpression property is set internally by the Blazor compiler if you use the @bind﻿ attribute for the Value property to implement two-way binding.
		/// </remarks>
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
		//	await ValueChanged.InvokeAsync(Value);
		//}
	}
}
