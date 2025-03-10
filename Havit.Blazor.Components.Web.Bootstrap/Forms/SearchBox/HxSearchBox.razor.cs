using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A search input component with automatic suggestions, initial dropdown template, and support for free-text queries.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSearchBox">https://havit.blazor.eu/components/HxSearchBox</see>
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class HxSearchBox<TItem> : IAsyncDisposable, IInputWithSize, IInputWithLabelType
{
	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual SearchBoxSettings GetDefaults() => HxSearchBox.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxSearchBox.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public SearchBoxSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual SearchBoxSettings GetSettings() => Settings;

	/// <summary>
	/// Method (delegate) that provides data for the suggestions.
	/// </summary>
	[Parameter] public SearchBoxDataProviderDelegate<TItem> DataProvider { get; set; }

	/// <summary>
	/// Allows you to disable the input. The default is <c>true</c>.
	/// </summary>
	[Parameter] public bool Enabled { get; set; } = true;

	/// <summary>
	/// Text written by the user (input text).
	/// </summary>
	[Parameter]
	public string TextQuery { get; set; }
	[Parameter] public EventCallback<string> TextQueryChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="TextQueryChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeTextQueryChangedAsync(string newTextQueryValue) => TextQueryChanged.InvokeAsync(newTextQueryValue);

	/// <summary>
	/// Raised when the enter key is pressed or when the text-query item is selected in the dropdown menu.
	/// (Does not trigger when <see cref="AllowTextQuery"/> is <c>false</c>.)
	/// </summary>
	[Parameter] public EventCallback<string> OnTextQueryTriggered { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnTextQueryTriggered"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnTextQueryTriggeredAsync(string textQuery) => OnTextQueryTriggered.InvokeAsync(textQuery);

	/// <summary>
	/// Occurs when any of the suggested items (other than plain text-query) is selected.
	/// </summary>
	[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnItemSelected"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnItemSelectedAsync(TItem selectedItem) => OnItemSelected.InvokeAsync(selectedItem);

	/// <summary>
	/// Behavior when the item is selected.
	/// </summary>
	[Parameter] public SearchBoxItemSelectionBehavior? ItemSelectionBehavior { get; set; }
	protected SearchBoxItemSelectionBehavior ItemSelectionBehaviorEffective => ItemSelectionBehavior ?? GetSettings()?.ItemSelectionBehavior ?? GetDefaults().ItemSelectionBehavior ?? throw new InvalidOperationException(nameof(ItemSelectionBehavior) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Placeholder text for the search input.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Selector to display the item title from the data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTitleSelector { get; set; }

	/// <summary>
	/// Selector to display the item subtitle from the data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemSubtitleSelector { get; set; }

	/// <summary>
	/// Selector to display the icon from the data item.
	/// </summary>
	[Parameter] public Func<TItem, IconBase> ItemIconSelector { get; set; }

	/// <summary>
	/// Template for the item content.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// Template for the text-query item content (requires <c><see cref="AllowTextQuery"/>="true"</c>).
	/// </summary>
	[Parameter] public RenderFragment<string> TextQueryItemTemplate { get; set; }

	/// <summary>
	/// Rendered when the <see cref="DataProvider" /> doesn't return any data.
	/// </summary>
	[Parameter] public RenderFragment NotFoundTemplate { get; set; }

	/// <summary>
	/// Rendered when no input is entered (i.e. initial state).
	/// </summary>
	[Parameter] public RenderFragment DefaultContentTemplate { get; set; }

	/// <summary>
	/// Additional CSS classes for the dropdown.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS classes for the items in the dropdown menu.
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }
	protected string ItemCssClassEffective => ItemCssClass ?? GetSettings()?.ItemCssClass ?? GetDefaults().ItemCssClass;

	/// <summary>
	/// Additional CSS classes for the search box input.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }
	protected string InputCssClassEffective => InputCssClass ?? GetSettings()?.InputCssClass ?? GetDefaults().InputCssClass;

	/// <summary>
	/// Custom CSS class to render with the input-group span.
	/// </summary>
	[Parameter] public string InputGroupCssClass { get; set; }
	protected string InputGroupCssClassEffective => InputGroupCssClass ?? GetSettings()?.InputGroupCssClass ?? GetDefaults().InputGroupCssClass;

	/// <summary>
	/// Icon of the input when no text is written.
	/// </summary>
	[Parameter] public IconBase SearchIcon { get; set; }
	protected IconBase SearchIconEffective => SearchIcon ?? GetSettings()?.SearchIcon ?? GetDefaults().SearchIcon;

	/// <summary>
	/// Placement of the search icon.<br/>
	/// Default is <see cref="SearchBoxSearchIconPlacement.End"/>.
	/// </summary>
	[Parameter] public SearchBoxSearchIconPlacement? SearchIconPlacement { get; set; }
	protected SearchBoxSearchIconPlacement SearchIconPlacementEffective => SearchIconPlacement ?? GetSettings()?.SearchIconPlacement ?? GetDefaults().SearchIconPlacement ?? throw new InvalidOperationException(nameof(SearchIconPlacement) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Icon of the input, displayed when text is entered, allowing the user to clear the text.
	/// </summary>
	[Parameter] public IconBase ClearIcon { get; set; }
	protected IconBase ClearIconEffective => ClearIcon ?? GetSettings()?.ClearIcon ?? GetDefaults().ClearIcon;

	/// <summary>
	/// Offset between the dropdown and the input.
	/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
	/// </summary>
	[Parameter] public (int Skidding, int Distance) DropdownOffset { get; set; } = (0, 4);

	/// <summary>
	/// Label of the input field.
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <inheritdoc cref="Bootstrap.LabelType" />
	[Parameter] public LabelType? LabelType { get; set; }
	protected LabelType LabelTypeEffective => LabelType ?? GetSettings()?.LabelType ?? GetDefaults()?.LabelType ?? HxSetup.Defaults.LabelType;
	LabelType IInputWithLabelType.LabelTypeEffective => LabelTypeEffective;

	/// <summary>
	/// Input size of the input field.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize;
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <summary>
	/// Defines whether the input may be checked for spelling errors. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Spellcheck { get; set; }
	protected bool? SpellcheckEffective => Spellcheck ?? GetSettings()?.Spellcheck ?? GetDefaults()?.Spellcheck;

	/// <summary>
	/// Minimum length to call the data provider (display any results). Default is <c>2</c>.
	/// </summary>
	[Parameter] public int? MinimumLength { get; set; }
	protected int MinimumLengthEffective => MinimumLength ?? GetSettings()?.MinimumLength ?? GetDefaults().MinimumLength ?? throw new InvalidOperationException(nameof(MinimumLength) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Debounce delay in milliseconds. Default is <c>300</c> ms.
	/// </summary>
	[Parameter] public int? Delay { get; set; }
	protected int DelayEffective => Delay ?? GetSettings()?.Delay ?? GetDefaults().Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Indicates whether text-query mode is enabled (accepts free text in addition to suggested items).<br/>
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool AllowTextQuery { get; set; } = true;

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// Input-group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// Input-group at the end of the input.<br/>
	/// Hides the search icon when used!
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// Input-group at the end of the input.<br/>
	/// Hides the search icon when used!
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	/// <summary>
	/// Fired immediately when the 'hide' method of the dropdown is called.
	/// To prevent hiding, set <see cref="DropdownHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// Exposed to allow derived custom components to cancel hiding the dropdown, for example, when the dropdown contains draggable content and the mouseup event is fired outside the dropdown.
	/// </remarks>
	[Parameter] public EventCallback<DropdownHidingEventArgs> OnHiding { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroups => HasInputGroupStart || HasInputGroupEnd;
	private bool HasInputGroupStart => !String.IsNullOrWhiteSpace(InputGroupStartText) || (InputGroupStartTemplate is not null);
	private bool HasInputGroupEnd => !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupEndTemplate is not null);
	private bool HasClearButton => !HasInputGroupEnd
							&& !_dataProviderInProgress
							&& !string.IsNullOrEmpty(TextQuery)
							&& (ClearIconEffective is not null);

	private string _dropdownToggleElementId = "hx" + Guid.NewGuid().ToString("N");
	private string _dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private string _inputId = "hx" + Guid.NewGuid().ToString("N");
	private ElementReference _inputElementReference;
	private List<TItem> _searchResults = new();
	private HxDropdownToggleElement _dropdownToggle;
	private bool _dropdownMenuActive = false;
	private bool _initialized = false;
	/// <summary>
	/// Indicates whether the <see cref="TextQuery"/> has been below minimum required length recently (before data provider loading is completed).
	/// </summary>
	private bool _textQueryHasBeenBelowMinimumLength = true;
	private System.Timers.Timer _timer;
	private CancellationTokenSource _cancellationTokenSource;
	private bool _dataProviderInProgress;
	private bool _inputFormHasFocus;
	private bool _scrollToFocusedItem;
	private IJSObjectReference _jsModule;
	private DotNetObjectReference<HxSearchBox<TItem>> _dotnetObjectReference;
	private bool _clickIsComing;
	private bool _disposed = false;

	public HxSearchBox()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		if ((LabelTypeEffective == Bootstrap.LabelType.Floating) && !String.IsNullOrEmpty(Placeholder))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Cannot use {nameof(Placeholder)} with floating labels.");
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_initialized = true;

			await EnsureJsModule();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _inputId, _dotnetObjectReference, new string[] { KeyCodes.ArrowUp, KeyCodes.ArrowDown });
		}

		if (_scrollToFocusedItem)
		{
			_scrollToFocusedItem = false;
			await _jsModule.InvokeVoidAsync("scrollToFocusedItem");
		}
	}

	/// <summary>
	/// Gives focus to the input element.
	/// </summary>
	public async Task FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(_inputElementReference, default))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The component reference is not available. You are most likely calling the method too early. The first render must complete before calling this method.");
		}
		await _inputElementReference.FocusAsync();
	}

	protected async Task EnsureJsModule()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxSearchBox));
	}

	protected async Task ClearInputAsync()
	{
		if (TextQuery != string.Empty)
		{
			TextQuery = string.Empty;
			await HandleTextQueryValueChanged(string.Empty);

			// #644 [HxSearchBox] Clear icon does not refresh data in typical usage scenarios (regression)
			// Although we can discuss whether the intention of the user was to trigger the text query, we invoke the callback to signalize the user submitted a new text query state.
			// Without this, the related UI won't be updated unless the TextQueryChanged callback is properly handled (which is not comfortable).
			await HandleTextQueryTriggered();
		}
	}

	protected async Task UpdateSuggestionsAsync()
	{
		if ((TextQuery?.Length ?? 0) < MinimumLengthEffective)
		{
			return;
		}

		// Cancelation is performed in HandleTextQueryValueChanged method
		_cancellationTokenSource?.Dispose();

		_cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = _cancellationTokenSource.Token;

		_dataProviderInProgress = true;
		StateHasChanged();

		SearchBoxDataProviderRequest request = new()
		{
			UserInput = TextQuery,
			CancellationToken = cancellationToken
		};
		SearchBoxDataProviderResult<TItem> result = null;

		try
		{
			result = await DataProvider.Invoke(request);
		}
		catch (OperationCanceledException)
		{
			return;
		}

		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		_dataProviderInProgress = false;

		// KeyboardNavigation
		if (AllowTextQuery)
		{
			_focusedItemIndex = InputKeyboardNavigationIndex; // Move focus to the input, so that free-text can be easily confirmed with Enter.
		}
		else
		{
			_focusedItemIndex = 0; // Move focus to the first item.
		}

		_searchResults = result?.Data?.ToList() ?? new();

		_textQueryHasBeenBelowMinimumLength = false;
		await ShowDropdownAsync();

		StateHasChanged();
	}

	private async Task HandleTextQueryValueChanged(string newTextQuery)
	{
		TextQuery = newTextQuery;

		CancelDataProviderAndDebounce();

		// start new time interval
		if ((TextQuery?.Length ?? 0) >= MinimumLengthEffective)
		{
			if (DelayEffective > 0)
			{
				if (_timer == null)
				{
					_timer = new System.Timers.Timer
					{
						AutoReset = false // just once
					};
					_timer.Elapsed += HandleTimerElapsed;
				}
				_timer.Interval = DelayEffective;
				_timer.Start();
			}
			else
			{
				await UpdateSuggestionsAsync();
			}
		}
		else
		{
			_textQueryHasBeenBelowMinimumLength = true;
		}

		if (ShouldDropdownMenuBeDisplayed())
		{
			await ShowDropdownAsync();
		}
		else if (_dropdownMenuActive)
		{
			await HideDropdownAsync();
		}
		await InvokeTextQueryChangedAsync(newTextQuery);
	}

	#region KeyboardNavigation
	private int _focusedItemIndex = -1;

	/// <summary>
	/// Input's index for the keyboard navigation. If this is the current index, then no item is selected.
	/// </summary>
	private const int InputKeyboardNavigationIndex = -1;

	private bool HasItemFocus(TItem item)
	{
		if ((_focusedItemIndex > InputKeyboardNavigationIndex) && (_focusedItemIndex < GetFreeTextItemIndex()))
		{
			TItem focusedItem = GetItemByIndex(_focusedItemIndex);
			if ((focusedItem is not null) && (!focusedItem.Equals(default)))
			{
				return item.Equals(focusedItem);
			}
		}

		return false;
	}

	private bool HasFreeTextItemFocus()
	{
		return _focusedItemIndex == GetFreeTextItemIndex();
	}

	[JSInvokable("HxSearchBox_HandleInputKeyDown")]
	public async Task HandleInputKeyDown(string keyCode)
	{
		// Confirm selection on the focused item if an item is focused and the enter key is pressed.
		TItem focusedItem = GetItemByIndex(_focusedItemIndex);
		if ((keyCode == KeyCodes.Enter) || (keyCode == KeyCodes.NumpadEnter))
		{
			if ((focusedItem is not null) && (!focusedItem.Equals(default)))
			{
				await HandleItemSelected(focusedItem);
			}
			else if (_focusedItemIndex == InputKeyboardNavigationIndex || _focusedItemIndex == GetFreeTextItemIndex()) // Confirm free-text (text query) if the input or the free-text item is focused and the enter key is pressed.
			{
				await HandleTextQueryTriggered();
			}
		}

		// Move focus up or down.
		if (keyCode == KeyCodes.ArrowUp)
		{
			int previousItemIndex = _focusedItemIndex - 1;
			int minimumIndex = AllowTextQuery ? InputKeyboardNavigationIndex : 0;

			if (previousItemIndex >= minimumIndex)
			{
				_focusedItemIndex = previousItemIndex;
				_scrollToFocusedItem = true;
				StateHasChanged();
			}
		}
		else if (keyCode == KeyCodes.ArrowDown)
		{
			int nextItemIndex = _focusedItemIndex + 1;
			int maximumIndex = AllowTextQuery ? GetFreeTextItemIndex() : (_searchResults?.Count ?? 0) - 1;

			if (nextItemIndex <= maximumIndex)
			{
				_focusedItemIndex = nextItemIndex;
				_scrollToFocusedItem = true;
				StateHasChanged();
			}
		}
	}

	[JSInvokable("HxSearchBox_HandleInputMouseDown")]
	public void HandleInputMouseDown()
	{
		_clickIsComing = true;
	}

	[JSInvokable("HxSearchBox_HandleInputMouseUp")]
	public void HandleInputMouseUp()
	{
		_clickIsComing = false;
	}

	[JSInvokable("HxSearchBox_HandleInputMouseLeave")]
	public void HandleInputMouseLeave()
	{
		_clickIsComing = false;
	}

	private TItem GetItemByIndex(int index)
	{
		if ((index >= 0) && (index < _searchResults.Count))
		{
			return _searchResults[index];
		}
		else
		{
			return default;
		}
	}

	private int GetFreeTextItemIndex()
	{
		return _searchResults.Count;
	}
	#endregion KeyboardNavigation

	[SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Required by Timer")]
	private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		// when a time interval reached, update suggestions
		await InvokeAsync(async () =>
		{
			await UpdateSuggestionsAsync();
		});
	}

	private async Task HandleInputFocus()
	{
		_inputFormHasFocus = true;

		// When MinimumLength is 0, we need to load/update initial suggestions
		if (((TextQuery?.Length ?? 0) == 0) && (MinimumLengthEffective == 0))
		{
			await UpdateSuggestionsAsync();
		}

		await ShowDropdownAsync();
	}

	private void HandleInputBlur()
	{
		_inputFormHasFocus = false;

		if (!_dropdownMenuActive)
		{
			ClearInputValueIfTextQueryDisabled();
		}
		CancelDataProviderAndDebounce();
	}

	private void ClearInputValueIfTextQueryDisabled()
	{
		if (!AllowTextQuery)
		{
			TextQuery = string.Empty;
		}
	}

	private void CancelDataProviderAndDebounce()
	{
		_timer?.Stop(); // if waiting for an interval, stop it
		_cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		_dataProviderInProgress = false; // data provider is no longer in progress
	}

	private async Task HandleTextQueryTriggered()
	{
		if (AllowTextQuery
			&& (((TextQuery?.Length ?? 0) >= MinimumLengthEffective) || ((TextQuery?.Length ?? 0) == 0)))
		{
			CancelDataProviderAndDebounce();

			await HideDropdownAsync();
			await InvokeOnTextQueryTriggeredAsync(TextQuery);
		}
	}

	private async Task HandleItemSelected(TItem item)
	{
		switch (ItemSelectionBehaviorEffective)
		{
			case SearchBoxItemSelectionBehavior.SelectAndClearTextQuery:
				TextQuery = String.Empty;
				break;
			case SearchBoxItemSelectionBehavior.SelectAndReplaceTextQueryWithItemTitle:
				TextQuery = ItemTitleSelector?.Invoke(item) ?? null;
				break;
			default:
				throw new InvalidOperationException($"Invalid {nameof(SearchBoxItemSelectionBehavior)} value: {ItemSelectionBehaviorEffective}");
		}

		await HideDropdownAsync();
		await InvokeTextQueryChangedAsync(TextQuery);
		await InvokeOnItemSelectedAsync(item);
	}

	private async Task HandleDropdownMenuShown()
	{
		_dropdownMenuActive = true;

		if (!ShouldDropdownMenuBeDisplayed())
		{
			await HideDropdownAsync();
		}
	}

	private void HandleDropdownMenuHidden()
	{
		_dropdownMenuActive = false;
		if (!_inputFormHasFocus)
		{
			ClearInputValueIfTextQueryDisabled();
		}
	}

	private async Task ShowDropdownAsync()
	{
		if (!_clickIsComing)
		{
			// clickIsComing logic fixes #572 - Initial suggestions disappear when the DataProvider response is quick
			// If click is coming, we do not want to show the dropdown as it will be toggled by the later click event (if we open it here, onfocus, click will hide it)
			await _dropdownToggle.ShowAsync();
		}
	}

	/// <summary>
	/// Hides the dropdown menu.
	/// </summary>
	/// <remarks>
	/// Allows custom actions from <see cref="DefaultContentTemplate" /> or <see cref="NotFoundTemplate" /> to hide the dropdown menu.
	/// </remarks>
	public async Task HideDropdownAsync()
	{
		await _dropdownToggle.HideAsync();
	}

	/// <summary>
	/// If the <see cref="DefaultContentTemplate"/> is empty, we don't want to display anything when nothing (or below the minimum amount of characters) is typed into the input.
	/// </summary>
	/// <returns></returns>
	private bool ShouldDropdownMenuBeDisplayed()
	{
		if (_textQueryHasBeenBelowMinimumLength
			&& ((TextQuery?.Length ?? 0) >= MinimumLengthEffective))
		{
			return false;
		}

		if ((DefaultContentTemplate is null)
			&& ((TextQuery?.Length ?? 0) < MinimumLengthEffective))
		{
			return false;
		}

		return true;
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
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
				await _jsModule.InvokeVoidAsync("dispose", _inputId);
				await _dropdownToggle.DisposeAsync();
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
