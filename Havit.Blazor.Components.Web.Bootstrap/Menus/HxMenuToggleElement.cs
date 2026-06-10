using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/">Bootstrap Menu</see> toggle button which triggers the <see cref="HxMenu"/> to open.
/// </summary>
public class HxMenuToggleElement : ComponentBase, IHxMenuToggle, IAsyncDisposable
{
	/// <summary>
	/// Gets or sets the name of the element to render. Default is <c>span</c>.
	/// </summary>
	[Parameter] public string ElementName { get; set; } = "span";


	/// <summary>
	/// Reference element of the menu menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
	/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
	/// For more information, refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
	/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
	/// </summary>
	[Parameter] public string MenuReference { get; set; }

	/// <summary>
	/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
	/// of the menu relative to its target. Default is <c>(0, 2)</c>.
	/// </summary>
	[Parameter] public (int Skidding, int Distance)? MenuOffset { get; set; }

	/// <summary>
	/// Custom CSS class to render with the toggle element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// By default, the menu menu is closed when clicking inside or outside the menu menu (<see cref="MenuAutoClose.True"/>).
	/// You can use the AutoClose parameter to change this behavior of the menu.
	/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#auto-close-behavior</see>.
	/// The parameter can be used to override the settings of the <see cref="MenuContainer"/> component or to specify the auto-close behavior when the component is not used.
	/// </summary>
	[Parameter] public MenuAutoClose? AutoClose { get; set; }
	protected MenuAutoClose AutoCloseEffective => AutoClose ?? MenuContainer?.AutoClose ?? MenuAutoClose.True;

	/// <summary>
	/// Placement of the menu relative to the toggle. The default is <see cref="MenuPlacement.BottomStart"/>.
	/// The parameter can be used to override the settings of the <see cref="MenuContainer"/> component or to specify the placement when the component is used standalone.
	/// </summary>
	[Parameter] public MenuPlacement? Placement { get; set; }
	protected MenuPlacement PlacementEffective => Placement ?? MenuContainer?.Placement ?? MenuPlacement.BottomStart;

	/// <summary>
	/// Raw <c>data-bs-placement</c> value allowing responsive placements (e.g. <c>bottom-start md:bottom-end</c>).
	/// Takes precedence over <see cref="Placement"/>.
	/// </summary>
	[Parameter] public string ResponsivePlacement { get; set; }
	protected string ResponsivePlacementEffective => ResponsivePlacement ?? MenuContainer?.ResponsivePlacement;

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

	/// <summary>
	/// Fired when the menu has been made visible to the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

	/// <summary>
	/// Fired immediately when the 'hide' instance method is called.
	/// To cancel hiding, set <see cref="MenuHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// There is intentionally no <c>virtual InvokeOnHidingAsync()</c> method to override to avoid confusion.
	/// The <code>hide.bs.menu</code> event is only subscribed to when the <see cref="OnHiding"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback<MenuHidingEventArgs> OnHiding { get; set; }

	/// <summary>
	/// Fired when the menu has finished being hidden from the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();

	/// <summary>
	/// Value for cases when the menu is used as an <code>input</code> element.
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

	[CascadingParameter] protected HxMenu MenuContainer { get; set; }
	[CascadingParameter] protected HxNav NavContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// Returns the element reference of rendered element.
	/// </summary>
	internal ElementReference ElementReference => _elementReference;

	private ElementReference _elementReference;
	private DotNetObjectReference<HxMenuToggleElement> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private string _currentMenuJsOptionsReference;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	public HxMenuToggleElement()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, ElementName);

		builder.AddAttribute(1, "data-bs-toggle", "menu");
		builder.AddAttribute(2, "aria-expanded", "false");

		var dataBsAutoCloseAttributeValue = AutoCloseEffective switch
		{
			MenuAutoClose.True => "true",
			MenuAutoClose.False => "false",
			MenuAutoClose.Inside => "inside",
			MenuAutoClose.Outside => "outside",
			_ => throw new InvalidOperationException($"Unknown {nameof(MenuAutoClose)} value {AutoCloseEffective}.")
		};
		builder.AddAttribute(3, "data-bs-auto-close", dataBsAutoCloseAttributeValue);

		if (MenuOffset is not null)
		{
			builder.AddAttribute(4, "data-bs-offset", $"{MenuOffset.Value.Skidding},{MenuOffset.Value.Distance}");
		}

		builder.AddAttribute(5, "data-bs-reference", MenuToggleExtensions.GetMenuDataBsReference(this));

		string dataBsPlacement = ResponsivePlacementEffective ?? ((PlacementEffective != MenuPlacement.BottomStart) ? PlacementEffective.ToDataBsPlacement() : null);
		if (dataBsPlacement is not null)
		{
			builder.AddAttribute(6, "data-bs-placement", dataBsPlacement);
		}
		builder.AddAttribute(7, "class", GetCssClass());

		if (String.Equals(ElementName, "input", StringComparison.OrdinalIgnoreCase))
		{
			builder.AddAttribute(10, "value", Value);
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
			// TODO VSTHRD101 via RuntimeHelpers.CreateInferredBindSetter?
			builder.AddAttribute(11, "onchange", EventCallback.Factory.CreateBinder<string>(this, async (string value) => await InvokeValueChangedAsync(value), Value));
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
			builder.SetUpdatesAttributeName("value");
		}

		builder.AddMultipleAttributes(99, AdditionalAttributes);
		builder.AddElementReferenceCapture(104, capturedRef => _elementReference = capturedRef);
		builder.AddContent(105, ChildContent);

		builder.CloseElement();
	}

	protected virtual string GetCssClass()
	{
		return CssClassHelper.Combine(
			CssClass,
			(NavContainer is not null) ? "nav-link" : null);
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		var menuJsOptionsReference = MenuToggleExtensions.GetMenuJsOptionsReference(this);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			_currentMenuJsOptionsReference = menuJsOptionsReference;
			await _jsModule.InvokeVoidAsync("create", _elementReference, _dotnetObjectReference, GetMenuJsOptions(_currentMenuJsOptionsReference), OnHiding.HasDelegate);
		}
		else
		{
			if (menuJsOptionsReference != _currentMenuJsOptionsReference)
			{
				_currentMenuJsOptionsReference = menuJsOptionsReference;
				if (_jsModule is not null)
				{
					await _jsModule.InvokeVoidAsync("update", _elementReference, GetMenuJsOptions(menuJsOptionsReference));
				}
			}
		}

		// for show/hide/... the menu has to be created/updated first 
		while (_onAfterRenderTasksQueue.TryDequeue(out var task))
		{
			await task();
		}
	}

	/// <summary>
	/// Override this method to provide additional options for the menu (allows specific customizations such as menu with backdrop).
	/// </summary>
	/// <param name="referenceOption"><c>reference</c> option to be used</param>
	protected virtual Dictionary<string, object> GetMenuJsOptions(string referenceOption)
	{
		return new()
		{
			["reference"] = referenceOption
		};
	}

	/// <summary>
	/// Shows the menu.
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
	/// Hides the menu.
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
	/// Receives notification from JavaScript when menu is shown.
	/// </summary>
	/// <remarks>
	/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
	/// </remarks>
	[JSInvokable("HxMenu_HandleJsShown")]
	public async Task HandleJsShown()
	{
		((IMenuContainer)MenuContainer).IsOpen = true;
		await InvokeOnShownAsync();
	}

	/// <summary>
	/// Receives notification from JS for <c>hide.bs.menu</c> event.
	/// </summary>
	[JSInvokable("HxMenu_HandleJsHide")]
	public async Task<bool> HandleJsHide()
	{
		var eventArgs = new MenuHidingEventArgs();
		await OnHiding.InvokeAsync(eventArgs);
		return eventArgs.Cancel;
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxMenu_HandleJsHidden")]
	public async Task HandleJsHidden()
	{
		((IMenuContainer)MenuContainer).IsOpen = false;
		await InvokeOnHiddenAsync();
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxMenu));
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
