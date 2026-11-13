namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Animation of the placeholders (for <see cref="HxPlaceholderContainer.Animation"/>).
/// </summary>
public enum PlaceholderAnimation
{
	/// <summary>
	/// No animation.
	/// </summary>
	None = 0,

	/// <summary>
	/// Opacity pulse animation (<c>.placeholder-pulse</c>).
	/// </summary>
	Pulse = 1,

	/// <summary>
	/// Animation with a glowing effect. Bootstrap 6 keeps <c>.placeholder-glow</c> only as a compatibility alias of <c>.placeholder-pulse</c>.
	/// </summary>
	[Obsolete("Use PlaceholderAnimation.Pulse instead (Bootstrap 6 renamed .placeholder-glow to .placeholder-pulse).")]
	Glow = Pulse,

	/// <summary>
	/// Animation with a waving effect (<c>.placeholder-wave</c>). The Bootstrap 6 default loading animation.
	/// </summary>
	Wave = 2
}
