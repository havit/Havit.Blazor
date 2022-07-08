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

		private HxCollapse collapseComponent;
		private bool animating;

		private async Task HandleItemClicked()
		{
			await this.OnItemSelected.InvokeAsync(this.Item);
		}

		private async Task HandleItemExpanderClicked()
		{
			if (animating)
			{
				return;
			}

			animating = true;

			if (this.IsExpanded.GetValueOrDefault())
			{
				this.IsExpanded = false;
				await collapseComponent.HideAsync();
			}
			else
			{
				this.IsExpanded = true;
				await collapseComponent.ShowAsync();
			}
		}

		private void HandleAnimationCompleted()
		{
			animating = false;
		}
	}
}