﻿using Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxInputDateInternal<TValue> : InputBase<TValue>, IAsyncDisposable, IInputWithSize, IInputWithLabelType
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

	protected DateTime GetCalendarDisplayMonthEffective => DateHelper.GetDateTimeFromValue(CurrentValue) ?? CalendarDisplayMonth;

	private HxDropdownToggleElement _hxDropdownToggleElement;
	private ElementReference _iconWrapperElement;
	private IJSObjectReference _jsModule;
	private bool _firstRenderCompleted;

	protected override string FormatValueAsString(TValue value) => HxInputDate<TValue>.FormatValue(value);

	private void HandleValueChanged(string newInputValue)
	{
		CurrentValueAsString = newInputValue;
	}

	protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
	{
		if (DateHelper.TryParseDateFromString<TValue>(value, TimeProviderEffective, out var date))
		{
			result = date;
			validationErrorMessage = null;
			return true;
		}
		else
		{
			result = default;
			validationErrorMessage = ParsingErrorMessageEffective;
			return false;
		}
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
		await _hxDropdownToggleElement.ShowAsync();
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
		CurrentValueAsString = date?.ToShortDateString(); // we need to trigger the logic in CurrentValueAsString setter
	}

	private string GetNameAttributeValue()
	{
		return String.IsNullOrEmpty(NameAttributeValue) ? null : NameAttributeValue;
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
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