using System.ComponentModel.DataAnnotations;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Base class for HxSelect and custom-implemented SELECT-pickers.
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public abstract class HxSelectBase<TValue, TItem> : HxInputBaseWithInputGroups<TValue>, IInputWithSize, IInputWithLabelType
{
	/// <summary>
	/// Return <see cref="HxSelect{TValue, TItem}"/> defaults.
	/// Enables not sharing defaults in descendants with base classes.
	/// Enables having multiple descendants that differ in the default values.
	/// </summary>
	protected override SelectSettings GetDefaults() => HxSelect.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxSelect.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public SelectSettings Settings { get; set; }

	/// <inheritdoc cref="Bootstrap.LabelType" />
	[Parameter] public LabelType? LabelType { get; set; }
	protected LabelType LabelTypeEffective => LabelType ?? GetSettings()?.LabelType ?? GetDefaults()?.LabelType ?? HxSetup.Defaults.LabelType;
	LabelType IInputWithLabelType.LabelTypeEffective => LabelTypeEffective;

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected override SelectSettings GetSettings() => Settings;


	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize;
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <summary>
	/// Indicates when <c>null</c> is a valid value.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected bool? NullableImpl { get; set; }

	/// <summary>
	/// Indicates when <c>null</c> is a valid value.
	/// Uses (in order) to get the effective value: Nullable property, RequiresAttribute on bounded property (<c>false</c>) Nullable type on bounded property (<c>true</c>), class (<c>true</c>), default (<c>false</c>).
	/// </summary>
	protected bool NullableEffective
	{
		get
		{
			if (NullableImpl != null)
			{
				return NullableImpl.Value;
			}

			if (GetValueAttribute<RequiredAttribute>() != null)
			{
				return false;
			}

			if (Nullable.GetUnderlyingType(typeof(TValue)) != null)
			{
				return true;
			}

			if (typeof(TValue).IsClass)
			{
				return true;
			}

			return true;
		}
	}

	/// <summary>
	/// Text to display for <c>null</c> value.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected string NullTextImpl { get; set; }

	/// <summary>
	/// Text to display when <see cref="DataImpl"/> is <c>null</c>.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected string NullDataTextImpl { get; set; }

	/// <summary>
	/// Selects value from item.
	/// Not required when TValue is the same as TItem.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, TValue> ValueSelectorImpl { get; set; }

	/// <summary>
	/// Items to display. 
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected IEnumerable<TItem> DataImpl { get; set; }

	/// <summary>
	/// Selects text to display from item.
	/// When not set ToString() is used.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, string> TextSelectorImpl { get; set; }

	/// <summary>
	/// Selects value to sort items. Uses <see cref="TextSelectorImpl"/> property when not set.
	/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, IComparable> SortKeySelectorImpl { get; set; }

	/// <summary>
	/// When <c>true</c>, items are sorted before displaying in select.
	/// Default value is <c>true</c>.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected bool AutoSortImpl { get; set; } = true;

	/// <summary>
	/// When set, determines whether an item is disabled (non-selectable and greyed out).
	/// When returns <c>true</c>, the corresponding option will be rendered with <c>disabled</c> attribute.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, bool> ItemDisabledSelectorImpl { get; set; }

	/// <summary>
	/// Enables filtering capabilities (renders the Bootstrap Combobox instead of the native <c>select</c>).
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected bool? AllowFilteringImpl { get; set; }

	/// <summary>
	/// Effective value of the filtering capabilities (<see cref="AllowFilteringImpl"/> property, settings, defaults).
	/// </summary>
	protected bool AllowFilteringEffective => AllowFilteringImpl ?? GetSettings()?.AllowFiltering ?? GetDefaults().AllowFiltering;

	/// <summary>
	/// Defines a custom filtering predicate to apply to the list of items.
	/// If not specified, the default behavior filters items based on whether the item text (obtained via <see cref="TextSelectorImpl"/>) contains the filter query string.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, string, bool> FilterPredicateImpl { get; set; }

	/// <summary>
	/// When enabled, the filter will be cleared when the menu is closed.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected bool? ClearFilterOnHideImpl { get; set; }

	/// <summary>
	/// Effective value of clearing the filter when the menu is closed (<see cref="ClearFilterOnHideImpl"/> property, settings, defaults).
	/// </summary>
	protected bool ClearFilterOnHideEffective => ClearFilterOnHideImpl ?? GetSettings()?.ClearFilterOnHide ?? GetDefaults().ClearFilterOnHide;

	/// <summary>
	/// Template that defines what should be rendered in case of empty filtered items.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected RenderFragment FilterEmptyResultTemplateImpl { get; set; }

	/// <summary>
	/// Text to display when the filtered results list is empty and when not using <see cref="FilterEmptyResultTemplateImpl"/>.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected string FilterEmptyResultTextImpl { get; set; }

	/// <summary>
	/// Icon displayed in the filter input for searching the filter.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected IconBase FilterSearchIconImpl { get; set; }

	/// <summary>
	/// Effective value of the filter search icon (<see cref="FilterSearchIconImpl"/> property, settings, defaults).
	/// </summary>
	protected IconBase FilterSearchIconEffective => FilterSearchIconImpl ?? GetSettings()?.FilterSearchIcon ?? GetDefaults().FilterSearchIcon;

	/// <summary>
	/// Icon displayed in the filter input for clearing the filter.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected IconBase FilterClearIconImpl { get; set; }

	/// <summary>
	/// Effective value of the filter clear icon (<see cref="FilterClearIconImpl"/> property, settings, defaults).
	/// </summary>
	protected IconBase FilterClearIconEffective => FilterClearIconImpl ?? GetSettings()?.FilterClearIcon ?? GetDefaults().FilterClearIcon;

	/// <inheritdoc cref="HxInputBase{TValue}.EnabledEffective" />
	protected override bool EnabledEffective => base.EnabledEffective && (_itemsToRender != null);

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

	private protected override string CoreInputCssClass => AllowFilteringEffective ? "form-control combobox-toggle" : "form-control";

	/// <inheritdoc/>
	protected override LabelValueRenderOrder RenderOrder =>
		(AllowFilteringEffective && (LabelTypeEffective == Bootstrap.LabelType.Floating))
			? LabelValueRenderOrder.ValueOnly // floating label is rendered inside the HxSelectInternal component
			: base.RenderOrder;

	private IEqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;
	private List<TItem> _itemsToRender;
	private int _selectedItemIndex;
	private string _chipValue;
	private HxSelectInternal<TValue, TItem> _hxSelectInternalComponent;

	/// <inheritdoc/>
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		_chipValue = null;

		RefreshState();

		if (AllowFilteringEffective)
		{
			BuildRenderInput_Combobox(builder);
			return;
		}

		bool enabledEffective = EnabledEffective;

		builder.OpenElement(100, "select");
		BuildRenderInput_AddCommonAttributes(builder, null);

		builder.AddAttribute(1000, "value", _selectedItemIndex);
		builder.AddAttribute(1001, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
		builder.SetUpdatesAttributeName("value");
		builder.AddEventStopPropagationAttribute(1002, "onclick", true);
		builder.AddElementReferenceCapture(1003, elementReference => InputElement = elementReference);

		if (_itemsToRender != null)
		{
			if ((NullableEffective && enabledEffective) || (_selectedItemIndex == -1))
			{
				builder.OpenElement(2000, "option");
				builder.AddAttribute(2001, "value", -1);
				builder.AddContent(2003, NullTextImpl);
				builder.CloseElement();
			}

			for (int i = 0; i < _itemsToRender.Count; i++)
			{
				var item = _itemsToRender[i];
				if (item != null)
				{
					bool selected = (i == _selectedItemIndex);

					if (enabledEffective || selected) /* when not enabled only selected item is rendered */
					{
						string text = SelectorHelpers.GetText(TextSelectorImpl, item);
						bool disabled = SelectorHelpers.GetDisabled(ItemDisabledSelectorImpl, item);

						builder.OpenElement(3000, "option");
						builder.SetKey(i.ToString());
						builder.AddAttribute(3001, "value", i.ToString());
						if (disabled)
						{
							builder.AddAttribute(3002, "disabled", true);
						}
						builder.AddContent(3003, text);
						builder.CloseElement();

						if (selected)
						{
							_chipValue = text;
						}
					}
				}
			}
		}
		else
		{
			if (!String.IsNullOrEmpty(NullDataTextImpl))
			{
				builder.OpenElement(4000, "option");
				builder.AddAttribute(4001, "value", -1);
				builder.AddContent(4002, NullDataTextImpl);
				builder.CloseElement();
			}
		}
		builder.CloseElement();
	}

	/// <summary>
	/// Renders the Bootstrap Combobox (used when filtering is enabled, <see cref="AllowFilteringEffective"/>).
	/// </summary>
	private void BuildRenderInput_Combobox(RenderTreeBuilder builder)
	{
		if ((_itemsToRender != null) && (_selectedItemIndex > -1))
		{
			_chipValue = SelectorHelpers.GetText(TextSelectorImpl, _itemsToRender[_selectedItemIndex]);
		}

		builder.OpenComponent<HxSelectInternal<TValue, TItem>>(100);
		builder.AddAttribute(101, nameof(HxSelectInternal<TValue, TItem>.InputId), InputId);
		builder.AddAttribute(102, nameof(HxSelectInternal<TValue, TItem>.InputCssClass), GetInputCssClassToRender());
		builder.AddAttribute(103, nameof(HxSelectInternal<TValue, TItem>.InputSizeEffective), InputSizeEffective);
		builder.AddAttribute(104, nameof(HxSelectInternal<TValue, TItem>.EnabledEffective), EnabledEffective);
		builder.AddAttribute(105, nameof(HxSelectInternal<TValue, TItem>.LabelTypeEffective), LabelTypeEffective);
		builder.AddAttribute(106, nameof(HxSelectInternal<TValue, TItem>.FormValueComponent), this);
		builder.AddAttribute(107, nameof(HxSelectInternal<TValue, TItem>.ItemsToRender), _itemsToRender);
		builder.AddAttribute(108, nameof(HxSelectInternal<TValue, TItem>.SelectedItemIndex), _selectedItemIndex);
		builder.AddAttribute(109, nameof(HxSelectInternal<TValue, TItem>.SelectedItemIndexChanged), EventCallback.Factory.Create<int>(this, HandleComboboxSelectedItemIndexChanged));
		builder.AddAttribute(110, nameof(HxSelectInternal<TValue, TItem>.NullableEffective), NullableEffective);
		builder.AddAttribute(111, nameof(HxSelectInternal<TValue, TItem>.NullText), NullTextImpl);
		builder.AddAttribute(112, nameof(HxSelectInternal<TValue, TItem>.NullDataText), NullDataTextImpl);
		builder.AddAttribute(113, nameof(HxSelectInternal<TValue, TItem>.TextSelector), TextSelectorImpl);
		builder.AddAttribute(114, nameof(HxSelectInternal<TValue, TItem>.ItemDisabledSelector), ItemDisabledSelectorImpl);
		builder.AddAttribute(115, nameof(HxSelectInternal<TValue, TItem>.FilterPredicate), FilterPredicateImpl);
		builder.AddAttribute(116, nameof(HxSelectInternal<TValue, TItem>.ClearFilterOnHide), ClearFilterOnHideEffective);
		builder.AddAttribute(117, nameof(HxSelectInternal<TValue, TItem>.FilterEmptyResultTemplate), FilterEmptyResultTemplateImpl);
		builder.AddAttribute(118, nameof(HxSelectInternal<TValue, TItem>.FilterEmptyResultText), FilterEmptyResultTextImpl);
		builder.AddAttribute(119, nameof(HxSelectInternal<TValue, TItem>.FilterSearchIcon), FilterSearchIconEffective);
		builder.AddAttribute(120, nameof(HxSelectInternal<TValue, TItem>.FilterClearIcon), FilterClearIconEffective);

		builder.AddMultipleAttributes(200, AdditionalAttributes);

		builder.AddComponentReferenceCapture(300, r => _hxSelectInternalComponent = (HxSelectInternal<TValue, TItem>)r);

		builder.CloseComponent();
	}

	/// <summary>
	/// Handles the item selection in the combobox rendering (<see cref="AllowFilteringEffective"/>).
	/// Receives the index of the selected item within the sorted items (<c>-1</c> for the <c>null</c> item) and sets the <c>CurrentValue</c>.
	/// </summary>
	private void HandleComboboxSelectedItemIndexChanged(int selectedItemIndex)
	{
		CurrentValueAsString = selectedItemIndex.ToString();
	}

	private void RefreshState()
	{
		if (DataImpl != null)
		{
			_itemsToRender = DataImpl.ToList();

			// AutoSort
			if (AutoSortImpl && (_itemsToRender.Count > 1))
			{
				if (SortKeySelectorImpl != null)
				{
					_itemsToRender = _itemsToRender.OrderBy(SortKeySelectorImpl).ToList();
				}
				else if (TextSelectorImpl != null)
				{
					_itemsToRender = _itemsToRender.OrderBy(TextSelectorImpl).ToList();
				}
				else
				{
					_itemsToRender = _itemsToRender.OrderBy(i => i.ToString()).ToList();
				}
			}

			// set next properties for rendering
			_selectedItemIndex = _itemsToRender.FindIndex(item => _comparer.Equals(Value, SelectorHelpers.GetValue<TItem, TValue>(ValueSelectorImpl, item)));

			if ((Value != null) && (_selectedItemIndex == -1))
			{
				throw new InvalidOperationException($"[{GetType().Name}] Data does not contain item for current value '{Value}'.");
			}
		}
		else
		{
			_itemsToRender = null;
			_selectedItemIndex = -1;
		}
	}

	/// <inheritdoc/>
	protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
	{
		int index = int.Parse(value);
		result = (index == -1)
			? default(TValue)
			: SelectorHelpers.GetValue<TItem, TValue>(ValueSelectorImpl, _itemsToRender[index]);

		validationErrorMessage = null;
		return true;
	}

	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		if ((_chipValue is null) && (Value != null) && (DataImpl != null))
		{
			// fallback for initial rendering without chipValue
			// does not help when DataImpl is not set yet (loaded asynchronously)
			var item = DataImpl.FirstOrDefault(item => _comparer.Equals(Value, SelectorHelpers.GetValue<TItem, TValue>(ValueSelectorImpl, item)));
			_chipValue = SelectorHelpers.GetText(TextSelectorImpl, item);
		}
		builder.AddContent(0, _chipValue);
	}

	string IInputWithSize.GetInputSizeCssClass() => InputSizeEffective.AsFormSelectCssClass();

	/// <summary>
	/// Focuses the component.
	/// </summary>
	public async ValueTask FocusAsync()
	{
		if (AllowFilteringEffective)
		{
			if (_hxSelectInternalComponent == null)
			{
				throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The component reference is not available. You are most likely calling the method too early. The first render must complete before calling this method.");
			}
			await _hxSelectInternalComponent.FocusAsync();
			return;
		}

		await InputElement.FocusOrThrowAsync(this);
	}

}
