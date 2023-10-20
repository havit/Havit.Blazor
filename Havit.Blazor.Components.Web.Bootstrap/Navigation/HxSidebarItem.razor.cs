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
	/// Item navigation target.
	/// </summary>
	[Parameter] public string Href { get; set; }

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

	private string id = "hx" + Guid.NewGuid().ToString("N");

	protected List<HxSidebarItem> childItems;
	internal CollectionRegistration<HxSidebarItem> ChildItemsRegistration { get; }

	protected HxCollapse collapseComponent;
	protected bool disposed;
	protected bool isMatch;
	protected bool expanded;

	public HxSidebarItem()
	{
		childItems = new();
		ChildItemsRegistration = new(childItems, async () => await InvokeAsync(this.StateHasChanged), () => disposed);
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

	protected bool ShouldBeExpanded => ExpandOnMatch && !ParentSidebar.Collapsed && this.childItems.Any(i => i.isMatch && i.ExpandOnMatch);
	protected bool HasExpandableContent => (this.ChildContent is not null);

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender && ShouldBeExpanded && (collapseComponent is not null))
		{
			await collapseComponent.ShowAsync();
		}
	}

	private void HandleCollapseShown()
	{
		expanded = true;
	}

	private void HandleCollapseHidden()
	{
		expanded = false;
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

	#region ShouldMatch (initial expansion) logic replicated from NavLink
#pragma warning disable IDE1006 // Naming Styles
	private string _hrefAbsolute;
#pragma warning restore IDE1006 // Naming Styles
	private bool ShouldMatch(string currentUriAbsolute)
	{
		if (_hrefAbsolute == null)
		{
			return false;
		}

		if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
		{
			return true;
		}

		if (Match == NavLinkMatch.Prefix
			&& IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute))
		{
			return true;
		}

		return false;
	}

	private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
	{
		Debug.Assert(_hrefAbsolute != null);

		if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
		{
			// Special case: highlight links to http://host/path/ even if you're
			// at http://host/path (with no trailing slash)
			//
			// This is because the router accepts an absolute URI value of "same
			// as base URI but without trailing slash" as equivalent to "base URI",
			// which in turn is because it's common for servers to return the same page
			// for http://host/vdir as they do for host://host/vdir/ as it's no
			// good to display a blank page in that case.
			if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
				&& _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}

	private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
	{
		var prefixLength = prefix.Length;
		if (value.Length > prefixLength)
		{
			return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
				&& (
					// Only match when there's a separator character either at the end of the
					// prefix or right after it.
					// Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
					// Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
					prefixLength == 0
					|| !char.IsLetterOrDigit(prefix[prefixLength - 1])
					|| !char.IsLetterOrDigit(value[prefixLength])
				);
		}
		else
		{
			return false;
		}
	}
	#endregion
}
