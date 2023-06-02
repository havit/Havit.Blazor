namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for the <see cref="HxGrid{TItem}"/> component.
/// </summary>
/// <remarks>
/// Marker for resources for <see cref="HxGrid{TItem}"/>.
/// It is unfriendly to create resources for generic classes.
/// </remarks>
public sealed class HxGrid
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxGrid{TItem}"/> and derived components.
	/// </summary>
	public static GridSettings Defaults { get; set; }

	static HxGrid()
	{
		Defaults = new GridSettings()
		{
			ContentNavigationMode = GridContentNavigationMode.Pagination,
			SortAscendingIcon = BootstrapIcon.SortAlphaDown,
			SortDescendingIcon = BootstrapIcon.SortAlphaDownAlt,
			ItemRowHeight = 41, // 41px = row-height of regular table-row within Bootstrap 5 default theme
			OverscanCount = 3,
			PageSize = 20,
			PlaceholdersRowCount = 5,
			ShowFooterWhenEmptyData = false,
			Responsive = false,
			Striped = false,
			LoadMoreButtonSettings = new ButtonSettings()
			{
				Color = ThemeColor.Secondary
			}
		};
	}
}
