using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Date range picker. Form input component for entering a start date and an end date.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputDateRange">https://havit.blazor.eu/components/HxInputDateRange</see>
/// </summary>
public class HxInputDateRange : HxInputBase<DateTimeRange>, IInputWithSize
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputDateRange"/> component.
	/// </summary>
	public static InputDateRangeSettings Defaults { get; set; }

	static HxInputDateRange()
	{
		Defaults = new InputDateRangeSettings()
		{
			MinDate = HxCalendar.DefaultMinDate,
			MaxDate = HxCalendar.DefaultMaxDate,
			ShowClearButton = true,
			ShowPredefinedDateRanges = true,
			PredefinedDateRanges = null,
		};
	}

	/// <summary>
	/// Returns the application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected override InputDateRangeSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputDateRangeSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputDateRangeSettings GetSettings() => Settings;

	/// <summary>
	/// When enabled (default is <c>true</c>), shows predefined days (from <see cref="PredefinedDateRanges"/>, e.g. Today).
	/// </summary>
	[Parameter] public bool? ShowPredefinedDateRanges { get; set; }
	protected bool ShowPredefinedDateRangesEffective => ShowPredefinedDateRanges ?? Settings?.ShowPredefinedDateRanges ?? GetDefaults().ShowPredefinedDateRanges ?? throw new InvalidOperationException(nameof(ShowPredefinedDateRanges) + " default for " + nameof(HxInputDateRange) + " has to be set.");

	/// <summary>
	/// Predefined dates to be displayed.
	/// </summary>
	[Parameter] public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRanges { get; set; }
	private IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRangesEffective => PredefinedDateRanges ?? GetSettings()?.PredefinedDateRanges ?? GetDefaults().PredefinedDateRanges;

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize;
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <summary>
	/// Placeholder for the start-date input.
	/// If not set, localized default is used ("From" + localizations).
	/// </summary>
	[Parameter] public string FromPlaceholder { get; set; }
	public string FromPlaceholderEffective => FromPlaceholder ?? GetSettings()?.FromPlaceholder ?? GetDefaults().FromPlaceholder; // null = use localizations

	/// <summary>
	/// Placeholder for the end-date input.
	/// If not set, localized default is used ("End" + localizations).
	/// </summary>
	[Parameter] public string ToPlaceholder { get; set; }
	public string ToPlaceholderEffective => ToPlaceholder ?? GetSettings()?.ToPlaceholder ?? GetDefaults().ToPlaceholder; // null = use localizations

	/// <summary>
	/// Gets or sets the error message used when displaying a &quot;from&quot; parsing error.
	/// Used with <c>String.Format(...)</c>, <c>{0}</c> is replaced by the Label property, <c>{1}</c> is replaced by the name of the bounded property.
	/// </summary>
	[Parameter] public string FromParsingErrorMessage { get; set; }

	/// <summary>
	/// Gets or sets the error message used when displaying a &quot;to&quot; parsing error.
	/// Used with <c>String.Format(...)</c>, <c>{0}</c> is replaced by the Label property, <c>{1}</c> is replaced by the name of the bounded property.
	/// </summary>
	[Parameter] public string ToParsingErrorMessage { get; set; }

	/// <summary>
	/// Indicates whether the <i>Clear</i> button in the dropdown calendar should be visible.<br/>
	/// The default is <c>true</c> (configurable in <see cref="HxInputDate.Defaults"/>).
	/// </summary>
	[Parameter] public bool? ShowClearButton { get; set; }
	protected bool ShowClearButtonEffective => ShowClearButton ?? GetSettings()?.ShowClearButton ?? GetDefaults().ShowClearButton ?? throw new InvalidOperationException(nameof(ShowClearButton) + " default for " + nameof(HxInputDateRange) + " has to be set.");

	/// <summary>
	/// The first date selectable from the dropdown calendar.<br />
	/// The default is <c>1.1.1900</c> (configurable from <see cref="HxInputDateRange.Defaults"/>).
	/// </summary>
	[Parameter] public DateTime? MinDate { get; set; }
	protected DateTime MinDateEffective => MinDate ?? GetSettings()?.MinDate ?? GetDefaults().MinDate ?? throw new InvalidOperationException(nameof(MinDate) + " default for " + nameof(HxInputDateRange) + " has to be set.");

	/// <summary>
	/// The last date selectable from the dropdown calendar.<br />
	/// The default is <c>31.12.2099</c> (configurable from <see cref="HxInputDateRange.Defaults"/>).
	/// </summary>
	[Parameter] public DateTime? MaxDate { get; set; }
	protected DateTime MaxDateEffective => MaxDate ?? GetSettings()?.MaxDate ?? GetDefaults().MaxDate ?? throw new InvalidOperationException(nameof(MaxDate) + " default for " + nameof(HxInputDateRange) + " has to be set.");

	/// <summary>
	/// Allows customization of the dates in the dropdown calendars.<br />
	/// The default customization is configurable with <see cref="HxInputDateRange.Defaults"/>.
	/// </summary>
	[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProvider { get; set; }
	protected CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective => CalendarDateCustomizationProvider ?? GetSettings()?.CalendarDateCustomizationProvider ?? GetDefaults().CalendarDateCustomizationProvider;

	/// <summary>
	/// The month to display in the from calendar when no start date is selected.
	/// </summary>
	[Parameter] public DateTime FromCalendarDisplayMonth { get; set; }
	/// <summary>
	/// The month to display in the to calendar when no end date or start date is selected. It will default to <see cref="HxInputDateRange.FromCalendarDisplayMonth"/>.
	/// </summary>
	[Parameter] public DateTime ToCalendarDisplayMonth { get; set; }

	[Inject] protected TimeProvider TimeProviderFromServices { get; set; }

	/// <summary>
	/// TimeProvider is resolved in the following order:<br />
	///		1. TimeProvider from this parameter <br />
	///		2. Settings TimeProvider (configurable from <see cref="HxInputDateRange.Settings"/>)<br />
	///		3. Defaults TimeProvider (configurable from <see cref="HxInputDateRange.Defaults"/>)<br />
	///		4. TimeProvider from DependencyInjection<br />
	/// </summary>
	[Parameter] public TimeProvider TimeProvider { get; set; } = null;
	protected TimeProvider TimeProviderEffective => TimeProvider ?? GetSettings()?.TimeProvider ?? GetDefaults().TimeProvider ?? TimeProviderFromServices;

	[Parameter] public IconBase CalendarIcon { get; set; }
	protected IconBase CalendarIconEffective => CalendarIcon ?? GetSettings()?.CalendarIcon ?? GetDefaults().CalendarIcon;

	[Inject] private IStringLocalizer<HxInputDateRange> StringLocalizer { get; set; }


	private HxInputDateRangeInternal _hxInputDateRangeInternalComponent;

	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		RenderWithAutoCreatedEditContextAsCascadingValue(builder, 0, BuildRenderInputCore);
	}

	protected virtual void BuildRenderInputCore(RenderTreeBuilder builder)
	{
		builder.OpenComponent(1, typeof(HxInputDateRangeInternal));

		builder.AddAttribute(100, nameof(HxInputDateRangeInternal.CurrentValue), Value);
		builder.AddAttribute(101, nameof(HxInputDateRangeInternal.CurrentValueChanged), EventCallback.Factory.Create<DateTimeRange>(this, value => CurrentValue = value));

		builder.AddAttribute(110, nameof(HxInputDateRangeInternal.EditContext), EditContext);
		builder.AddAttribute(111, nameof(HxInputDateRangeInternal.FieldIdentifier), FieldIdentifier);

		builder.AddAttribute(200, nameof(HxInputDateRangeInternal.FromInputId), InputId);
		builder.AddAttribute(201, nameof(HxInputDateRangeInternal.InputCssClass), GetInputCssClassToRender());
		builder.AddAttribute(203, nameof(HxInputDateRangeInternal.InputSizeEffective), InputSizeEffective);
		builder.AddAttribute(204, nameof(HxInputDateRangeInternal.CalendarIconEffective), CalendarIconEffective);
		builder.AddAttribute(205, nameof(HxInputDateRangeInternal.EnabledEffective), EnabledEffective);
		builder.AddAttribute(206, nameof(HxInputDateRangeInternal.FromPlaceholderEffective), FromPlaceholderEffective);
		builder.AddAttribute(206, nameof(HxInputDateRangeInternal.ToPlaceholderEffective), ToPlaceholderEffective);
		builder.AddAttribute(206, nameof(HxInputDateRangeInternal.FromParsingErrorMessageEffective), GetFromParsingErrorMessage());
		builder.AddAttribute(207, nameof(HxInputDateRangeInternal.ToParsingErrorMessageEffective), GetToParsingErrorMessage());
		builder.AddAttribute(208, nameof(HxInputDateRangeInternal.ValidationMessageModeEffective), ValidationMessageModeEffective);
		builder.AddAttribute(209, nameof(HxInputDateRangeInternal.PredefinedDateRangesEffective), PredefinedDateRangesEffective);
		builder.AddAttribute(210, nameof(HxInputDateRangeInternal.ShowPredefinedDateRangesEffective), ShowPredefinedDateRangesEffective);
		builder.AddAttribute(211, nameof(HxInputDateRangeInternal.ShowClearButtonEffective), ShowClearButtonEffective);
		builder.AddAttribute(212, nameof(HxInputDateRangeInternal.MinDateEffective), MinDateEffective);
		builder.AddAttribute(213, nameof(HxInputDateRangeInternal.MaxDateEffective), MaxDateEffective);
		builder.AddAttribute(214, nameof(HxInputDateRangeInternal.FromCalendarDisplayMonth), FromCalendarDisplayMonth);
		builder.AddAttribute(215, nameof(HxInputDateRangeInternal.ToCalendarDisplayMonth), ToCalendarDisplayMonth);
		builder.AddAttribute(220, nameof(HxInputDateRangeInternal.CalendarDateCustomizationProviderEffective), CalendarDateCustomizationProviderEffective);
		builder.AddAttribute(221, nameof(HxInputDateRangeInternal.TimeProviderEffective), TimeProviderEffective);

		builder.AddMultipleAttributes(300, AdditionalAttributes);
		builder.AddComponentReferenceCapture(400, (__value) => _hxInputDateRangeInternalComponent = (HxInputDateRangeInternal)__value);

		builder.CloseComponent();
	}

	public async ValueTask FocusAsync()
	{
		if (_hxInputDateRangeInternalComponent is null)
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The component reference is not available. You are most likely calling the method too early. The first render must complete before calling this method.");
		}

		await _hxInputDateRangeInternalComponent.FocusAsync();
	}

	protected override void BuildRenderValidationMessage(RenderTreeBuilder builder)
	{
		// NOOP
	}

	private protected override void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
	{
		throw new NotSupportedException();
	}

	// For generating chips
	/// <inheritdocs />
	protected override string FormatValueAsString(DateTimeRange value)
	{
		string from = null;
		string to = null;

		if (value.StartDate != null)
		{
			from = StringLocalizer["From"] + " " + value.StartDate.Value.ToShortDateString();
		}

		if (value.EndDate != null)
		{
			to = StringLocalizer["To"] + " " + value.EndDate.Value.ToShortDateString();
		}

		return String.Join(" ", from, to);
	}

	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out DateTimeRange result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	/// <summary>
	/// Returns message for &quot;from&quot; parsing error.
	/// </summary>
	protected virtual string GetFromParsingErrorMessage()
	{
		var message = !String.IsNullOrEmpty(FromParsingErrorMessage)
			? FromParsingErrorMessage
			: StringLocalizer["FromParsingErrorMessage"];
		return String.Format(message, DisplayName ?? Label ?? FieldIdentifier.FieldName);
	}

	/// <summary>
	/// Returns message for &quot;to&quot;  parsing error.
	/// </summary>
	protected virtual string GetToParsingErrorMessage()
	{
		var message = !String.IsNullOrEmpty(ToParsingErrorMessage)
			? ToParsingErrorMessage
			: StringLocalizer["ToParsingErrorMessage"];
		return String.Format(message, DisplayName ?? Label ?? FieldIdentifier.FieldName);
	}
}