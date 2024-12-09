namespace Havit.Blazor.Components.Web.Bootstrap.Internal;
/// <summary>
/// Represents an input with a toggle button option
/// </summary>
public interface IInputWithToggleButton
{
	/// <summary>
	/// Gets or sets the input as toggle or regular.
	/// </summary>
	InputAsToggle? InputAsToggle { get; set; }

	/// <summary>
	/// Gets the effective input type.
	/// </summary>
	InputAsToggle InputAsToggleEffective => InputAsToggle ?? Bootstrap.InputAsToggle.Regular;
}
