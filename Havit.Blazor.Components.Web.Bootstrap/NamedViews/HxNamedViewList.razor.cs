namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// List of named-views for <see cref="HxListLayout{TFilterModel}" />.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxListLayout#named-views">https://havit.blazor.eu/components/HxListLayout#named-views</see>
/// </summary>
public partial class HxNamedViewList<TItem>
{
	/// <summary>
	/// The items to display.
	/// </summary>
	[Parameter, EditorRequired] public IEnumerable<TItem> Items { get; set; }

	/// <summary>
	/// Selected item (highlighted in the list with <c>.active</c> CSS class).
	/// </summary>
	[Parameter] public TItem SelectedItem { get; set; }
	[Parameter] public EventCallback<TItem> SelectedItemChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="SelectedItemChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeSelectedItemChangedAsync(TItem itemSelected) => SelectedItemChanged.InvokeAsync(itemSelected);

	/// <summary>
	/// Selects the text to display from the item.
	/// You must set either this parameter or <see cref="ItemTemplate"/>.
	/// There is a special case when <c>TItem</c> is <see cref="NamedView{TFilterModel}"/> - in that case, the <see cref="NamedView{TFilterModel}.Name"/> is used as the default text selector.
	/// </summary>
	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	/// <summary>
	/// The template that defines how items in the component are displayed (optional).
	/// You must set either this parameter or <see cref="TextSelector"/>.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	protected override void OnParametersSet()
	{
		if ((TextSelector == null) && (ItemTemplate == null))
		{
			// special case for NamedView<TFilterModel>
			if (typeof(TItem).IsGenericType && (typeof(TItem).GetGenericTypeDefinition() == typeof(NamedView<>)))
			{
				TextSelector = item => (string)typeof(TItem).GetProperty(nameof(NamedView<object>.Name)).GetValue(item);
			}
			else
			{
				throw new InvalidOperationException($"Either {nameof(TextSelector)} or {nameof(ItemTemplate)} must be set.");
			}
		}
	}
}
