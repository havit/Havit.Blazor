namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxBadge"/> and derived components.
/// </summary>
public record BadgeSettings
{
	/// <summary>
	/// Badge color (background).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Visual variant of the badge (solid, subtle, outline), composed with <see cref="Color"/>.
	/// </summary>
	public BadgeVariant? Variant { get; set; }

	/// <summary>
	/// Badge type - Regular or rounded pills.
	/// </summary>
	public BadgeType? Type { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	public string CssClass { get; set; }
}
