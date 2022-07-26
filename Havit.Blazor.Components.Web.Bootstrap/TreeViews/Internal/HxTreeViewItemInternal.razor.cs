namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxTreeViewItemInternal<TItem> : ComponentBase
	{
		[Parameter] public TItem Item { get; set; }
		[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }

		[Parameter] public bool? IsExpanded { get; set; }

		[Parameter] public Func<TItem, string> TitleSelector { get; set; }
		[Parameter] public Func<TItem, IconBase> IconSelector { get; set; }
		[Parameter] public Func<TItem, bool> InitialExpandedSelector { get; set; }
		[Parameter] public string CssClass { get; set; }
		[Parameter] public Func<TItem, string> CssClassSelector { get; set; }
		[Parameter] public Func<TItem, IEnumerable<TItem>> ChildrenSelector { get; set; }
		[Parameter] public int Level { get; set; }
		[Parameter] public RenderFragment<TItem> ContentTemplate { get; set; }

		[CascadingParameter] protected HxTreeView<TItem> TreeViewContainer { get; set; }

		protected HxCollapse collapseComponent;
		/// <summary>
		/// Indicates whether the collapse is currently animating (expanding or collapsing).
		/// </summary>
		protected bool animating;

		protected bool initiallyExpanded;

		protected override void OnInitialized()
		{
			initiallyExpanded = this.InitialExpandedSelector?.Invoke(this.Item) ?? false;
			if (initiallyExpanded)
			{
				IsExpanded = true;
			}
		}

		private async Task HandleItemClicked()
		{
			await this.OnItemSelected.InvokeAsync(this.Item);
		}

		private async Task HandleItemExpanderClicked()
		{
			if (this.IsExpanded.GetValueOrDefault())
			{
				await CollapseAsync();
			}
			else
			{
				await ExpandAsync();
			}
		}

		/// <summary>
		/// Expand the item with an animation.
		/// </summary>
		/// <returns></returns>
		protected async Task ExpandAsync()
		{
			if (collapseComponent is null || animating)
			{
				return;
			}

			animating = true;
			IsExpanded = true;
			await collapseComponent.ShowAsync();
		}

		/// <summary>
		/// Collapse the item with an animation.
		/// </summary>
		/// <returns></returns>
		protected async Task CollapseAsync()
		{
			if (collapseComponent is null || animating)
			{
				return;
			}

			animating = true;
			IsExpanded = false;
			await collapseComponent.HideAsync();
		}

		private void HandleExpansionCompleted()
		{
			IsExpanded = true;
			animating = false;
		}

		private void HandleCollapseCompleted()
		{
			IsExpanded = false;
			animating = false;
		}
	}
}