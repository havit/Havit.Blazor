using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Renders a multi-selection list of <see cref="HxCheckbox"/> controls.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckboxList">https://havit.blazor.eu/components/HxCheckboxList</see>
/// </summary>
public class HxCheckboxList<TValue, TItem> : HxInputBase<List<TValue>> // cannot use an array: https://github.com/dotnet/aspnetcore/issues/15014
{
	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override CheckboxListSettings GetDefaults() => HxCheckboxList.Defaults;

	/// <summary>
	/// Items to display. 
	/// </summary>
	[Parameter] public IEnumerable<TItem> Data { get; set; }

	/// <summary>
	/// Selects the text to display from the item.
	/// When not set, <c>ToString()</c> is used.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTextSelector { get; set; }

	/// <summary>
	/// Selects the value from the item.
	/// Not required when TValue is the same as TItem.
	/// </summary>
	[Parameter] public Func<TItem, TValue> ItemValueSelector { get; set; }

	/// <summary>
	/// Selects the value for item sorting. When not set, the <see cref="ItemTextSelector"/> property will be used.
	/// If you need complex sorting, pre-sort the data manually or create a custom comparable property.
	/// </summary>
	[Parameter] public Func<TItem, IComparable> ItemSortKeySelector { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the underlying <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemCssClassSelector { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the input element of the <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public string ItemInputCssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the input element of the <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemInputCssClassSelector { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the text of the <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public string ItemTextCssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the text of the <see cref="HxCheckbox" />.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTextCssClassSelector { get; set; }

	/// <summary>
	/// When <c>true</c>, items are sorted before displaying in the select.
	/// The default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool AutoSort { get; set; } = true;

	/// <summary>
	/// Allows grouping checkboxes on the same horizontal row by rendering them inline. The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool Inline { get; set; }

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputDate.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public CheckboxListSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	protected override CheckboxListSettings GetSettings() => Settings;

	private List<TItem> _itemsToRender;

	private void RefreshState()
	{
		_itemsToRender = Data?.ToList() ?? new List<TItem>();

		// AutoSort
		if (AutoSort && (_itemsToRender.Count > 1))
		{
			if (ItemSortKeySelector != null)
			{
				_itemsToRender = _itemsToRender.OrderBy(ItemSortKeySelector).ToList();
			}
			else if (ItemTextSelector != null)
			{
				_itemsToRender = _itemsToRender.OrderBy(ItemTextSelector).ToList();
			}
			else
			{
				_itemsToRender = _itemsToRender.OrderBy(i => i.ToString()).ToList();
			}
		}
	}

	/// <summary>
	/// Checkbox render mode.
	/// </summary>
	[Parameter] public CheckboxListRenderMode RenderMode { get; set; } = CheckboxListRenderMode.Checkboxes;

	/// <summary>
	/// Color for <see cref="CheckboxListRenderMode.ToggleButtons"/>.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor? ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color; // can be null, HxCheckbox.Color remains unset

	/// <summary>
	/// Indicates whether to use <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" buttons</see>.
	/// for <see cref="CheckboxListRenderMode.ToggleButtons"/> and <see cref="CheckboxListRenderMode.ButtonGroup"/>.
	/// </summary>
	[Parameter] public bool? Outline { get; set; }
	protected bool? OutlineEffective => Outline ?? GetSettings()?.Outline ?? GetDefaults().Outline; // can be null, HxCheckbox.Outline remains unset

	/// <inheritdoc/>
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		RefreshState();

		if (_itemsToRender.Count > 0)
		{
			builder.OpenElement(0, "div");
			builder.AddAttribute(1, "aria-labelledby", InputId);
			builder.AddAttribute(2, "class", InputCssClass);
			builder.OpenRegion(3);

			if (RenderMode == CheckboxListRenderMode.ButtonGroup)
			{
				builder.OpenComponent(1, typeof(HxButtonGroup));
				builder.AddAttribute(2, nameof(HxButtonGroup.Orientation), Inline ? ButtonGroupOrientation.Horizontal : ButtonGroupOrientation.Vertical);
				builder.AddAttribute(3, nameof(HxButtonGroup.ChildContent), (RenderFragment)BuildRenderInputItems);
				builder.CloseComponent();
			}
			else if (RenderMode == CheckboxListRenderMode.ToggleButtons)
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", Inline ? "d-flex gap-1" : "d-inline-flex flex-column gap-1");
				BuildRenderInputItems(builder);
				builder.CloseElement();
			}
			else
			{
				BuildRenderInputItems(builder);
			}

			builder.CloseRegion();
			builder.CloseElement();
		}
	}

	private void BuildRenderInputItems(RenderTreeBuilder builder)
	{
		UglyHack uglyHack = new UglyHack(); // see comment below

		CheckboxRenderMode checkboxRenderMode = GetCheckboxRenderMode();

		foreach (var item in _itemsToRender)
		{
			TValue value = SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelector, item);

			builder.OpenComponent(1, typeof(HxCheckbox));

			builder.AddAttribute(2, nameof(HxCheckbox.Text), SelectorHelpers.GetText(ItemTextSelector, item));
			builder.AddAttribute(3, nameof(HxCheckbox.Value), Value?.Contains(value) ?? false);
			builder.AddAttribute(4, nameof(HxCheckbox.ValueChanged), EventCallback.Factory.Create<bool>(this, @checked => HandleValueChanged(@checked, item)));
			builder.AddAttribute(5, nameof(HxCheckbox.Enabled), EnabledEffective);

			builder.AddAttribute(6, nameof(HxCheckbox.CssClass), CssClassHelper.Combine(ItemCssClass, ItemCssClassSelector?.Invoke(item)));
			builder.AddAttribute(7, nameof(HxCheckbox.InputCssClass), (RenderMode == CheckboxListRenderMode.ButtonGroup)
				? ItemInputCssClassSelector?.Invoke(item)
				: CssClassHelper.Combine(ItemInputCssClass, ItemInputCssClassSelector?.Invoke(item)));
			builder.AddAttribute(8, nameof(HxCheckbox.TextCssClass), CssClassHelper.Combine(ItemTextCssClass, ItemTextCssClassSelector?.Invoke(item)));

			builder.AddAttribute(9, nameof(HxCheckbox.RenderMode), checkboxRenderMode);
			builder.AddAttribute(10, nameof(HxCheckbox.Color), ColorEffective);
			builder.AddAttribute(11, nameof(HxCheckbox.Outline), OutlineEffective);

			// We need ValueExpression. Ehm, HxCheckbox needs ValueExpression. Because it is InputBase<T> which needs ValueExpression.
			// We have nothing to give the HxCheckbox. So we make own class with property which we assign to the ValueExpression.
			// Impacts? Unknown. Maybe none.
			builder.AddAttribute(50, nameof(HxCheckbox.ValueExpression), (Expression<Func<bool>>)(() => uglyHack.HackProperty));

			builder.AddAttribute(51, nameof(HxCheckbox.ValidationMessageMode), Havit.Blazor.Components.Web.Bootstrap.ValidationMessageMode.None);
			builder.AddAttribute(52, nameof(HxCheckbox.Inline), Inline);
			builder.AddAttribute(53, nameof(HxCheckbox.GenerateChip), false);

			builder.AddMultipleAttributes(100, AdditionalAttributes);

			builder.CloseComponent();
		}
	}

	private CheckboxRenderMode GetCheckboxRenderMode()
	{
		return RenderMode switch
		{
			CheckboxListRenderMode.Checkboxes => CheckboxRenderMode.Checkbox,
			CheckboxListRenderMode.Switches => CheckboxRenderMode.Switch,
			CheckboxListRenderMode.NativeSwitches => CheckboxRenderMode.NativeSwitch,
			CheckboxListRenderMode.ToggleButtons => CheckboxRenderMode.ToggleButton,
			CheckboxListRenderMode.ButtonGroup => CheckboxRenderMode.ToggleButton, // return (checkbox render mode) ToggleButton for (a checkboxlist render mode) ButtonGroup
			_ => throw new InvalidOperationException(RenderMode.ToString())
		};
	}

	private void HandleValueChanged(bool @checked, TItem item)
	{
		var newValue = Value == null ? new List<TValue>() : new List<TValue>(Value);
		TValue value = SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelector, item);
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

	private class UglyHack
	{
		public bool HackProperty { get; set; }
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(List<TValue> value)
	{
		// Used for CurrentValueAsString (which is used for the chip generator).
		if ((!value?.Any() ?? true) || (Data == null))
		{
			// don't care about chip generator, it does not call this method for null/empty value
			return String.Empty;
		}

		// Take itemsToRender because they are sorted.
		List<TItem> selectedItems = _itemsToRender.Where(item => value.Contains(SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelector, item))).ToList();
		return String.Join(", ", selectedItems.Select(ItemTextSelector));
	}

	/// <inheritdoc />
	protected override bool ShouldRenderChipGenerator()
	{
		return CurrentValue?.Any() ?? false;
	}

	/// <inheritdoc />
	protected override List<TValue> GetChipRemoveValue()
	{
		return new List<TValue>();
	}
}