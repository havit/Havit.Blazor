﻿using Havit.Blazor.Components.Web.Infrastructure;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Base class for <see cref="HxRadioButtonList{TValue, TItem}"/> and custom-implemented pickers.
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public abstract class HxRadioButtonListBase<TValue, TItem> : HxInputBase<TValue>
{
	/// <summary>
	/// Allows grouping radios on the same horizontal row by rendering them inline. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool Inline { get; set; }

	/// <summary>
	/// Selects a value from an item.
	/// Not required when <c>TValueType</c> is the same as <c>TItemTime</c>.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, TValue> ItemValueSelectorImpl { get; set; }

	/// <summary>
	/// Items to display. 
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected IEnumerable<TItem> DataImpl { get; set; }

	/// <summary>
	/// Gets the text to display from an item. Used also for chip text.
	/// When not set, ToString() is used.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, string> ItemTextSelectorImpl { get; set; }

	/// <summary>
	/// Gets the HTML to display from an item.
	/// When not set, <see cref="ItemTextSelectorImpl"/> is used.
	/// </summary>
	protected RenderFragment<TItem> ItemTemplateImpl { get; set; }

	/// <summary>
	/// Selects a value to sort items. Uses the <see cref="ItemTextSelectorImpl"/> property when not set.
	/// When complex sorting is required, sort the data manually and don't let this component sort them. Alternatively, create a custom comparable property.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected Func<TItem, IComparable> ItemSortKeySelectorImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for underlying radio-buttons (wrapping <c>div</c> element).
	/// </summary>
	protected string ItemCssClassImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for underlying radio-buttons (wrapping <c>div</c> element).
	/// </summary>
	protected Func<TItem, string> ItemCssClassSelectorImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the <c>input</c> element of underlying radio-buttons.
	/// </summary>
	protected string ItemInputCssClassImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the <c>input</c> element of underlying radio-button.
	/// </summary>
	protected Func<TItem, string> ItemInputCssClassSelectorImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the text of the underlying radio-buttons.
	/// </summary>
	protected string ItemTextCssClassImpl { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the text of the underlying radio-buttons.
	/// </summary>
	protected Func<TItem, string> ItemTextCssClassSelectorImpl { get; set; }

	/// <summary>
	/// When <c>true</c>, items are sorted before displaying in the select.
	/// The default value is <c>true</c>.
	/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
	/// </summary>
	protected bool AutoSortImpl { get; set; } = true;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputDate.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public RadioButtonListSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	protected override RadioButtonListSettings GetSettings() => Settings;

	/// <inheritdoc cref="HxInputBase{TValue}.EnabledEffective" />
	protected override bool EnabledEffective => base.EnabledEffective && (_itemsToRender != null);

	private protected override string CoreInputCssClass => throw new NotSupportedException();

	private IEqualityComparer<TValue> _comparer = EqualityComparer<TValue>.Default;
	private List<TItem> _itemsToRender;
	private int _selectedItemIndex;
	private string _chipValue;

	protected string GroupName { get; private set; }

	protected HxRadioButtonListBase()
	{
		GroupName = Guid.NewGuid().ToString("N");
	}

	/// <inheritdoc/>
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		_chipValue = null;

		RefreshState();

		if (_itemsToRender != null)
		{
			for (int i = 0; i < _itemsToRender.Count; i++)
			{
				builder.OpenRegion(0);
				BuildRenderInput_RenderRadioItem(builder, i);
				builder.CloseRegion();
			}
		}
	}

	protected void BuildRenderInput_RenderRadioItem(RenderTreeBuilder builder, int index)
	{
		var item = _itemsToRender[index];
		if (item != null)
		{
			bool selected = (index == _selectedItemIndex);
			if (selected)
			{
				_chipValue = SelectorHelpers.GetText(ItemTextSelectorImpl, item);
			}

			string inputId = GroupName + "_" + index.ToString();

			builder.OpenElement(100, "div");

			// TODO CoreCssClass
			builder.AddAttribute(101, "class", CssClassHelper.Combine("form-check", Inline ? "form-check-inline" : null, ItemCssClassImpl, ItemCssClassSelectorImpl?.Invoke(item)));

			builder.OpenElement(200, "input");
			builder.AddAttribute(201, "class", CssClassHelper.Combine("form-check-input", ItemInputCssClassImpl, ItemInputCssClassSelectorImpl?.Invoke(item)));
			builder.AddAttribute(202, "type", "radio");
			builder.AddAttribute(203, "name", GroupName);
			builder.AddAttribute(204, "id", inputId);
			builder.AddAttribute(205, "value", index.ToString());
			builder.AddAttribute(206, "checked", selected);
			builder.AddAttribute(207, "disabled", !CascadeEnabledComponent.EnabledEffective(this));
			int j = index;
			builder.AddAttribute(208, "onclick", EventCallback.Factory.Create(this, () => HandleInputClick(j)));
#if NET8_0_OR_GREATER
			builder.SetUpdatesAttributeName("checked");
#endif
			builder.AddEventStopPropagationAttribute(209, "onclick", true);
			builder.AddMultipleAttributes(250, AdditionalAttributes);
			builder.CloseElement(); // input

			builder.OpenElement(300, "label");
			builder.AddAttribute(301, "class", CssClassHelper.Combine("form-check-label", ItemTextCssClassImpl, ItemTextCssClassSelectorImpl?.Invoke(item)));
			builder.AddAttribute(302, "for", inputId);
			if (ItemTemplateImpl != null)
			{
				builder.AddContent(303, ItemTemplateImpl(item));
			}
			else
			{
				builder.AddContent(304, SelectorHelpers.GetText(ItemTextSelectorImpl, item));
			}
			builder.CloseElement(); // label

			builder.CloseElement(); // div
		}
	}

	private void HandleInputClick(int index)
	{
		CurrentValue = SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelectorImpl, _itemsToRender[index]);
	}

	private void RefreshState()
	{
		if (DataImpl != null)
		{
			_itemsToRender = DataImpl.ToList();

			// AutoSort
			if (AutoSortImpl && (_itemsToRender.Count > 1))
			{
				if (ItemSortKeySelectorImpl != null)
				{
					_itemsToRender = _itemsToRender.OrderBy(ItemSortKeySelectorImpl).ToList();
				}
				else if (ItemTextSelectorImpl != null)
				{
					_itemsToRender = _itemsToRender.OrderBy(ItemTextSelectorImpl).ToList();
				}
				else
				{
					_itemsToRender = _itemsToRender.OrderBy(i => i.ToString()).ToList();
				}
			}

			// set next properties for rendering
			_selectedItemIndex = _itemsToRender.FindIndex(item => _comparer.Equals(Value, SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelectorImpl, item)));

			if ((Value != null) && (_selectedItemIndex == -1))
			{
				throw new InvalidOperationException($"Data does not contain item for current value '{Value}'.");
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
		throw new NotSupportedException();
	}

	protected override void RenderChipGenerator(RenderTreeBuilder builder)
	{
		if (!String.IsNullOrEmpty(_chipValue))
		{
			base.RenderChipGenerator(builder);
		}
	}

	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, _chipValue);
	}

}
