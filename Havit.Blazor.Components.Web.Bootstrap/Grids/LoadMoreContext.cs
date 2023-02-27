namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Context for "load more" template.
/// </summary>
public sealed class LoadMoreContext
{
	private readonly Func<Task> loadMoreAsyncFunc;

	/// <summary>
	/// Instructs to load next page.
	/// </summary>
	public async Task LoadMoreAsync()
	{
		await loadMoreAsyncFunc.Invoke();
	}

	/// <remark>
	/// Not used HxGrid because we have HxGrid&lt;TItem&gt; which leads to LoadMoreContext&lt;TItem&gt;.
	/// </remark>
	private LoadMoreContext(Func<Task> loadMoreAsyncFunc)
	{
		this.loadMoreAsyncFunc = loadMoreAsyncFunc;
	}

	public static LoadMoreContext CreateFor<TItem>(HxGrid<TItem> hxGrid)
	{
		return new LoadMoreContext(hxGrid.LoadMoreAsync);
	}

}
