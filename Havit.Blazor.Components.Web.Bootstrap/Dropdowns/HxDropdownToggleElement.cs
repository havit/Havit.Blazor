using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">Bootstrap Dropdown</see> toggle button which triggers the <see cref="HxDropdown"/> to open.
/// </summary>
public class HxDropdownToggleElement : ComponentBase, IHxDropdownToggle, IAsyncDisposable
{
	/// <summary>
	/// Gets or sets the name of the element to render. Default is <c>span</c>.
	/// </summary>
	[Parameter] public string ElementName { get; set; } = "span";

	/// <summary>
	/// Gets or sets whether to display caret in the toggle.<br />
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool Caret { get; set; }

	/// <summary>
	/// Reference element of the dropdown menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
	/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
	/// For more information, refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
	/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
	/// </summary>
	[Parameter] public string DropdownReference { get; set; }

	/// <summary>
	/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
	/// of the dropdown relative to its target. Default is <c>(0, 2)</c>.
	/// </summary>
	[Parameter] public (int Skidding, int Distance)? DropdownOffset { get; set; }

	/// <summary>
	/// Custom CSS class to render with the toggle element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
	/// You can use the AutoClose parameter to change this behavior of the dropdown.
	/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior">https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior</see>.
	/// The parameter can be used to override the settings of the <see cref="DropdownContainer"/> component or to specify the auto-close behavior when the component is not used.
	/// </summary>
	[Parameter] public DropdownAutoClose? AutoClose { get; set; }
	protected DropdownAutoClose AutoCloseEffective => AutoClose ?? DropdownContainer?.AutoClose ?? DropdownAutoClose.True;

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

	/// <summary>
	/// Fired when the dropdown has been made visible to the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

	/// <summary>
	/// Fired immediately when the 'hide' instance method is called.
	/// To cancel hiding, set <see cref="DropdownHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// There is intentionally no <c>virtual InvokeOnHidingAsync()</c> method to override to avoid confusion.
	/// The <code>hide.bs.dropdown</code> event is only subscribed to when the <see cref="OnHiding"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback<DropdownHidingEventArgs> OnHiding { get; set; }

	/// <summary>
	/// Fired when the dropdown has finished being hidden from the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();

	/// <summary>
	/// Value for cases when the dropdown is used as an <code>input</code> element.
	/// </summary>
	[Parameter] public string Value { get; set; }
	/// <summary>
	/// Raised when the value changes (binds to <code>onchange</code> input event).
	/// </summary>
	[Parameter] public EventCallback<string> ValueChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="ValueChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeValueChangedAsync(string newValue) => ValueChanged.InvokeAsync(newValue);

	[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }
	[CascadingParameter] protected HxNav NavContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// Returns the element reference of rendered element.
	/// </summary>
	internal ElementReference ElementReference => _elementReference;

	private ElementReference _elementReference;
	private DotNetObjectReference<HxDropdownToggleElement> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private string _currentDropdownJsOptionsReference;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	public HxDropdownToggleElement()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, ElementName);

		builder.AddAttribute(1, "data-bs-toggle", "dropdown");
		builder.AddAttribute(2, "aria-expanded", "false");

		var dataBsAutoCloseAttributeValue = AutoCloseEffective switch
		{
			DropdownAutoClose.True => "true",
			DropdownAutoClose.False => "false",
			DropdownAutoClose.Inside => "inside",
			DropdownAutoClose.Outside => "outside",
			_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {AutoCloseEffective}.")
		};
		builder.AddAttribute(3, "data-bs-auto-close", dataBsAutoCloseAttributeValue);

		if (DropdownOffset is not null)
		{
			builder.AddAttribute(4, "data-bs-offset", $"{DropdownOffset.Value.Skidding},{DropdownOffset.Value.Distance}");
		}

		builder.AddAttribute(5, "data-bs-reference", DropdownToggleExtensions.GetDropdownDataBsReference(this));
		builder.AddAttribute(6, "class", GetCssClass());

		if (String.Equals(ElementName, "input", StringComparison.OrdinalIgnoreCase))
		{
			builder.AddAttribute(10, "value", Value);
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
			// TODO VSTHRD101 via RuntimeHelpers.CreateInferredBindSetter?
			builder.AddAttribute(11, "onchange", EventCallback.Factory.CreateBinder<string>(this, async (string value) => await InvokeValueChangedAsync(value), Value));
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
#if NET8_0_OR_GREATER
			builder.SetUpdatesAttributeName("value");
#endif
		}

		builder.AddMultipleAttributes(99, AdditionalAttributes);
		builder.AddElementReferenceCapture(104, capturedRef => _elementReference = capturedRef);
		builder.AddContent(105, ChildContent);

		builder.CloseElement();
	}

	protected virtual string GetCssClass()
	{
		/*
		 * Despite the name the .dropdown-toggle is the class which does nothing else than the caret (arrow ::after and no-wrap to prevent caret wrapping).
		 * If it later turns out to be used for other reasons we will need to add the .dropdown-toggle-no-caret class to prevent the caret from being displayed.
		 */
		return CssClassHelper.Combine(
			CssClass,
			(Caret ? "dropdown-toggle" : null),
			((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
			((DropdownContainer as HxDropdownButtonGroup)?.Split ?? false) ? "dropdown-toggle-split" : null,
			(NavContainer is not null) ? "nav-link" : null);
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		var dropdownJsOptionsReference = DropdownToggleExtensions.GetDropdownJsOptionsReference(this);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			_currentDropdownJsOptionsReference = dropdownJsOptionsReference;
			await _jsModule.InvokeVoidAsync("create", _elementReference, _dotnetObjectReference, _currentDropdownJsOptionsReference, OnHiding.HasDelegate);
		}
		else
		{
			if (dropdownJsOptionsReference != _currentDropdownJsOptionsReference)
			{
				_currentDropdownJsOptionsReference = dropdownJsOptionsReference;
				if (_jsModule is not null)
				{
					await _jsModule.InvokeVoidAsync("update", _elementReference, dropdownJsOptionsReference);
				}
			}
		}

		// for show/hide/... the dropdown has to be created/updated first 
		while (_onAfterRenderTasksQueue.TryDequeue(out var task))
		{
			await task();
		}
	}

	/// <summary>
	/// Shows the dropdown.
	/// </summary>
	public Task ShowAsync()
	{
		_onAfterRenderTasksQueue.Enqueue(async () =>
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("show", _elementReference);
		});

		StateHasChanged(); // ensure re-rendering

		return Task.CompletedTask;
	}

	/// <summary>
	/// Hides the dropdown.
	/// </summary>
	public Task HideAsync()
	{
		_onAfterRenderTasksQueue.Enqueue(async () =>
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("hide", _elementReference);
		});

		StateHasChanged(); // ensure re-rendering

		return Task.CompletedTask;
	}

	/// <summary>
	/// Receives notification from JavaScript when dropdown is shown.
	/// </summary>
	/// <remarks>
	/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
	/// </remarks>
	[JSInvokable("HxDropdown_HandleJsShown")]
	public async Task HandleJsShown()
	{
		((IDropdownContainer)DropdownContainer).IsOpen = true;
		await InvokeOnShownAsync();
	}

	/// <summary>
	/// Receives notification from JS for <c>hide.bs.dropdown</c> event.
	/// </summary>
	[JSInvokable("HxDropdown_HandleJsHide")]
	public async Task<bool> HandleJsHide()
	{
		var eventArgs = new DropdownHidingEventArgs();
		await OnHiding.InvokeAsync(eventArgs);
		return eventArgs.Cancel;
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxDropdown_HandleJsHidden")]
	public async Task HandleJsHidden()
	{
		((IDropdownContainer)DropdownContainer).IsOpen = false;
		await InvokeOnHiddenAsync();
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDropdown));
	}

	/// <inheritdoc/>

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
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

		_dotnetObjectReference.Dispose();
	}
}
