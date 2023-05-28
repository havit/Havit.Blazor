using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Common implementation for <see cref="HxTooltip"/> and <see cref="HxPopover"/>.
/// </summary>
/// <remarks>
/// We do not want HxPopover to derive from HxTooltip directly as we want
/// to keep the API consistent, e.g. HxPopover.Placement = PopoverPlacement.Auto, not TooltipPlacement.Auto or anything else.
/// </remarks>
public abstract class HxTooltipInternalBase : ComponentBase, IAsyncDisposable
{
	protected string TitleInternal { get; set; }
	protected string ContentInternal { get; set; }
	protected TooltipPlacement PlacementInternal { get; set; }
	protected TooltipTrigger TriggerInternal { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider Settings in components descendants (by returning a derived settings class).
	/// </remarks>
	protected abstract TooltipInternalSettings GetSettings();

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected abstract TooltipInternalSettings GetDefaults();

	/// <summary>
	/// Allows you to insert HTML. If <c>false</c>, <c>innerText</c> property will be used to insert content into the DOM.
	/// Use text if you're worried about XSS attacks.
	/// </summary>
	[Parameter] public bool Html { get; set; }

	/// <summary>
	/// Appends the tooltip/popover to a specific element. Default is <c>body</c>.
	/// </summary>
	[Parameter] public string Container { get; set; }
	protected string ContainerEffective => this.Container ?? this.GetSettings()?.Container ?? GetDefaults().Container;

	/// <summary>
	/// Enable or disable the sanitization. If activated HTML content will be sanitized. <see href="https://getbootstrap.com/docs/5.2/getting-started/javascript/#sanitizer">See the sanitizer section in Bootstrap JavaScript documentation</see>.
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool Sanitize { get; set; } = true;

	/// <summary>
	/// Offset of the component relative to its target (ChildContent).
	/// </summary>
	[Parameter] public (int X, int Y)? Offset { get; set; }
	protected (int X, int Y)? OffsetEffective => this.Offset ?? this.GetSettings()?.Offset ?? GetDefaults().Offset;

	/// <summary>
	/// Apply a CSS fade transition to the tooltip (enable/disable).<br/>
	/// Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool? Animation { get; set; }
	protected bool? AnimationEffective => this.Animation ?? this.GetSettings()?.Animation ?? GetDefaults().Animation;

	/// <summary>
	/// Custom CSS class to add.
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
	/// </summary>
	[Parameter] public string WrapperCssClass { get; set; }
	protected string WrapperCssClassEffective => this.WrapperCssClass ?? this.GetSettings()?.WrapperCssClass ?? GetDefaults().WrapperCssClass;

	/// <summary>
	/// Child content to wrap.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Fired when the content has been made visible to the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

	/// <summary>
	/// Fired when the content has finished being hidden from the user and CSS transitions have completed.
	/// </summary>
	[Parameter] public EventCallback OnHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();


	[Inject] public IJSRuntime JSRuntime { get; set; }

	protected abstract string JsModuleName { get; }
	protected abstract string DataBsToggle { get; }

	private DotNetObjectReference<HxTooltipInternalBase> dotnetObjectReference;
	private IJSObjectReference jsModule;
	private ElementReference spanElement;
	private string lastTitle;
	private string lastContent;
	private bool shouldRenderSpan;
	private bool isInitialized;
	private bool disposed;

	protected HxTooltipInternalBase()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected bool ShouldRenderSpan()
	{
		// Once the span is rendered it does not disappear to enable spanElement to be used at OnAfterRender to safely remove a tooltip/popover.
		// It is not a common situation to remove a tooltip/popover.
		shouldRenderSpan |= !String.IsNullOrEmpty(TitleInternal)
							|| !String.IsNullOrWhiteSpace(this.WrapperCssClass)
							|| !String.IsNullOrWhiteSpace(this.ContentInternal);
		return shouldRenderSpan;
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ShouldRenderSpan())
		{
			builder.OpenElement(1, "span");
			builder.AddAttribute(2, "class", CssClassHelper.Combine("d-inline-block", this.WrapperCssClassEffective));
			builder.AddAttribute(3, "data-bs-container", this.ContainerEffective);
			builder.AddAttribute(4, "data-bs-trigger", GetTriggers());
			builder.AddAttribute(5, "data-bs-placement", PlacementInternal.ToString().ToLower());
			builder.AddAttribute(6, "data-bs-custom-class", this.CssClassEffective);
			if (this.AnimationEffective is not null)
			{
				builder.AddAttribute(7, "data-bs-animation", this.AnimationEffective.ToString().ToLower());
			}
			builder.AddAttribute(8, "data-bs-title", TitleInternal);
			if (!String.IsNullOrWhiteSpace(ContentInternal))
			{
				// used only by HxPopover
				builder.AddAttribute(9, "data-bs-content", ContentInternal);
			}
			if (Html)
			{
				builder.AddAttribute(10, "data-bs-html", "true");
			}
			builder.AddAttribute(11, "data-bs-toggle", DataBsToggle);
			if (this.OffsetEffective is not null)
			{
				builder.AddAttribute(12, "data-bs-offset", $"{OffsetEffective.Value.X},{OffsetEffective.Value.Y}");
			}
			builder.AddElementReferenceCapture(13, element => spanElement = element);
		}

		builder.AddContent(20, ChildContent);

		if (ShouldRenderSpan())
		{
			builder.CloseElement();
		}
	}

	protected string GetTriggers()
	{
		string result = null;
		foreach (var flag in Enum.GetValues<TooltipTrigger>())
		{
			if (this.TriggerInternal.HasFlag(flag))
			{
				result = result + " " + flag.ToString().ToLower();
			}
		}
		return result?.Trim();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (ShouldRenderSpan())
		{
			if (!isInitialized)
			{
				isInitialized = true;
				lastTitle = TitleInternal;
				lastContent = ContentInternal;

				await EnsureJsModuleAsync();

				var options = new
				{
					Sanitize = this.Sanitize
				};
				if (disposed)
				{
					return;
				}
				await jsModule.InvokeVoidAsync("initialize", spanElement, dotnetObjectReference, options);
			}
			else if ((lastTitle != TitleInternal) || (lastContent != ContentInternal))
			{
				if (String.IsNullOrWhiteSpace(TitleInternal) && String.IsNullOrWhiteSpace(ContentInternal))
				{
					// no content, remove the tooltip/popover
					lastTitle = TitleInternal;
					lastContent = ContentInternal;
					isInitialized = false;

					if (disposed)
					{
						return;
					}
					await jsModule.InvokeVoidAsync("dispose", spanElement);
				}
				else
				{
					// changed content, update the tooltip/popover
					lastTitle = TitleInternal;
					lastContent = ContentInternal;

					if (disposed)
					{
						return;
					}
					await jsModule.InvokeVoidAsync("setContent", spanElement, GetNewContentForUpdate());
				}
			}
		}
	}

	protected abstract Dictionary<string, string> GetNewContentForUpdate();

	private async Task EnsureJsModuleAsync()
	{
		if (disposed)
		{
			return;
		}
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(JsModuleName);
	}

	/// <summary>
	/// Shows the component.
	/// </summary>
	public async Task ShowAsync()
	{
		if (!isInitialized)
		{
			return;
		}

		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("show", spanElement);
	}

	/// <summary>
	/// Hides the component.
	/// </summary>
	public async Task HideAsync()
	{
		if (!isInitialized)
		{
			return;
		}

		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("hide", spanElement);
	}

	/// <summary>
	/// Gives the component the ability to be shown.<br />
	/// The component is enabled by default (i.e. this method is not necessary to be called if the component has not been disabled).
	/// </summary>
	public async Task EnableAsync()
	{
		if (!isInitialized)
		{
			return;
		}

		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("enable", spanElement);
	}

	/// <summary>
	/// Removes the ability for the component to be shown.<br />
	/// It will only be able to be shown if it is re-enabled.
	/// </summary>
	public async Task DisableAsync()
	{
		if (!isInitialized)
		{
			return;
		}

		await EnsureJsModuleAsync();
		await jsModule.InvokeVoidAsync("disable", spanElement);
	}

	/// <summary>
	/// Receives notification from javascript when content is shown.
	/// </summary>
	/// <remarks>
	/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
	/// </remarks>
	[JSInvokable("HxHandleJsShown")]
	public async Task HandleJsShown()
	{
		await InvokeOnShownAsync();
	}

	/// <summary>
	/// Receives notification from javascript when content is hidden.
	/// </summary>
	[JSInvokable("HxHandleJsHidden")]
	public async Task HandleJsHidden()
	{
		await InvokeOnHiddenAsync();
	}


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
			if (isInitialized)
			{
				try
				{
					await jsModule.InvokeVoidAsync("dispose", spanElement);
				}
				catch (JSDisconnectedException)
				{
					// NOOP
				}
			}
			try
			{
				await jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			jsModule = null;
			isInitialized = false;
		}

		dotnetObjectReference.Dispose();
	}
}
