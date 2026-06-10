namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Auto-close behavior of the <see cref="HxMenu"/> (<see cref="HxMenuToggleButton"/> respectively).
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior"/>.
/// </summary>
public enum MenuAutoClose
{
	/// <summary>
	/// The menu will be closed by clicking outside or inside the menu. Default.
	/// </summary>
	True = 0,

	/// <summary>
	/// The menu will be closed by clicking the toggle button. (It will also not be closed by pressing the esc key).
	/// </summary>
	False = 1,

	/// <summary>
	/// The menu will be closed (only) by clicking inside the menu.
	/// </summary>
	Inside = 2,

	/// <summary>
	/// The menu will be closed (only) by clicking outside the menu.
	/// </summary>
	Outside = 3
}
