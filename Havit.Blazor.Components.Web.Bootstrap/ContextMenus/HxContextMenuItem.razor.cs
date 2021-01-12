using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxContextMenuItem : ComponentBase
	{
		/// <summary>
		/// Item text.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Custom item content to be rendered.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Item icon (use <see cref="BootstrapIcon" />).
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Displays <see cref="HxMessageBox" /> to get a confirmation.
		/// </summary>
		[Parameter] public string ConfirmationQuestion { get; set; }

		/// <summary>
		/// Item clicked event.
		/// </summary>
		[Parameter] public EventCallback OnClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>true</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; } = true;

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		public async Task HandleClick()
		{
			if (!String.IsNullOrEmpty(ConfirmationQuestion))
			{
				if (!await JSRuntime.InvokeAsync<bool>("confirm", ConfirmationQuestion)) // TODO: HxMessageBox/HxModal spíš než JS-confirm
				{
					return; // No Action
				}
			}
			await OnClick.InvokeAsync(null);
		}
	}
}