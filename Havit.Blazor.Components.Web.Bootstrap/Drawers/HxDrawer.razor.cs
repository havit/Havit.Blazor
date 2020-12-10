using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxDrawer
	{
		[Parameter] public string Title { get; set; }

		[Parameter] public bool IsOpen { get; set; }

		[Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

		/// <summary>
		/// Indicates, whether the content (TitleSection, BodySection, CommandsSection, Close-button) should be rendered even if the drawer is closed.
		/// Default is <b>false</b>.
		/// </summary>
		/// <remarks>
		/// If <c>false</c> (default), new instance of content-components is being created on every open.
		/// If <c>true</c>, the content remains rendered and instantiated (OnIntialized not called on every open).
		/// </remarks>
		[Parameter] public bool PreRenderContent { get; set; } = false;

		[Parameter] public RenderFragment TitleSection { get; set; }

		[Parameter] public RenderFragment BodySection { get; set; }

		[Parameter] public RenderFragment CommandsSection { get; set; }

		private async Task SetState(bool isOpen)
		{
			if (IsOpen != isOpen)
			{
				IsOpen = isOpen;
				await IsOpenChanged.InvokeAsync(isOpen);
				StateHasChanged();
			}
		}
	}
}