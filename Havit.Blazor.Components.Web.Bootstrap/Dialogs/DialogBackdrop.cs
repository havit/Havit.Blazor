namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Options for controlling the behavior of the <see cref="HxDialog.Backdrop"/>.
/// </summary>
public enum DialogBackdrop
{
	/// <summary>
	/// A backdrop will be rendered. The dialog will be closed upon clicking on the backdrop.
	/// </summary>
	True = 0,

	/// <summary>
	/// No (visible) backdrop will be rendered and clicking outside the dialog does not close it.
	/// The dialog is still modal, the rest of the page stays inert. Use <see cref="HxDialog.NonModal"/> to let users interact with the page.
	/// </summary>
	False = 1,

	/// <summary>
	/// A static backdrop will be rendered. The dialog will not be closed upon clicking on the backdrop.
	/// </summary>
	Static = 2
}
