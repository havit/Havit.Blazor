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

	[Parameter] public LabelType LabelTypeEffective { get; set; }

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


	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	private string dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private System.Timers.Timer timer;
	private string userInput = String.Empty;
	private CancellationTokenSource cancellationTokenSource;
	private List<TItem> suggestions;
	private bool userInputModified;
	private bool isDropdownOpened = false;
	private bool blurInProgress;
	private bool currentlyFocused;
	private bool disposed;
	private IJSObjectReference jsModule;
	private HxAutosuggestInputInternal autosuggestInput;
	private TValue lastKnownValue;
	private bool dataProviderInProgress;
	private DotNetObjectReference<HxAutosuggestInternal<TItem, TValue>> dotnetObjectReference;

	internal string ChipValue => userInput;

	public HxAutosuggestInternal()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	private async Task SetValueItemWithEventCallback(TItem selectedItem)
	{
		TValue value = SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, selectedItem);

		if (!EqualityComparer<TValue>.Default.Equals(Value, value))
		{
			Value = value;
			lastKnownValue = Value;
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
			if (!EqualityComparer<TValue>.Default.Equals(Value, lastKnownValue))
			{
				if ((ItemFromValueResolver == null) && (typeof(TValue) == typeof(TItem)))
				{
					userInput = TextSelectorEffective((TItem)(object)Value);
				}
				else
				{
					Contract.Requires<InvalidOperationException>(ItemFromValueResolver is not null, $"{GetType()} requires a {nameof(ItemFromValueResolver)} parameter.");
					userInput = TextSelectorEffective(await ItemFromValueResolver(Value));
				}
			}
		}
		else
		{
			userInput = TextSelectorEffective(default);
		}
		userInputModified = false;
		lastKnownValue = Value;

		if (string.IsNullOrWhiteSpace(InputId))
		{
			InputId = "hx" + Guid.NewGuid().ToString("N");
		}
	}

	public async ValueTask FocusAsync()
	{
		await autosuggestInput.FocusAsync();
	}

	private async Task HandleInputInput(string newUserInput)
	{
		Contract.Requires<InvalidOperationException>(EnabledEffective, $"The {GetType().Name} component is in a disabled state.");

		// user changes an input
		userInput = newUserInput;
		userInputModified = true;

		timer?.Stop(); // if waiting for an interval, stop it
		cancellationTokenSource?.Cancel(); // if already loading data, cancel it
		dataProviderInProgress = false; // data provider is no more in progress

		// start new time interval
		if (userInput.Length >= MinimumLengthEffective)
		{
			if (DelayEffective == 0)
			{
				await UpdateSuggestionsAsync();
			}
			else
			{
				if (timer == null)
				{
					timer = new System.Timers.Timer();
					timer.AutoReset = false; // just once
					timer.Elapsed += HandleTimerElapsed;
				}
				timer.Interval = DelayEffective;
				timer.Start();
			}
		}
		else
		{
			// or close a dropdown
			suggestions = null;
			await DestroyDropdownAsync();
		}
	}

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
		if (!EnabledEffective)
		{
			return;
		}

		currentlyFocused = true;
		if (string.IsNullOrEmpty(userInput) && MinimumLengthEffective <= 0)
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

		currentlyFocused = false;
		// when user clicks back button in browser this method can be called after it is disposed!
		blurInProgress = true;
		timer?.Stop(); // if waiting for an interval, stop it
		cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		dataProviderInProgress = false; // data provider is no more in progress
	}

	private async Task UpdateSuggestionsAsync()
	{
		// Cancelation is performed in HandleInputInput method
		cancellationTokenSource?.Dispose();

		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = cancellationTokenSource.Token;

		dataProviderInProgress = true;
		StateHasChanged();

		AutosuggestDataProviderRequest request = new AutosuggestDataProviderRequest
		{
			UserInput = userInput,
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

		dataProviderInProgress = false;

		// KeyboardNavigation
		focusedItemIndex = 0; // First item in the searchResults collection.

		suggestions = result.Data?.ToList();

		if ((suggestions?.Any() ?? false) || (EmptyTemplate != null))
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
	private int focusedItemIndex = -1;

	[JSInvokable("HxAutosuggestInternal_HandleInputKeyDown")]
	public async Task HandleInputKeyDown(string keyCode)
	{
		// Confirm selection on the focused item if an item is focused and the enter key is pressed.
		TItem focusedItem = GetItemByIndex(focusedItemIndex);
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
			int previousItemIndex = focusedItemIndex - 1;
			if (previousItemIndex >= 0)
			{
				focusedItemIndex = previousItemIndex;
				StateHasChanged();

				await jsModule.InvokeVoidAsync("scrollToSelectedItem", dropdownId);
			}
		}
		else if (keyCode == KeyCodes.ArrowDown)
		{
			int nextItemIndex = focusedItemIndex + 1;
			if (nextItemIndex < suggestions?.Count)
			{
				focusedItemIndex = nextItemIndex;
				StateHasChanged();

				await jsModule.InvokeVoidAsync("scrollToSelectedItem", dropdownId);
			}
		}
	}

	private TItem GetItemByIndex(int index)
	{
		if ((index >= 0) && (index < suggestions?.Count))
		{
			return suggestions[index];
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
		userInput = TextSelectorEffective(item);
		userInputModified = false;
	}

	private async Task HandleCrossClick()
	{
		// user clicked on a cross button (x)
		await SetValueItemWithEventCallback(default);
		userInput = TextSelectorEffective(default);
		userInputModified = false;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			await jsModule.InvokeVoidAsync("initialize", InputId, dotnetObjectReference, new string[] { KeyCodes.ArrowDown, KeyCodes.ArrowUp });
		}

		if (blurInProgress)
		{
			blurInProgress = false;
			if (userInputModified && !isDropdownOpened)
			{
				await SetValueItemWithEventCallback(default);
				userInput = TextSelectorEffective(default);
				userInputModified = false;
				StateHasChanged();
			}
		}
	}
	#region OpenDropdownAsync, DestroyDropdownAsync, EnsureJsModuleAsync
	private async Task OpenDropdownAsync()
	{
		if (!isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			await jsModule.InvokeVoidAsync("open", autosuggestInput.InputElement, dotnetObjectReference);
			isDropdownOpened = true;
		}
	}

	private async Task DestroyDropdownAsync()
	{
		if (isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("destroy", autosuggestInput.InputElement);
			isDropdownOpened = false;
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAutosuggest));
	}

	[JSInvokable("HxAutosuggestInternal_HandleDropdownHidden")]
	public async Task HandleDropdownHidden()
	{
		if (userInputModified && !currentlyFocused)
		{
			await SetValueItemWithEventCallback(default);
			userInput = TextSelectorEffective(default);
			userInputModified = false;
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
		disposed = true;

		timer?.Dispose();
		timer = null;

		cancellationTokenSource?.Dispose();
		cancellationTokenSource = null;

		if (jsModule != null)
		{
			try
			{
				await jsModule.InvokeVoidAsync("dispose", InputId);
				await jsModule.DisposeAsync();
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

		dotnetObjectReference?.Dispose();
	}
}