namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestItems<TItem>
	{
		[Parameter] public List<TItem> Items { get; set; }

		[Parameter] public EventCallback<TItem> OnItemClick { get; set; }

		[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

		[Parameter] public RenderFragment EmptyTemplate { get; set; }
		[Parameter] public string CssClass { get; set; }

		private async Task HandleItemClick(TItem value)
		{
			await OnItemClick.InvokeAsync(value);
		}
	}
}
