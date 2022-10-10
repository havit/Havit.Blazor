namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Select. Consider creating custom picker derived from <see cref="HxRadioButtonListBase{TValueType, TItem}"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxRadioButtonList">https://havit.blazor.eu/components/HxRadioButtonList</see>
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public class HxRadioButtonList<TValue, TItem> : HxRadioButtonListBase<TValue, TItem>
{
	/// <summary>
	/// Selects value from item.
	/// Not required when TValueType is same as TItemTime.
	/// </summary>
	[Parameter]
	public Func<TItem, TValue> ItemValueSelector
	{
		get => ItemValueSelectorImpl;
		set => ItemValueSelectorImpl = value;
	}

	/// <summary>
	/// <see cref="ValueSelector"/> is obsolete, please use <see cref="ItemValueSelector"/> instead.
	/// </summary>
	[Parameter]
	[Obsolete($"{nameof(ValueSelector)} is obsolete, use {nameof(ItemValueSelector)} instead.")]
	public Func<TItem, TValue> ValueSelector
	{
		get => ItemValueSelector;
		set => ItemValueSelector = value;
	}

	/// <summary>
	/// Items to display. 
	/// </summary>
	[Parameter]
	public IEnumerable<TItem> Data
	{
		get => DataImpl;
		set => DataImpl = value;
	}

	/// <summary>
	/// Selects text to display from item. Also used for chip text.
	/// When not set <c>ToString()</c> is used.
	/// </summary>
	[Parameter]
	public Func<TItem, string> ItemTextSelector
	{
		get => ItemTextSelectorImpl;
		set => ItemTextSelectorImpl = value;
	}

	/// <summary>
	/// <see cref="TextSelector"/> is obsolete, please use <see cref="ItemTextSelector"/> instead.
	/// </summary>
	[Parameter]
	[Obsolete($"{nameof(TextSelector)} is obsolete, use {nameof(ItemTextSelector)} instead.")]
	public Func<TItem, string> TextSelector
	{
		get => ItemTextSelector;
		set => ItemTextSelector = value;
	}

	/// <summary>
	/// Gets html to display from item.
	/// When not set <see cref="ItemTextSelector"/> is used.
	/// </summary>
	[Parameter]
	public RenderFragment<TItem> ItemTemplate
	{
		get => ItemTemplateImpl;
		set => ItemTemplateImpl = value;
	}

	/// <summary>
	/// Selects value to sort items. Uses <see cref="TextSelector"/> property when not set.
	/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
	/// </summary>
	[Parameter]
	public Func<TItem, IComparable> ItemSortKeySelector
	{
		get => ItemSortKeySelectorImpl;
		set => ItemSortKeySelectorImpl = value;
	}

	/// <summary>
	/// <see cref="SortKeySelector"/> is obsolete, please use <see cref="ItemSortKeySelector"/> instead.
	/// </summary>
	[Parameter]
	[Obsolete($"{nameof(SortKeySelector)} is obsolete, use {nameof(ItemSortKeySelector)} instead.")]
	public Func<TItem, IComparable> SortKeySelector
	{
		get => ItemSortKeySelector;
		set => ItemSortKeySelector = value;
	}

	/// <summary>
	/// When <c>true</c>, items are sorted before displaying in select.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter]
	public bool AutoSort
	{
		get => AutoSortImpl;
		set => AutoSortImpl = value;
	}
}
