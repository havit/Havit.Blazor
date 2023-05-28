namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Helps keeping some properties/methods hidden from regular API (accessible via interface).
/// </summary>
public interface IDropdownContainer
{
	bool IsOpen { get; set; }
}
