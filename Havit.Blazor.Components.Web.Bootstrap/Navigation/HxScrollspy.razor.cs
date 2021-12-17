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
	/// <a href="https://getbootstrap.com/docs/5.0/components/scrollspy/">Bootstrap Scrollspy</a> component.
	/// </summary>
	public partial class HxScrollspy : IAsyncDisposable
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
		/// Content to be spied. Elements with IDs are required (corresponding IDs to be used in <see cref="HxNavLink.Href"/>).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;
		private ElementReference scrollspyElement;
		private bool initialized;

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender && !initialized)
			{
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("initialize", scrollspyElement, TargetId);
				initialized = true;
			}
		}

		/// <summary>
		/// When using scrollspy in conjunction with adding or removing of elements from the DOM (e.g. asynchronnous data load), you’ll need to refresh the scrollspy explicitly.
		/// </summary>
		/// <returns></returns>
		public async Task RefreshAsync()
		{
			if (initialized)
			{
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("refresh", scrollspyElement);
			}
			else
			{
				// NOOP - will be initialized OnAfterRenderAsync (a therefor the refresh is not needed)
			}
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxScrollspy) + ".js");
		}

		public virtual async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", scrollspyElement);
				await jsModule.DisposeAsync();
			}
		}
	}
}
