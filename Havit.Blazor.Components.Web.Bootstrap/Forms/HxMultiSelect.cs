using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// MultiSelect. Unlike a normal select, multiselect allows the user to select multiple options at once.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMultiSelect">https://havit.blazor.eu/components/HxMultiSelect</see>
/// </summary>
/// <typeparam name="TValue">Type of values.</typeparam>
/// <typeparam name="TItem">Type of items.</typeparam>
public class HxMultiSelect<TValue, TItem> : HxInputBase<List<TValue>>, IInputWithSize
{
	/// <summary>
	/// Return <see cref="HxMultiSelect{TValue, TItem}"/> defaults.
	/// Enables to not share defaults in descendants with base classes.
	/// Enables to have multiple descendants which differs in the default values.
	/// </summary>
	protected override MultiSelectSettings GetDefaults() => HxMultiSelect.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxMultiSelect.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public MultiSelectSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override MultiSelectSettings GetSettings() => this.Settings;

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => this.InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxMultiSelect) + " has to be set.");
	InputSize IInputWithSize.InputSizeEffective => this.InputSizeEffective;
	string IInputWithSize.GetInputSizeCssClass() => this.InputSizeEffective.AsFormSelectCssClass();

	private protected override string CoreInputCssClass => "hx-multi-select-input form-select user-select-none";

	/// <summary>
	/// Items to display. 
	/// </summary>
	[Parameter] public IEnumerable<TItem> Data { get; set; }

	/// <summary>
	/// Selects text to display from item.<br/>
	/// When not set, <c>ToString()</c> is used.
	/// </summary>
	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	/// <summary>
	/// Selects value from item.<br/>
	/// Not required when <c>TValue</c> is same as <c>TItem</c>.
	/// </summary>
	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	/// <summary>
	/// Selects value for items sorting. When not set, <see cref="TextSelector"/> property will be used.<br/>
	/// If you need complex sorting, pre-sort data manually or create a custom comparable property.
	/// </summary>
	[Parameter] public Func<TItem, IComparable> SortKeySelector { get; set; }

	/// <summary>
	/// When set to <c>false</c>, items will no longer be sorted.<br/>
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool AutoSort { get; set; } = true;

	/// <summary>
	/// Text to display when the selection is empty (the <c>Value</c> property is <c>null</c> or empty).
	/// </summary>
	[Parameter] public string EmptyText { get; set; }

	/// <summary>
	/// Text to display when <see cref="Data"/> is <c>null</c>.
	/// </summary>
	[Parameter] public string NullDataText { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// Input-group at the end of the input.
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// Text to display in the input (default is a list of selected values).
	/// </summary>
	[Parameter] public string InputText { get; set; }

	/// <summary>
	/// Function to build the text to be displayed in the input from selected items (default is a list of selected values).
	/// </summary>
	/// <remarks>Currently does not affect the chip being generated. Override <c>RenderChipValue()</c> method to influence the chip.</remarks>
	[Parameter] public Func<IEnumerable<TItem>, string> InputTextSelector { get; set; }

	/// <summary>
	/// Input-group at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	/// <summary>
	/// Enables filtering capabilities.
	/// </summary>
	[Parameter] public bool? AllowFiltering { get; set; }
	protected bool AllowFilteringEffective => this.AllowFiltering ?? this.GetSettings()?.AllowFiltering ?? GetDefaults().AllowFiltering;

	/// <summary>
	/// Defines a custom filtering predicate to apply to the list of items.
	/// If not specified, the default behavior filters items based on whether the item text (obtained via TextSelector) contains the filter query string.
	/// </summary>
	[Parameter] public Func<TItem, string, bool> FilterPredicate { get; set; }

	/// <summary>
	/// When enabled the filter will be cleared when the dropdown is closed.
	/// </summary>
	[Parameter] public bool? ClearFilterOnHide { get; set; }
	protected bool ClearFilterOnHideEffective => this.ClearFilterOnHide ?? this.GetSettings()?.ClearFilterOnHide ?? GetDefaults().ClearFilterOnHide;

	/// <summary>
	/// Template that defines what should be rendered in case of empty items.
	/// </summary>
	[Parameter] public RenderFragment FilterEmptyResultTemplate { get; set; }

	/// <summary>
	/// Text to display when the filtered results list is empty and when not using <see cref="FilterEmptyResultTemplate"/>.
	/// </summary>
	[Parameter] public string FilterEmptyResultText { get; set; }

	/// <summary>
	/// Enables select all capabilities.
	/// </summary>
	[Parameter] public bool? AllowSelectAll { get; set; }
	protected bool AllowSelectAllEffective => this.AllowSelectAll ?? this.GetSettings()?.AllowSelectAll ?? GetDefaults().AllowSelectAll;

	/// <summary>
	/// Text to display for the select all dropdown option.
	/// </summary>
	[Parameter] public string SelectAllText { get; set; }

	/// <summary>
	/// Icon displayed in filter input for searching the filter.
	/// </summary>
	[Parameter] public IconBase FilterSearchIcon { get; set; }
	protected IconBase FilterSearchIconEffective => this.FilterSearchIcon ?? this.GetSettings()?.FilterSearchIcon ?? GetDefaults().FilterSearchIcon;

	/// <summary>
	/// Icon displayed in filter input for clearing the filter.
	/// </summary>
	[Parameter] public IconBase FilterClearIcon { get; set; }
	protected IconBase FilterClearIconEffective => this.FilterClearIcon ?? this.GetSettings()?.FilterClearIcon ?? GetDefaults().FilterClearIcon;

	private List<TItem> itemsToRender;
	private HxMultiSelectInternal<TValue, TItem> hxMultiSelectInternalComponent;

	private void RefreshState()
	{
		itemsToRender = Data?.ToList();

		// AutoSort
		if (AutoSort && (itemsToRender?.Count > 1))
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

	protected override bool TryParseValueFromString(string value, out List<TValue> result, out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc cref="HxInputBase{TValue}.FocusAsync"/>
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
		builder.AddAttribute(108, nameof(HxMultiSelectInternal<TValue, TItem>.SelectedValues), Value);
		builder.AddAttribute(101, nameof(HxMultiSelectInternal<TValue, TItem>.SelectedValuesChanged), EventCallback.Factory.Create<List<TValue>>(this, value => CurrentValue = value));
		builder.AddAttribute(109, nameof(HxMultiSelectInternal<TValue, TItem>.NullDataText), NullDataText);
		builder.AddAttribute(111, nameof(HxMultiSelectInternal<TValue, TItem>.InputGroupStartText), InputGroupStartText);
		builder.AddAttribute(112, nameof(HxMultiSelectInternal<TValue, TItem>.InputGroupStartTemplate), InputGroupStartTemplate);
		builder.AddAttribute(113, nameof(HxMultiSelectInternal<TValue, TItem>.InputGroupEndText), InputGroupEndText);
		builder.AddAttribute(114, nameof(HxMultiSelectInternal<TValue, TItem>.InputGroupEndTemplate), InputGroupEndTemplate);
		builder.AddAttribute(115, nameof(HxMultiSelectInternal<TValue, TItem>.AllowFiltering), AllowFilteringEffective);
		builder.AddAttribute(116, nameof(HxMultiSelectInternal<TValue, TItem>.FilterPredicate), FilterPredicate);
		builder.AddAttribute(117, nameof(HxMultiSelectInternal<TValue, TItem>.ClearFilterOnHide), ClearFilterOnHideEffective);
		builder.AddAttribute(118, nameof(HxMultiSelectInternal<TValue, TItem>.FilterEmptyResultTemplate), FilterEmptyResultTemplate);
		builder.AddAttribute(119, nameof(HxMultiSelectInternal<TValue, TItem>.FilterEmptyResultText), FilterEmptyResultText);
		builder.AddAttribute(120, nameof(HxMultiSelectInternal<TValue, TItem>.AllowSelectAll), AllowSelectAllEffective);
		builder.AddAttribute(121, nameof(HxMultiSelectInternal<TValue, TItem>.SelectAllText), SelectAllText);
		builder.AddAttribute(122, nameof(HxMultiSelectInternal<TValue, TItem>.FilterSearchIcon), FilterSearchIconEffective);
		builder.AddAttribute(123, nameof(HxMultiSelectInternal<TValue, TItem>.FilterClearIcon), FilterClearIconEffective);
		builder.AddAttribute(124, nameof(HxMultiSelectInternal<TValue, TItem>.InputSizeEffective), ((IInputWithSize)this).InputSizeEffective);

		builder.AddMultipleAttributes(200, this.AdditionalAttributes);

		builder.AddComponentReferenceCapture(300, r => hxMultiSelectInternalComponent = (HxMultiSelectInternal<TValue, TItem>)r);

		builder.CloseComponent();
	}

	private string GetInputText()
	{
		if (!string.IsNullOrEmpty(InputText))
		{
			return InputText;
		}

		if ((InputTextSelector is null) || (Data is null) || (CurrentValue is null))
		{
			return CurrentValueAsString;
		}

		var currentItems = Data.Where(i => CurrentValue.Contains(SelectorHelpers.GetValue(ValueSelector, i)));
		return SelectorHelpers.GetValue(InputTextSelector, currentItems);
	}

	/// <inheritdoc />
	protected override string FormatValueAsString(List<TValue> value)
	{
		// Used for CurrentValueAsString (which is used for the input element and for the chip generator).
		// That's why we do not use NullDataText here.

		if ((!value?.Any() ?? true) || (Data == null))
		{
			// don't care about chip generator, it does not call this method for null/empty value
			return EmptyText;
		}

		// Take itemsToRender because they are sorted.
		List<TItem> selectedItems = itemsToRender.Where(item => value.Contains(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item))).ToList();
		return String.Join(", ", selectedItems.Select(TextSelector));
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
