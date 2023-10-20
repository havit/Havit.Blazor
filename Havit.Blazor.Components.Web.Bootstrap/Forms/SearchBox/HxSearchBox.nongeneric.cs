namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxSearchBox{TItem}" />.
/// </summary>
public static class HxSearchBox
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxSearchBox{TItem}"/> and derived components.
	/// </summary>
	public static SearchBoxSettings Defaults { get; set; }

	static HxSearchBox()
	{
		Defaults = new SearchBoxSettings()
		{
			InputSize = InputSize.Regular,
			SearchIcon = BootstrapIcon.Search,
			ClearIcon = BootstrapIcon.XLg,
			MinimumLength = 2,
			Delay = 300,
			ItemSelectionBehavior = SearchBoxItemSelectionBehavior.SelectAndClearTextQuery,
		};
	}
}

