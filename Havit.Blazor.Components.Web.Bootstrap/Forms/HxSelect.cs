namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Select - DropDownList - single-item picker. Consider creating custom picker derived from <see cref="HxSelectBase{TValueType, TItem}"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSelect">https://havit.blazor.eu/components/HxSelect</see>
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
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
	/// Text to display for <c>null</c> value.
	/// </summary>
	[Parameter]
	public string NullText
	{
		get => NullTextImpl;
		set => NullTextImpl = value;
	}

	/// <summary>
	/// Text to display when <see cref="Data"/> is <c>null</c>.
	/// </summary>
	[Parameter]
	public string NullDataText
	{
		get => NullDataTextImpl;
		set => NullDataTextImpl = value;
	}

	/// <summary>
	/// Selects value from item.
	/// Not required when <c>TValueType</c> is same as <c>TItemTime</c>.
	/// </summary>
	[Parameter]
	public Func<TItem, TValue> ValueSelector
	{
		get => ValueSelectorImpl;
		set => ValueSelectorImpl = value;
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
	/// Selects text to display from item.
	/// When not set <c>ToString()</c> is used.
	/// </summary>
	[Parameter]
	public Func<TItem, string> TextSelector
	{
		get => TextSelectorImpl;
		set => TextSelectorImpl = value;
	}

	/// <summary>
	/// Selects value to sort items. Uses <see cref="TextSelector"/> property when not set.
	/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
	/// </summary>
	[Parameter]
	public Func<TItem, IComparable> SortKeySelector
	{
		get => SortKeySelectorImpl;
		set => SortKeySelectorImpl = value;
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
