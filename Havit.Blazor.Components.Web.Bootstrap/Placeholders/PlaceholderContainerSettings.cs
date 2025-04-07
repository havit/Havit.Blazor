namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxPlaceholderContainer"/> and derived components.
/// </summary>
public record PlaceholderContainerSettings
{
	/// <summary>
	/// Gets or sets the name of the element to render.
	/// </summary>
	public string ElementName { get; set; }

	/// <summary>
	/// Animation of the placeholders in the container.
	/// </summary>
	public PlaceholderAnimation? Animation { get; set; }

	/// <summary>
	/// Additional CSS class for the <see cref="HxPlaceholderContainer"/>.
	/// </summary>
	public string CssClass { get; set; }
}
