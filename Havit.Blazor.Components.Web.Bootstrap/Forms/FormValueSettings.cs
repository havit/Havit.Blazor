namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxFormValue"/> and derived components.
/// </summary>
public record FormValueSettings
{
	/// <summary>
	/// Input size. The default is <see cref="InputSize.Regular"/>.
	/// </summary>
	public InputSize? InputSize { get; set; }
}
