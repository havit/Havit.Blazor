using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxInputDateRangeInternal
	{
		[Parameter] public bool ShowValidationMessage { get; set; } = true;

		[Parameter] public IEnumerable<DateRangeItem> DateRanges { get; set; }

		[Inject] public IStringLocalizer<HxInputDateRange> Localizer { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }


		private bool fromPreviousParsingAttemptFailed;
		private bool toPreviousParsingAttemptFailed;
		private ValidationMessageStore validationMessageStore;

		private FieldIdentifier fromFieldIdentifier;
		private FieldIdentifier toFieldIdentifier;

		private ElementReference fromInputElement;
		private ElementReference toInputElement;
		private IJSObjectReference jsModule;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			validationMessageStore ??= new ValidationMessageStore(EditContext);
			fromFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.StartDate));
			toFieldIdentifier = new FieldIdentifier(FieldIdentifier.Model, FieldIdentifier.FieldName + "." + nameof(DateTimeRange.EndDate));
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

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxinputdaterange.js");

			bool fromValid = !EditContext.GetValidationMessages(FieldIdentifier).Any() && !EditContext.GetValidationMessages(fromFieldIdentifier).Any();
			bool toValid = !EditContext.GetValidationMessages(toFieldIdentifier).Any();
			await jsModule.InvokeVoidAsync(fromValid ? "setInputValid" : "setInputInvalid", fromInputElement);
			await jsModule.InvokeVoidAsync(toValid ? "setInputValid" : "setInputInvalid", toInputElement);
		}

		protected void HandleFromChange(ChangeEventArgs changeEventArgs)
		{
			bool parsingFailed;

			validationMessageStore.Clear(fromFieldIdentifier);

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var fromDate))
			{
				parsingFailed = false;
				CurrentValue = new DateTimeRange
				{
					StartDate = fromDate?.DateTime,
					EndDate = Value.EndDate
				};
				EditContext.NotifyFieldChanged(fromFieldIdentifier);
			}
			else
			{
				parsingFailed = true;
				validationMessageStore.Add(fromFieldIdentifier, "Zadej správně FROM.");
			}

			// We can skip the validation notification if we were previously valid and still are
			if (parsingFailed || fromPreviousParsingAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				fromPreviousParsingAttemptFailed = parsingFailed;
			}
		}

		protected void HandleToChange(ChangeEventArgs changeEventArgs)
		{
			bool parsingFailed;
			validationMessageStore.Clear(toFieldIdentifier);

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var toDate))
			{
				parsingFailed = false;
				CurrentValue = new DateTimeRange
				{
					StartDate = Value.StartDate,
					EndDate = toDate?.DateTime
				};
				EditContext.NotifyFieldChanged(toFieldIdentifier);
			}
			else
			{
				parsingFailed = true;
				validationMessageStore.Add(toFieldIdentifier, "Zadej správně TO.");
			}

			// We can skip the validation notification if we were previously valid and still are
			if (parsingFailed || toPreviousParsingAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				toPreviousParsingAttemptFailed = parsingFailed;
			}
		}

		protected async Task HandleFromFocusAsync()
		{
			await CloseDropDownAsync(toInputElement);
		}

		protected async Task HandleToFocusAsync()
		{
			await CloseDropDownAsync(fromInputElement);
		}

		private async Task HandleFromClearClickAsync()
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = null,
				EndDate = Value.EndDate
			};
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

			await CloseDropDownAsync(fromInputElement);
		}

		private async Task HandleToClearClickAsync()
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = Value.StartDate,
				EndDate = null
			};
			EditContext.NotifyFieldChanged(toFieldIdentifier);
			ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

			await CloseDropDownAsync(toInputElement);
		}

		private async Task HandleFromOKClickAsync()
		{
			await CloseDropDownAsync(fromInputElement);
		}

		private async Task HandleToOKClickAsync()
		{
			await CloseDropDownAsync(toInputElement);
		}

		private async Task OpenDropDownAsync(ElementReference triggerElement)
		{
			Contract.Assert<InvalidOperationException>(jsModule != null, nameof(jsModule));
			await jsModule.InvokeVoidAsync("open", triggerElement);
		}

		private async Task CloseDropDownAsync(ElementReference triggerElement)
		{
			Contract.Assert<InvalidOperationException>(jsModule != null, nameof(jsModule));
			await jsModule.InvokeVoidAsync("destroy", triggerElement);
		}

		private async Task HandleFromCalendarValueChangedAsync(DateTime? date)
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = date,
				EndDate = Value.EndDate
			};
			EditContext.NotifyFieldChanged(fromFieldIdentifier);
			ClearPreviousParsingMessage(ref fromPreviousParsingAttemptFailed, fromFieldIdentifier);

			await CloseDropDownAsync(fromInputElement);
			await OpenDropDownAsync(toInputElement);
		}

		private async Task HandleToCalendarValueChanged(DateTime? date)
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = Value.StartDate,
				EndDate = date
			};
			EditContext.NotifyFieldChanged(toFieldIdentifier);
			ClearPreviousParsingMessage(ref toPreviousParsingAttemptFailed, toFieldIdentifier);

			await CloseDropDownAsync(toInputElement);
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

		// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				await CloseDropDownAsync(fromInputElement);
				await CloseDropDownAsync(toInputElement);

				await jsModule.DisposeAsync();
			}
		}
	}
}
