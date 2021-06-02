using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxInputDateRangeInternal
	{
		[Inject] public IStringLocalizer<HxInputDateRange> Localizer { get; set; }

		private ValidationMessageStore fromValidationMessageStore;
		private ValidationMessageStore toValidationMessageStore;

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
			//bool parsingFailed;
			fromValidationMessageStore?.Clear();

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var fromDate))
			{
				//parsingFailed = false;
				CurrentValue = new DateTimeRange
				{
					StartDate = fromDate?.DateTime,
					EndDate = Value.EndDate
				};
			}
			else
			{
				//parsingFailed = true;

				fromValidationMessageStore ??= new ValidationMessageStore(EditContext);
				fromValidationMessageStore.Add(this.FieldIdentifier, "Zadej správně FROM.");
			}

			// TODO: Jen při změně
			EditContext.NotifyValidationStateChanged();
		}

		protected void HandleToChange(ChangeEventArgs changeEventArgs)
		{
			//bool parsingFailed;
			toValidationMessageStore?.Clear();

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var toDate))
			{
				//parsingFailed = false;
				CurrentValue = new DateTimeRange
				{
					StartDate = Value.StartDate,
					EndDate = toDate?.DateTime
				};
			}
			else
			{
				//parsingFailed = true;

				toValidationMessageStore ??= new ValidationMessageStore(EditContext);
				toValidationMessageStore.Add(this.FieldIdentifier, "Zadej správně TO.");
			}

			// TODO: Jen při změně
			EditContext.NotifyValidationStateChanged();
		}

		protected async Task HandleFromFocusAsync()
		{
			await Task.Yield();
		}

		protected async Task HandleFromBlurAsync()
		{
			await Task.Yield();
		}



		protected async Task HandleToFocusAsync()
		{
			await Task.Yield();
		}

		protected async Task HandleToBlurAsync()
		{
			await Task.Yield();
		}

		//	parsingFailed = true;

		//                   if (_parsingValidationMessages == null)
		//                   {
		//                       _parsingValidationMessages = new ValidationMessageStore(EditContext);
		//}

		//_parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

		//                   // Since we're not writing to CurrentValue, we'll need to notify about modification from here
		//                   EditContext.NotifyFieldChanged(FieldIdentifier);
		//               }
	}
}
