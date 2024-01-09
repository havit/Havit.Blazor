namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Used in a component that can generate chips.
/// </summary>
public interface IHxChipGenerator
{
	/// <summary>
	/// Get chips from the component.
	/// </summary>
	IEnumerable<ChipItem> GetChips();
}
