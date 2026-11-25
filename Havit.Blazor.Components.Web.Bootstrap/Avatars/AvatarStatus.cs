namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Status indicator for <see cref="HxAvatar"/>.
/// </summary>
public enum AvatarStatus
{
	/// <summary>
	/// No status indicator (default).
	/// </summary>
	None = 0,

	/// <summary>
	/// Online status — green circle (<c>status-online</c>).
	/// </summary>
	Online,

	/// <summary>
	/// Offline status — gray rounded square (<c>status-offline</c>).
	/// </summary>
	Offline,

	/// <summary>
	/// Busy status — red rounded square (<c>status-busy</c>).
	/// </summary>
	Busy,

	/// <summary>
	/// Away status — yellow circle (<c>status-away</c>).
	/// </summary>
	Away
}
