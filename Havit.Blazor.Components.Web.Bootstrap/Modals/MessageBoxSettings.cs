namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxMessageBox"/> and derived components.
/// </summary>
public record MessageBoxSettings
{
	/// <summary>
	/// Settings for the primary dialog button.
	/// </summary>
	public ButtonSettings PrimaryButtonSettings { get; set; }

	/// <summary>
	/// Settings for the secondary dialog button(s).
	/// </summary>
	public ButtonSettings SecondaryButtonSettings { get; set; }

	/// <summary>
	/// Settings for the underlying <see cref="HxModal"/> component.
	/// </summary>
	public ModalSettings ModalSettings { get; set; }
}
