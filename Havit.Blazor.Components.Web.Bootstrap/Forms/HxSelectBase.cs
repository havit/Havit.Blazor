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

	/// <inheritdoc cref="HxInputBase{TValue}.EnabledEffective" />
	protected override bool EnabledEffective => base.EnabledEffective && (_itemsToRender != null);

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

	private protected override string CoreInputCssClass => "form-select";

	private IEqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;
	private List<TItem> _itemsToRender;
	private int _selectedItemIndex;
	private string _chipValue;

	/// <inheritdoc/>
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		_chipValue = null;

		RefreshState();

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
	public async ValueTask FocusAsync() => await InputElement.FocusOrThrowAsync(this);

}
