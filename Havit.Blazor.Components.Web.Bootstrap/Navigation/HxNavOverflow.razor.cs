using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/nav-overflow/">Bootstrap Nav overflow</see> component (new in Bootstrap 6, "Priority+" pattern).<br />
/// Automatically moves nav items which do not fit the available width into a "More" overflow menu.
/// The overflow is responsive to the container width (uses a <c>ResizeObserver</c>), not the viewport width.<br />
/// Unlike the Bootstrap <c>NavOverflow</c> plugin (which clones nav items into the menu and is therefore incompatible with Blazor rendering),
/// the component renders the nav content twice (once in the nav, once in the overflow menu) and only toggles the visibility of the individual items.
/// All items remain fully Blazor-owned (re-rendering, event handlers, and active state work in both places).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxNavOverflow">https://havit.blazor.eu/components/HxNavOverflow</see>
/// </summary>
public partial class HxNavOverflow : IAsyncDisposable
{
	/// <summary>
	/// CSS class which prevents an item from being moved to the overflow menu (Bootstrap <c>nav-overflow-keep</c>).
	/// Apply to the nav item, e.g. <c>&lt;HxNavLink CssClass="@HxNavOverflow.KeepVisibleCssClass" /&gt;</c>.
	/// </summary>
	public const string KeepVisibleCssClass = "nav-overflow-keep";

	/// <summary>
	/// Application-wide defaults for <see cref="HxNavOverflow"/> and derived components.
	/// </summary>
	public static NavOverflowSettings Defaults { get; set; }

	static HxNavOverflow()
	{
		Defaults = new NavOverflowSettings()
		{
			MoreText = "More",
			MoreIcon = BootstrapIcon.ThreeDots,
			IconPlacement = NavOverflowIconPlacement.Start,
			MenuPlacement = Bootstrap.MenuPlacement.BottomEnd,
			MinimumVisibleItems = 0,
			CollapseBelow = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual NavOverflowSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public NavOverflowSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual NavOverflowSettings GetSettings() => Settings;

	/// <summary>
	/// ID of the nav element.
	/// </summary>
	[Parameter] public string Id { get; set; } = "hx-" + Guid.NewGuid().ToString("N");

	/// <summary>
	/// The visual variant of the nav items.
	/// The default value is <see cref="NavVariant.Standard"/>.
	/// </summary>
	[Parameter] public NavVariant Variant { get; set; } = NavVariant.Standard;

	/// <summary>
	/// Text of the "More" toggle button. The default is <c>More</c>.
	/// Set to <c>null</c> or an empty string to render an icon-only toggle.
	/// </summary>
	[Parameter] public string MoreText { get; set; }
	protected string MoreTextEffective => MoreText ?? GetSettings()?.MoreText ?? GetDefaults().MoreText;

	/// <summary>
	/// Icon of the "More" toggle button. The default is <see cref="BootstrapIcon.ThreeDots"/>.
	/// </summary>
	[Parameter] public IconBase MoreIcon { get; set; }
	protected IconBase MoreIconEffective => MoreIcon ?? GetSettings()?.MoreIcon ?? GetDefaults().MoreIcon;

	/// <summary>
	/// Position of the icon relative to the text in the "More" toggle button.
	/// The default is <see cref="NavOverflowIconPlacement.Start"/>.
	/// </summary>
	[Parameter] public NavOverflowIconPlacement? IconPlacement { get; set; }
	protected NavOverflowIconPlacement IconPlacementEffective => IconPlacement ?? GetSettings()?.IconPlacement ?? GetDefaults().IconPlacement ?? NavOverflowIconPlacement.Start;

	/// <summary>
	/// Placement of the overflow menu relative to the "More" toggle button.
	/// The default is <see cref="MenuPlacement.BottomEnd"/>.
	/// </summary>
	[Parameter] public MenuPlacement? MenuPlacement { get; set; }
	protected MenuPlacement MenuPlacementEffective => MenuPlacement ?? GetSettings()?.MenuPlacement ?? GetDefaults().MenuPlacement ?? Bootstrap.MenuPlacement.BottomEnd;

	/// <summary>
	/// Minimum number of items to keep visible before the remaining items overflow into the menu
	/// (mirrors the Bootstrap <c>threshold</c> option). The default is <c>0</c>.
	/// </summary>
	[Parameter] public int? MinimumVisibleItems { get; set; }
	protected int MinimumVisibleItemsEffective => MinimumVisibleItems ?? GetSettings()?.MinimumVisibleItems ?? GetDefaults().MinimumVisibleItems ?? 0;

	/// <summary>
	/// Container width threshold below which all items collapse into the overflow menu
	/// (mirrors the Bootstrap <c>collapseBelow</c> option).
	/// A breakpoint name (e.g. <c>md</c>, resolved from the <c>--bs-breakpoint-{name}</c> CSS variable) or a pixel value (e.g. <c>768</c>).
	/// Items with the <see cref="KeepVisibleCssClass"/> CSS class remain visible.
	/// The default is <c>null</c> (disabled).
	/// </summary>
	[Parameter] public string CollapseBelow { get; set; }
	protected string CollapseBelowEffective => CollapseBelow ?? GetSettings()?.CollapseBelow ?? GetDefaults().CollapseBelow;

	/// <summary>
	/// Raised when the number of items in the overflow menu changes (including back to zero).
	/// </summary>
	[Parameter] public EventCallback<NavOverflowChangedEventArgs> OnOverflowChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnOverflowChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnOverflowChangedAsync(NavOverflowChangedEventArgs args) => OnOverflowChanged.InvokeAsync(args);

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Content of the nav (typically <see cref="HxNavLink"/> components).
	/// The content is rendered twice&#8212;once as the nav items and once as the overflow menu items
	/// (visibility of the individual items is toggled based on the available width).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>&lt;nav&gt;</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private ElementReference _navOverflowElement;
	private DotNetObjectReference<HxNavOverflow> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _disposed;

	public HxNavOverflow()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		// Re-initialization is intentional on every render — the JS side updates the options
		// and recalculates the overflow (a re-render can change the items and their visibility classes).
		await EnsureJsModuleAsync();
		if (_disposed)
		{
			return;
		}
		await _jsModule.InvokeVoidAsync("initialize", _navOverflowElement, _dotnetObjectReference, new
		{
			MinimumVisibleItems = MinimumVisibleItemsEffective,
			CollapseBelow = CollapseBelowEffective
		});
	}

	/// <summary>
	/// Recalculates which items should overflow. Called automatically on container resize and on nav content changes,
	/// use only when the automatic detection is not sufficient.
	/// </summary>
	public async Task UpdateAsync()
	{
		await EnsureJsModuleAsync();
		if (_disposed)
		{
			return;
		}
		await _jsModule.InvokeVoidAsync("update", _navOverflowElement);
	}

	/// <summary>
	/// Receives a notification from JavaScript when the overflow changes.
	/// </summary>
	[JSInvokable("HxNavOverflow_HandleOverflowChanged")]
	public Task HandleOverflowChanged(int overflowCount, int visibleCount)
	{
		return InvokeOnOverflowChangedAsync(new NavOverflowChangedEventArgs
		{
			OverflowCount = overflowCount,
			VisibleCount = visibleCount
		});
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxNavOverflow));
	}

	protected virtual string GetClasses()
	{
		return CssClassHelper.Combine(
			GetCoreCssClass(),
			"nav-overflow",
			GetVariantCssClass(),
			CssClassEffective);
	}

	protected virtual string GetCoreCssClass()
	{
		if (NavbarContainer is not null)
		{
			return "navbar-nav";
		}
		return "nav";
	}

	protected virtual string GetVariantCssClass()
	{
		return Variant switch
		{
			NavVariant.Standard => null,
			NavVariant.Pills => "nav-pills",
			NavVariant.Tabs => "nav-tabs",
			NavVariant.Underline => "nav-underline",
			_ => throw new InvalidOperationException($"Unknown {nameof(NavVariant)} value {Variant}.")
		};
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule is not null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", _navOverflowElement);
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			catch (TaskCanceledException)
			{
				// NOOP
			}
		}

		_dotnetObjectReference?.Dispose();
	}
}
