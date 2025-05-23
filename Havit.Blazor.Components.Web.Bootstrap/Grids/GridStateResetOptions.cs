namespace Havit.Blazor.Components.Web.Bootstrap;


/// <summary>
/// Specifies which aspects of the grid state should be reset.
/// </summary>
[Flags]
public enum GridStateResetOptions
{
	None = 0,
	ResetPosition = 1 << 0,
	// TODO ResetSorting = 1 << 1,

	// Add more options here in the future as needed
	// TODO All = ResetPosition | ResetSorting
}
