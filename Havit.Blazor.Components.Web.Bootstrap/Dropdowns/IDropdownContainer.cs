namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Helps keeping some properties/methods hidden from regular API (accesible via interface).
	/// </summary>
	public interface IDropdownContainer
	{
		bool IsOpen { get; set; }
	}
}
