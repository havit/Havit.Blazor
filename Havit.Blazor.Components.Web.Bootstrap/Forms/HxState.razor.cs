using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	public partial class HxState
	{
		[CascadingParameter] protected FormState CascadingFormState { get; set; }

		[Parameter] public bool? IsEnabled { get; set; }
		[Parameter] public bool IsVisible { get; set; } = true;
		[Parameter] public RenderFragment ChildContent { get; set; }

		private FormState CreateNewCascadingFormState()
		{
			return new FormState
			{
				IsEnabled = IsEnabled ?? CascadingFormState?.IsEnabled,
			};
		}

	}

}
