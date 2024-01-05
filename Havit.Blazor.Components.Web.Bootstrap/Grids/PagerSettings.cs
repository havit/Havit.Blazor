namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxPager"/>.
/// </summary>
public record PagerSettings
{
	/// <summary>
	/// Icon for the "First page" button.
	/// </summary>
	public IconBase FirstPageIcon { get; set; }

	/// <summary>
	/// Icon for the "Last page" button.
	/// </summary>
	public IconBase LastPageIcon { get; set; }

	/// <summary>
	/// Icon for the "Previous page" button.
	/// </summary>
	public IconBase PreviousPageIcon { get; set; }

	/// <summary>
	/// Icon for the "Next page" button.
	/// </summary>
	public IconBase NextPageIcon { get; set; }

	/// <summary>
	/// Number of buttons with numbers to display.
	/// </summary>
	public int? NumericButtonsCount { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	public string CssClass { get; set; }
}
