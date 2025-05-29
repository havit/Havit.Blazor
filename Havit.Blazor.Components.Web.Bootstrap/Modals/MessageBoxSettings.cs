using Microsoft.Extensions.Localization;

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

	/// <summary>
	/// Text for the OK button.
	/// </summary>
	public string OkButtonText { get; set; }

	/// <summary>
	/// Text for the Cancel button.
	/// </summary>
	public string CancelButtonText { get; set; }

	/// <summary>
	/// Text for the Abort button.
	/// </summary>
	public string AbortButtonText { get; set; }

	/// <summary>
	/// Text for the Yes button.
	/// </summary>
	public string YesButtonText { get; set; }

	/// <summary>
	/// Text for the No button.
	/// </summary>
	public string NoButtonText { get; set; }

	/// <summary>
	/// Text for the Retry button.
	/// </summary>
	public string RetryButtonText { get; set; }

	/// <summary>
	/// Text for the Ignore button.
	/// </summary>
	public string IgnoreButtonText { get; set; }
}
