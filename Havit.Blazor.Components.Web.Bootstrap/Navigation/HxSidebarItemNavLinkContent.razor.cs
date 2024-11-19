namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Innter content of the <see cref="HxSidebarItem"/>.
/// </summary>
public partial class HxSidebarItemNavLinkContent
{
	[Parameter] public string Text { get; set; }
	[Parameter] public bool Expandable { get; set; }
	[Parameter] public bool? Expanded { get; set; }
	[Parameter] public IconBase Icon { get; set; }
	[Parameter] public string InnerCssClass { get; set; }
	[Parameter] public RenderFragment<SidebarItemContentTemplateContext> ContentTemplate { get; set; }
}