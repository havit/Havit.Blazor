using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.0/components/scrollspy/">Bootstrap Scrollspy</see> component.
	/// </summary>
	public partial class HxScrollspy
	{
		/// <summary>
		/// ID of the <see cref="HxNav"/> or list-group with scrollspy navigation.
		/// </summary>
		// TODO [EditorRequired]
		[Parameter] public string TargetId { get; set; }

		/// <summary>
		/// Scrollspy additional CSS class. Added to main div (.hx-scrollspy).
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Content to be spyied. Elements with IDs are required (corresponding IDs to be used in <see cref="HxNavItem.Href"/>).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;
		private ElementReference scrollspyElement;

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxscrollspy.js");
				await jsModule.InvokeVoidAsync("activate", scrollspyElement, TargetId);
			}
		}
	}
}
