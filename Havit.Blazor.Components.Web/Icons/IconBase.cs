namespace Havit.Blazor.Components.Web;

/// <summary>
/// Base class for icons.
/// </summary>
public abstract class IconBase
{
	/// <summary>
	/// Renderer of the icon. Must have an Icon property which receives the instance of the icon (IconBase descendant instance).
	/// See BootstrapIcon and <see cref="HxIcon"/> implementations for more details.
	/// </summary>
	public abstract Type RendererComponentType { get; }
}
