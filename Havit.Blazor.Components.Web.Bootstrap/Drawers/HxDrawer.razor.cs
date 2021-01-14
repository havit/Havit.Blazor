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
		[Parameter] public string HeaderText { get; set; }

		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		[Parameter] public RenderFragment BodyTemplate { get; set; }

		[Parameter] public RenderFragment FooterTemplate { get; set; }

		[Parameter] public bool IsOpen { get; set; }

		[Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

		/// <summary>
		/// Indicates, whether the content (HeaderTemplate, BodyTemplate, FooterTemplate, Close-button) should be rendered even if the drawer is closed.
		/// Default is <b>false</b>.
		/// </summary>
		/// <remarks>
		/// If <c>false</c> (default), new instance of content-components is being created on every open.
		/// If <c>true</c>, the content remains rendered and instantiated (OnIntialized not called on every open).
		/// </remarks>
		[Parameter] public bool PreRenderContent { get; set; } = false;

		/// <summary>
		/// Size of the drawer. Default is <see cref="DrawerSize.Regular"/>.
		/// </summary>
		[Parameter] public DrawerSize Size { get; set; } = DrawerSize.Regular;

		private async Task SetState(bool isOpen)
		{
			if (IsOpen != isOpen)
			{
				IsOpen = isOpen;
				await IsOpenChanged.InvokeAsync(isOpen);
				StateHasChanged();
			}
		}

		private string GetOpenCssClass()
		{
			return this.IsOpen ? "show" : "hide";
		}

		private string GetSizeCssClass()
		{
			return this.Size switch
			{
				DrawerSize.Regular => null,
				DrawerSize.Small => "drawer-sm",
				DrawerSize.Large => "drawer-lg",
				_ => throw new InvalidOperationException($"Unknown HxDrawer.Size value {this.Size:g}.")
			};
		}
	}
}