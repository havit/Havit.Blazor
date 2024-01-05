namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Auto-close behavior of the <see cref="HxDropdown"/> (<see cref="HxDropdownToggleButton"/> respectively).
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior"/>.
/// </summary>
public enum DropdownAutoClose
{
	/// <summary>
	/// The dropdown will be closed by clicking outside or inside the dropdown menu. Default.
	/// </summary>
	True = 0,

	/// <summary>
	/// The dropdown will be closed by clicking the toggle button. (It will also not be closed by pressing the esc key).
	/// </summary>
	False = 1,

	/// <summary>
	/// The dropdown will be closed (only) by clicking inside the dropdown menu.
	/// </summary>
	Inside = 2,

	/// <summary>
	/// The dropdown will be closed (only) by clicking outside the dropdown menu.
	/// </summary>
	Outside = 3
}
