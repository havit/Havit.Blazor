using System.Diagnostics;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for the <see cref="HxSidebar"/>.
/// </summary>
public partial class HxSidebarItem : IAsyncDisposable
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
	/// Text content of tooltip displayed while hovering item of collapsed Sidebar.
	/// </summary>
	[Parameter] public string TooltipText { get; set; }

	/// <summary>
	/// Item navigation target.
	/// </summary>
	[Parameter] public string Href { get; set; }

	/// <summary>
	/// Raised after the item is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Stops onClick-event propagation. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool OnClickStopPropagation { get; set; }

	/// <summary>
	/// Prevents the default action for the onclick event. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool OnClickPreventDefault { get; set; }

	/// <summary>
	/// URL matching behavior for the underlying <see cref="NavLink"/>.
	/// Default is <see cref="NavLinkMatch.Prefix"/>.
	/// </summary>
	[Parameter] public NavLinkMatch? Match { get; set; } = NavLinkMatch.Prefix;

	/// <summary>
	/// Set to <c>false</c> if you don't want to expand the sidebar if URL matches.<br/>
	/// Default is <c>true</c>.
	/// NOTE: The expansion is only applied on initial load, the sidebar does not track the URL changes (this may change in the future).
	/// </summary>
	[Parameter] public bool ExpandOnMatch { get; set; } = true;

	/// <summary>
	/// Set to <c>false</c> if you don't want to highlight the item if one of the children items is active (URL matches).<br/>
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool HighlightOnActiveChild { get; set; } = true;

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
	/// Inner custom content for the <see cref="HxSidebarItem"/>.
	/// </summary>
	[Parameter] public RenderFragment<SidebarItemContentTemplateContext> ContentTemplate { get; set; }

	/// <summary>
	/// Sub-items (not intended to be used for any other purpose).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	[CascadingParameter] protected HxSidebar ParentSidebar { get; set; }
	[CascadingParameter] protected HxSidebarItem ParentSidebarItem { get; set; }
	[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }

	private string _id = "hx" + Guid.NewGuid().ToString("N");

	protected List<HxSidebarItem> childItems;
	internal CollectionRegistration<HxSidebarItem> ChildItemsRegistration { get; }

	protected HxCollapse collapseComponent;
	protected bool disposed;
	protected bool isMatch;
	protected bool expanded;

	public HxSidebarItem()
	{
		childItems = new();
		ChildItemsRegistration = new(childItems, async () => await InvokeAsync(StateHasChanged), () => disposed);
	}

	protected override void OnInitialized()
	{
		ParentSidebarItem?.ChildItemsRegistration.Register(this);
	}

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>(ParentSidebar is not null, $"{nameof(HxSidebarItem)} has to be placed inside {nameof(HxSidebar)}.");

		_hrefAbsolute = (Href == null) ? null : NavigationManager.ToAbsoluteUri(Href).AbsoluteUri;
		isMatch = ShouldMatch(NavigationManager.Uri);
	}

	protected bool ShouldBeExpanded => ExpandOnMatch && !ParentSidebar.Collapsed && childItems.Any(i => i.isMatch && i.ExpandOnMatch);
	protected bool HasExpandableContent => (ChildContent is not null);

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender && ShouldBeExpanded && (collapseComponent is not null))
		{
			await collapseComponent.ShowAsync();
		}
	}

	protected virtual Task HandleCollapseShown()
	{
		expanded = true;
		return Task.CompletedTask;
	}

	protected virtual Task HandleCollapseHidden()
	{
		expanded = false;
		return Task.CompletedTask;
	}

	// Bootstrap Collapse (data-bs-toggle="collapse") prevents default action (navigation) on click
	// This is a workaround to handle navigation manually
	private async Task HandleExpandableItemClick(MouseEventArgs args)
	{
		await OnClick.InvokeAsync(args);
		if (!String.IsNullOrWhiteSpace(Href) && !OnClickPreventDefault)
		{
			NavigationManager.NavigateTo(Href);
		}
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);

		disposed = true;
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (ParentSidebarItem is not null)
		{
			await ParentSidebarItem.ChildItemsRegistration.UnregisterAsync(this);
		}
	}
}
