namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxAvatarStack"/> and derived components.
/// </summary>
public record AvatarStackSettings
{
	/// <summary>
	/// Size of the avatars in the stack.
	/// </summary>
	public AvatarSize? Size { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	public string CssClass { get; set; }
}
