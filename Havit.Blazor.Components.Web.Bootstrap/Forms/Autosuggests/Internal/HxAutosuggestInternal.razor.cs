using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxAutosuggestInternal<TItem, TValue> : IAsyncDisposable
{
	[Parameter] public TValue Value { get; set; }
	[Parameter] public EventCallback<TValue> ValueChanged { get; set; }

	[Parameter] public AutosuggestDataProviderDelegate<TItem> DataProvider { get; set; }

	/// <summary>
	/// Selects value from item.
	/// Not required when TValue is same as TItemTime.
	/// </summary>
	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	/// <summary>
	/// Selects text to display from item.
	/// When not set ToString() is used.
	/// </summary>
	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	/// <summary>
	/// Template to display item.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// Template to display when items collection is empty
	/// </summary>
	[Parameter] public RenderFragment EmptyTemplate { get; set; }

	/// <summary>
	/// Gets item from <see cref="Value"/>.
	/// </summary>
	[Parameter] public Func<TValue, Task<TItem>> ItemFromValueResolver { get; set; }

	/// <summary>
	/// Minimal number of characters to start suggesting. Default is <c>2</c>.
	/// </summary>
	[Parameter] public int MinimumLengthEffective { get; set; } = 2;

	/// <summary>
	/// Short hint displayed in the input field before the user enters a value.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Debounce delay in milliseconds. Default is <c>300 ms</c>.
	/// </summary>
	[Parameter] public int DelayEffective { get; set; } = 300;

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public string InputId { get; set; }

	[Parameter] public IconBase SearchIconEffective { get; set; }

	[Parameter] public IconBase ClearIconEffective { get; set; }

	[Parameter] public bool EnabledEffective { get; set; } = true;
	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public LabelType LabelTypeEffective { get; set; }

	[Parameter] public bool? SpellcheckEffective { get; set; }

	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	/// <summary>
	/// Offset between the dropdown and the input.
	/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
	/// </summary>
	[Parameter] public (int Skidding, int Distance) DropdownOffset { get; set; } = (0, 4);

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

	[Parameter] public string NameAttributeValue { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] protected IStringLocalizer<HxAutosuggest> HxAutosuggestLocalizer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	private string _dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private System.Timers.Timer _timer;
	private string _userInput = String.Empty;
	private CancellationTokenSource _cancellationTokenSource;
	private List<TItem> _suggestions;
	private bool _userInputModified;
	private bool _isDropdownOpened = false;
	private bool _blurInProgress;
	private bool _currentlyFocused;
	private bool _disposed;
	private IJSObjectReference _jsModule;
	private HxAutosuggestInputInternal _autosuggestInput;
	private TValue _lastKnownValue;
	private bool _dataProviderInProgress;
	private DotNetObjectReference<HxAutosuggestInternal<TItem, TValue>> _dotnetObjectReference;

	internal string ChipValue => _userInput;

	public HxAutosuggestInternal()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	private async Task SetValueItemWithEventCallback(TItem selectedItem)
	{
		TValue value = SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, selectedItem);

		if (!EqualityComparer<TValue>.Default.Equals(Value, value))
		{
			Value = value;
			_lastKnownValue = Value;
			await ValueChanged.InvokeAsync(Value);
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Contract.Requires<InvalidOperationException>(DataProvider != null, $"{GetType()} requires a {nameof(DataProvider)} parameter.");

		if (!EqualityComparer<TValue>.Default.Equals(Value, default))
		{
			// we do not want to re-resolve the Text (userInput) if the Value did not change
			if (!EqualityComparer<TValue>.Default.Equals(Value, _lastKnownValue))
			{
				if ((ItemFromValueResolver == null) && (typeof(TValue) == typeof(TItem)))
				{
					_userInput = TextSelectorEffective((TItem)(object)Value);
				}
				else
				{
					Contract.Requires<InvalidOperationException>(ItemFromValueResolver is not null, $"{GetType()} requires a {nameof(ItemFromValueResolver)} parameter.");
					_userInput = TextSelectorEffective(await ItemFromValueResolver(Value));
				}
			}
		}
		else
		{
			_userInput = TextSelectorEffective(default);
		}
		_userInputModified = false;
		_lastKnownValue = Value;

		if (string.IsNullOrWhiteSpace(InputId))
		{
			InputId = "hx" + Guid.NewGuid().ToString("N");
		}
	}

	public async ValueTask FocusAsync()
	{
		await _autosuggestInput.FocusAsync();
	}

	private async Task HandleInputInput(string newUserInput)
	{
		Contract.Requires<InvalidOperationException>(EnabledEffective, $"The {GetType().Name} component is in a disabled state.");

		// user changes an input
		_userInput = newUserInput;
		_userInputModified = true;

		_timer?.Stop(); // if waiting for an interval, stop it
#pragma warning disable VSTHRD103 // Call async methods when in an async method
		// TODO Consider CancelAsync() for net8+
		_cancellationTokenSource?.Cancel(); // if already loading data, cancel it
#pragma warning restore VSTHRD103 // Call async methods when in an async method
		_dataProviderInProgress = false; // data provider is no more in progress

		// start new time interval
		if (_userInput.Length >= MinimumLengthEffective)
		{
			if (DelayEffective == 0)
			{
				await UpdateSuggestionsAsync();
			}
			else
			{
				if (_timer == null)
				{
					_timer = new System.Timers.Timer();
					_timer.AutoReset = false; // just once
					_timer.Elapsed += HandleTimerElapsed;
				}
				_timer.Interval = DelayEffective;
				_timer.Start();
			}
		}
		else
		{
			// or close a dropdown
			_suggestions = null;
			await DestroyDropdownAsync();
		}
	}

	[SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required for Timer")]
	private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		// when a time interval reached, update suggestions
		await InvokeAsync(async () =>
		{
			await UpdateSuggestionsAsync();
		});
	}

	private async Task HandleInputClick()
	{
		if (_currentlyFocused && (MinimumLengthEffective == 0) && !_isDropdownOpened)
		{
			await UpdateSuggestionsAsync();
		}
	}

	private async Task HandleInputFocus()
	{
		if (!EnabledEffective)
		{
			return;
		}

		_currentlyFocused = true;
		if (string.IsNullOrEmpty(_userInput) && MinimumLengthEffective <= 0)
		{
			await UpdateSuggestionsAsync();
			return;
		}
	}

	// Due to HTML update and Bootstrap Dropdown collision we are not allowed to re-render HTML in InputBlur!
	private void HandleInputBlur()
	{
		if (!EnabledEffective)
		{
			return;
		}

		_currentlyFocused = false;
		// when user clicks back button in browser this method can be called after it is disposed!
		_blurInProgress = true;
		_timer?.Stop(); // if waiting for an interval, stop it
		_cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		_dataProviderInProgress = false; // data provider is no more in progress
	}

	private async Task UpdateSuggestionsAsync()
	{
		// Cancelation is performed in HandleInputInput method
		_cancellationTokenSource?.Dispose();

		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = _cancellationTokenSource.Token;

		_dataProviderInProgress = true;
		StateHasChanged();

		AutosuggestDataProviderRequest request = new AutosuggestDataProviderRequest
		{
			UserInput = _userInput,
			CancellationToken = cancellationToken
		};

		AutosuggestDataProviderResult<TItem> result;
		try
		{
			result = await DataProvider.Invoke(request);
		}
		catch (OperationCanceledException) // gRPC stack does not set the operationFailedException.CancellationToken, do not check in when-clause
		{
			return;
		}

		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		_dataProviderInProgress = false;

		// KeyboardNavigation
		_focusedItemIndex = 0; // First item in the searchResults collection.

		_suggestions = result.Data?.ToList();

		if ((_suggestions?.Any() ?? false) || (EmptyTemplate != null))
		{
			await OpenDropdownAsync();
		}
		else
		{
			await DestroyDropdownAsync();
		}

		StateHasChanged();
	}

	#region KeyboardNavigation
	private int _focusedItemIndex = -1;

	[JSInvokable("HxAutosuggestInternal_HandleInputKeyDown")]
	public async Task HandleInputKeyDown(string keyCode)
	{
		// Confirm selection on the focused item if an item is focused and the enter key is pressed.
		TItem focusedItem = GetItemByIndex(_focusedItemIndex);
		if ((keyCode == KeyCodes.Enter) || (keyCode == KeyCodes.NumpadEnter))
		{
			if ((focusedItem is not null) && (!focusedItem.Equals(default)))
			{
				await DestroyDropdownAsync();
				await HandleItemSelected(focusedItem);
				StateHasChanged();
			}
		}

		// Move focus up or down.
		if (keyCode == KeyCodes.ArrowUp)
		{
			int previousItemIndex = _focusedItemIndex - 1;
			if (previousItemIndex >= 0)
			{
				_focusedItemIndex = previousItemIndex;
				StateHasChanged();

				await _jsModule.InvokeVoidAsync("scrollToSelectedItem", _dropdownId);
			}
		}
		else if (keyCode == KeyCodes.ArrowDown)
		{
			int nextItemIndex = _focusedItemIndex + 1;
			if (nextItemIndex < _suggestions?.Count)
			{
				_focusedItemIndex = nextItemIndex;
				StateHasChanged();

				await _jsModule.InvokeVoidAsync("scrollToSelectedItem", _dropdownId);
			}
		}
	}

	private TItem GetItemByIndex(int index)
	{
		if ((index >= 0) && (index < _suggestions?.Count))
		{
			return _suggestions[index];
		}
		else
		{
			return default;
		}
	}
	#endregion KeyboardNavigation

	private async Task HandleItemSelected(TItem item)
	{
		// user selected an item in the "dropdown".
		await SetValueItemWithEventCallback(item);
		_userInput = TextSelectorEffective(item);
		_userInputModified = false;
	}

	private async Task ClearInputAsync()
	{
		// user clicked on a cross button (x)
		await SetValueItemWithEventCallback(default);
		_userInput = TextSelectorEffective(default);
		_userInputModified = false;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			string[] keysToPreventDefault = [KeyCodes.ArrowDown, KeyCodes.ArrowUp, KeyCodes.Enter, KeyCodes.NumpadEnter];
			await _jsModule.InvokeVoidAsync("initialize", InputId, _dotnetObjectReference, keysToPreventDefault);
		}

		if (_blurInProgress)
		{
			_blurInProgress = false;
			if (_userInputModified && !_isDropdownOpened)
			{
				await SetValueItemWithEventCallback(default);
				_userInput = TextSelectorEffective(default);
				_userInputModified = false;
				StateHasChanged();
			}
		}
	}
	#region OpenDropdownAsync, DestroyDropdownAsync, EnsureJsModuleAsync
	private async Task OpenDropdownAsync()
	{
		if (!_isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("open", _autosuggestInput.InputElement, _dotnetObjectReference);
			_isDropdownOpened = true;
		}
	}

	private async Task DestroyDropdownAsync()
	{
		if (_isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("destroy", _autosuggestInput.InputElement);
			_isDropdownOpened = false;
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAutosuggest));
	}

	[JSInvokable("HxAutosuggestInternal_HandleDropdownHidden")]
	public async Task HandleDropdownHidden()
	{
		if (_userInputModified && !_currentlyFocused)
		{
			await SetValueItemWithEventCallback(default);
			_userInput = TextSelectorEffective(default);
			_userInputModified = false;
			StateHasChanged();
		}
		await DestroyDropdownAsync();
	}
	#endregion

	private string TextSelectorEffective(TItem item)
	{
		return (item == null)
			? String.Empty
			: SelectorHelpers.GetText(TextSelector, item);
	}


	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		_timer?.Dispose();
		_timer = null;

		_cancellationTokenSource?.Dispose();
		_cancellationTokenSource = null;

		if (_jsModule != null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", InputId);
				await _jsModule.DisposeAsync();
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

		_dotnetObjectReference?.Dispose();
	}
}