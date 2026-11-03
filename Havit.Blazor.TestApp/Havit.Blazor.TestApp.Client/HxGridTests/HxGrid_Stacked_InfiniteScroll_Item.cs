namespace Havit.Blazor.TestApp.Client.HxGridTests;

/// <summary>
/// Data item of the HxGrid stacked infinite scroll E2E test pages.
/// </summary>
public record HxGrid_Stacked_InfiniteScroll_Item(int Id, string Name, string Email, string City)
{
	public const int ItemsCount = 500;

	/// <summary>
	/// Every third item has no email (an empty value must keep its line in the stacked layout).
	/// </summary>
	public static IReadOnlyList<HxGrid_Stacked_InfiniteScroll_Item> Items { get; } = Enumerable.Range(1, ItemsCount)
		.Select(i => new HxGrid_Stacked_InfiniteScroll_Item(
			i,
			$"Item {i:000}",
			(i % 3 == 0) ? null : $"item{i:000}@example.com",
			$"City {i % 17}"))
		.ToList();
}
