using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Internal implementation for <see cref="HxInputTags"/>.
/// </summary>
public partial class HxInputTagsInternal
{
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

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML input.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }


	[Inject] protected IJSRuntime JSRuntime { get; set; }

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	private string dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private System.Timers.Timer timer;
	private string userInput = String.Empty;
	private CancellationTokenSource cancellationTokenSource;
	private List<string> suggestions;
	private bool isDropdownOpened = false;
	private bool currentlyFocused;
	private bool mouseDownFocus;
	private bool disposed;
	private IJSObjectReference jsModule;
	private HxInputTagsInputInternal inputComponent;
	private bool dataProviderInProgress;
	private DotNetObjectReference<HxInputTagsInternal> dotnetObjectReference;

	public HxInputTagsInternal()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
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

		Value = Value.Except(new string[] { tag }).ToList();
		await ValueChanged.InvokeAsync(Value);
	}

	public async Task FocusAsync()
	{
		if (disposed)
		{
			return;
		}
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("tryFocus", inputComponent.InputElement);
	}

	private async Task HandleInputInput(string newUserInput)
	{
		// user changes an input
		userInput = newUserInput ?? String.Empty;

		CancelDelayedSuggestionsUpdate();

		// tag delimiters
		await TryProcessCustomTagsAsync(keepLastTagForSuggestion: true);

		if (DataProvider is not null)
		{
			if (userInput.Length >= SuggestMinimumLengthEffective)
			{
				if (SuggestDelayEffective == 0)
				{
					await UpdateSuggestionsAsync();
				}
				else
				{
					// start new time interval
					if (timer == null)
					{
						timer = new System.Timers.Timer();
						timer.AutoReset = false; // just once
						timer.Elapsed += HandleTimerElapsed;
					}
					timer.Interval = SuggestDelayEffective;
					timer.Start();
				}
			}
			else
			{
				// or close a dropdown
				suggestions = null;
				await TryDestroyDropdownAsync();
			}
		}
	}

	private void CancelDelayedSuggestionsUpdate()
	{
		timer?.Stop(); // if waiting for an interval, stop it
		cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		dataProviderInProgress = false; // data provider is no longer in progress				 
	}

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
		if (!currentlyFocused)
		{
			mouseDownFocus = true;
		}
	}

	private async Task HandleInputClick()
	{
		if (currentlyFocused && (SuggestMinimumLengthEffective == 0) && !isDropdownOpened)
		{
			await UpdateSuggestionsAsync();
		}
	}

	private async Task HandleInputFocus()
	{
		currentlyFocused = true;

		if (SuggestMinimumLengthEffective == 0)
		{
			await UpdateSuggestionsAsync(delayDropdownShow: mouseDownFocus);
		}
		mouseDownFocus = false;
	}

	[JSInvokable("HxInputTagsInternal_HandleInputBlur")]
	public async Task HandleInputBlur(bool isWithinDropdown)
	{
		currentlyFocused = false;

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
			var delimiterIndex = userInput.IndexOfAny(delimitersArray);
			while (delimiterIndex >= 0)
			{
				var tag = userInput.Substring(0, delimiterIndex).Trim(delimitersArray);
				userInput = userInput.Substring(delimiterIndex).TrimStart(delimitersArray);

				if (!String.IsNullOrWhiteSpace(tag))
				{
					await AddTagWithEventCallbackAsync(tag);
				}

				delimiterIndex = userInput.IndexOfAny(delimitersArray);
			}
		}

		// last tag
		if (!keepLastTagForSuggestion)
		{
			if (AllowCustomTags)
			{
				var newTag = userInput?.Trim(DelimitersEffective.ToArray());
				if (!String.IsNullOrWhiteSpace(newTag))
				{
					await AddTagWithEventCallbackAsync(newTag);
				}
			}
			userInput = String.Empty;
		}

		StateHasChanged();
	}

	private async Task UpdateSuggestionsAsync(bool delayDropdownShow = false)
	{
		// Cancelation is performed in HandleInputInput method
		cancellationTokenSource?.Dispose();

		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = cancellationTokenSource.Token;

		// TODO Do we want spinner? Configurable?
		//dataProviderInProgress = true;
		//StateHasChanged();

		InputTagsDataProviderRequest request = new InputTagsDataProviderRequest
		{
			UserInput = userInput,
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

		dataProviderInProgress = false;

		// KeyboardNavigation
		focusedItemIndex = -1; // No item is focused after the suggestions are updated.

		suggestions = result.Data.ToList();

		if (suggestions?.Any() ?? false)
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
	private int focusedItemIndex = -1;

	[JSInvokable("HxInputTagsInternal_HandleInputKeyDown")]
	public async Task HandleInputKeyDown(string keyCode)
	{
		if ((keyCode == KeyCodes.Backspace) && String.IsNullOrWhiteSpace(userInput) && ValueEffective.Any())
		{
			await RemoveTagWithEventCallbackAsync(ValueEffective.Last());
		}

		// Confirm selection on the focused item if an item is focused and the enter key is pressed.
		// Otherwise, if the user presses enter, try to process the custom tag(s).
		string focusedItem = GetItemByIndex(focusedItemIndex);
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
			int previousItemIndex = focusedItemIndex - 1;
			if (previousItemIndex >= 0)
			{
				focusedItemIndex = previousItemIndex;
				StateHasChanged();
			}
		}
		else if (keyCode == KeyCodes.ArrowDown)
		{
			int nextItemIndex = focusedItemIndex + 1;
			if (nextItemIndex < suggestions?.Count)
			{
				focusedItemIndex = nextItemIndex;
				StateHasChanged();
			}
		}
	}

	private string GetItemByIndex(int index)
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

	private async Task HandleItemSelected(string tag)
	{
		isDropdownOpened = false; // dropdown is closed because the user selected an item

		// user clicked on an item in the "dropdown".
		userInput = String.Empty;
		await AddTagWithEventCallbackAsync(tag);

		if (!currentlyFocused)
		{
			await FocusAsync();
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			string[] keysToPreventDefault = [KeyCodes.ArrowDown, KeyCodes.ArrowUp, KeyCodes.Enter, KeyCodes.NumpadEnter];
			await jsModule.InvokeVoidAsync("initialize", InputId, dotnetObjectReference, keysToPreventDefault);
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
		if (!isDropdownOpened)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			await jsModule.InvokeVoidAsync("open", inputComponent.InputElement, dotnetObjectReference, delayShow);
			isDropdownOpened = true;
		}
	}

	private async Task TryDestroyDropdownAsync()
	{
		if (isDropdownOpened)
		{
			await DestroyDropdownAsync();
		}
	}

	private async Task DestroyDropdownAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("destroy", inputComponent.InputElement);

		isDropdownOpened = false;
		StateHasChanged();
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxInputTags));
	}

	[JSInvokable("HxInputTagsInternal_HandleDropdownHidden")]
	public async Task HandleDropdownHidden()
	{
		isDropdownOpened = false;
		if (!currentlyFocused)
		{
			await TryProcessCustomTagsAsync();
			userInput = String.Empty;
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
		return CssClassHelper.Combine(this.CoreFormControlCssClass, this.InputSizeEffective.AsFormControlCssClass());
	}

	protected string GetNakedCssClasses()
	{
		if (!Naked)
		{
			return null;
		}
		return CssClassHelper.Combine("hx-input-tags-naked", this.InputSizeEffective switch
		{
			InputSize.Regular => null,
			InputSize.Small => "hx-input-tags-naked-sm",
			InputSize.Large => "hx-input-tags-naked-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {this.InputSizeEffective}.")
		});
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

		dotnetObjectReference.Dispose();
	}
}
