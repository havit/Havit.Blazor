using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Select - DropDown &amp; Check box list - multi-item picker.
/// </summary>
/// <typeparam name="TValue">Type of value.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public class HxMultiSelect<TValue, TItem> : HxInputBase<List<TValue>>
{
	// TODO: Renderování chipu/chipů
	// TODO: Naming EmptyText
	// TODO: Naming AndMoreText, AndMoreAfter
	// TODO: Func<List<TItem>, string> SelectionTextSelector?
	// TODO: Template pro items?

	/// <summary>
	/// Items to display. 
	/// </summary>
	[Parameter] public IEnumerable<TItem> Data { get; set; }

	/// <summary>
	/// Selects text to display from item.
	/// When not set, ToString() is used.
	/// </summary>
	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	/// <summary>
	/// Selects value from item.
	/// Not required when TValue is same as TItem.
	/// </summary>
	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	/// <summary>
	/// Selects value for items sorting. When not set, <see cref="TextSelector"/> property will be used.
	/// If you need complex sorting, pre-sort data manually or create a custom comparable property.
	/// </summary>
	[Parameter] public Func<TItem, IComparable> SortKeySelector { get; set; }

	/// <summary>
	/// When <c>true</c>, items are sorted before displaying in select.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool AutoSort { get; set; } = true;

	/// <summary>
	/// Text to display when the selection is empty (the Value property is null or empty).
	/// </summary>
	[Parameter] public string EmptyText { get; set; }

	/// <summary>
	/// Text text to display after <see cref="AndMoreAfter" /> items count.
	/// Formatted with count of more items.
	/// </summary>
	[Parameter] public string AndMoreText { get; set; }

	/// <summary>
	/// This is the maximum number of items. Next items are shown using <see cref="AndMoreText"/>.
	/// </summary>
	[Parameter] public int AndMoreAfter { get; set; } = 3;

	[Inject] private IStringLocalizer<HxMultiSelect> StringLocalizer { get; set; }

	private List<TItem> itemsToRender;
	private HxMultiSelectInternal<TValue, TItem> hxMultiSelectInternalComponent;

	private void RefreshState()
	{
		itemsToRender = Data?.ToList() ?? new List<TItem>();

		// AutoSort
		if (AutoSort && (itemsToRender.Count > 1))
		{
			if (SortKeySelector != null)
			{
				itemsToRender = itemsToRender.OrderBy(this.SortKeySelector).ToList();
			}
			else if (TextSelector != null)
			{
				itemsToRender = itemsToRender.OrderBy(this.TextSelector).ToList();
			}
			else
			{
				itemsToRender = itemsToRender.OrderBy(i => i.ToString()).ToList();
			}
		}
	}

	private void HandleItemSelectionChanged(bool @checked, TItem item)
	{
		var newValue = Value == null ? new List<TValue>() : new List<TValue>(Value);
		TValue value = SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item);
		if (@checked)
		{
			newValue.Add(value);
		}
		else
		{
			newValue.Remove(value);
		}

		CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
	}

	protected override bool TryParseValueFromString(string value, out List<TValue> result, out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	/// <summary>
	/// Throws NotSupportedException - giving focus to an input element is not supported on the HxMultiSelect.
	/// </summary>
	public override async ValueTask FocusAsync()
	{
		if (hxMultiSelectInternalComponent == null)
		{
			throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
		}

		await hxMultiSelectInternalComponent.FocusAsync();
	}

	/// <inheritdoc/>
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		RefreshState();

		builder.OpenComponent<HxMultiSelectInternal<TValue, TItem>>(100);
		builder.AddAttribute(101, nameof(HxMultiSelectInternal<TValue, TItem>.InputId), InputId);
		builder.AddAttribute(102, nameof(HxMultiSelectInternal<TValue, TItem>.InputCssClass), GetInputCssClassToRender());
		builder.AddAttribute(103, nameof(HxMultiSelectInternal<TValue, TItem>.InputText), GetInputText());
		builder.AddAttribute(104, nameof(HxMultiSelectInternal<TValue, TItem>.EnabledEffective), EnabledEffective);
		builder.AddAttribute(105, nameof(HxMultiSelectInternal<TValue, TItem>.ItemsToRender), itemsToRender);
		builder.AddAttribute(106, nameof(HxMultiSelectInternal<TValue, TItem>.TextSelector), TextSelector);
		builder.AddAttribute(107, nameof(HxMultiSelectInternal<TValue, TItem>.ValueSelector), ValueSelector);
		builder.AddAttribute(108, nameof(HxMultiSelectInternal<TValue, TItem>.Value), Value);
		builder.AddAttribute(109, nameof(HxMultiSelectInternal<TValue, TItem>.ItemSelectionChanged), EventCallback.Factory.Create<HxMultiSelectInternal<TValue, TItem>.SelectionChangedArgs>(this, args => HandleItemSelectionChanged(args.Checked, args.Item)));
		builder.AddComponentReferenceCapture(110, r => hxMultiSelectInternalComponent = (HxMultiSelectInternal<TValue, TItem>)r);

		builder.CloseComponent();
	}

	private string GetInputText()
	{
		if (!Value?.Any() ?? true)
		{
			return EmptyText;
		}

		// Take itemsToRender because they are sorted.
		List<TItem> selectedItems = itemsToRender.Where(item => Value.Contains(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item))).ToList();

		if (AndMoreAfter == 0)
		{
			return String.Join(", ", selectedItems.Select(TextSelector));
		}
		else
		{
			string result = String.Join(", ", selectedItems.Take(AndMoreAfter).Select(TextSelector));
			int moreItemsCount = selectedItems.Count - AndMoreAfter;

			if (moreItemsCount > 0)
			{
				string text = AndMoreText ?? StringLocalizer["AndMore"];
				result += " " + String.Format(text, moreItemsCount);
			}
			return result;
		}
	}
}
