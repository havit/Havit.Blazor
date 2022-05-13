using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item for the <see cref="HxSidebar"/>.
	/// </summary>
	public partial class HxSidebarItem
	{
		/// <summary>
		/// Item text.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Item icon (optional).
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Item navigation target.
		/// </summary>
		[Parameter] public string Href { get; set; }

		/// <summary>
		/// URL matching behavior for the underlying <see cref="NavLink"/>.
		/// Default is <see cref="NavLinkMatch.Prefix"/>.
		/// </summary>
		[Parameter] public NavLinkMatch? Match { get; set; } = NavLinkMatch.Prefix;

		/// <summary>
		/// Allows you to disable the item with <c>false</c>.
		/// Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool Enabled { get; set; } = true;

		/// <summary>
		/// Any additional CSS class to add.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Sub-items (not intended to be used for any other purpose).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[CascadingParameter] protected HxSidebar ParentSidebar { get; set; }
		[CascadingParameter] protected HxSidebarItem ParentItem { get; set; }
		[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }

		[Inject] protected NavigationManager NavigationManager { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;
		private string navLinkId = "hx" + Guid.NewGuid().ToString("N");

		private string id = "hx" + Guid.NewGuid().ToString("N");

		protected override void OnParametersSet()
		{
			Contract.Requires<InvalidOperationException>(ParentSidebar is not null, $"{nameof(HxSidebarItem)} has to be placed inside {nameof(HxSidebar)}.");
		}

		protected bool HasExpandableContent => (this.ChildContent is not null);

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				NavigationManager.LocationChanged += OnLocationChanged;
				await ExpandParentIfActive();
			}
		}

		/// <summary>
		/// A function to be called when the location changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected async void OnLocationChanged(object sender, LocationChangedEventArgs e)
		{
			await ExpandParentIfActive();
		}

		/// <summary>
		/// Checks if the nav link has the ".active" class and therefore corresponds to the current URL, then expands the parent item if those conditions are met.
		/// </summary>
		/// <returns></returns>
		protected async Task ExpandParentIfActive()
		{
			await EnsureJsModule();

			bool active = await jsModule.InvokeAsync<bool>("isElementActive", navLinkId); // Are we on the page that this item is pointing to?
			if (ParentItem is not null && !ParentItem.Text.Contains("All") && active) // If the parent item is "All components", we don't want to expand it since it would expand with almost every request.
			{
				await ParentItem.Expand();
			}
		}

		/// <summary>
		/// Ensures the JS module is loaded.
		/// </summary>
		/// <returns></returns>
		protected async Task EnsureJsModule()
		{
			jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxSidebarItem));
		}

		/// <summary>
		/// Shows the sub-items of this item by adding the <c>.show</c> class.
		/// </summary>
		public async Task Expand()
		{
			await EnsureJsModule();
			await jsModule.InvokeVoidAsync("expand", id, navLinkId);
		}
	}
}
