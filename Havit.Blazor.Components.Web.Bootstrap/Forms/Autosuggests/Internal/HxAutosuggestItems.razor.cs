namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestItems<TItem>
	{
		[Parameter] public List<TItem> Items { get; set; }

		[Parameter] public EventCallback<TItem> OnItemClick { get; set; }

		[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

		/// <summary>
		/// Visually highlights the first suggestion.
		/// </summary>
		[Parameter] public bool HighlightFirstSuggestionEffective { get; set; }

		[Parameter] public RenderFragment EmptyTemplate { get; set; }
		[Parameter] public string CssClass { get; set; }

		private bool hasFocus = false;

		private ElementReference FirstItemReference
		{
			get
			{
				return firstItemReference;
			}
			set
			{
				firstItemReference = firstItemReference.Equals(default(ElementReference)) ? value : firstItemReference;
			}
		}
		private ElementReference firstItemReference;

		public async Task FocusFirstItemAsync()
		{
			await FirstItemReference.FocusAsync();
		}

		private async Task HandleItemClick(TItem value)
		{
			await OnItemClick.InvokeAsync(value);
		}
	}
}
