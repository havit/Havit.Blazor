namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Presents a list of chips as badges.<br/>
	/// Usualy being used to present filter-criteria gathered by <see cref="HxFilterForm{TModel}"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxChipList">https://havit.blazor.eu/components/HxChipList</see>
	/// </summary>
	public partial class HxChipList
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxChipList"/>.
		/// </summary>
		public static ChipListSettings Defaults { get; set; }

		static HxChipList()
		{
			Defaults = new ChipListSettings()
			{
				ChipBadgeSettings = new BadgeSettings()
				{
					Color = ThemeColor.Secondary,
				}
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual ChipListSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public ChipListSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual ChipListSettings GetSettings() => this.Settings;

		/// <summary>
		/// Chips to be presented.
		/// </summary>
		[Parameter] public IEnumerable<ChipItem> Chips { get; set; }

		/// <summary>
		/// Settings for the <see cref="HxBadge"/> used to render chips.
		/// Default brings <c>Color="<see cref="ThemeColor.Secondary" />".</c>
		/// </summary>
		[Parameter] public BadgeSettings ChipBadgeSettings { get; set; }
		protected BadgeSettings ChipBadgeSettingsEffective => this.ChipBadgeSettings ?? this.GetSettings()?.ChipBadgeSettings ?? this.GetDefaults().ChipBadgeSettings ?? throw new InvalidOperationException(nameof(ChipBadgeSettings) + " default for " + nameof(HxChipList) + " has to be set.");

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }
		protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

		/// <summary>
		/// Called when chip remove button is clicked.
		/// </summary>
		[Parameter] public EventCallback<ChipItem> OnChipRemoveClick { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnChipRemoveClick"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnChipRemoveClickAsync(ChipItem chipRemoved) => OnChipRemoveClick.InvokeAsync(chipRemoved);

		private async Task HandleRemoveClick(ChipItem chipItemToRemove)
		{
			await InvokeOnChipRemoveClickAsync(chipItemToRemove);
		}
	}
}