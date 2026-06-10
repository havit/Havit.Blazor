using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Presents a list of chips (rendered as Bootstrap <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/chips/">Chip</see> elements).<br/>
/// Usually used to present filter criteria gathered by <see cref="HxFilterForm{TModel}"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxChipList">https://havit.blazor.eu/components/HxChipList</see>
/// </summary>
public partial class HxChipList
{
	[Inject] protected IStringLocalizer<HxChipList> HxChipListLocalizer { get; set; }
	/// <summary>
	/// Application-wide defaults for the <see cref="HxChipList"/> component.
	/// </summary>
	public static ChipListSettings Defaults { get; set; }

	static HxChipList()
	{
		Defaults = new ChipListSettings()
		{
			ShowResetButton = false,
			Color = ThemeColor.None,
		};
	}

	/// <summary>
	/// Returns the application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ChipListSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ChipListSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ChipListSettings GetSettings() => Settings;

	/// <summary>
	/// Chips to be presented.
	/// </summary>
	[Parameter] public IEnumerable<ChipItem> Chips { get; set; }

	/// <summary>
	/// The theme color of the chips (applied as a Bootstrap <c>theme-*</c> class).
	/// Bootstrap 6 chips use the subtle theme tokens by default.
	/// The default is <see cref="ThemeColor.None"/> (the native grayscale chip appearance).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
#pragma warning disable CS0618 // ChipBadgeSettings is obsolete, we still honor its Color and CssClass for backward compatibility
	protected ThemeColor ColorEffective => Color
		?? ChipBadgeSettings?.Color
		?? GetSettings()?.Color
		?? GetSettings()?.ChipBadgeSettings?.Color
		?? GetDefaults().Color
		?? GetDefaults().ChipBadgeSettings?.Color
		?? ThemeColor.None;

	/// <summary>
	/// Additional CSS class(es) rendered with the individual chips (legacy <see cref="ChipBadgeSettings"/> CssClass).
	/// </summary>
	protected string ChipCssClassEffective => ChipBadgeSettings?.CssClass ?? GetSettings()?.ChipBadgeSettings?.CssClass ?? GetDefaults().ChipBadgeSettings?.CssClass;
#pragma warning restore CS0618

	/// <summary>
	/// Settings for the <see cref="HxBadge"/> previously used to render chips.
	/// </summary>
	/// <remarks>
	/// Chips are no longer rendered as badges since Bootstrap 6. Only the <see cref="BadgeSettings.Color"/> (mapped to a <c>theme-*</c> class)
	/// and <see cref="BadgeSettings.CssClass"/> values are honored.
	/// </remarks>
	[Obsolete("Chips are no longer rendered as badges since Bootstrap 6 (the native Chip component is used). Use the Color parameter (mapped to a theme-* class) instead. Only the Color and CssClass values of these settings are honored.")]
	[Parameter] public BadgeSettings ChipBadgeSettings { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Called when the chip remove button is clicked.
	/// </summary>
	[Parameter] public EventCallback<ChipItem> OnChipRemoveClick { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnChipRemoveClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnChipRemoveClickAsync(ChipItem chipRemoved) => OnChipRemoveClick.InvokeAsync(chipRemoved);

	/// <summary>
	/// Called when the reset button is clicked (when using the ready-made reset button, not the <see cref="ResetButtonTemplate"/> where you are expected to wire the event on your own).
	/// </summary>
	[Parameter] public EventCallback<ChipItem> OnResetClick { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnResetClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnResetClickAsync() => OnResetClick.InvokeAsync();

	/// <summary>
	/// Enables or disables the reset button.
	/// The default is <c>false</c> (can be changed with <code>HxChipList.Defaults.ShowResetButton</code>).
	/// </summary>
	[Parameter] public bool? ShowResetButton { get; set; }
	protected bool ShowResetButtonEffective => ShowResetButton ?? GetSettings()?.ShowResetButton ?? GetDefaults().ShowResetButton ?? throw new InvalidOperationException(nameof(ShowResetButton) + " default for " + nameof(HxChipList) + " has to be set.");

	/// <summary>
	/// Text of the reset button.
	/// </summary>
	[Parameter] public string ResetButtonText { get; set; }

	/// <summary>
	/// Template for the reset button.
	/// If used, the <see cref="ResetButtonText"/> is ignored and the <see cref="OnResetClick"/> callback is not triggered (you are expected to wire the reset logic on your own).
	/// </summary>
	[Parameter] public RenderFragment ResetButtonTemplate { get; set; }
}
