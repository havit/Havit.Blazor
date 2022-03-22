using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A full-text search component.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class HxSearchBox<TItem>
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
	[Parameter] public RenderFragment EmptyTemplate { get; set; }

	/// <summary>
	/// Rendered when no input is entered (i.e. initial state).
	/// </summary>
	[Parameter] public RenderFragment EmptyInputTemplate { get; set; }

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
	/// Label of the input field.
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <summary>
	/// Label type of the input field.
	/// </summary>
	[Parameter] public LabelType LabelType { get; set; }

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


	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference jsModule;
	private string dropdownToggleElementId = "hx" + Guid.NewGuid().ToString("N");
	private List<TItem> searchResults = new();
	private bool dropdownMenuVisible = false;

	public async Task ClearInputAsync()
	{
		if (TextQuery != string.Empty)
		{
			TextQuery = string.Empty;
			await HandleTextQueryValueChanged(string.Empty);
			await HandleTextQueryTriggered();
		}
	}

	public async Task UpdateSuggestionsAsync()
	{
		if (string.IsNullOrEmpty(TextQuery) || TextQuery.Length < MinimumLengthEffective)
		{
			return;
		}

		SearchBoxDataProviderRequest request = new()
		{
			UserInput = TextQuery,
			CancellationToken = default // TODO debounce Delay
		};

		var result = await DataProvider.Invoke(request);
		searchResults = result.Data.ToList();

		await InvokeAsync(StateHasChanged);
	}

	protected async Task HandleTextQueryValueChanged(string newTextQuery)
	{
		this.TextQuery = newTextQuery;
		await InvokeTextQueryChangedAsync(newTextQuery);
		await UpdateSuggestionsAsync();
	}

	protected async Task HandleTextQueryTriggered()
	{
		if (AllowTextQuery)
		{
			await InvokeOnTextQueryTriggeredAsync(this.TextQuery);
			await ToggleDropdownMenu();
		}
	}

	protected async Task HandleItemSelected(TItem item)
	{
		await InvokeOnItemSelectedAsync(item);
		await ToggleDropdownMenu();
		await ClearInputAsync();
	}

	protected async Task EnsureJsModule()
	{
		jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxSearchBox<TItem>) + ".js");
	}

	private async Task EnsureDropdownMenuVisibility()
	{
		if (dropdownMenuVisible)
		{
			await EnsureJsModule();
			await jsModule.InvokeVoidAsync("clickElement", dropdownToggleElementId);
		}
	}
	private async Task MakeDropdownMenuVisible()
	{
		if (!dropdownMenuVisible)
		{
			await ToggleDropdownMenu();
		}
	}
	private async Task ToggleDropdownMenu()
	{
		await EnsureJsModule();
		await jsModule.InvokeVoidAsync("clickElement", dropdownToggleElementId);
	}

	private async Task SetDropdownMenuVisibility(bool value)
	{
		dropdownMenuVisible = value;
		await InvokeAsync(StateHasChanged);
	}
}
