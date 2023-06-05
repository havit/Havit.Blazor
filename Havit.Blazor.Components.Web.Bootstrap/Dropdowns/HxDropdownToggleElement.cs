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
	/// For more information refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
	/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
	/// </summary>
	[Parameter] public string DropdownReference { get; set; }

	/// <summary>
	/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
	/// of the dropdown relative to its target.  Default is <c>(0, 2)</c>.
	/// </summary>
	[Parameter] public (int Skidding, int Distance)? DropdownOffset { get; set; }

	/// <summary>
	/// Custom CSS class to render with the toggle element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

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
	/// Fired when the dropdown has finished being hidden from the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();

	[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }
	[CascadingParameter] protected HxNav NavContainer { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// Returns the element reference of rendered element.
	/// </summary>
	internal ElementReference ElementReference => elementReference;

	private ElementReference elementReference;
	private DotNetObjectReference<HxDropdownToggleElement> dotnetObjectReference;
	private IJSObjectReference jsModule;
	private bool disposed;

	public HxDropdownToggleElement()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, ElementName);

		builder.AddAttribute(1, "data-bs-toggle", "dropdown");
		builder.AddAttribute(2, "aria-expanded", "false");

		var dataBsAutoCloseAttributeValue = (DropdownContainer?.AutoClose ?? DropdownAutoClose.True) switch
		{
			DropdownAutoClose.True => "true",
			DropdownAutoClose.False => "false",
			DropdownAutoClose.Inside => "inside",
			DropdownAutoClose.Outside => "outside",
			_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {DropdownContainer.AutoClose}.")
		};
		builder.AddAttribute(3, "data-bs-auto-close", dataBsAutoCloseAttributeValue);

		if (this.DropdownOffset is not null)
		{
			builder.AddAttribute(4, "data-bs-offset", $"{DropdownOffset.Value.Skidding},{DropdownOffset.Value.Distance}");
		}

		builder.AddAttribute(5, "data-bs-reference", DropdownToggleExtensions.GetDropdownDataBsReference(this));
		builder.AddAttribute(6, "class", GetCssClass());

		builder.AddMultipleAttributes(99, AdditionalAttributes);
		builder.AddElementReferenceCapture(4, capturedRef => elementReference = capturedRef);
		builder.AddContent(5, ChildContent);

		builder.CloseElement();
	}

	protected virtual string GetCssClass()
	{
		/*
		 * Despite the name the .dropdown-toggle is the class which does nothing else than the caret (arrow ::after and no-wrap to prevent caret wrapping).
		 * If it later turns out to be used for other reasons we will need to add the .dropdown-toggle-no-caret class to prevent the caret from being displayed.
		 */
		return CssClassHelper.Combine(
			this.CssClass,
			(this.Caret ? "dropdown-toggle" : null),
			((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
			((DropdownContainer as HxDropdownButtonGroup)?.Split ?? false) ? "dropdown-toggle-split" : null,
			(NavContainer is not null) ? "nav-link" : null);
	}

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
			await jsModule.InvokeVoidAsync("create", elementReference, dotnetObjectReference, DropdownToggleExtensions.GetDropdownJsOptionsReference(this));
		}
	}

	/// <summary>
	/// Shows the dropdown.
	/// </summary>
	public async Task ShowAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("show", elementReference);
	}

	/// <summary>
	/// Hides the dropdown.
	/// </summary>
	public async Task HideAsync()
	{
		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("hide", elementReference);
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
				await jsModule.InvokeVoidAsync("dispose", elementReference);
				await jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}

		dotnetObjectReference.Dispose();
	}
}
