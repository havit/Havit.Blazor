using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxMultiSelectInternal<TValue, TItem> : IAsyncDisposable
{
	[Parameter] public string InputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public string InputText { get; set; }

	[Parameter] public bool EnabledEffective { get; set; }

	[Parameter] public List<TItem> ItemsToRender { get; set; }

	[Parameter] public List<TValue> SelectedValues { get; set; }

	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	[Parameter] public string NullDataText { get; set; }

	[Parameter] public EventCallback<SelectionChangedArgs> ItemSelectionChanged { get; set; }

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
	/// Enables filtering capabilities.
	/// </summary>
	[Parameter] public bool AllowFiltering { get; set; }

	[Parameter] public Func<TItem, string, bool> FilterPredicate { get; set; }

	[Parameter] public bool ClearFilterOnHide { get; set; }

	/// <summary>
	/// This event is fired when a dropdown element has been made visible to the user.
	/// </summary>
	[Parameter] public EventCallback<string> OnShown { get; set; }

	/// <summary>
	/// This event is fired when a dropdown element has been hidden from the user.
	/// </summary>
	[Parameter] public EventCallback<string> OnHidden { get; set; }

	/// <summary>
	/// Template that defines what should be rendered in case of empty items.
	/// </summary>
	[Parameter] public RenderFragment FilterEmptyResultTemplate { get; set; }

	[Parameter] public string FilterEmptyResultText { get; set; }

	[Parameter] public bool AllowSelectAll { get; set; }

	[Parameter] public EventCallback<bool> SelectAllChanged { get; set; }

	[Parameter] public string SelectAllText { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; }

	[Inject] private IStringLocalizer<HxMultiSelect> StringLocalizer { get; set; }

	protected bool HasInputGroupsEffective => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);

	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync(string elementId) => OnHidden.InvokeAsync(elementId);

	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync(string elementId) => OnShown.InvokeAsync(elementId);

	private IJSObjectReference jsModule;
	private readonly DotNetObjectReference<HxMultiSelectInternal<TValue, TItem>> dotnetObjectReference;
	private ElementReference elementReference;
	private ElementReference filterInputReference;
	private bool isShown;
	private string filterText = string.Empty;
	private bool selectAll;
	private bool disposed;

	public HxMultiSelectInternal()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}

			await jsModule.InvokeVoidAsync("initialize", elementReference, dotnetObjectReference);
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxMultiSelect));
	}

	private async Task HandleItemSelectionChangedAsync(bool newChecked, TItem item, bool triggerSelectAllEvent = true)
	{
		await ItemSelectionChanged.InvokeAsync(new SelectionChangedArgs
		{
			Checked = newChecked,
			Item = item
		});

		if (triggerSelectAllEvent)
		{
			await ChangeSelectAllAsync(false);
		}
	}

	private async Task HandleSelectAllClickedAsync()
	{
		var filteredItems = GetFilteredItems();

		// If all items are already selected then they should be deselected, otherwise only records that aren't selected should be
		if (selectAll)
		{
			foreach (var item in filteredItems)
			{
				// We don't want to trigger select all triggers each time, an item is selected, just once at the end
				await HandleItemSelectionChangedAsync(false, item, false);
			}
		}
		else
		{
			foreach (var item in filteredItems)
			{
				var value = SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item);
				var itemSelected = DoSelectedValuesContainValue(value);

				// If the item is already selected we don't need to reselect it
				if (!itemSelected)
				{
					// We don't want to trigger select all triggers each time, an item is selected, just once at the end
					await HandleItemSelectionChangedAsync(!itemSelected, item, false);
				}
			}
		}

		await ChangeSelectAllAsync(!selectAll);
	}

	private bool DoSelectedValuesContainValue(TValue value)
	{
		return SelectedValues?.Contains(value) ?? false;
	}

	private Task ChangeSelectAllAsync(bool selectAll)
	{
		this.selectAll = selectAll;
		return SelectAllChanged.InvokeAsync(selectAll);
	}

	private Task HandleFilterInputChangedAsync(ChangeEventArgs e)
	{
		filterText = e.Value?.ToString() ?? string.Empty;
		return ChangeSelectAllAsync(false);
	}

	private List<TItem> GetFilteredItems()
	{
		if (!AllowFiltering || string.IsNullOrEmpty(filterText))
		{
			return ItemsToRender;
		}

		var filterPredicate = FilterPredicate ?? DefaultFilterPredicate;
		return ItemsToRender.Where(x => filterPredicate(x, filterText)).ToList();

		bool DefaultFilterPredicate(TItem item, string filter)
		{
			return string.IsNullOrEmpty(filter) || TextSelector(item).Contains(filter, StringComparison.OrdinalIgnoreCase);
		}
	}

	private string GetSelectAllText()
	{
		if (SelectAllText is not null)
		{
			return SelectAllText;
		}

		return StringLocalizer["SelectAllDefaultText"];
	}

	private string GetFilterEmptyResultText()
	{
		if (FilterEmptyResultText is not null)
		{
			return FilterEmptyResultText;
		}

		return StringLocalizer["FilterEmptyResultDefaultText"];
	}

	public async ValueTask FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(elementReference, default))
		{
			throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
		}
		await elementReference.FocusAsync();
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxMultiSelect_HandleJsHidden")]
	public Task HandleJsHidden()
	{
		isShown = false;

		if (ClearFilterOnHide)
		{
			filterText = string.Empty;
		}

		return InvokeOnHiddenAsync(this.InputId);
	}

	[JSInvokable("HxMultiSelect_HandleJsShown")]
	public async Task HandleJsShown()
	{
		isShown = true;
		await filterInputReference.FocusAsync();
		await InvokeOnShownAsync(this.InputId);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		disposed = true;

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
		}

		dotnetObjectReference?.Dispose();
	}



	public class SelectionChangedArgs
	{
		public bool Checked { get; set; }
		public TItem Item { get; set; }
	}
}