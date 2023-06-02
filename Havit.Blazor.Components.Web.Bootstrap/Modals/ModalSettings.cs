namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxModal"/> component
/// </summary>
public record ModalSettings
{
	/// <summary>
	/// For modals that simply appear rather than fade in to view, setting <c>false</c> removes the <c>.fade</c> class from your modal markup.
	/// Default value is <c>true</c>.
	/// </summary>
	public bool? Animated { get; set; }

	/// <summary>
	/// Indicates whether the modal shows close button in header.
	/// </summary>
	public bool? ShowCloseButton { get; set; }

	/// <summary>
	/// Close icon to be used in header.
	/// </summary>
	public IconBase CloseButtonIcon { get; set; }

	/// <summary>
	/// Indicates whether the modal closes when escape key is pressed.
	/// </summary>
	public bool? CloseOnEscape { get; set; }

	/// <summary>
	/// Size of the modal.
	/// </summary>
	public ModalSize? Size { get; set; }

	/// <summary>
	/// Fullscreen behavior of the modal.
	/// </summary>
	public ModalFullscreen? Fullscreen { get; set; }

	/// <summary>
	/// Indicates whether to apply a backdrop on body while the modal is open.
	/// If set to <see cref="ModalBackdrop.Static"/>, the modal cannot be closed by clicking on the backdrop.
	/// Default value (from <see cref="HxModal.Defaults"/>) is <see cref="ModalBackdrop.True"/>.
	/// </summary>
	public ModalBackdrop? Backdrop { get; set; }

	/// <summary>
	/// Allows scrolling the modal body.
	/// </summary>
	public bool? Scrollable { get; set; }

	/// <summary>
	/// Allows vertical centering of the modal.
	/// </summary>
	public bool? Centered { get; set; }

	/// <summary>
	/// Additional CSS class for the main element (<c>div.modal</c>).
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the dialog (<c>div.modal-dialog</c> element).
	/// </summary>
	public string DialogCssClass { get; set; }

	/// <summary>
	/// Additional header CSS class (<c>div.modal-header</c>).
	/// </summary>
	public string HeaderCssClass { get; set; }

	/// <summary>
	/// Additional content CSS class (<c>div.modal-content</c>).
	/// </summary>
	public string ContentCssClass { get; set; }

	/// <summary>
	/// Additional body CSS class (<c>div.modal-body</c>).
	/// </summary>
	public string BodyCssClass { get; set; }

	/// <summary>
	/// Additional footer CSS class (<c>div.modal-footer</c>).
	/// </summary>
	public string FooterCssClass { get; set; }
}
