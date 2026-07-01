namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Modal render mode.
/// </summary>
public enum ModalRenderMode
{
	/// <summary>
	/// Modal content is rendered only when it is open.
	/// Suitable for item-detail, item-edit, etc.
	/// </summary>
	OpenOnly = 0,

	/// <summary>
	/// Modal content is always rendered (and hidden with CSS if not open).
	/// Needed for rendering chips from filter, etc.
	/// </summary>
	Always = 1
}