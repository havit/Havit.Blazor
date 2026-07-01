namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Helps keep some properties/methods hidden from the regular API (accessible via interface).
/// </summary>
public interface IDropdownContainer
{
	bool IsOpen { get; set; }
}
