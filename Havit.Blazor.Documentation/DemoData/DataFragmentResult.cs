namespace Havit.Blazor.Documentation.DemoData;

public record DataFragmentResult<TItem>
{
	public required List<TItem> Data { get; init; }

	public required int TotalCount { get; init; }
}
