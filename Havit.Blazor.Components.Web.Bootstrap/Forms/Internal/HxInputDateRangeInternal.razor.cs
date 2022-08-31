using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxInputDateRangeInternal : InputBase<DateTimeRange>, IAsyncDisposable, IInputWithSize
	{
		[Parameter] public string FromInputId { get; set; }

		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public InputSize InputSizeEffective { get; set; }

		[Parameter] public bool EnabledEffective { get; set; } = true;

		[Parameter] public ValidationMessageDisplayMode ValidationMessageDisplayModeEffective { get; set; }

		[Parameter] public bool ShowPredefinedDateRangesEffective { get; set; }
		[Parameter] public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRangesEffective { get; set; }

		[Parameter] public string FromParsingErrorMessageEffective { get; set; }

		[Parameter] public string ToParsingErrorMessageEffective { get; set; }

		[Parameter] public bool ShowCalendarButtonsEffective { get; set; } = true;

		[Parameter] public DateTime MinDateEffective { get; set; }

		[Parameter] public DateTime MaxDateEffective { get; set; }

		[Parameter] public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProviderEffective { get; set; }

		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private DateTimeRange previousValue;
		private bool fromPreviousParsingAttemptFailed;
		private bool toPreviousParsingAttemptFailed;
		private ValidationMessageStore validationMessageStore;

		private FieldIdentifier fromFieldIdentifier;
		private FieldIdentifier toFieldIdentifier;
		private string[] validationFieldNames;

		private HxDropdownToggleElement fromDropdownToggleElement;
		private HxDropdownToggleElement toDropdownToggleElement;

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

		protected override bool TryParseValueFromString(string value, out DateTimeRange result, out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private string FormatDate(DateTime? date)
		{
			// nenabízíme hodnotu 1.1.0001, atp.
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
				parsingFailed = false;
				CurrentValue = Value with { StartDate = fromDate?.DateTime };
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
				parsingFailed = false;
				CurrentValue = Value with { EndDate = toDate?.DateTime };
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
			CurrentValue = Value with { StartDate = null };
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

			await CloseDropdownAsync(fromDropdownToggleElement);
		}

		private async Task HandleToClearClickAsync()
		{
			CurrentValue = Value with { EndDate = null };
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
			CurrentValue = Value with { StartDate = date };
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

			await CloseDropdownAsync(fromDropdownToggleElement);
			await OpenDropDownAsync(toDropdownToggleElement);
		}

		private async Task HandleToCalendarValueChanged(DateTime? date)
		{
			CurrentValue = Value with { EndDate = date };
			EditContext.NotifyFieldChanged(toFieldIdentifier);
			ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

			await CloseDropdownAsync(toDropdownToggleElement);
		}

		protected void HandleDateRangeClick(DateTimeRange value)
		{
			CurrentValue = value;
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
			EditContext.NotifyFieldChanged(toFieldIdentifier);

			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);
			ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);
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

#if NET6_0_OR_GREATER
			try
			{
				await CloseDropdownAsync(fromDropdownToggleElement);
				await CloseDropdownAsync(toDropdownToggleElement);
			}
			catch (JSDisconnectedException)
			{

			}
#else
			await CloseDropdownAsync(fromDropdownToggleElement);
			await CloseDropdownAsync(toDropdownToggleElement);
#endif

			Dispose(false);
		}
	}
}
