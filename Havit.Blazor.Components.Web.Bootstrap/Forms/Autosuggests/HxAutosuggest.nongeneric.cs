namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxAutosuggest{TItem, TValue}" />.
/// </summary>
public class HxAutosuggest
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxAutosuggest{TItem, TValue}"/> and derived components.
	/// </summary>
	public static AutosuggestSettings Defaults { get; set; }

	static HxAutosuggest()
	{
		Defaults = new AutosuggestSettings()
		{
			SearchIcon = BootstrapIcon.Search,
			ClearIcon = BootstrapIcon.XLg,
			MinimumLength = 2,
			Delay = 300,
			Spellcheck = false
		};
	}
}
