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

		private bool initialExpansionStarted = false;

		private IconBase icon;
		private string title;
		private string cssClassFromSelector;
		private IEnumerable<TItem> children;
		private bool hasChildren => children?.Any() ?? false;
		private bool isSelected => this.Item.Equals(this.TreeViewContainer.SelectedItem);

		protected override void OnParametersSet()
		{
			icon = this.IconSelector?.Invoke(this.Item) ?? null;
			title = this.TitleSelector?.Invoke(this.Item) ?? null;
			cssClassFromSelector = this.CssClassSelector?.Invoke(this.Item) ?? null;
			children = this.ChildrenSelector(this.Item);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!initialExpansionStarted && !firstRender)
			{
				this.IsExpanded ??= this.InitialExpandedSelector?.Invoke(this.Item) ?? false;

				if (this.IsExpanded.GetValueOrDefault() && collapseComponent is not null)
				{
					initialExpansionStarted = true;
					await Expand();
				}
			}
			else if (firstRender)
			{
				Task delayedStateHasChanged = DelayedStateHasChanged();
			}
		}

		private async Task DelayedStateHasChanged()
		{
			await Task.Delay(50);
			StateHasChanged();
		}

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

			if (this.IsExpanded.GetValueOrDefault())
			{
				await Collapse();
			}
			else
			{
				await Expand();
			}
		}

		private async Task Expand()
		{
			animating = true;
			IsExpanded = true;
			StateHasChanged();
			await collapseComponent.ShowAsync();
		}

		private async Task Collapse()
		{
			animating = true;
			IsExpanded = false;
			await collapseComponent.HideAsync();
		}

		private void HandleAnimationCompleted()
		{
			animating = false;
		}
	}
}