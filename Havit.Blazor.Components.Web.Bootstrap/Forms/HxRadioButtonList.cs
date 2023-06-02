namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data-based list of radio buttons. Consider creating custom picker derived from <see cref="HxRadioButtonListBase{TValueType, TItem}"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxRadioButtonList">https://havit.blazor.eu/components/HxRadioButtonList</see>
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public class HxRadioButtonList<TValue, TItem> : HxRadioButtonListBase<TValue, TItem>
{
	/// <summary>
	/// Selects value from item.
	/// Not required when <c>TValue</c> is same as <c>TItem</c>.
	/// </summary>
	[Parameter]
	public Func<TItem, TValue> ItemValueSelector
	{
		get => ItemValueSelectorImpl;
		set => ItemValueSelectorImpl = value;
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
	/// Selects value to sort items. Uses <see cref="ItemTextSelector"/> property when not set.
	/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
	/// </summary>
	[Parameter]
	public Func<TItem, IComparable> ItemSortKeySelector
	{
		get => ItemSortKeySelectorImpl;
		set => ItemSortKeySelectorImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for underlying radio-buttons (wrapping <c>div</c> element).
	/// </summary>
	[Parameter]
	public string ItemCssClass
	{
		get => ItemCssClassImpl;
		set => ItemCssClassImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for underlying radio-buttons (wrapping <c>div</c> element).
	/// </summary>
	[Parameter]
	public Func<TItem, string> ItemCssClassSelector
	{
		get => ItemCssClassSelectorImpl;
		set => ItemCssClassSelectorImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for the <c>input</c> element of underlying radio-buttons.
	/// </summary>
	[Parameter]
	public string ItemInputCssClass
	{
		get => ItemInputCssClassImpl;
		set => ItemInputCssClassImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for the <c>input</c> element of underlying radio-button.
	/// </summary>
	[Parameter]
	public Func<TItem, string> ItemInputCssClassSelector
	{
		get => ItemInputCssClassSelectorImpl;
		set => ItemInputCssClassSelectorImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for the text of the underlying radio-buttons.
	/// </summary>
	[Parameter]
	public string ItemTextCssClass
	{
		get => ItemTextCssClassImpl;
		set => ItemTextCssClassImpl = value;
	}

	/// <summary>
	/// Additional CSS class(es) for the text of the underlying radio-buttons.
	/// </summary>
	[Parameter]
	public Func<TItem, string> ItemTextCssClassSelector
	{
		get => ItemTextCssClassSelectorImpl;
		set => ItemTextCssClassSelectorImpl = value;
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
