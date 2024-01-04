namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Renders <see cref="IFormValueComponent" />.
/// The purpose of this class is to allow the use of a custom renderer.
/// </summary>
public abstract class FormValueRenderer
{
	/// <summary>
	/// Current renderer.
	/// </summary>
	public static FormValueRenderer Current { get; set; } = new HxFormValueRenderer();

	/// <summary>
	/// Renders <see cref="IFormValueComponent" />.
	/// </summary>
	public abstract void Render(int sequence, RenderTreeBuilder builder, IFormValueComponent data);
}
