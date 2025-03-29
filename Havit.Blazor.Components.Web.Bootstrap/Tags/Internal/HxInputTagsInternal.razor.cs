using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Internal implementation for <see cref="HxInputTags"/>.
/// </summary>
public partial class HxInputTagsInternal
{
	[Inject] protected IStringLocalizer<HxInputTags> HxInputTagsLocalizer { get; set; }
	/// <summary>
	/// Indicates whether you are restricted to suggested items only (<c>false</c>).
	/// Default is <c>true</c> (you can type your own tags).
	/// </summary>
	[Parameter] public bool AllowCustomTags { get; set; } = true;

	[Parameter] public List<string> Value { get; set; }
	protected List<string> ValueEffective => Value ?? new();
	[Parameter] public EventCallback<List<string>> ValueChanged { get; set; }

	[Parameter] public InputTagsDataProviderDelegate DataProvider { get; set; }

	/// <summary>
	/// Minimal number of characters to start suggesting. Default is <c>2</c>.
	/// </summary>
	[Parameter] public int SuggestMinimumLengthEffective { get; set; } = 2;

	/// <summary>
	/// Characters, when typed, divide the current input into separate tags.
	/// Default is comma, semicolon and space.
	/// </summary>
	[Parameter] public List<char> DelimitersEffective { get; set; } = new() { ',', ';', ' ' };

	/// <summary>
	/// Indicates whether the add-icon (+) should be displayed.
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool ShowAddButtonEffective { get; set; } = false;

	/// <summary>
	/// Optional text for the add-button.
	/// Displayed only when there are no tags (the <see cref="Value"/> is empty).
	/// Default is <c>null</c> (none).
	/// </summary>
	[Parameter] public string AddButtonText { get; set; }

	/// <summary>
	/// Indicates whether a "naked" variant should be displayed (no border).
	/// Default is <c>false</c>.
	/// Consider enabling <see cref="HxInputTags.ShowAddButton"/> when using <c>Naked</c>.
	/// </summary>
	[Parameter] public bool Naked { get; set; } = false;

	/// <summary>
	/// Short hint displayed in the input field before the user enters a value.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Settings for the <see cref="HxBadge"/> used to render tags.
	/// </summary>
	[Parameter] public BadgeSettings TagBadgeSettingsEffective { get; set; }

	[Parameter] public int SuggestDelayEffective { get; set; } = 300;

	/// <summary>
	/// CSS of the wrapping .form-control container (corresponds to InputCssClass on regular inputs)
	/// </summary>
	[Parameter] public string CoreFormControlCssClass { get; set; }

	[Parameter] public string InputId { get; set; }

	[Parameter] public bool EnabledEffective { get; set; } = true;

	[Parameter] public LabelType LabelTypeEffective { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	/// <summary>
	/// Offset between dropdown input and dropdown menu
	/// </summary>
	[Parameter] public (int X, int Y) InputOffset { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

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

	[Parameter] public bool? SpellcheckEffective { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML input.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }


	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	private string _dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private System.Timers.Timer _timer;
	private string _userInput = String.Empty;
	private CancellationTokenSource _cancellationTokenSource;
	private List<string> _suggestions;
	private bool _isDropdownOpened = false;
	private bool _currentlyFocused;
	private bool _mouseDownFocus;
	private bool _disposed;
	private IJSObjectReference _jsModule;
	private HxInputTagsInputInternal _inputComponent;
	private bool _dataProviderInProgress;
	private DotNetObjectReference<HxInputTagsInternal> _dotnetObjectReference;

	public HxInputTagsInternal()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	private async Task AddTagWithEventCallbackAsync(string tag)
	{
		if ((Value != null) && Value.Contains(tag))
		{
			return;
		}

		if (Value == null)
		{
			Value = [tag];
		}
		else
		{
			// do not change the instance, create a new one
			Value = [.. Value, tag];
		}
		await ValueChanged.InvokeAsync(Value);
	}

	private async Task RemoveTagWithEventCallbackAsync(string tag)
	{
		if (Value == null)
		{
			return;
		}

		Value = Value.Except([tag]).ToList();
		await ValueChanged.InvokeAsync(Value);
	}

	public async Task FocusAsync()
	{
		if (_disposed)
		{
			return;
		}
		await EnsureJsModuleAsync();
		await _jsModule.InvokeVoidAsync("tryFocus", _inputComponent.InputElement);
	}

	private async Task HandleInputInput(string newUserInput)
	{
		// user changes an input
		_userInput = newUserInput ?? String.Empty;

		CancelDelayedSuggestionsUpdate();

		// tag delimiters
		await TryProcessCustomTagsAsync(keepLastTagForSuggestion: true);

		if (DataProvider is not null)
		{
			if (_userInput.Length >= SuggestMinimumLengthEffective)
			{
				if (SuggestDelayEffective == 0)
				{
					await UpdateSuggestionsAsync();
				}
				else
				{
					// start new time interval
					if (_timer == null)
					{
						_timer = new System.Timers.Timer();
						_timer.AutoReset = false; // just once
						_timer.Elapsed += HandleTimerElapsed;
					}
					_timer.Interval = SuggestDelayEffective;
					_timer.Start();
				}
			}
			else
			{
				// or close a dropdown
				_suggestions = null;
				await TryDestroyDropdownAsync();
			}
		}
	}

	private void CancelDelayedSuggestionsUpdate()
	{
		_timer?.Stop(); // if waiting for an interval, stop it
		_cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		_dataProviderInProgress = false; // data provider is no longer in progress				 
	}

	[SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required by Timer")]
	private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		// when a time interval reached, update suggestions
		await InvokeAsync(async () =>
		{
			await UpdateSuggestionsAsync();
		});
	}

	private void HandleInputMouseDown()
	{
		if (!_currentlyFocused)
		{
			_mouseDownFocus = true;
		}
	}

	private async Task HandleInputClick()
	{
		if (_currentlyFocused && (SuggestMinimumLengthEffective == 0) && !_isDropdownOpened)
		{
			await UpdateSuggestionsAsync();
		}
	}

	private async Task HandleInputFocus()
	{
		_currentlyFocused = true;

		if (SuggestMinimumLengthEffective == 0)
		{
			await UpdateSuggestionsAsync(delayDropdownShow: _mouseDownFocus);
		}
		_mouseDownFocus = false;
	}

	[JSInvokable("HxInputTagsInternal_HandleInputBlur")]
	public async Task HandleInputBlur(bool isWithinDropdown)
	{
		_currentlyFocused = false;

		CancelDelayedSuggestionsUpdate();

		if (!isWithinDropdown)
		{
			await TryProcessCustomTagsAsync();
		}
	}

	private async Task TryProcessCustomTagsAsync(bool keepLastTagForSuggestion = false)
	{
		if (AllowCustomTags)
		{
			// tags before last delimiter
			char[] delimitersArray = DelimitersEffective.ToArray();
			var delimiterIndex = _userInput.IndexOfAny(delimitersArray);
			while (delimiterIndex >= 0)
			{
				var tag = _userInput.Substring(0, delimiterIndex).Trim(delimitersArray);
				_userInput = _userInput.Substring(delimiterIndex).TrimStart(delimitersArray);

				if (!String.IsNullOrWhiteSpace(tag))
				{
					await AddTagWithEventCallbackAsync(tag);
				}

				delimiterIndex = _userInput.IndexOfAny(delimitersArray);
			}
		}

		// last tag
		if (!keepLastTagForSuggestion)
		{
			if (AllowCustomTags)
			{
				var newTag = _userInput?.Trim(DelimitersEffective.ToArray());
				if (!String.IsNullOrWhiteSpace(newTag))
				{
					await AddTagWithEventCallbackAsync(newTag);
				}
			}
			_userInput = String.Empty;
		}

		StateHasChanged();
	}

	private async Task UpdateSuggestionsAsync(bool delayDropdownShow = false)
	{
		// Cancelation is performed in HandleInputInput method
		_cancellationTokenSource?.Dispose();

		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = _cancellationTokenSource.Token;

		// TODO Do we want spinner? Configurable?
		//dataProviderInProgress = true;
		//StateHasChanged();

		InputTagsDataProviderRequest request = new InputTagsDataProviderRequest
		{
			UserInput = _userInput,
			CancellationToken = cancellationToken
		};

		InputTagsDataProviderResult result;
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
		_focusedItemIndex = -1; // No item is focused after the suggestions are updated.

		_suggestions = result.Data.ToList();

		if (_suggestions?.Any() ?? false)
		{
			await OpenDropdownAsync(delayDropdownShow);
		}
		else
		{
			await TryDestroyDropdownAsync();
		}

		StateHasChanged();
	}

	#region KeyboardNavigation
	private int _focusedItemIndex = -1;

	[JSInvokable("HxInputTagsInternal_HandleInputKeyDown")]
	public async Task HandleInputKeyDown(string keyCode)
	{
		if ((keyCode == KeyCodes.Backspace) && String.IsNullOrWhiteSpace(_userInput) && ValueEffective.Any())
		{
			await RemoveTagWithEventCallbackAsync(ValueEffective.Last());
		}

		// Confirm selection on the focused item if an item is focused and the enter key is pressed.
		// Otherwise, if the user presses enter, try to process the custom tag(s).
		string focusedItem = GetItemByIndex(_focusedItemIndex);
		if ((keyCode == KeyCodes.Enter) || (keyCode == KeyCodes.NumpadEnter))
		{
			CancelDelayedSuggestionsUpdate();
			await TryDestroyDropdownAsync();
			if ((focusedItem is not null) && (!focusedItem.Equals(default)))
			{
				await HandleItemSelected(focusedItem);
			}
			else
			{
				await TryProcessCustomTagsAsync();
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
			}
		}
		else if (keyCode == KeyCodes.ArrowDown)
		{
			int nextItemIndex = _focusedItemIndex + 1;
			if (nextItemIndex < _suggestions?.Count)
			{
				_focusedItemIndex = nextItemIndex;
				StateHasChanged();
			}
		}
	}

	private string GetItemByIndex(int index)
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

	private async Task HandleItemSelected(string tag)
	{
		_isDropdownOpened = false; // dropdown is closed because the user selected an item

		// user clicked on an item in the "dropdown".
		_userInput = String.Empty;
		await AddTagWithEventCallbackAsync(tag);

		if (!_currentlyFocused)
		{
			await FocusAsync();
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
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
	}

	protected override void OnParametersSet()
	{
		if (string.IsNullOrWhiteSpace(InputId))
		{
			InputId = "hx" + Guid.NewGuid().ToString("N");
		}
	}

	private async Task OpenDropdownAsync(bool delayShow = false)
	{
		if (!_isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("open", _inputComponent.InputElement, _dotnetObjectReference, delayShow);
			_isDropdownOpened = true;
		}
	}

	private async Task TryDestroyDropdownAsync()
	{
		if (_isDropdownOpened)
		{
			await DestroyDropdownAsync();
		}
	}

	private async Task DestroyDropdownAsync()
	{
		await EnsureJsModuleAsync();
		await _jsModule.InvokeVoidAsync("destroy", _inputComponent.InputElement);

		_isDropdownOpened = false;
		StateHasChanged();
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxInputTags));
	}

	[JSInvokable("HxInputTagsInternal_HandleDropdownHidden")]
	public async Task HandleDropdownHidden()
	{
		_isDropdownOpened = false;
		if (!_currentlyFocused)
		{
			await TryProcessCustomTagsAsync();
			_userInput = String.Empty;
			StateHasChanged();
		}
		if (SuggestMinimumLengthEffective > 0)
		{
			await DestroyDropdownAsync();
		}
	}

	protected async Task HandleRemoveClickAsync(string tag)
	{
		await RemoveTagWithEventCallbackAsync(tag);
	}

	protected string GetFormControlCssClasses()
	{
		if (Naked)
		{
			return null;
		}
		return CssClassHelper.Combine(CoreFormControlCssClass, InputSizeEffective.AsFormControlCssClass());
	}

	protected string GetNakedCssClasses()
	{
		if (!Naked)
		{
			return null;
		}
		return CssClassHelper.Combine("hx-input-tags-naked", InputSizeEffective switch
		{
			InputSize.Regular => null,
			InputSize.Small => "hx-input-tags-naked-sm",
			InputSize.Large => "hx-input-tags-naked-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {InputSizeEffective}.")
		});
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

		_dotnetObjectReference.Dispose();
	}
}
