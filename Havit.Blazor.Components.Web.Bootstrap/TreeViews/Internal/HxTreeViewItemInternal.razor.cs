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
		[Parameter] public Func<TItem, ICollection<TItem>> ChildrenSelector { get; set; }
		[Parameter] public int Level { get; set; }
		[Parameter] public RenderFragment<TItem> ContentTemplate { get; set; }
		[Parameter] public ICollection<TItem> Siblings { get; set; }

		[CascadingParameter] protected HxTreeView<TItem> TreeViewContainer { get; set; }

		private async Task HandleItemClicked()
		{
			await this.OnItemSelected.InvokeAsync(this.Item);
		}

		private Task HandleItemExpanderClicked()
		{
			this.IsExpanded = !this.IsExpanded;

			return Task.CompletedTask;
		}

		/// <summary>
		/// Sets the <see cref="HxTreeView{TItem}.TransportedItem"/> to point to this item when it's being dragged.
		/// </summary>
		protected void OnDragStart()
		{
			TreeViewContainer.TransportedItem = Item;
			TreeViewContainer.TransportedItemsSiblings = Siblings;
			Console.WriteLine($"Drag start: {TreeViewContainer.TransportedItem}");
		}

		/// <summary>
		/// Gets called when an item is dropped onto this item.
		/// Adds the dropped item to this items children.
		/// </summary>
		protected void OnDrop()
		{
			Console.WriteLine($"Drop: {TreeViewContainer.TransportedItem} to {Item}");
			Console.WriteLine($"Children: {ChildrenSelector(Item)?.Count}");

			if (ChildrenSelector(Item) is null)
			{
				return;
			}

			Console.WriteLine(TreeViewContainer.TransportedItemsSiblings.Remove(TreeViewContainer.TransportedItem));

			ChildrenSelector(Item).Add(TreeViewContainer.TransportedItem);
			Console.WriteLine($"Children: {ChildrenSelector(Item)?.Count()}");

			TreeViewContainer.UpdateItemsAsync();
		}
	}
}

/*
	Drag & drop current issues:
	- Adding doesn't work when the underlying type is an array.
	- Can't add to an item that has null items.
 */