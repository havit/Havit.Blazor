using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
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
		/// Allows you to insert HTML. If <c>false</c>, <c>innerText</c> property will be used to insert content into the DOM.
		/// Use text if you're worried about XSS attacks.
		/// </summary>
		[Parameter] public bool Html { get; set; }

		/// <summary>
		/// Enable or disable the sanitization. If activated HTML content will be sanitized. <see href="https://getbootstrap.com/docs/5.1/getting-started/javascript/#sanitizer">See the sanitizer section in Bootstrap JavaScript documentation</see>.
		/// </summary>
		[Parameter] public bool Sanitize { get; set; } = true;

		/// <summary>
		/// Offset of the component relative to its target (ChildContent).
		/// </summary>
		[Parameter] public (int X, int Y) Offset { get; set; }

		/// <summary>
		/// Custom CSS class to add.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
		/// </summary>
		[Parameter] public string WrapperCssClass { get; set; }

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

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// Once the span is rendered it does not disappear to enable spanElement to be used at OnAfterRender to safely remove a tooltip/popover.
			// It is not a common situation to remove a tooltip/popover.
			shouldRenderSpan |= !String.IsNullOrEmpty(TitleInternal)
								|| !String.IsNullOrWhiteSpace(this.WrapperCssClass)
								|| !String.IsNullOrWhiteSpace(this.ContentInternal);
			if (shouldRenderSpan)
			{
				builder.OpenElement(1, "span");
				builder.AddAttribute(2, "class", WrapperCssClass);
				builder.AddAttribute(3, "data-bs-container", "body");
				builder.AddAttribute(4, "data-bs-trigger", GetTriggers());
				builder.AddAttribute(5, "data-bs-placement", PlacementInternal.ToString().ToLower());
				builder.AddAttribute(6, "data-bs-custom-class", CssClass);
				builder.AddAttribute(7, "title", TitleInternal);
				if (!String.IsNullOrWhiteSpace(ContentInternal))
				{
					// used only by HxPopover
					builder.AddAttribute(8, "data-bs-content", ContentInternal);
				}
				if (Html)
				{
					builder.AddAttribute(9, "data-bs-html", "true");
				}
				builder.AddAttribute(10, "data-bs-toggle", DataBsToggle);
				if (Offset != (default, default))
				{
					builder.AddAttribute(11, "data-bs-offset", $"{Offset.X},{Offset.Y}");
				}
				builder.AddElementReferenceCapture(11, element => spanElement = element);
			}

			builder.AddContent(20, ChildContent);

			if (shouldRenderSpan)
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

			if (!disposed
				&& ((lastTitle != TitleInternal) || (lastContent != ContentInternal)))
			{
				// carefully, lastText can be null but Text empty string

				bool shouldCreateOrUpdateTooltip = !String.IsNullOrEmpty(TitleInternal) || !String.IsNullOrEmpty(ContentInternal);

				bool shouldDestroyTooltip =
					isInitialized
					&& String.IsNullOrEmpty(TitleInternal)
					&& String.IsNullOrEmpty(ContentInternal)
					&& (!String.IsNullOrEmpty(lastTitle) || !String.IsNullOrEmpty(lastContent));

				lastTitle = TitleInternal;
				lastContent = ContentInternal;

				await EnsureJsModuleAsync();

				if (shouldCreateOrUpdateTooltip)
				{
					var options = new
					{
						Sanitize = this.Sanitize
					};
					if (disposed)
					{
						return;
					}
					await jsModule.InvokeVoidAsync("createOrUpdate", spanElement, dotnetObjectReference, options);
					isInitialized = true;
				}

				if (shouldDestroyTooltip)
				{
					await jsModule.InvokeVoidAsync("destroy", spanElement);
					isInitialized = false;
				}
			}
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModule(JsModuleName);
		}

		/// <summary>
		/// Shows the component.
		/// </summary>
		public async Task ShowAsync()
		{
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("show", spanElement);
		}

		/// <summary>
		/// Hides the component.
		/// </summary>
		public async Task HideAsync()
		{
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("hide", spanElement);
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
				if (isInitialized
					&& (!String.IsNullOrEmpty(TitleInternal) || !String.IsNullOrEmpty(ContentInternal)))
				{
					await jsModule.InvokeVoidAsync("destroy", spanElement);
				}
				await jsModule.DisposeAsync();
				jsModule = null;
				isInitialized = false;
			}

			dotnetObjectReference.Dispose();
		}
	}
}
