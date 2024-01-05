namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context for the "load more" template of the <see cref="HxGrid{TItem}"/>.
/// </summary>
public sealed class GridLoadMoreTemplateContext
{
	private readonly Func<Task> loadMoreAsyncFunc;

	/// <summary>
	/// Instructs the grid to load the next page.
	/// </summary>
	public async Task LoadMoreAsync()
	{
		await loadMoreAsyncFunc.Invoke();
	}

	/// <remark>
	/// We do not use HxGrid because we have HxGrid&lt;TItem&gt;, which leads to GridLoadMoreTemplateContext&lt;TItem&gt;.
	/// </remark>
	private GridLoadMoreTemplateContext(Func<Task> loadMoreAsyncFunc)
	{
		this.loadMoreAsyncFunc = loadMoreAsyncFunc;
	}

	internal static GridLoadMoreTemplateContext CreateFor<TItem>(HxGrid<TItem> hxGrid)
	{
		return new GridLoadMoreTemplateContext(hxGrid.LoadMoreAsync);
	}
}
