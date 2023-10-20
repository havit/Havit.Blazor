using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component for single item selection with dynamic suggestions (based on typed characters).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAutosuggest">https://havit.blazor.eu/components/HxAutosuggest</see>
/// </summary>
/// <remarks>
/// Defaults located in separate non-generic type <see cref="HxAutosuggest"/>.
/// </remarks>
public class HxAutosuggest<TItem, TValue> : HxInputBase<TValue>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
{
	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override AutosuggestSettings GetDefaults() => HxAutosuggest.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxAutosuggest.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public AutosuggestSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override AutosuggestSettings GetSettings() => this.Settings;


	/// <summary>
	/// Method (delegate) which provides data of the suggestions.
	/// </summary>
	[Parameter] public AutosuggestDataProviderDelegate<TItem> DataProvider { get; set; }

	/// <summary>
	/// Selects value from item.
	/// Not required when <c>TValue</c> is same as  <c>TItem</c>.
	/// </summary>
	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	/// <summary>
	/// Selects text to display from item.
	/// When not set <c>ToString()</c> is used.
	/// </summary>
	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	/// <summary>
	/// Template to display item.
	/// When not set, <see cref="TextSelector"/> is used.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// Template to display when items are empty.
	/// </summary>
	[Parameter] public RenderFragment EmptyTemplate { get; set; }

	/// <summary>
	/// Icon displayed in input when no item is selected.
	/// </summary>
	[Parameter] public IconBase SearchIcon { get; set; }
	protected IconBase SearchIconEffective => this.SearchIcon ?? this.GetSettings()?.SearchIcon ?? GetDefaults().SearchIcon;

	/// <summary>
	/// Icon displayed in input on selection clear button when item is selected.
	/// </summary>
	[Parameter] public IconBase ClearIcon { get; set; }
	protected IconBase ClearIconEffective => this.ClearIcon ?? this.GetSettings()?.ClearIcon ?? GetDefaults().ClearIcon;

	/// <summary>
	/// Minimal number of characters to start suggesting.
	/// </summary>
	[Parameter] public int? MinimumLength { get; set; }
	protected int MinimumLengthEffective => this.MinimumLength ?? this.GetSettings()?.MinimumLength ?? GetDefaults().MinimumLength ?? throw new InvalidOperationException(nameof(MinimumLength) + " default for " + nameof(HxAutosuggest) + " has to be set.");

	/// <summary>
	/// Debounce delay in milliseconds.
	/// </summary>
	[Parameter] public int? Delay { get; set; }
	protected int DelayEffective => this.Delay ?? this.GetSettings()?.Delay ?? GetDefaults().Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxAutosuggest) + " has to be set.");

	/// <summary>
	/// Short hint displayed in the input field before the user enters a value.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => this.InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxAutosuggest) + " has to be set.");
	InputSize IInputWithSize.InputSizeEffective => this.InputSizeEffective;

	/// <inheritdoc cref="Bootstrap.LabelType" />
	[Parameter] public LabelType? LabelType { get; set; }

	/// <summary>
	/// Offset between the dropdown and the input.
	/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
	/// </summary>
	protected virtual (int Skidding, int Distance) DropdownOffset { get; set; } = (0, 4);

	/// <summary>
	/// Returns corresponding item for (selected) Value.
	/// </summary>
	/// <remarks>
	/// We do not have full list of possible items to be able to select one by value.
	/// </remarks>
	[Parameter] public Func<TValue, Task<TItem>> ItemFromValueResolver { get; set; }

	protected override LabelValueRenderOrder RenderOrder => (LabelType == Bootstrap.LabelType.Floating) ? LabelValueRenderOrder.ValueOnly /* Label rendered by HxAutosuggestInternal */ : LabelValueRenderOrder.LabelValue;

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// Input-group at the end of the input.<br/>
	/// Hides the search icon when used!
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// Input-group at the end of the input.<br/>
	/// Hides the search icon when used!
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	private protected override string CoreInputCssClass => "form-control";
	private protected override string CoreCssClass => "hx-autosuggest position-relative";

	private HxAutosuggestInternal<TItem, TValue> hxAutosuggestInternalComponent;

	/// <inheritdoc cref="ComponentBase.BuildRenderTree(RenderTreeBuilder)" />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		LabelType labelTypeEffective = (this as IInputWithLabelType).LabelTypeEffective;

		builder.OpenComponent<HxAutosuggestInternal<TItem, TValue>>(1);

		builder.AddAttribute(1000, nameof(HxAutosuggestInternal<TItem, TValue>.Value), Value);
		builder.AddAttribute(1001, nameof(HxAutosuggestInternal<TItem, TValue>.ValueChanged), EventCallback.Factory.Create<TValue>(this, HandleValueChanged));
		builder.AddAttribute(1002, nameof(HxAutosuggestInternal<TItem, TValue>.DataProvider), DataProvider);
		builder.AddAttribute(1003, nameof(HxAutosuggestInternal<TItem, TValue>.ValueSelector), ValueSelector);
		builder.AddAttribute(1004, nameof(HxAutosuggestInternal<TItem, TValue>.TextSelector), TextSelector);
		builder.AddAttribute(1005, nameof(HxAutosuggestInternal<TItem, TValue>.ItemTemplate), ItemTemplate);
		builder.AddAttribute(1006, nameof(HxAutosuggestInternal<TItem, TValue>.MinimumLengthEffective), MinimumLengthEffective);
		builder.AddAttribute(1007, nameof(HxAutosuggestInternal<TItem, TValue>.DelayEffective), DelayEffective);
		builder.AddAttribute(1008, nameof(HxAutosuggestInternal<TItem, TValue>.InputId), InputId);
		builder.AddAttribute(1009, nameof(HxAutosuggestInternal<TItem, TValue>.InputCssClass), GetInputCssClassToRender()); // we may render "is-invalid" which has no sense here (there is no invalid-feedback following the element).
		builder.AddAttribute(1010, nameof(HxAutosuggestInternal<TItem, TValue>.EnabledEffective), EnabledEffective);
		builder.AddAttribute(1011, nameof(HxAutosuggestInternal<TItem, TValue>.ItemFromValueResolver), ItemFromValueResolver);
		builder.AddAttribute(1012, nameof(HxAutosuggestInternal<TItem, TValue>.Placeholder), (labelTypeEffective == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating) ? "placeholder" : Placeholder);
		builder.AddAttribute(1013, nameof(HxAutosuggestInternal<TItem, TValue>.LabelTypeEffective), labelTypeEffective);
		builder.AddAttribute(1014, nameof(HxAutosuggestInternal<TItem, TValue>.FormValueComponent), this);
		builder.AddAttribute(1015, nameof(HxAutosuggestInternal<TItem, TValue>.EmptyTemplate), EmptyTemplate);
		builder.AddAttribute(1016, nameof(HxAutosuggestInternal<TItem, TValue>.SearchIconEffective), SearchIconEffective);
		builder.AddAttribute(1017, nameof(HxAutosuggestInternal<TItem, TValue>.ClearIconEffective), ClearIconEffective);
		builder.AddAttribute(1018, nameof(HxAutosuggestInternal<TItem, TValue>.DropdownOffset), DropdownOffset);
		builder.AddAttribute(1021, nameof(HxAutosuggestInternal<TItem, TValue>.InputGroupStartText), this.InputGroupStartText);
		builder.AddAttribute(1023, nameof(HxAutosuggestInternal<TItem, TValue>.InputGroupStartTemplate), this.InputGroupStartTemplate);
		builder.AddAttribute(1024, nameof(HxAutosuggestInternal<TItem, TValue>.InputGroupEndText), this.InputGroupEndText);
		builder.AddAttribute(1025, nameof(HxAutosuggestInternal<TItem, TValue>.InputGroupEndTemplate), this.InputGroupEndTemplate);

		builder.AddMultipleAttributes(2000, this.AdditionalAttributes);

		builder.AddComponentReferenceCapture(3000, component => hxAutosuggestInternalComponent = (HxAutosuggestInternal<TItem, TValue>)component);

		builder.CloseComponent();
	}

	private void HandleValueChanged(TValue newValue)
	{
		CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	/// <inheritdoc />
	public override async ValueTask FocusAsync()
	{
		if (hxAutosuggestInternalComponent == null)
		{
			throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
		}

		await hxAutosuggestInternalComponent.FocusAsync();
	}

	/// <inheritdoc />
	protected override void RenderChipGenerator(RenderTreeBuilder builder)
	{
		if (!String.IsNullOrEmpty(hxAutosuggestInternalComponent?.ChipValue))
		{
			base.RenderChipGenerator(builder);
		}
	}

	/// <inheritdoc />
	protected override void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, hxAutosuggestInternalComponent.ChipValue);
	}
}
