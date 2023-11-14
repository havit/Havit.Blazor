using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxInputDateRangeInternal : InputBase<DateTimeRange>, IAsyncDisposable, IInputWithSize
{
	[Parameter] public string FromInputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public bool EnabledEffective { get; set; } = true;

	[Parameter] public ValidationMessageMode ValidationMessageModeEffective { get; set; }

	[Parameter] public bool ShowPredefinedDateRangesEffective { get; set; }
	[Parameter] public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRangesEffective { get; set; }

	[Parameter] public string FromParsingErrorMessageEffective { get; set; }

	[Parameter] public string ToParsingErrorMessageEffective { get; set; }

	[Parameter] public bool ShowClearButtonEffective { get; set; } = true;

	[Parameter] public DateTime MinDateEffective { get; set; }

	[Parameter] public DateTime MaxDateEffective { get; set; }

	[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective { get; set; }

	[Parameter] public DateTime FromCalendarDisplayMonth { get; set; }
	[Parameter] public DateTime ToCalendarDisplayMonth { get; set; }

	[Parameter] public TimeProvider TimeProviderEffective { get; set; }

	[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

	private DateTimeRange previousValue;
	private bool fromPreviousParsingAttemptFailed;
	private bool toPreviousParsingAttemptFailed;
	private ValidationMessageStore validationMessageStore;

	private FieldIdentifier fromFieldIdentifier;
	private FieldIdentifier toFieldIdentifier;
	private string[] validationFieldNames;

	private HxDropdownToggleElement fromDropdownToggleElement;
	private HxDropdownToggleElement toDropdownToggleElement;

	private DateTime GetFromCalendarDisplayMonthEffective => CurrentValue.StartDate ?? FromCalendarDisplayMonth;

	private DateTime GetToCalendarDisplayMonthEffective
	{
		get
		{
			if (CurrentValue.EndDate != null)
			{
				return CurrentValue.EndDate.Value;
			}
			if (CurrentValue.StartDate != null && CurrentValue.StartDate != default)
			{
				if (ToCalendarDisplayMonth != default && ToCalendarDisplayMonth > CurrentValue.StartDate)
				{
					return ToCalendarDisplayMonth;
				}
				return CurrentValue.StartDate.Value;
			}
			if (ToCalendarDisplayMonth != default)
			{
				return ToCalendarDisplayMonth;
			}
			return FromCalendarDisplayMonth;
		}
	}

	private bool firstRenderCompleted;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		validationMessageStore ??= new ValidationMessageStore(EditContext);
		fromFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.StartDate));
		toFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.EndDate));
		validationFieldNames ??= new string[] { FieldIdentifier.FieldName, fromFieldIdentifier.FieldName, toFieldIdentifier.FieldName };

		// clear parsing error after new value is set
		if (previousValue != Value)
		{
			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);
			ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);
			previousValue = Value;
		}
	}

	protected override void OnAfterRender(bool firstRender)
	{
		firstRenderCompleted = true;
	}

	protected override bool TryParseValueFromString(string value, out DateTimeRange result, out string validationErrorMessage)
	{
		throw new NotSupportedException();
	}

	private string FormatDate(DateTime? date)
	{
		// no 1.1.0001 etc.
		if (date == null || (date.Value == default))
		{
			return null;
		}

		return date.Value.ToShortDateString();
	}

	private CalendarDateCustomizationResult GetCalendarDateCustomizationFrom(CalendarDateCustomizationRequest request)
	{
		return CalendarDateCustomizationProviderEffective?.Invoke(request with { Target = CalendarDateCustomizationTarget.InputDateRangeFrom }) ?? null;
	}

	private CalendarDateCustomizationResult GetCalendarDateCustomizationTo(CalendarDateCustomizationRequest request)
	{
		return CalendarDateCustomizationProviderEffective?.Invoke(request with { Target = CalendarDateCustomizationTarget.InputDateRangeTo }) ?? null;
	}

	protected void HandleFromChanged(ChangeEventArgs changeEventArgs)
	{
		bool parsingFailed;

		validationMessageStore.Clear(fromFieldIdentifier);

		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var fromDate))
		{
			DateTimeRange newValue = Value with { StartDate = fromDate?.DateTime };

			parsingFailed = false;
			previousValue = newValue;
			CurrentValue = newValue;
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
		}
		else
		{
			parsingFailed = true;
			validationMessageStore.Add(fromFieldIdentifier, FromParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || fromPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			fromPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	protected void HandleToChanged(ChangeEventArgs changeEventArgs)
	{
		bool parsingFailed;
		validationMessageStore.Clear(toFieldIdentifier);

		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var toDate))
		{
			DateTimeRange newValue = Value with { EndDate = toDate?.DateTime };

			parsingFailed = false;
			previousValue = newValue;
			CurrentValue = newValue;
			EditContext.NotifyFieldChanged(toFieldIdentifier);
		}
		else
		{
			parsingFailed = true;
			validationMessageStore.Add(toFieldIdentifier, ToParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || toPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			toPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	private async Task HandleFromClearClickAsync()
	{
		DateTimeRange newValue = Value with { StartDate = null };

		previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(fromFieldIdentifier);
		ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

		await CloseDropdownAsync(fromDropdownToggleElement);
	}

	private async Task HandleToClearClickAsync()
	{
		DateTimeRange newValue = Value with { EndDate = null };

		previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(toFieldIdentifier);
		ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

		await CloseDropdownAsync(toDropdownToggleElement);
	}

	private async Task OpenDropDownAsync(HxDropdownToggleElement triggerElement)
	{
		Contract.Assert<ArgumentNullException>(triggerElement != null, nameof(triggerElement));
		await triggerElement.ShowAsync();
	}

	private async Task CloseDropdownAsync(HxDropdownToggleElement triggerElement)
	{
		Contract.Assert<ArgumentNullException>(triggerElement != null, nameof(triggerElement));
		await triggerElement.HideAsync();
	}

	private async Task HandleFromCalendarValueChangedAsync(DateTime? date)
	{
		DateTimeRange newValue = Value with { StartDate = date };

		previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(fromFieldIdentifier);
		ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

		await CloseDropdownAsync(fromDropdownToggleElement);
		await OpenDropDownAsync(toDropdownToggleElement);
	}

	private async Task HandleToCalendarValueChanged(DateTime? date)
	{
		DateTimeRange newValue = Value with { EndDate = date };

		previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(toFieldIdentifier);
		ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

		await CloseDropdownAsync(toDropdownToggleElement);
	}

	protected async Task HandleDateRangeClick(DateTimeRange value, HxDropdownToggleElement dropdownElement)
	{
		// previousValue does not need to be set
		CurrentValue = value;
		EditContext.NotifyFieldChanged(fromFieldIdentifier);
		EditContext.NotifyFieldChanged(toFieldIdentifier);

		ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);
		ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

		await CloseDropdownAsync(dropdownElement);
	}

	private void ClearPreviousParsingMessage(ref bool previousParsingAttemptFailed, FieldIdentifier fieldIdentifier)
	{
		if (previousParsingAttemptFailed)
		{
			previousParsingAttemptFailed = false;
			validationMessageStore.Clear(fieldIdentifier);
			EditContext.NotifyValidationStateChanged();
		}
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		validationMessageStore?.Clear();

		if (firstRenderCompleted)
		{
			try
			{
				if (fromDropdownToggleElement is not null)
				{
					await CloseDropdownAsync(fromDropdownToggleElement);
				}
				if (toDropdownToggleElement is not null)
				{
					await CloseDropdownAsync(toDropdownToggleElement);
				}
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}

		Dispose(false);
	}
}
