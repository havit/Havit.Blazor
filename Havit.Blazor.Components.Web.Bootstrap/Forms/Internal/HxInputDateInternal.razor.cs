using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxInputDateInternal<TValue> : InputBase<TValue>, IAsyncDisposable, IInputWithSize
{
	[Parameter] public string InputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public bool EnabledEffective { get; set; } = true;

	[Parameter] public bool ShowPredefinedDatesEffective { get; set; }

	[Parameter] public IEnumerable<InputDatePredefinedDatesItem> PredefinedDatesEffective { get; set; }

	[Parameter] public string ParsingErrorMessageEffective { get; set; }

	[Parameter] public string Placeholder { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public IconBase CalendarIconEffective { get; set; }

	[Parameter] public bool ShowClearButtonEffective { get; set; } = true;

	[Parameter] public DateTime MinDateEffective { get; set; }

	[Parameter] public DateTime MaxDateEffective { get; set; }

	[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective { get; set; }

	[Parameter] public LabelType LabelTypeEffective { get; set; }

	/// <summary>
	/// Custom CSS class to render with input-group span.
	/// </summary>
	[Parameter] public string InputGroupCssClass { get; set; }

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
	/// Input-group at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	[Parameter] public TimeProvider TimeProviderEffective { get; set; }

	[Parameter] public DateTime CalendarDisplayMonth { get; set; }

	[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroups => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);
	protected bool HasInputGroupEnd => !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupEndTemplate is not null);
	protected bool HasInputGroupStart => !String.IsNullOrWhiteSpace(InputGroupStartText) || (InputGroupStartTemplate is not null);

	protected bool RenderPredefinedDates => ShowPredefinedDatesEffective && (PredefinedDatesEffective != null) && PredefinedDatesEffective.Any();
	protected bool HasCalendarIcon => CalendarIconEffective is not null;

	protected DateTime GetCalendarDisplayMonthEffective => GetDateTimeFromValue(CurrentValue) ?? CalendarDisplayMonth;

#if !NET8_0_OR_GREATER
	private TValue _previousValue;
	private bool _previousParsingAttemptFailed;
	private ValidationMessageStore _validationMessageStore;
#endif

	private HxDropdownToggleElement _hxDropdownToggleElement;
	private ElementReference _iconWrapperElement;
	private IJSObjectReference _jsModule;
	private bool _firstRenderCompleted;

#if !NET8_0_OR_GREATER
	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		_validationMessageStore ??= new ValidationMessageStore(EditContext);

		// clear parsing error after new value is set
		if (!EqualityComparer<TValue>.Default.Equals(_previousValue, Value))
		{
			ClearPreviousParsingMessage();
			_previousValue = Value;
		}
	}
#endif

	protected override string FormatValueAsString(TValue value) => HxInputDate<TValue>.FormatValue(value);

	private void HandleValueChanged(string newInputValue)
	{
#if NET8_0_OR_GREATER
		CurrentValueAsString = newInputValue;
#else
		// HandleValueChanged is used instead of TryParseValueFromString
		// When TryParseValueFromString is used (pre net8), invalid input is replaced by previous value.		
		bool parsingFailed;
		_validationMessageStore.Clear(FieldIdentifier);

		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(newInputValue, null, out var date))
		{
			parsingFailed = false;
			CurrentValue = GetValueFromDateTimeOffset(date);
		}
		else
		{
			parsingFailed = true;
			_validationMessageStore.Add(FieldIdentifier, ParsingErrorMessageEffective);
		}

		// We can skip the validation notification if we were previously valid and still are
		if (parsingFailed || _previousParsingAttemptFailed)
		{
			EditContext.NotifyValidationStateChanged();
			_previousParsingAttemptFailed = parsingFailed;
		}
#endif
	}

	protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
	{
#if NET8_0_OR_GREATER
		if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString(value, null, out var date))
		{
			result = GetValueFromDateTimeOffset(date);
			validationErrorMessage = null;
			return true;
		}
		else
		{
			result = default;
			validationErrorMessage = ParsingErrorMessageEffective;
			return false;
		}
#else
		throw new NotSupportedException();
#endif
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		_firstRenderCompleted = true;

		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && HasCalendarIcon)
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxInputDate));
			await _jsModule.InvokeVoidAsync("addOpenAndCloseEventListeners", _hxDropdownToggleElement.ElementReference, (CalendarIconEffective is not null) ? _iconWrapperElement : null);
		}
	}

	public async ValueTask FocusAsync()
	{
		await _hxDropdownToggleElement.ElementReference.FocusAsync();
	}

	private CalendarDateCustomizationResult GetCalendarDateCustomization(CalendarDateCustomizationRequest request)
	{
		return CalendarDateCustomizationProviderEffective?.Invoke(request with { Target = CalendarDateCustomizationTarget.InputDate }) ?? null;
	}

	private async Task HandleClearClickAsync()
	{
		SetCurrentDate(null);
		await CloseDropdownAsync();
	}

	private async Task CloseDropdownAsync()
	{
		Contract.Requires<InvalidOperationException>(_hxDropdownToggleElement != null);
		await _hxDropdownToggleElement.HideAsync();
	}

	private async Task HandleCalendarValueChangedAsync(DateTime? date)
	{
		SetCurrentDate(date);
		await CloseDropdownAsync();
	}

	protected async Task HandleCustomDateClick(DateTime value)
	{
		SetCurrentDate(value);
		await CloseDropdownAsync();
	}

	protected void SetCurrentDate(DateTime? date)
	{
#if NET8_0_OR_GREATER
		CurrentValueAsString = date?.ToShortDateString(); // we need to trigger the logic in CurrentValueAsString setter
#else
		if (date == null)
		{
			CurrentValue = default;
		}
		else
		{
			CurrentValue = GetValueFromDateTimeOffset(new DateTimeOffset(DateTime.SpecifyKind(date.Value, DateTimeKind.Unspecified), TimeSpan.Zero));
		}
		ClearPreviousParsingMessage();
#endif
	}

#if !NET8_0_OR_GREATER
	private void ClearPreviousParsingMessage()
	{
		if (_previousParsingAttemptFailed)
		{
			_previousParsingAttemptFailed = false;
			EditContext.NotifyValidationStateChanged();
		}
	}
#endif

	private string GetNameAttributeValue()
	{
#if NET8_0_OR_GREATER
		return String.IsNullOrEmpty(NameAttributeValue) ? null : NameAttributeValue;
#else
		return null;
#endif
	}

	internal static TValue GetValueFromDateTimeOffset(DateTimeOffset? value)
	{
		if (value == null)
		{
			return default;
		}

		var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

		if (targetType == typeof(DateTime))
		{
			return (TValue)(object)value.Value.DateTime;
		}
		else if (targetType == typeof(DateTimeOffset))
		{
			return (TValue)(object)value.Value;
		}
		else
		{
			throw new InvalidOperationException("Unsupported type.");
		}
	}

	internal static DateTime? GetDateTimeFromValue(TValue value)
	{
		if (EqualityComparer<TValue>.Default.Equals(value, default))
		{
			return null;
		}

		switch (value)
		{
			case DateTime dateTimeValue:
				return dateTimeValue;
			case DateTimeOffset dateTimeOffsetValue:
				return dateTimeOffsetValue.DateTime;
			default:
				throw new InvalidOperationException("Unsupported type.");
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
#if !NET8_0_OR_GREATER
		_validationMessageStore?.Clear();
#endif

		try
		{
			if (_firstRenderCompleted)
			{
				if (_hxDropdownToggleElement is not null)
				{
					await CloseDropdownAsync();
				}

				if (_jsModule is not null)
				{
					await _jsModule.DisposeAsync();
				}
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

		Dispose(false);
	}
}