namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Ready-made context menu (based on <see href="https://getbootstrap.com/docs/5.2/components/dropdowns/">Bootstrap Dropdown</see>) with built-in support for confirmation messages after clicking on the menu items.<br />
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
			Icon = BootstrapIcon.ThreeDotsVertical
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descandants (use separate set of defaults).
	/// </summary>
	protected virtual ContextMenuSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
	/// </summary>
	[Parameter] public ContextMenuSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
	/// </remarks>
	protected virtual ContextMenuSettings GetSettings() => this.Settings;

	/// <summary>
	/// Additional CSS class(es) for the context menu.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS class(es) for the context menu dropdown (container).
	/// </summary>
	[Parameter] public string DropdownCssClass { get; set; }
	protected string DropdownCssClassEffective => this.DropdownCssClass ?? this.GetSettings()?.DropdownCssClass ?? GetDefaults().DropdownCssClass;

	/// <summary>
	/// Additional CSS class(es) for the context menu dropdown menu.
	/// </summary>
	[Parameter] public string DropdownMenuCssClass { get; set; }
	protected string DropdownMenuCssClassEffective => this.DropdownMenuCssClass ?? this.GetSettings()?.DropdownMenuCssClass ?? GetDefaults().DropdownMenuCssClass;

	/// <summary>
	/// Icon carring the menu (use <see cref="BootstrapIcon" /> or any other <see cref="IconBase"/>).<br />
	/// Default is <see cref="BootstrapIcon.ThreeDotsVertical"/>.
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }
	protected IconBase IconEffective => this.Icon ?? this.GetSettings()?.Icon ?? GetDefaults().Icon ?? throw new InvalidOperationException(nameof(Icon) + " default for " + nameof(HxContextMenu) + " has to be set.");

	/// <summary>
	/// Additional CSS class(es) for the context menu icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
	protected string IconCssClassEffective => this.IconCssClass ?? this.GetSettings()?.IconCssClass ?? GetDefaults().IconCssClass;

	/// <summary>
	/// Alignment for shown menu. Similar to <see cref="HxDropdownMenu.Alignment" /> 
	/// </summary>
	[Parameter] public DropdownMenuAlignment? MenuAlignment { get; set; }
	protected string DropDownAlignmentCssClass => MenuAlignment switch
	{
		DropdownMenuAlignment.Start => "dropdown-menu-start",
		DropdownMenuAlignment.End => "dropdown-menu-end",
		null => "dropdown-menu-end", // for default behaviour not to change

		_ => throw new InvalidOperationException($"Unknown {nameof(DropdownMenuAlignment)} value {MenuAlignment}.")
	};

	/// <summary>
	/// Items of the context menu. Use <see cref="HxContextMenuItem"/> components.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	private string id = "hx" + Guid.NewGuid().ToString("N");
}
