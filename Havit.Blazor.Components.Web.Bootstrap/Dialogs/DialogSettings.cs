namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxDialog"/> component
/// </summary>
public record DialogSettings
{
	/// <summary>
	/// Open/close animation of the dialog.
	/// </summary>
	public DialogAnimation? Animation { get; set; }

	/// <summary>
	/// Indicates whether the dialog shows a close button in the header.
	/// </summary>
	public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// The close icon to be used in the header.
	/// </summary>
	public IconBase CloseButtonIcon { get; set; }

	/// <summary>
	/// Settings for the close button in the header.
	/// </summary>
	public CloseButtonSettings CloseButtonSettings { get; set; }

	/// <summary>
	/// Indicates whether the dialog closes when the escape key is pressed.
	/// </summary>
	public bool? CloseOnEscape { get; set; }

	/// <summary>
	/// The size of the dialog.
	/// </summary>
	public DialogSize? Size { get; set; }

	/// <summary>
	/// The fullscreen behavior of the dialog.
	/// </summary>
	public DialogFullscreen? Fullscreen { get; set; }

	/// <summary>
	/// Indicates whether to apply a backdrop to the body while the dialog is open.
	/// If set to <see cref="DialogBackdrop.Static"/>, the dialog cannot be closed by clicking on the backdrop.
	/// The default value (from <see cref="HxDialog.Defaults"/>) is <see cref="DialogBackdrop.True"/>.
	/// </summary>
	public DialogBackdrop? Backdrop { get; set; }

	/// <summary>
	/// Allows scrolling of the dialog body.
	/// </summary>
	public bool? Scrollable { get; set; }

	/// <summary>
	/// When <c>true</c>, the dialog opens as non-modal (no backdrop, no focus trap, page stays interactive).
	/// </summary>
	public bool? NonModal { get; set; }

	/// <summary>
	/// Additional CSS class for the main <c>&lt;dialog&gt;</c> element.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Additional header CSS class (<c>div.dialog-header</c>).
	/// </summary>
	public string HeaderCssClass { get; set; }

	/// <summary>
	/// Additional body CSS class (<c>div.dialog-body</c>).
	/// </summary>
	public string BodyCssClass { get; set; }

	/// <summary>
	/// Additional footer CSS class (<c>div.dialog-footer</c>).
	/// </summary>
	public string FooterCssClass { get; set; }
}
