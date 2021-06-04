using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxInputDateRangeInternal
	{
		[Inject] public IStringLocalizer<HxInputDateRange> Localizer { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private bool previousParsingFromAttemptFailed;
		private bool previousParsingToAttemptFailed;
		private ValidationMessageStore fromValidationMessageStore;
		private ValidationMessageStore toValidationMessageStore;

		private ElementReference fromInputElement;
		private ElementReference toInputElement;
		private IJSObjectReference jsModule;

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxinputdaterange.js");
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

		protected void HandleFromChange(ChangeEventArgs changeEventArgs)
		{
			bool parsingFailed;

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var fromDate))
			{
				parsingFailed = false;
				fromValidationMessageStore?.Clear();
				CurrentValue = new DateTimeRange
				{
					StartDate = fromDate?.DateTime,
					EndDate = Value.EndDate
				};
			}
			else
			{
				parsingFailed = true;

				fromValidationMessageStore ??= new ValidationMessageStore(EditContext);
				fromValidationMessageStore.Clear();
				fromValidationMessageStore.Add(this.FieldIdentifier, "Zadej správně FROM.");
			}

			// We can skip the validation notification if we were previously valid and still are
			if (parsingFailed || previousParsingFromAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				previousParsingFromAttemptFailed = parsingFailed;
			}
		}

		protected void HandleToChange(ChangeEventArgs changeEventArgs)
		{
			bool parsingFailed;

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var toDate))
			{
				parsingFailed = false;
				toValidationMessageStore?.Clear();
				CurrentValue = new DateTimeRange
				{
					StartDate = Value.StartDate,
					EndDate = toDate?.DateTime
				};
			}
			else
			{
				parsingFailed = true;

				toValidationMessageStore ??= new ValidationMessageStore(EditContext);
				toValidationMessageStore.Clear();
				toValidationMessageStore.Add(this.FieldIdentifier, "Zadej správně TO.");
			}

			// We can skip the validation notification if we were previously valid and still are
			if (parsingFailed || previousParsingToAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				previousParsingToAttemptFailed = parsingFailed;
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

			if (previousParsingFromAttemptFailed)
			{
				previousParsingFromAttemptFailed = false;
				fromValidationMessageStore.Clear();
				EditContext.NotifyValidationStateChanged();
			}

			await CloseDropDownAsync(fromInputElement);
		}

		private async Task HandleToClearClickAsync()
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = Value.StartDate,
				EndDate = null
			};

			if (previousParsingToAttemptFailed)
			{
				previousParsingToAttemptFailed = false;
				toValidationMessageStore.Clear();
				EditContext.NotifyValidationStateChanged();
			}

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
			await jsModule.InvokeVoidAsync("open", triggerElement);
		}

		private async Task CloseDropDownAsync(ElementReference triggerElement)
		{
			await jsModule.InvokeVoidAsync("destroy", triggerElement);
		}

		private async Task HandleFromCalendarValueChangedAsync(DateTime? date)
		{
			CurrentValue = new DateTimeRange
			{
				StartDate = date,
				EndDate = Value.EndDate
			};

			if (previousParsingFromAttemptFailed)
			{
				previousParsingFromAttemptFailed = false;
				fromValidationMessageStore.Clear();
				EditContext.NotifyValidationStateChanged();
			}

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

			if (previousParsingToAttemptFailed)
			{
				previousParsingToAttemptFailed = false;
				toValidationMessageStore.Clear();
				EditContext.NotifyValidationStateChanged();
			}

			await CloseDropDownAsync(toInputElement);
		}

		// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			await CloseDropDownAsync(fromInputElement);
			await CloseDropDownAsync(toInputElement);

			await jsModule.DisposeAsync();
		}
	}
}
