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

	[Parameter] public IconBase CalendarIconEffective { get; set; }

	[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private DateTimeRange _previousValue;
	private bool _fromPreviousParsingAttemptFailed;
	private string _incomingFromValueBeforeParsing;
	private bool _toPreviousParsingAttemptFailed;
	private string _incomingToValueBeforeParsing;
	private ValidationMessageStore _validationMessageStore;

	private FieldIdentifier _fromFieldIdentifier;
	private FieldIdentifier _toFieldIdentifier;
	private string[] _validationFieldNames;
	private ElementReference _fromIconWrapperElement;
	private ElementReference _toIconWrapperElement;

	private HxDropdownToggleElement _fromDropdownToggleElement;
	private HxDropdownToggleElement _toDropdownToggleElement;

	private DateTime GetFromCalendarDisplayMonthEffective => CurrentValue.StartDate ?? FromCalendarDisplayMonth;

	private IJSObjectReference _jsModule;

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

	private bool _firstRenderCompleted;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		_validationMessageStore ??= new ValidationMessageStore(EditContext);
		_fromFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.StartDate));
		_toFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.EndDate));
		_validationFieldNames ??= new string[] { FieldIdentifier.FieldName, _fromFieldIdentifier.FieldName, _toFieldIdentifier.FieldName };

		// clear parsing error after new value is set
		if (_previousValue != Value)
		{
			ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
			ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);
			_previousValue = Value;
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		_firstRenderCompleted = true;

		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && (CalendarIconEffective is not null))
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxInputDateRange));
			await _jsModule.InvokeVoidAsync("addOpenAndCloseEventListeners", _fromDropdownToggleElement.ElementReference, (CalendarIconEffective is not null) ? _fromIconWrapperElement : null);
			await _jsModule.InvokeVoidAsync("addOpenAndCloseEventListeners", _toDropdownToggleElement.ElementReference, (CalendarIconEffective is not null) ? _toIconWrapperElement : null);
		}
	}

	public async ValueTask FocusAsync()
	{
		await _fromDropdownToggleElement.ElementReference.FocusAsync();
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

	protected void HandleFromChanged(string newInputValue)
	{
		_incomingFromValueBeforeParsing = newInputValue;
		bool parsingFailed;

		_validationMessageStore.Clear(_fromFieldIdentifier);

		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(newInputValue, null, out var fromDate))
		{
			DateTimeRange newValue = Value with { StartDate = fromDate?.DateTime };

			parsingFailed = false;
			_previousValue = newValue;
			CurrentValue = newValue;
			EditContext.NotifyFieldChanged(_fromFieldIdentifier);
		}
		else
		{
			parsingFailed = true;
			_validationMessageStore.Add(_fromFieldIdentifier, FromParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || _fromPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			_fromPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	protected void HandleToChanged(string newInputValue)
	{
		_incomingToValueBeforeParsing = newInputValue;
		bool parsingFailed;
		_validationMessageStore.Clear(_toFieldIdentifier);

		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(newInputValue, null, out var toDate))
		{
			DateTimeRange newValue = Value with { EndDate = toDate?.DateTime };

			parsingFailed = false;
			_previousValue = newValue;
			CurrentValue = newValue;
			EditContext.NotifyFieldChanged(_toFieldIdentifier);
		}
		else
		{
			parsingFailed = true;
			_validationMessageStore.Add(_toFieldIdentifier, ToParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || _toPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			_toPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	private async Task HandleFromClearClickAsync()
	{
		DateTimeRange newValue = Value with { StartDate = null };

		_previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(_fromFieldIdentifier);
		ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);

		await CloseDropdownAsync(_fromDropdownToggleElement);
	}

	private async Task HandleToClearClickAsync()
	{
		DateTimeRange newValue = Value with { EndDate = null };

		_previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(_toFieldIdentifier);
		ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);

		await CloseDropdownAsync(_toDropdownToggleElement);
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

		_previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(_fromFieldIdentifier);
		ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);

		await CloseDropdownAsync(_fromDropdownToggleElement);
		await OpenDropDownAsync(_toDropdownToggleElement);
	}

	private async Task HandleToCalendarValueChanged(DateTime? date)
	{
		DateTimeRange newValue = Value with { EndDate = date };

		_previousValue = newValue;
		CurrentValue = newValue;
		EditContext.NotifyFieldChanged(_toFieldIdentifier);
		ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);

		await CloseDropdownAsync(_toDropdownToggleElement);
	}

	protected async Task HandleDateRangeClick(DateTimeRange value, HxDropdownToggleElement dropdownElement)
	{
		// previousValue does not need to be set
		CurrentValue = value;
		EditContext.NotifyFieldChanged(_fromFieldIdentifier);
		EditContext.NotifyFieldChanged(_toFieldIdentifier);

		ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
		ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);

		await CloseDropdownAsync(dropdownElement);
	}

	private void ClearPreviousParsingMessage(ref bool previousParsingAttemptFailed, FieldIdentifier fieldIdentifier)
	{
		if (previousParsingAttemptFailed)
		{
			previousParsingAttemptFailed = false;
			_validationMessageStore.Clear(fieldIdentifier);
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
		_validationMessageStore?.Clear();

		if (_firstRenderCompleted)
		{
			try
			{
				if (_fromDropdownToggleElement is not null)
				{
					await CloseDropdownAsync(_fromDropdownToggleElement);
				}
				if (_toDropdownToggleElement is not null)
				{
					await CloseDropdownAsync(_toDropdownToggleElement);
				}
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			catch (TaskCanceledException)
			{
				// NOOP
			}
		}

		Dispose(false);
	}
}
