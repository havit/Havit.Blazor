using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxSelectInternal<TValue, TItem> : IAsyncDisposable
{
	[Parameter] public string InputId { get; set; }

	[Parameter] public string InputCssClass { get; set; }

	[Parameter] public InputSize InputSizeEffective { get; set; }

	[Parameter] public bool EnabledEffective { get; set; }

	[Parameter] public LabelType LabelTypeEffective { get; set; }

	[Parameter] public IFormValueComponent FormValueComponent { get; set; }

	[Parameter] public List<TItem> ItemsToRender { get; set; }

	/// <summary>
	/// Index of the selected item within <see cref="ItemsToRender"/> (<c>-1</c> for no selection or the <c>null</c> item).
	/// </summary>
	[Parameter] public int SelectedItemIndex { get; set; }
	[Parameter] public EventCallback<int> SelectedItemIndexChanged { get; set; }

	[Parameter] public bool NullableEffective { get; set; }

	[Parameter] public string NullText { get; set; }

	[Parameter] public string NullDataText { get; set; }

	[Parameter] public Func<TItem, string> TextSelector { get; set; }

	[Parameter] public Func<TItem, bool> ItemDisabledSelector { get; set; }

	[Parameter] public Func<TItem, string, bool> FilterPredicate { get; set; }

	[Parameter] public bool ClearFilterOnHide { get; set; }

	[Parameter] public RenderFragment FilterEmptyResultTemplate { get; set; }

	[Parameter] public string FilterEmptyResultText { get; set; }

	[Parameter] public IconBase FilterSearchIcon { get; set; }

	[Parameter] public IconBase FilterClearIcon { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; }

	[Inject] private IStringLocalizer<HxSelect> StringLocalizer { get; set; }

	private bool ShowPlaceholderStyle => (ItemsToRender == null) || (SelectedItemIndex == -1);

	private IJSObjectReference _jsModule;
	private readonly DotNetObjectReference<HxSelectInternal<TValue, TItem>> _dotnetObjectReference;
	private ElementReference _toggleElementReference;
	private ElementReference _filterInputReference;
	private bool _isShown;
	private string _filterText = string.Empty;
	private bool _disposed;

	public HxSelectInternal()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
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

			await _jsModule.InvokeVoidAsync("initialize", _toggleElementReference, _dotnetObjectReference);
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxSelect));
	}

	private async Task HandleItemClickAsync(int index)
	{
		// The Bootstrap Menu closes the menu itself (a click on a menu-item with autoClose=true).
		// We update the state proactively to keep the C#-owned state consistent (and to support environments where the JS has not been loaded).
		_isShown = false;

		if (index != SelectedItemIndex)
		{
			await SelectedItemIndexChanged.InvokeAsync(index);
		}
	}

	private void HandleFilterInputChanged(ChangeEventArgs e)
	{
		_filterText = e.Value?.ToString() ?? string.Empty;
	}

	private void HandleClearIconClick()
	{
		_filterText = string.Empty;
	}

	private string GetToggleText()
	{
		if (ItemsToRender == null)
		{
			return NullDataText;
		}

		if (SelectedItemIndex > -1)
		{
			return SelectorHelpers.GetText(TextSelector, ItemsToRender[SelectedItemIndex]);
		}

		return NullText;
	}

	private List<int> GetFilteredItemIndexes()
	{
		if (ItemsToRender == null)
		{
			return new List<int>();
		}

		if (string.IsNullOrEmpty(_filterText))
		{
			return Enumerable.Range(0, ItemsToRender.Count).Where(i => ItemsToRender[i] != null).ToList();
		}

		var filterPredicate = FilterPredicate ?? DefaultFilterPredicate;
		return Enumerable.Range(0, ItemsToRender.Count)
			.Where(i => (ItemsToRender[i] != null) && filterPredicate(ItemsToRender[i], _filterText))
			.ToList();

		bool DefaultFilterPredicate(TItem item, string filter)
		{
			return string.IsNullOrEmpty(filter) || (SelectorHelpers.GetText(TextSelector, item)?.Contains(filter, StringComparison.OrdinalIgnoreCase) ?? false);
		}
	}

	private string GetFilterEmptyResultText()
	{
		return FilterEmptyResultText ?? StringLocalizer["FilterEmptyResultDefaultText"];
	}

	/// <summary>
	/// Focuses the toggle.
	/// </summary>
	public async ValueTask FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(_toggleElementReference, default))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus. The method must be called after first render.");
		}
		await _toggleElementReference.FocusAsync();
	}

	/// <summary>
	/// Receives notification from JavaScript when the menu is shown.
	/// </summary>
	[JSInvokable("HxSelect_HandleJsShown")]
	public async Task HandleJsShown()
	{
		_isShown = true;
		StateHasChanged();

		await _filterInputReference.FocusAsync();
	}

	/// <summary>
	/// Receives notification from JavaScript when the menu is hidden.
	/// </summary>
	[JSInvokable("HxSelect_HandleJsHidden")]
	public Task HandleJsHidden()
	{
		_isShown = false;

		if (ClearFilterOnHide && (_filterText != string.Empty))
		{
			_filterText = string.Empty;
		}

		StateHasChanged();

		return Task.CompletedTask;
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
				await _jsModule.InvokeVoidAsync("dispose", _toggleElementReference);
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
