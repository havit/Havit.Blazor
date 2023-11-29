using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/">Bootstrap Dropdown</see> toggle button which triggers the <see cref="HxDropdownButtonGroup"/> to open.
/// </summary>
public class HxDropdownToggleButton : HxButton, IAsyncDisposable, IHxDropdownToggle
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxDropdownToggleButton"/> and derived components.
	/// </summary>
	public static new ButtonSettings Defaults { get; set; }

	static HxDropdownToggleButton()
	{
		Defaults = HxButton.Defaults with
		{
			Color = ThemeColor.Link
		};
	}

	/// <inheritdoc cref="HxButton.GetDefaults"/>
	protected override ButtonSettings GetDefaults() => Defaults;

	/// <summary>
	/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
	/// of the dropdown relative to its target.  Default is <c>(0, 2)</c>.
	/// </summary>
	[Parameter] public (int Skidding, int Distance)? DropdownOffset { get; set; }

	/// <summary>
	/// Reference element of the dropdown menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
	/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
	/// For more information refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
	/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
	/// </summary>
	[Parameter] public string DropdownReference { get; set; }

	/// <summary>
	/// Fired when the dropdown has been made visible to the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

	/// <summary>
	/// Fired when the dropdown has finished being hidden from the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();

	/// <summary>
	/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
	/// You can use the AutoClose parameter to change this behavior of the dropdown.
	/// <see href="https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior">https://getbootstrap.com/docs/5.3/components/dropdowns/#auto-close-behavior</see>.
	/// The parameter can be used to override the settings of the <see cref="DropdownContainer"/> component or to specify the auto-close behavior when the component is not used.
	/// </summary>
	[Parameter] public DropdownAutoClose? AutoClose { get; set; }
	protected DropdownAutoClose AutoCloseEffective => AutoClose ?? DropdownContainer?.AutoClose ?? DropdownAutoClose.True;

	[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }
	[CascadingParameter] protected HxNav NavContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private DotNetObjectReference<HxDropdownToggleButton> dotnetObjectReference;
	private IJSObjectReference jsModule;
	private bool disposed;

	public HxDropdownToggleButton()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void OnParametersSet()
	{
		if ((Color is null) && (NavContainer is not null))
		{
			Color = ThemeColor.Link;
		}

		base.OnParametersSet();

		if ((DropdownContainer is not null) && (DropdownContainer is not HxDropdownButtonGroup))
		{
			throw new InvalidOperationException($"{nameof(HxDropdownToggleButton)} is expected to used inside {nameof(HxDropdownButtonGroup)} rather than generic {nameof(HxDropdown)} (breaking-change in v2.6.0).");
		}

		if (!String.IsNullOrEmpty(this.Tooltip))
		{
			throw new InvalidOperationException($"{nameof(HxDropdownToggleButton)} does not support {nameof(Tooltip)}.");
		}

		AdditionalAttributes ??= new Dictionary<string, object>();
		AdditionalAttributes["data-bs-toggle"] = "dropdown";
		AdditionalAttributes["aria-expanded"] = "false";
		AdditionalAttributes["data-bs-auto-close"] = AutoCloseEffective switch
		{
			DropdownAutoClose.True => "true",
			DropdownAutoClose.False => "false",
			DropdownAutoClose.Inside => "inside",
			DropdownAutoClose.Outside => "outside",
			_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {AutoCloseEffective}.")
		};

		if (this.DropdownOffset is not null)
		{
			AdditionalAttributes["data-bs-offset"] = $"{DropdownOffset.Value.Skidding},{DropdownOffset.Value.Distance}";
		}

		AdditionalAttributes["data-bs-reference"] = DropdownToggleExtensions.GetDropdownDataBsReference(this);
	}

	protected override string CoreCssClass =>
		CssClassHelper.Combine(
			base.CoreCssClass,
			"dropdown-toggle",
			((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
			((DropdownContainer as HxDropdownButtonGroup)?.Split ?? false) ? "dropdown-toggle-split" : null,
			(NavContainer is not null) ? "nav-link" : null);


	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
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
			await jsModule.InvokeVoidAsync("create", buttonElementReference, dotnetObjectReference, DropdownToggleExtensions.GetDropdownJsOptionsReference(this));
		}
	}

	/// <summary>
	/// Shows the dropdown.
	/// </summary>
	public async Task ShowAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("show", buttonElementReference);
	}

	/// <summary>
	/// Hides the dropdown.
	/// </summary>
	public async Task HideAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("hide", buttonElementReference);
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
		if (DropdownContainer is IDropdownContainer container)
		{
			container.IsOpen = true;
		}
		await InvokeOnShownAsync();
	}

	/// <summary>
	/// Receives notification from JavaScript when item is hidden.
	/// </summary>
	[JSInvokable("HxDropdown_HandleJsHidden")]
	public async Task HandleJsHidden()
	{
		if (DropdownContainer is IDropdownContainer container)
		{
			container.IsOpen = false;
		}
		await InvokeOnHiddenAsync();
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDropdown));
	}

	/// <inheritdoc/>

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		disposed = true;

		if (jsModule != null)
		{
			try
			{
				await jsModule.InvokeVoidAsync("dispose", buttonElementReference);
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
