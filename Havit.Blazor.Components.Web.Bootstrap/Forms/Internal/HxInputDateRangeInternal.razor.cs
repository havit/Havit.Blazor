using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxInputDateRangeInternal : ComponentBase, IAsyncDisposable, IInputWithSize
{
	[Parameter] public string FromInputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public DateTimeRange CurrentValue { get; set; }

	[Parameter] public EventCallback<DateTimeRange> CurrentValueChanged { get; set; }

	[Parameter] public EditContext EditContext { get; set; }

	[Parameter] public FieldIdentifier FieldIdentifier { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public bool EnabledEffective { get; set; } = true;

	[Parameter] public ValidationMessageMode ValidationMessageModeEffective { get; set; }

	[Parameter] public bool ShowPredefinedDateRangesEffective { get; set; }
	[Parameter] public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRangesEffective { get; set; }

	[Parameter] public string FromPlaceholderEffective { get; set; }

	[Parameter] public string ToPlaceholderEffective { get; set; }

	[Parameter] public string FromParsingErrorMessageEffective { get; set; }

	[Parameter] public string ToParsingErrorMessageEffective { get; set; }

	[Parameter] public bool RequireDateOrderEffective { get; set; }

	[Parameter] public string DateOrderErrorMessageEffective { get; set; }

	[Parameter] public bool ShowClearButtonEffective { get; set; } = true;

	[Parameter] public DateTime MinDateEffective { get; set; }

	[Parameter] public DateTime MaxDateEffective { get; set; }

	[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective { get; set; }

	[Parameter] public DateTime FromCalendarDisplayMonth { get; set; }
	[Parameter] public DateTime ToCalendarDisplayMonth { get; set; }

	[Parameter] public TimeProvider TimeProviderEffective { get; set; }

	[Parameter] public IconBase CalendarIconEffective { get; set; }

	[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private DateTimeRange _previousValue;
	private bool _fromPreviousParsingAttemptFailed;
	private string _incomingFromValueBeforeParsing;
	private bool _toPreviousParsingAttemptFailed;
	private string _incomingToValueBeforeParsing;
	private ValidationMessageStore _validationMessageStore;
	private bool _previousRangeValidationAttemptFailed;

	private FieldIdentifier _fromFieldIdentifier;
	private FieldIdentifier _toFieldIdentifier;
	private FieldIdentifier[] _validationFieldIdentifiers;
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
		_validationFieldIdentifiers ??= [FieldIdentifier, _fromFieldIdentifier, _toFieldIdentifier];

		// clear parsing error after new value is set
		if (_previousValue != CurrentValue)
		{
			ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
			ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);
			_previousValue = CurrentValue;
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
		await _fromDropdownToggleElement.ShowAsync();
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

	protected async Task HandleFromChangedAsync(string newInputValue)
	{
		_incomingFromValueBeforeParsing = newInputValue;
		bool parsingFailed = false;
		bool rangeValidationFailed = false;

		_validationMessageStore.Clear(_fromFieldIdentifier);
		_validationMessageStore.Clear(FieldIdentifier);

		if (DateHelper.TryParseDateFromString<DateTime?>(newInputValue, TimeProviderEffective, out var fromDate))
		{
			DateTimeRange newValue = CurrentValue with { StartDate = fromDate };

			// Validate the range if required
			if (RequireDateOrderEffective && newValue.StartDate.HasValue && newValue.EndDate.HasValue)
			{
				if (newValue.StartDate.Value > newValue.EndDate.Value)
				{
					rangeValidationFailed = true;
					_validationMessageStore.Add(FieldIdentifier, DateOrderErrorMessageEffective);
				}
			}

			if (!rangeValidationFailed)
			{
				// Only set the value if range validation passes
				parsingFailed = false;
				_previousValue = newValue;
				CurrentValue = newValue;
				await CurrentValueChanged.InvokeAsync(newValue);
				EditContext.NotifyFieldChanged(_fromFieldIdentifier);
				ClearPreviousRangeValidationMessage();
			}
		}
		else
		{
			parsingFailed = true;
			_validationMessageStore.Add(_fromFieldIdentifier, FromParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || rangeValidationFailed || _fromPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			_fromPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	protected async Task HandleToChangedAsync(string newInputValue)
	{
		_incomingToValueBeforeParsing = newInputValue;
		bool parsingFailed = false;
		bool rangeValidationFailed = false;
		_validationMessageStore.Clear(_toFieldIdentifier);
		_validationMessageStore.Clear(FieldIdentifier);

		if (DateHelper.TryParseDateFromString<DateTime?>(newInputValue, TimeProviderEffective, out var toDate))
		{
			DateTimeRange newValue = CurrentValue with { EndDate = toDate };

			// Validate the range if required
			if (RequireDateOrderEffective && newValue.StartDate.HasValue && newValue.EndDate.HasValue && newValue.StartDate.Value > newValue.EndDate.Value)
			{
				rangeValidationFailed = true;
				_validationMessageStore.Add(FieldIdentifier, DateOrderErrorMessageEffective);
			}

			if (!rangeValidationFailed)
			{
				// Only set the value if range validation passes
				parsingFailed = false;
				_previousValue = newValue;
				CurrentValue = newValue;
				await CurrentValueChanged.InvokeAsync(newValue);
				EditContext.NotifyFieldChanged(_toFieldIdentifier);
				ClearPreviousRangeValidationMessage();
			}
		}
		else
		{
			parsingFailed = true;
			_validationMessageStore.Add(_toFieldIdentifier, ToParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || rangeValidationFailed || _toPreviousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			_toPreviousParsingAttemptFailed = parsingFailed;
		}
	}

	private async Task HandleFromClearClickAsync()
	{
		DateTimeRange newValue = CurrentValue with { StartDate = null };

		_previousValue = newValue;
		CurrentValue = newValue;
		await CurrentValueChanged.InvokeAsync(newValue);
		EditContext.NotifyFieldChanged(_fromFieldIdentifier);
		ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
		ClearPreviousRangeValidationMessage();

		await CloseDropdownAsync(_fromDropdownToggleElement);
	}

	private async Task HandleToClearClickAsync()
	{
		DateTimeRange newValue = CurrentValue with { EndDate = null };

		_previousValue = newValue;
		CurrentValue = newValue;
		await CurrentValueChanged.InvokeAsync(newValue);
		EditContext.NotifyFieldChanged(_toFieldIdentifier);
		ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);
		ClearPreviousRangeValidationMessage();

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
		_validationMessageStore.Clear(FieldIdentifier);

		DateTimeRange newValue = CurrentValue with { StartDate = date };

		// Validate the range if required
		bool rangeValidationFailed = false;
		if (RequireDateOrderEffective && newValue.StartDate.HasValue && newValue.EndDate.HasValue && newValue.StartDate.Value > newValue.EndDate.Value)
		{
			rangeValidationFailed = true;
			_validationMessageStore.Add(FieldIdentifier, DateOrderErrorMessageEffective);
		}

		if (!rangeValidationFailed)
		{
			// Only set the value if range validation passes
			_previousValue = newValue;
			CurrentValue = newValue;
			await CurrentValueChanged.InvokeAsync(newValue);
			EditContext.NotifyFieldChanged(_fromFieldIdentifier);
			ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
			ClearPreviousRangeValidationMessage();

			await CloseDropdownAsync(_fromDropdownToggleElement);
			await OpenDropDownAsync(_toDropdownToggleElement);
		}
		else
		{
			// If validation failed, notify and track the state
			if (!_previousRangeValidationAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				_previousRangeValidationAttemptFailed = true;
			}
		}
		// If validation failed, we don't close the dropdown - user needs to select a different date
	}

	private async Task HandleToCalendarValueChanged(DateTime? date)
	{
		_validationMessageStore.Clear(FieldIdentifier);

		DateTimeRange newValue = CurrentValue with { EndDate = date };

		// Validate the range if required
		bool rangeValidationFailed = false;
		if (RequireDateOrderEffective && newValue.StartDate.HasValue && newValue.EndDate.HasValue && newValue.StartDate.Value > newValue.EndDate.Value)
		{
			rangeValidationFailed = true;
			_validationMessageStore.Add(FieldIdentifier, DateOrderErrorMessageEffective);
		}

		if (!rangeValidationFailed)
		{
			// Only set the value if range validation passes
			_previousValue = newValue;
			CurrentValue = newValue;
			await CurrentValueChanged.InvokeAsync(newValue);
			EditContext.NotifyFieldChanged(_toFieldIdentifier);
			ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);
			ClearPreviousRangeValidationMessage();

			await CloseDropdownAsync(_toDropdownToggleElement);
		}
		else
		{
			// If validation failed, notify and track the state
			if (!_previousRangeValidationAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				_previousRangeValidationAttemptFailed = true;
			}
		}
		// If validation failed, we don't close the dropdown - user needs to select a different date
	}

	protected async Task HandleDateRangeClick(DateTimeRange value, HxDropdownToggleElement dropdownElement)
	{
		_validationMessageStore.Clear(FieldIdentifier);

		// Validate the range if required
		bool rangeValidationFailed = false;
		if (RequireDateOrderEffective && value.StartDate.HasValue && value.EndDate.HasValue && value.StartDate.Value > value.EndDate.Value)
		{
			rangeValidationFailed = true;
			_validationMessageStore.Add(FieldIdentifier, DateOrderErrorMessageEffective);
		}

		if (!rangeValidationFailed)
		{
			// Only set the value if range validation passes
			CurrentValue = value;
			await CurrentValueChanged.InvokeAsync(value);
			EditContext.NotifyFieldChanged(_fromFieldIdentifier);
			EditContext.NotifyFieldChanged(_toFieldIdentifier);

			ClearPreviousParsingMessage(ref _fromPreviousParsingAttemptFailed, _fromFieldIdentifier);
			ClearPreviousParsingMessage(ref _toPreviousParsingAttemptFailed, _toFieldIdentifier);
			ClearPreviousRangeValidationMessage();

			await CloseDropdownAsync(dropdownElement);
		}
		else
		{
			// If validation failed, notify and track the state
			if (!_previousRangeValidationAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				_previousRangeValidationAttemptFailed = true;
			}
		}
		// If validation failed, we don't close the dropdown
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

	private void ClearPreviousRangeValidationMessage()
	{
		if (_previousRangeValidationAttemptFailed)
		{
			_previousRangeValidationAttemptFailed = false;
			_validationMessageStore.Clear(FieldIdentifier);
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

		// Dispose(false);
	}
}
