namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Inner content of the <see cref="HxSidebarItem"/>.
/// </summary>
public partial class HxSidebarItemNavLinkContentInternal
{
	[Parameter] public string Text { get; set; }
	[Parameter] public bool Expandable { get; set; }
	[Parameter] public bool? Expanded { get; set; }
	[Parameter] public IconBase Icon { get; set; }
	[Parameter] public string InnerCssClass { get; set; }
	[Parameter] public RenderFragment<SidebarItemContentTemplateContext> ContentTemplate { get; set; }
}