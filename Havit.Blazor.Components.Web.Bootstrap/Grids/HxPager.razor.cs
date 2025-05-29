namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Pager.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPager">https://havit.blazor.eu/components/HxPager</see>
/// </summary>
public partial class HxPager : ComponentBase
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxPager"/>.
	/// </summary>
	public static PagerSettings Defaults { get; set; }

	static HxPager()
	{
		Defaults = new PagerSettings()
		{
			FirstPageIcon = BootstrapIcon.ChevronDoubleLeft,
			LastPageIcon = BootstrapIcon.ChevronDoubleRight,
			PreviousPageIcon = BootstrapIcon.ChevronLeft,
			NextPageIcon = BootstrapIcon.ChevronRight,
			NumericButtonsCount = 10
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual PagerSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public PagerSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual PagerSettings GetSettings() => Settings;

	/// <summary>
	/// Total number of pages of data items.
	/// Has to be equal to or greater than <c>1</c>.
	/// </summary>
	[Parameter, EditorRequired] public int TotalPages { get; set; }

	/// <summary>
	/// Current page index. Zero-based.
	/// Displayed numbers start with 1, which is page index 0.
	/// </summary>
	[Parameter, EditorRequired] public int CurrentPageIndex { get; set; }

	/// <summary>
	/// Event raised when the page index is changed.
	/// </summary>
	[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="CurrentPageIndexChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeCurrentPageIndexChangedAsync(int newPageIndex) => CurrentPageIndexChanged.InvokeAsync(newPageIndex);

	/// <summary>
	/// Icon for the "First page" button.
	/// </summary>
	[Parameter] public IconBase FirstPageIcon { get; set; }
	protected IconBase FirstPageIconEffective => FirstPageIcon ?? GetSettings()?.FirstPageIcon ?? GetDefaults().FirstPageIcon ?? throw new InvalidOperationException(nameof(FirstPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Content for the "First page" button. If not set, the <see cref="FirstPageIcon"/> is used.
	/// </summary>
	[Parameter] public RenderFragment FirstPageContentTemplate { get; set; }

	/// <summary>
	/// Icon for the "Last page" button.
	/// </summary>
	[Parameter] public IconBase LastPageIcon { get; set; }
	protected IconBase LastPageIconEffective => LastPageIcon ?? GetSettings()?.LastPageIcon ?? GetDefaults().LastPageIcon ?? throw new InvalidOperationException(nameof(LastPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Content for the "Last page" button. If not set, the <see cref="LastPageIcon"/> is used.
	/// </summary>
	[Parameter] public RenderFragment LastPageContentTemplate { get; set; }

	/// <summary>
	/// Icon for the "Previous page" button.
	/// </summary>
	[Parameter] public IconBase PreviousPageIcon { get; set; }
	protected IconBase PreviousPageIconEffective => PreviousPageIcon ?? GetSettings()?.PreviousPageIcon ?? GetDefaults().PreviousPageIcon ?? throw new InvalidOperationException(nameof(PreviousPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Content for the "Previous page" button. If not set, the <see cref="PreviousPageIcon"/> is used.
	/// </summary>
	[Parameter] public RenderFragment PreviousPageContentTemplate { get; set; }

	/// <summary>
	/// Icon for the "Next page" button.
	/// </summary>
	[Parameter] public IconBase NextPageIcon { get; set; }
	protected IconBase NextPageIconEffective => NextPageIcon ?? GetSettings()?.NextPageIcon ?? GetDefaults().NextPageIcon ?? throw new InvalidOperationException(nameof(NextPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Content for the "Next page" button. If not set, the <see cref="NextPageIcon"/> is used.
	/// </summary>
	[Parameter] public RenderFragment NextPageContentTemplate { get; set; }

	/// <summary>
	/// Count of numbers to display. The default value is 10.
	/// </summary>
	[Parameter] public int? NumericButtonsCount { get; set; }
	protected int NumericButtonsCountEffective => NumericButtonsCount ?? GetSettings()?.NumericButtonsCount ?? GetDefaults().NumericButtonsCount ?? throw new InvalidOperationException(nameof(NumericButtonsCount) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	protected override void OnParametersSet()
	{
		if (TotalPages < 1)
		{
			throw new InvalidOperationException($"[{GetType().Name}] {nameof(TotalPages)} has to be greater than or equal to 1.");
		}
	}

	/// <summary>
	/// Changes the current page index and fires the event.
	/// </summary>
	protected async Task SetCurrentPageIndexTo(int newPageIndex)
	{
		Contract.Requires<ArgumentException>(newPageIndex >= 0);
		Contract.Requires<ArgumentException>(newPageIndex < TotalPages);

		CurrentPageIndex = newPageIndex;
		await InvokeCurrentPageIndexChangedAsync(CurrentPageIndex);
	}
}
