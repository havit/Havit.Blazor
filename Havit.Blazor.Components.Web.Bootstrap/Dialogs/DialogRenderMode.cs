namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Dialog render mode.
/// </summary>
public enum DialogRenderMode
{
	/// <summary>
	/// Dialog content is rendered only when it is open.
	/// Suitable for item-detail, item-edit, etc.
	/// </summary>
	OpenOnly = 0,

	/// <summary>
	/// Dialog content is always rendered (and hidden with CSS if not open).
	/// Needed for rendering chips from filter, etc.
	/// </summary>
	Always = 1
}