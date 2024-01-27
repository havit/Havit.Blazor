namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Ready-made context menu (based on <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">Bootstrap Dropdown</see>) with built-in support for confirmation messages after clicking on the menu items.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxContextMenu">https://havit.blazor.eu/components/HxContextMenu</see>
/// </summary>
public partial class HxContextMenu
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxContextMenu"/> and derived components.
	/// </summary>
	public static ContextMenuSettings Defaults { get; set; }

	static HxContextMenu()
	{
		Defaults = new ContextMenuSettings()
		{
			Icon = BootstrapIcon.ThreeDotsVertical,
			DropdownMenuAlignment = Bootstrap.DropdownMenuAlignment.End
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ContextMenuSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ContextMenuSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ContextMenuSettings GetSettings() => Settings;

	/// <summary>
	/// Additional CSS class(es) for the context menu.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS class(es) for the context menu dropdown (container).
	/// </summary>
	[Parameter] public string DropdownCssClass { get; set; }
	protected string DropdownCssClassEffective => DropdownCssClass ?? GetSettings()?.DropdownCssClass ?? GetDefaults().DropdownCssClass;

	/// <summary>
	/// Additional CSS class(es) for the context menu dropdown menu.
	/// </summary>
	[Parameter] public string DropdownMenuCssClass { get; set; }
	protected string DropdownMenuCssClassEffective => DropdownMenuCssClass ?? GetSettings()?.DropdownMenuCssClass ?? GetDefaults().DropdownMenuCssClass;

	/// <summary>
	/// Icon carrying the menu (use <see cref="BootstrapIcon" /> or any other <see cref="IconBase"/>).<br />
	/// The default is <see cref="BootstrapIcon.ThreeDotsVertical"/>.
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }
	protected IconBase IconEffective => Icon ?? GetSettings()?.Icon ?? GetDefaults().Icon ?? throw new InvalidOperationException(nameof(Icon) + " default for " + nameof(HxContextMenu) + " has to be set.");

	/// <summary>
	/// Additional CSS class(es) for the context menu icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
	protected string IconCssClassEffective => IconCssClass ?? GetSettings()?.IconCssClass ?? GetDefaults().IconCssClass;

	/// <summary>
	/// Alignment for the context menu dropdown menu.
	/// The default is <see cref="DropdownMenuAlignment.End"/>.
	/// </summary>
	[Parameter] public DropdownMenuAlignment? DropdownMenuAlignment { get; set; }
	protected DropdownMenuAlignment DropdownMenuAlignmentEffective => DropdownMenuAlignment ?? GetSettings()?.DropdownMenuAlignment ?? GetDefaults().DropdownMenuAlignment ?? throw new InvalidOperationException(nameof(DropdownMenuAlignment) + " default for " + nameof(HxContextMenu) + " has to be set.");

	/// <summary>
	/// Items of the context menu. Use <see cref="HxContextMenuItem"/> components.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	private string _id = "hx" + Guid.NewGuid().ToString("N");
}
