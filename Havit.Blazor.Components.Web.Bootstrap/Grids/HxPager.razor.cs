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
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected virtual PagerSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public PagerSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual PagerSettings GetSettings() => this.Settings;

	/// <summary>
	/// Total pages of data items.
	/// </summary>
	[Parameter, EditorRequired] public int TotalPages { get; set; }

	/// <summary>
	/// Current page index. Zero based.
	/// Displayed numbers start with 1 which is page index 0.
	/// </summary>
	[Parameter, EditorRequired] public int CurrentPageIndex { get; set; }

	/// <summary>
	/// Event raised where page index is changed.
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
	protected IconBase FirstPageIconEffective => this.FirstPageIcon ?? this.GetSettings()?.FirstPageIcon ?? GetDefaults().FirstPageIcon ?? throw new InvalidOperationException(nameof(FirstPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Icon for the "Last page" button.
	/// </summary>
	[Parameter] public IconBase LastPageIcon { get; set; }
	protected IconBase LastPageIconEffective => this.LastPageIcon ?? this.GetSettings()?.LastPageIcon ?? GetDefaults().LastPageIcon ?? throw new InvalidOperationException(nameof(LastPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Icon for the "Previous page" button.
	/// </summary>
	[Parameter] public IconBase PreviousPageIcon { get; set; }
	protected IconBase PreviousPageIconEffective => this.PreviousPageIcon ?? this.GetSettings()?.PreviousPageIcon ?? GetDefaults().PreviousPageIcon ?? throw new InvalidOperationException(nameof(PreviousPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Icon for the "Next page" button.
	/// </summary>
	[Parameter] public IconBase NextPageIcon { get; set; }
	protected IconBase NextPageIconEffective => this.NextPageIcon ?? this.GetSettings()?.NextPageIcon ?? GetDefaults().NextPageIcon ?? throw new InvalidOperationException(nameof(NextPageIcon) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Count of numbers to display. Default value is 10.
	/// </summary>
	[Parameter] public int? NumericButtonsCount { get; set; }
	protected int NumericButtonsCountEffective => this.NumericButtonsCount ?? this.GetSettings()?.NumericButtonsCount ?? GetDefaults().NumericButtonsCount ?? throw new InvalidOperationException(nameof(NumericButtonsCount) + " default for " + nameof(HxPager) + " has to be set.");

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Changes current page index and fires event.
	/// </summary>
	protected async Task SetCurrentPageIndexTo(int newPageIndex)
	{
		Contract.Requires(newPageIndex >= 0, nameof(newPageIndex));
		Contract.Requires(newPageIndex < TotalPages, nameof(newPageIndex));

		CurrentPageIndex = newPageIndex;
		await InvokeCurrentPageIndexChangedAsync(CurrentPageIndex);
	}
}
