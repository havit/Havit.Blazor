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
			InputSize = Bootstrap.InputSize.Regular,
			SearchIcon = BootstrapIcon.Search,
			ClearIcon = BootstrapIcon.XCircleFill,
			MinimumLength = 2,
			Delay = 300,
		};
	}
}

