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
	/// Additional CSS class(es) for the context menu dropdown (container).
	/// </summary>
	public string DropdownCssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu dropdown menu.
	/// </summary>
	public string DropdownMenuCssClass { get; set; }

	/// <summary>
	/// Alignment for the context menu dropdown menu.
	/// </summary>
	public DropdownMenuAlignment? DropdownMenuAlignment { get; set; }

	/// <summary>
	/// Icon carrying the menu (use <see cref="BootstrapIcon" /> or any other <see cref="IconBase"/>).<br />
	/// </summary>
	public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu icon.
	/// </summary>
	public string IconCssClass { get; set; }
}
