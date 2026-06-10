using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/">Bootstrap Menu</see> toggle button which triggers the <see cref="HxMenu"/> to open.
/// </summary>
public class HxMenuToggleButton : HxButton, IAsyncDisposable, IHxMenuToggle
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxMenuToggleButton"/> and derived components.
	/// </summary>
	public static new ButtonSettings Defaults { get; set; }

	static HxMenuToggleButton()
	{
		Defaults = HxButton.Defaults with
		{
			Color = ThemeColor.Link
		};
	}

	/// <inheritdoc cref="HxButton.GetDefaults"/>
	protected override ButtonSettings GetDefaults() => Defaults;

	/// <summary>
	/// Offset <c>(<see href="https://floating-ui.com/docs/offset#options">skidding</see>, <see href="https://floating-ui.com/docs/offset#options">distance</see>)</c>
	/// of the menu relative to its target.  Default is <c>(0, 2)</c>.
	/// </summary>
	[Parameter] public (int Skidding, int Distance)? MenuOffset { get; set; }

	/// <summary>
	/// Reference element of the menu menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
	/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
	/// For more information, refer to the <see href="https://floating-ui.com/docs/computePosition">Floating UI documentation</see>.
	/// </summary>
	[Parameter] public string MenuReference { get; set; }

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

	[CascadingParameter] protected HxMenu MenuContainer { get; set; }
	[CascadingParameter] protected HxNav NavContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private DotNetObjectReference<HxMenuToggleButton> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private string _currentMenuJsOptionsReference;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	public HxMenuToggleButton()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void OnParametersSet()
	{
		if ((Color is null) && (NavContainer is not null))
		{
			Color = ThemeColor.Link;
		}

		base.OnParametersSet();


		if (!String.IsNullOrEmpty(Tooltip))
		{
			throw new InvalidOperationException($"{nameof(HxMenuToggleButton)} does not support {nameof(Tooltip)}.");
		}

		AdditionalAttributes ??= new Dictionary<string, object>();
		AdditionalAttributes["data-bs-toggle"] = "menu";
		AdditionalAttributes["aria-expanded"] = "false";
		AdditionalAttributes["data-bs-auto-close"] = AutoCloseEffective switch
		{
			MenuAutoClose.True => "true",
			MenuAutoClose.False => "false",
			MenuAutoClose.Inside => "inside",
			MenuAutoClose.Outside => "outside",
			_ => throw new InvalidOperationException($"Unknown {nameof(MenuAutoClose)} value {AutoCloseEffective}.")
		};

		if (MenuOffset is not null)
		{
			AdditionalAttributes["data-bs-offset"] = $"{MenuOffset.Value.Skidding},{MenuOffset.Value.Distance}";
		}

		AdditionalAttributes["data-bs-reference"] = MenuToggleExtensions.GetMenuDataBsReference(this);

		string dataBsPlacement = ResponsivePlacementEffective ?? ((PlacementEffective != MenuPlacement.BottomStart) ? PlacementEffective.ToDataBsPlacement() : null);
		if (dataBsPlacement is not null)
		{
			AdditionalAttributes["data-bs-placement"] = dataBsPlacement;
		}
	}

	protected override string CoreCssClass =>
		CssClassHelper.Combine(
			base.CoreCssClass,
			(NavContainer is not null) ? "nav-link" : null);


	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender); // allows HxButton.OnAfterRenderAsync

		var menuJsOptionsReference = MenuToggleExtensions.GetMenuJsOptionsReference(this);

		if (firstRender)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			_currentMenuJsOptionsReference = menuJsOptionsReference;
			await _jsModule.InvokeVoidAsync("create", buttonElementReference, _dotnetObjectReference, GetMenuJsOptions(_currentMenuJsOptionsReference), OnHiding.HasDelegate);
		}
		else
		{
			if (menuJsOptionsReference != _currentMenuJsOptionsReference)
			{
				_currentMenuJsOptionsReference = menuJsOptionsReference;
				if (_jsModule is not null)
				{
					await _jsModule.InvokeVoidAsync("update", buttonElementReference, GetMenuJsOptions(menuJsOptionsReference));
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
			await _jsModule.InvokeVoidAsync("show", buttonElementReference);
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
			await _jsModule.InvokeVoidAsync("hide", buttonElementReference);
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
		if (MenuContainer is IMenuContainer container)
		{
			container.IsOpen = true;
		}
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
		if (MenuContainer is IMenuContainer container)
		{
			container.IsOpen = false;
		}
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
				await _jsModule.InvokeVoidAsync("dispose", buttonElementReference);
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
