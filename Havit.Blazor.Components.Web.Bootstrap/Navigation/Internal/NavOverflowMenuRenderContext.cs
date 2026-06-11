namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Cascading marker indicating that the content is being rendered inside the <see cref="HxNavOverflow"/> overflow menu
/// (the second render of the nav content). <see cref="HxNavLink"/> components render as menu items (<c>menu-item</c>)
/// instead of nav links (<c>nav-link</c>) in this context.
/// </summary>
internal sealed class NavOverflowMenuRenderContext
{
	internal static NavOverflowMenuRenderContext Default { get; } = new NavOverflowMenuRenderContext();

	private NavOverflowMenuRenderContext()
	{
		// no instances (use Default)
	}
}
