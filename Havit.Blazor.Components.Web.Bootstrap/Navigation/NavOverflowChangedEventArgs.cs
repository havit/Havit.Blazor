namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Arguments for the <see cref="HxNavOverflow.OnOverflowChanged"/> event.
/// </summary>
public class NavOverflowChangedEventArgs
{
	/// <summary>
	/// Number of items currently moved to the overflow menu.
	/// </summary>
	public int OverflowCount { get; set; }

	/// <summary>
	/// Number of items currently visible in the nav.
	/// </summary>
	public int VisibleCount { get; set; }
}
