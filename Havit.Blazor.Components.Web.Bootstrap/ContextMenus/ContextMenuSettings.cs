namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxContextMenu"/>.
/// </summary>
public record ContextMenuSettings
{
	/// <summary>
	/// Additional CSS class(es) for the context menu.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu <c>.menu</c> element.
	/// </summary>
	public string MenuCssClass { get; set; }

	/// <summary>
	/// Placement of the context menu.
	/// </summary>
	public MenuPlacement? MenuPlacement { get; set; }

	/// <summary>
	/// Icon that carries the menu (use <see cref="BootstrapIcon" /> or any other <see cref="IconBase"/>).<br />
	/// </summary>
	public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu icon.
	/// </summary>
	public string IconCssClass { get; set; }
}
