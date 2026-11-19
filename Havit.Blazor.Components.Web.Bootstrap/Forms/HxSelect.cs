namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Select - DropDownList - single-item picker. Consider creating a custom picker derived from <see cref="HxSelectBase{TValueType, TItem}"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSelect">https://havit.blazor.eu/components/HxSelect</see>
/// </summary>
/// <typeparam name="TValue">The type of value.</typeparam>
/// <typeparam name="TItem">The type of items.</typeparam>
public class HxSelect<TValue, TItem> : HxSelectBase<TValue, TItem>
{
	/// <summary>
	/// Indicates whether <c>null</c> is a valid value.
	/// </summary>
	[Parameter]
	public bool? Nullable
	{
		get => NullableImpl;
		set => NullableImpl = value;
	}

	/// <summary>
	/// The text to display for the <c>null</c> value.
	/// </summary>
	[Parameter]
	public string NullText
	{
		get => NullTextImpl;
		set => NullTextImpl = value;
	}

	/// <summary>
	/// The text to display when <see cref="Data"/> is <c>null</c>.
	/// </summary>
	[Parameter]
	public string NullDataText
	{
		get => NullDataTextImpl;
		set => NullDataTextImpl = value;
	}

	/// <summary>
	/// Selects the value from the item.
	/// Not required when <c>TValueType</c> is the same as <c>TItemTime</c>.
	/// </summary>
	[Parameter]
	public Func<TItem, TValue> ValueSelector
	{
		get => ValueSelectorImpl;
		set => ValueSelectorImpl = value;
	}

	/// <summary>
	/// The items to display. 
	/// </summary>
	[Parameter]
	public IEnumerable<TItem> Data
	{
		get => DataImpl;
		set => DataImpl = value;
	}

	/// <summary>
	/// Selects the text to display from the item.
	/// When not set, <c>ToString()</c> is used.
	/// </summary>
	[Parameter]
	public Func<TItem, string> TextSelector
	{
		get => TextSelectorImpl;
		set => TextSelectorImpl = value;
	}

	/// <summary>
	/// Selects the value to sort items. Uses the <see cref="TextSelector"/> property when not set.
	/// When complex sorting is required, sort the data manually and don't let this component sort them. Alternatively, create a custom comparable property.
	/// </summary>
	[Parameter]
	public Func<TItem, IComparable> SortKeySelector
	{
		get => SortKeySelectorImpl;
		set => SortKeySelectorImpl = value;
	}

	/// <summary>
	/// When <c>true</c>, the items are sorted before displaying in the select.
	/// The default value is <c>true</c>.
	/// </summary>
	[Parameter]
	public bool AutoSort
	{
		get => AutoSortImpl;
		set => AutoSortImpl = value;
	}

	/// <summary>
	/// When set, determines whether an item is disabled (non-selectable and greyed out).
	/// When returns <c>true</c>, the corresponding option will be rendered with <c>disabled</c> attribute.
	/// </summary>
	[Parameter]
	public Func<TItem, bool> ItemDisabledSelector
	{
		get => ItemDisabledSelectorImpl;
		set => ItemDisabledSelectorImpl = value;
	}

	/// <summary>
	/// Enables filtering capabilities.
	/// When enabled, the component renders the Bootstrap Combobox (a toggle with a menu containing a filter input) instead of the native <c>select</c> element.
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter]
	public bool? AllowFiltering
	{
		get => AllowFilteringImpl;
		set => AllowFilteringImpl = value;
	}

	/// <summary>
	/// Defines a custom filtering predicate to apply to the list of items.
	/// If not specified, the default behavior filters items based on whether the item text (obtained via <see cref="TextSelector"/>) contains the filter query string.
	/// </summary>
	[Parameter]
	public Func<TItem, string, bool> FilterPredicate
	{
		get => FilterPredicateImpl;
		set => FilterPredicateImpl = value;
	}

	/// <summary>
	/// When enabled, the filter will be cleared when the menu is closed.
	/// The default is <c>true</c>.
	/// </summary>
	[Parameter]
	public bool? ClearFilterOnHide
	{
		get => ClearFilterOnHideImpl;
		set => ClearFilterOnHideImpl = value;
	}

	/// <summary>
	/// Template that defines what should be rendered in case of empty filtered items.
	/// </summary>
	[Parameter]
	public RenderFragment FilterEmptyResultTemplate
	{
		get => FilterEmptyResultTemplateImpl;
		set => FilterEmptyResultTemplateImpl = value;
	}

	/// <summary>
	/// Text to display when the filtered results list is empty and when not using <see cref="FilterEmptyResultTemplate"/>.
	/// </summary>
	[Parameter]
	public string FilterEmptyResultText
	{
		get => FilterEmptyResultTextImpl;
		set => FilterEmptyResultTextImpl = value;
	}

	/// <summary>
	/// Icon displayed in filter input for searching the filter.
	/// </summary>
	[Parameter]
	public IconBase FilterSearchIcon
	{
		get => FilterSearchIconImpl;
		set => FilterSearchIconImpl = value;
	}

	/// <summary>
	/// Icon displayed in filter input for clearing the filter.
	/// </summary>
	[Parameter]
	public IconBase FilterClearIcon
	{
		get => FilterClearIconImpl;
		set => FilterClearIconImpl = value;
	}
}
