using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestInput
	{
		[Parameter] public string Value { get; set; }

		[Parameter] public EventCallback<string> OnInputInput { get; set; }

		[Parameter] public EventCallback OnInputFocus { get; set; }

		[Parameter] public EventCallback OnInputBlur { get; set; }

		[Parameter] public string InputId { get; set; }

		[Parameter] public string CssClass { get; set; }

		[Parameter] public bool EnabledEffective { get; set; }

		private async Task HandleInput(ChangeEventArgs changeEventArgs)
		{
			await OnInputInput.InvokeAsync((string)changeEventArgs.Value);
		}

		private async Task HandleFocus()
		{
			await OnInputFocus.InvokeAsync(null);
		}

		private async Task HandleBlur()
		{
			await OnInputBlur.InvokeAsync(null);
		}
	}
}
