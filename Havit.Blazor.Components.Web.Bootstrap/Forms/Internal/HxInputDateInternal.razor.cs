using System;
using System.Collections.Generic;
using System.Globalization;
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
	public partial class HxInputDateInternal<TValue> : IAsyncDisposable
	{
		[Parameter] public string InputId { get; set; }

		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public bool EnabledEffective { get; set; } = true;

		[Parameter] public List<DateItem> CustomDates { get; set; }

		[Parameter] public string ParsingErrorMessageEffective { get; set; }

		[Parameter] public string Placeholder { get; set; }

		[Inject] internal IStringLocalizer<HxInputDate> StringLocalizer { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private TValue previousValue;

		private bool previousParsingAttemptFailed;
		private ValidationMessageStore validationMessageStore;

		private ElementReference dateInputElement;
		private IJSObjectReference jsModule;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			validationMessageStore ??= new ValidationMessageStore(EditContext);

			// clear parsing error after new value is set
			if (!EqualityComparer<TValue>.Default.Equals(previousValue, Value))
			{
				ClearPreviousParsingMessage();
				previousValue = Value;
			}
		}

		protected override string FormatValueAsString(TValue value) => HxInputDate2<TValue>.FormatValue(value);

		private async Task HandleValueChangedAsync(ChangeEventArgs changeEventArgs)
		{
			// HandleValueChanged is used instead of TryParseValueFromString
			// When TryParseValueFromString is used, invalid input is replaced by previous value.		
			bool parsingFailed;
			validationMessageStore.Clear(FieldIdentifier);

			if (HxInputDate<DateTime>.TryParseDateTimeOffsetFromString((string)changeEventArgs.Value, null, out var date))
			{
				parsingFailed = false;
				CurrentValue = GetValueFromDateTimeOffset(date);
				await CloseDropDownAsync(dateInputElement);
			}
			else
			{
				parsingFailed = true;
				validationMessageStore.Add(FieldIdentifier, ParsingErrorMessageEffective);
			}

			// We can skip the validation notification if we were previously valid and still are
			if (parsingFailed || previousParsingAttemptFailed)
			{
				EditContext.NotifyValidationStateChanged();
				previousParsingAttemptFailed = parsingFailed;
			}

		}

		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxinputdaterange.js");
		}

		private async Task HandleClearClickAsync()
		{
			CurrentValue = default;
			ClearPreviousParsingMessage();

			await CloseDropDownAsync(dateInputElement);
		}

		private async Task HandleOKClickAsync()
		{
			await CloseDropDownAsync(dateInputElement);
		}

		//private async Task OpenDropDownAsync(ElementReference triggerElement)
		//{
		//	Contract.Assert<InvalidOperationException>(jsModule != null, nameof(jsModule));
		//	await jsModule.InvokeVoidAsync("open", triggerElement);
		//}

		private async Task CloseDropDownAsync(ElementReference triggerElement)
		{
			Contract.Assert<InvalidOperationException>(jsModule != null, nameof(jsModule));
			await jsModule.InvokeVoidAsync("destroy", triggerElement);
		}

		private async Task HandleCalendarValueChangedAsync(DateTime? date)
		{
			CurrentValue = GetValueFromDateTimeOffset((date != null) ? new DateTimeOffset(date.Value) : null);
			await CloseDropDownAsync(dateInputElement);
		}

		protected void HandleCustomDateClick(DateTime value)
		{
			CurrentValue = GetValueFromDateTimeOffset(new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Unspecified), TimeSpan.Zero));
			ClearPreviousParsingMessage();
		}

		private void ClearPreviousParsingMessage()
		{
			if (previousParsingAttemptFailed)
			{
				previousParsingAttemptFailed = false;
				validationMessageStore.Clear(FieldIdentifier);
				EditContext.NotifyValidationStateChanged();
			}
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
			validationMessageStore.Clear();

			if (jsModule != null)
			{
				await CloseDropDownAsync(dateInputElement);

				await jsModule.DisposeAsync();
			}

			Dispose(false);
		}
	}
}
