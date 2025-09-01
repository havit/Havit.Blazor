using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxMultiSelectInternal<TValue, TItem> : IAsyncDisposable
{
	[Parameter] public string InputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public string InputText { get; set; }

	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	[Parameter] public bool EnabledEffective { get; set; }

	[Parameter] public LabelType LabelTypeEffective { get; set; }

	[Parameter] public List<TItem> ItemsToRender { get; set; }

	[Parameter] public List<TValue> SelectedValues { get; set; }
	[Parameter] public EventCallback<List<TValue>> SelectedValuesChanged { get; set; }

	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

	[Parameter] public string NullDataText { get; set; }

	[Parameter] public string InputGroupCssClass { get; set; }

	[Parameter] public string InputGroupStartText { get; set; }

	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	[Parameter] public string InputGroupEndText { get; set; }

	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	[Parameter] public bool AllowFiltering { get; set; }

	[Parameter] public Func<TItem, string, bool> FilterPredicate { get; set; }

	[Parameter] public bool ClearFilterOnHide { get; set; }

	[Parameter] public RenderFragment FilterEmptyResultTemplate { get; set; }

	[Parameter] public string FilterEmptyResultText { get; set; }

	[Parameter] public bool AllowSelectAll { get; set; }

	[Parameter] public string SelectAllText { get; set; }

	[Parameter] public IconBase FilterSearchIcon { get; set; }

	[Parameter] public IconBase FilterClearIcon { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; }

	[Inject] private IStringLocalizer<HxMultiSelect> StringLocalizer { get; set; }

	protected bool HasInputGroups => !String.IsNullOrWhiteSpace(InputGroupStartText) || !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupStartTemplate is not null) || (InputGroupEndTemplate is not null);
	protected bool HasInputGroupEnd => !String.IsNullOrWhiteSpace(InputGroupEndText) || (InputGroupEndTemplate is not null);
	protected bool HasInputGroupStart => !String.IsNullOrWhiteSpace(InputGroupStartText) || (InputGroupStartTemplate is not null);

	private IJSObjectReference _jsModule;
	private readonly DotNetObjectReference<HxMultiSelectInternal<TValue, TItem>> _dotnetObjectReference;
	private ElementReference _inputElementReference;
	private ElementReference _elementReference;
	private ElementReference _filterInputReference;
	private bool _isShown;
	private string _filterText = string.Empty;
	private bool _selectAllChecked;
	private bool _disposed;
	private List<TValue> _currentSelectedValues;

	public HxMultiSelectInternal()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void OnParametersSet()
	{
		_currentSelectedValues = SelectedValues ?? new();

		SynchronizeSelectAllCheckbox();
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			await _jsModule.InvokeVoidAsync("initialize", _elementReference, _dotnetObjectReference);
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxMultiSelect));
	}

	private async Task HandleItemSelectionChangedAsync(bool newChecked, TItem item)
	{
		if (newChecked)
		{
			_currentSelectedValues = new List<TValue>(_currentSelectedValues);
			_currentSelectedValues.Add(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item));
		}
		else
		{
			_currentSelectedValues = new List<TValue>(_currentSelectedValues);
			_currentSelectedValues.Remove(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item));
		}

		SynchronizeSelectAllCheckbox();

		await SelectedValuesChanged.InvokeAsync(_currentSelectedValues);
	}

	private async Task HandleSelectAllClickedAsync()
	{
		var filteredItems = GetFilteredItems();

		var newCurrentSelectedValues = new List<TValue>(_currentSelectedValues);
		if (_selectAllChecked)
		{
			_selectAllChecked = false;

			foreach (var item in filteredItems)
			{
				newCurrentSelectedValues.Remove(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item));
			}
		}
		else
		{
			_selectAllChecked = true;

			foreach (var item in filteredItems)
			{
				// If the item is already selected we don't need to reselect it
				if (!newCurrentSelectedValues.Contains(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item)))
				{
					newCurrentSelectedValues.Add(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item));
				}
			}
		}
		_currentSelectedValues = newCurrentSelectedValues;

		await SelectedValuesChanged.InvokeAsync(_currentSelectedValues);
	}

	private void HandleClearIconClick()
	{
		_filterText = string.Empty;

		SynchronizeSelectAllCheckbox();
	}

	private void HandleFilterInputChanged(ChangeEventArgs e)
	{
		_filterText = e.Value?.ToString() ?? string.Empty;

		SynchronizeSelectAllCheckbox();
	}

	private void SynchronizeSelectAllCheckbox()
	{
		if (AllowSelectAll)
		{
			if (_currentSelectedValues.Any())
			{
				// If every item in the filtered list is contained in the selected values then select all should be checked
				var filteredItems = GetFilteredItems();
				_selectAllChecked = filteredItems.All(item => _currentSelectedValues.Contains(SelectorHelpers.GetValue<TItem, TValue>(ValueSelector, item)));
			}
			else
			{
				_selectAllChecked = false;
			}
		}
	}

	private List<TItem> GetFilteredItems()
	{
		if (!AllowFiltering || string.IsNullOrEmpty(_filterText))
		{
			return ItemsToRender;
		}

		var filterPredicate = FilterPredicate ?? DefaultFilterPredicate;
		return ItemsToRender.Where(x => filterPredicate(x, _filterText)).ToList();

		bool DefaultFilterPredicate(TItem item, string filter)
		{
			return string.IsNullOrEmpty(filter) || TextSelector(item).Contains(filter, StringComparison.OrdinalIgnoreCase);
		}
	}

	private string GetSelectAllText()
	{
		return SelectAllText ?? StringLocalizer["SelectAllDefaultText"];
	}

	private string GetFilterEmptyResultText()
	{
		return FilterEmptyResultText ?? StringLocalizer["FilterEmptyResultDefaultText"];
	}

	public async ValueTask FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(_inputElementReference, default))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The method must be called after first render.");
		}
		await _inputElementReference.FocusAsync();
		_isShown = true;
	}

	/// <summary>
	/// Shows the dropdown.
	/// </summary>
	public async Task ShowDropdownAsync()
	{
		await EnsureJsModuleAsync();
		if (_disposed)
		{
			return;
		}
		await _jsModule.InvokeVoidAsync("show", _elementReference);
	}

	/// <summary>
	/// Hides the dropdown.
	/// </summary>
	public async Task HideDropdownAsync()
	{
		await EnsureJsModuleAsync();
		if (_disposed)
		{
			return;
		}
		await _jsModule.InvokeVoidAsync("hide", _elementReference);
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxMultiSelect_HandleJsHidden")]
	public Task HandleJsHidden()
	{
		_isShown = false;

		if (ClearFilterOnHide && (_filterText != string.Empty))
		{
			_filterText = string.Empty;
			SynchronizeSelectAllCheckbox();
			StateHasChanged();
		}

		return Task.CompletedTask;
	}

	[JSInvokable("HxMultiSelect_HandleJsShown")]
	public async Task HandleJsShown()
	{
		_isShown = true;

		if (AllowFiltering)
		{
			await _filterInputReference.FocusAsync();
		}
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", _elementReference);
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