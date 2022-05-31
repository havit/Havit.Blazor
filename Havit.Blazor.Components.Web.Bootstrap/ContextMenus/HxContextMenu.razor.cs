namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Ready-made context menu (based on <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap Dropdown</see>).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxContextMenu">https://havit.blazor.eu/components/HxContextMenu</see>
	/// </summary>
	public partial class HxContextMenu
	{
		/// <summary>
		/// Additional CSS class(es) for the context menu.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the context menu dropdown (container).
		/// </summary>
		[Parameter] public string DropdownCssClass { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the context menu dropdown menu.
		/// </summary>
		[Parameter] public string DropdownMenuCssClass { get; set; }

		/// <summary>
		/// Icon holding the menu (use <see cref="BootstrapIcon" /> or any other <see cref="IconBase"/>).<br />
		/// Default is <see cref="BootstrapIcon.ThreeDotsVertical"/>.
		/// </summary>
		[Parameter] public IconBase Icon { get; set; } = BootstrapIcon.ThreeDotsVertical;

		/// <summary>
		/// Additional CSS class(es) for the context menu icon.
		/// </summary>
		[Parameter] public string IconCssClass { get; set; }

		/// <summary>
		/// Items of the context menu. Use <see cref="HxContextMenuItem"/> components.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		private string id = "hx" + Guid.NewGuid().ToString("N");
	}
}
