using System.Threading;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A search input component witch automatic suggestions, initial dropdown template and free-text queries support.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSearchBox">https://havit.blazor.eu/components/HxSearchBox</see>
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class HxSearchBox<TItem> : IAsyncDisposable
{
	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descandants (use separate set of defaults).
	/// </summary>
	protected virtual SearchBoxSettings GetDefaults() => HxSearchBox.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxSearchBox.Defaults"/>, overriden by individual parameters).
	/// </summary>
	[Parameter] public SearchBoxSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
	/// </remarks>
	protected virtual SearchBoxSettings GetSettings() => this.Settings;

	/// <summary>
	/// Method (delegate) which provides data of the suggestions.
	/// </summary>
	[Parameter] public SearchBoxDataProviderDelegate<TItem> DataProvider { get; set; }

	/// <summary>
	/// Allows you to disable the input. Default is <c>true</c>.
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
	/// Raised, when the enter key is pressed or when the text-query item is selected in the dropdown menu.
	/// (Does not trigger when <see cref="AllowTextQuery"/> is <c>false</c>.)
	/// </summary>
	[Parameter] public EventCallback<string> OnTextQueryTriggered { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnTextQueryTriggered"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnTextQueryTriggeredAsync(string textQuery) => OnTextQueryTriggered.InvokeAsync(textQuery);

	/// <summary>
	/// Occurs, when any of suggested items (other than plain text-query) is selected.
	/// </summary>
	[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnItemSelected"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnItemSelectedAsync(TItem selectedItem) => OnItemSelected.InvokeAsync(selectedItem);

	/// <summary>
	/// Placeholder text for the search input.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Selector to display item title from data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTitleSelector { get; set; }

	/// <summary>
	/// Selector to display item subtitle from data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemSubtitleSelector { get; set; }

	/// <summary>
	/// Selector to display icon from data item.
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
	/// Additional css classes for the dropdown.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS classes for the items in the dropdown menu.
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }
	protected string ItemCssClassEffective => this.ItemCssClass ?? this.GetSettings()?.ItemCssClass ?? GetDefaults().ItemCssClass;

	/// <summary>
	/// Additional CSS classes for the search box input.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }
	protected string InputCssClassEffective => this.ItemCssClass ?? this.GetSettings()?.InputCssClass ?? GetDefaults().InputCssClass;

	/// <summary>
	/// Icon of the input, when no text is written.
	/// </summary>
	[Parameter] public IconBase SearchIcon { get; set; }
	protected IconBase SearchIconEffective => this.SearchIcon ?? this.GetSettings()?.SearchIcon ?? GetDefaults().SearchIcon;

	/// <summary>
	/// Icon of the input, when text is written allowing the user to clear the text.
	/// </summary>
	[Parameter] public IconBase ClearIcon { get; set; }
	protected IconBase ClearIconEffective => this.ClearIcon ?? this.GetSettings()?.ClearIcon ?? GetDefaults().ClearIcon;

	/// <summary>
	/// Offset between the dropdown and the input.
	/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
	/// </summary>
	[Parameter] public (int Skidding, int Distance) DropdownOffset { get; set; } = (0, 4);

	/// <summary>
	/// Label of the input field.
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <summary>
	/// Label type of the input field.
	/// </summary>
	[Parameter] public LabelType LabelType { get; set; }

	/// <summary>
	/// Input size of the input field.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => this.InputSize ?? this.GetSettings()?.InputSize ?? this.GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Minimum lenght to call the data provider (display any results). Default is <c>2</c>.
	/// </summary>
	[Parameter] public int? MinimumLength { get; set; }
	protected int MinimumLengthEffective => this.MinimumLength ?? this.GetSettings()?.MinimumLength ?? GetDefaults().MinimumLength ?? throw new InvalidOperationException(nameof(MinimumLength) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Debounce delay in miliseconds. Default is <c>300</c> ms.
	/// </summary>
	[Parameter] public int? Delay { get; set; }
	protected int DelayEffective => this.Delay ?? this.GetSettings()?.Delay ?? GetDefaults().Delay ?? throw new InvalidOperationException(nameof(Delay) + " default for " + nameof(HxSearchBox) + " has to be set.");

	/// <summary>
	/// Indicates whether text-query mode is enabled (accepts free text in addition to suggested items).
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

	private string dropdownToggleElementId = "hx" + Guid.NewGuid().ToString("N");
	private string dropdownId = "hx" + Guid.NewGuid().ToString("N");
	private List<TItem> searchResults = new();
	private HxDropdownToggleElement dropdownToggle;
	private bool dropdownMenuActive = false;
	private bool initialized = false;
	/// <summary>
	/// Shows whether the <see cref="TextQuery"/> has been below minimum required length recently (before data provider loading is completed).
	/// </summary>
	private bool textQueryHasBeenBelowMinimumLength = true;

	private System.Timers.Timer timer;
	private CancellationTokenSource cancellationTokenSource;
	private bool dataProviderInProgress;

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			initialized = true;
		}
	}

	protected async Task ClearInputAsync()
	{
		if (TextQuery != string.Empty)
		{
			TextQuery = string.Empty;
			await HandleTextQueryValueChanged(string.Empty);
			await HandleTextQueryTriggered();
		}
	}

	protected async Task UpdateSuggestionsAsync()
	{
		await HideDropdownMenu();

		if (string.IsNullOrEmpty(TextQuery) || TextQuery.Length < MinimumLengthEffective)
		{
			return;
		}

		// Cancelation is performed in HandleTextQueryValueChanged method
		cancellationTokenSource?.Dispose();

		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = cancellationTokenSource.Token;

		dataProviderInProgress = true;
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

		dataProviderInProgress = false;
		searchResults = result?.Data.ToList();

		textQueryHasBeenBelowMinimumLength = false;
		await ShowDropdownMenu();

		StateHasChanged();
	}

	protected async Task HandleTextQueryValueChanged(string newTextQuery)
	{
		this.TextQuery = newTextQuery;

		CancelDataProviderAndDebounce();

		// start new time interval
		if (TextQuery.Length >= MinimumLengthEffective)
		{
			if (timer == null)
			{
				timer = new System.Timers.Timer
				{
					AutoReset = false // just once
				};
				timer.Elapsed += HandleTimerElapsed;
			}
			timer.Interval = DelayEffective;
			timer.Start();
		}
		else
		{
			textQueryHasBeenBelowMinimumLength = true;
		}

		if (ShouldDropdownMenuBeDisplayed())
		{
			await ShowDropdownMenu();
		}
		else if (dropdownMenuActive)
		{
			await HideDropdownMenu();
		}
		await InvokeTextQueryChangedAsync(newTextQuery);
	}

	private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		// when a time interval reached, update suggestions
		await InvokeAsync(async () =>
		{
			await UpdateSuggestionsAsync();
		});
	}
	private void HandleInputBlur()
	{
		CancelDataProviderAndDebounce();
	}

	private void CancelDataProviderAndDebounce()
	{
		timer?.Stop(); // if waiting for an interval, stop it
		cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		dataProviderInProgress = false; // data provider is no more in progress
	}

	protected async Task HandleTextQueryTriggered()
	{
		if (AllowTextQuery && (TextQuery.Length >= MinimumLengthEffective || TextQuery.Length == 0))
		{
			CancelDataProviderAndDebounce();

			await HideDropdownMenu();
			await InvokeOnTextQueryTriggeredAsync(this.TextQuery);
		}
	}

	protected async Task HandleItemSelected(TItem item)
	{
		await InvokeOnItemSelectedAsync(item);
		await ClearInputAsync();
	}

	protected async Task OnDropdownMenuShown()
	{
		dropdownMenuActive = true;

		if (!ShouldDropdownMenuBeDisplayed())
		{
			await HideDropdownMenu();
		}
	}

	protected async Task ShowDropdownMenu()
	{
		await dropdownToggle.ShowAsync();
	}
	protected async Task HideDropdownMenu()
	{
		await dropdownToggle.HideAsync();
	}

	/// <summary>
	/// If the <see cref="DefaultContentTemplate"/> is empty, we don't want to display anything when nothing (or below the minimum amount of characters) is typed into the input.
	/// </summary>
	/// <returns></returns>
	protected bool ShouldDropdownMenuBeDisplayed()
	{
		if (textQueryHasBeenBelowMinimumLength && (TextQuery is not null && TextQuery.Length >= MinimumLengthEffective))
		{
			return false;
		}

		if (DefaultContentTemplate is null && (TextQuery is null || TextQuery.Length < MinimumLengthEffective))
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
		timer?.Dispose();
		timer = null;

		cancellationTokenSource?.Dispose();
		cancellationTokenSource = null;

		await dropdownToggle.DisposeAsync();
	}
}
