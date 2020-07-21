using Havit.Blazor.Components.Web.Bootstrap.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxDateRangeFilter<TValue>
	{
		[Parameter]
		public TValue ValueFrom { get; set; }

		[Parameter]
		public EventCallback<TValue> ValueFromChanged { get; set; }

		[Parameter]
		public Expression<Func<TValue>> ValueFromExpression { get; set; }

		[Parameter]
		public string ValueFromParsingErrorMessage { get; set; }

		[Parameter]
		public TValue ValueTo { get; set; }

		[Parameter]
		public EventCallback<TValue> ValueToChanged { get; set; }

		[Parameter]
		public Expression<Func<TValue>> ValueToExpression { get; set; }

		[Parameter]
		public string ValueToParsingErrorMessage { get; set; }

		[Inject]
		private IStringLocalizer<HxDateRangeFilter> StringLocalizer { get; set; }

		protected async Task HandleValueFromChanged(TValue value)
		{
			ValueFrom = value;
			await ValueFromChanged.InvokeAsync(ValueFrom);
		}

		protected async Task HandleValueToChanged(TValue value)
		{
			ValueTo = value;
			await ValueToChanged.InvokeAsync(ValueTo);
		}

		protected string GetValueFromParsingError() => (!String.IsNullOrEmpty(ValueFromParsingErrorMessage)) ? ValueFromParsingErrorMessage : StringLocalizer["FromParsingErrorMessage"];
		protected string GetValueToParsingError() => (!String.IsNullOrEmpty(ValueToParsingErrorMessage)) ? ValueToParsingErrorMessage : StringLocalizer["ToParsingErrorMessage"];
	}
}
