namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxAvatar"/> and derived components.
/// </summary>
public record AvatarSettings
{
	/// <summary>
	/// Size of the avatar.
	/// </summary>
	public AvatarSize? Size { get; set; }

	/// <summary>
	/// Theme color of the avatar (background and contrasting content color).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// When <c>true</c>, the avatar uses the subtle variant of the theme color.
	/// </summary>
	public bool? Subtle { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	public string CssClass { get; set; }
}
