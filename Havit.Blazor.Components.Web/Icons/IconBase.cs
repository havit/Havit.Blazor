namespace Havit.Blazor.Components.Web;

/// <summary>
/// Base class for icons.
/// </summary>
public abstract class IconBase
{
	/// <summary>
	/// Specifies the renderer component type for the icon.
	/// For more details on implementing custom icons, refer to the 
	/// <see href="https://havit.blazor.eu/components/HxIcon#custom-icons">documentation section on custom icons</see>.
	/// </summary>
	public abstract Type RendererComponentType { get; }
}
