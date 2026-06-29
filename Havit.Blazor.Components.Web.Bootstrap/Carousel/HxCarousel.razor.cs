using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A slideshow component for cycling through elements—images or slides of text—like a carousel.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCarousel">https://havit.blazor.eu/components/HxCarousel</see>
/// </summary>
public partial class HxCarousel : IAsyncDisposable
{
	internal const string ItemsRegistrationCascadingValueName = "ItemsRegistration";

	/// <summary>
	/// Content of the carousel.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Carousel CSS class. Added to root div <c>.carousel</c>.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Display previous/next controls to switch between slides.
	/// </summary>
	[Parameter] public bool Controls { get; set; }

	/// <summary>
	/// Display indicators showing which slide is active. Can also be used to switch between slides.
	/// </summary>
	[Parameter] public bool Indicators { get; set; }

	/// <summary>
	/// Display a play/pause button that toggles autoplay. Most useful together with <see cref="Autoplay"/>.
	/// </summary>
	[Parameter] public bool PlayPauseControl { get; set; }

	/// <summary>
	/// Overlay the controls and indicators on top of the slides (<c>.carousel-overlay</c>) instead of
	/// rendering them below. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool Overlay { get; set; }

	/// <summary>
	/// Animate slides with a fade transition instead of sliding.
	/// </summary>
	[Parameter] public bool Crossfade { get; set; }

	/// <summary>
	/// Snap the active slide to the center of the viewport instead of its start (<c>.carousel-center</c>).
	/// Pairs nicely with <see cref="ItemsPeek"/>.
	/// </summary>
	[Parameter] public bool Center { get; set; }

	/// <summary>
	/// Let each <see cref="HxCarouselItem"/> size itself (<c>.carousel-auto</c>) for variable-width slides.
	/// Set the width of every item via its <see cref="HxCarouselItem.CssClass"/> or inline styles.
	/// </summary>
	[Parameter] public bool VariableWidth { get; set; }

	/// <summary>
	/// Number of whole slides visible per view (<c>--bs-carousel-items</c>). Default (<c>null</c>) shows one.
	/// </summary>
	[Parameter] public int? VisibleItems { get; set; }

	/// <summary>
	/// Space between slides as a CSS length (<c>--bs-carousel-items-gap</c>), e.g. <c>"1rem"</c>.
	/// </summary>
	[Parameter] public string ItemsGap { get; set; }

	/// <summary>
	/// How much of the neighboring slides to reveal, as a CSS length (<c>--bs-carousel-items-peek</c>), e.g. <c>"3rem"</c>.
	/// </summary>
	[Parameter] public string ItemsPeek { get; set; }

	/// <summary>
	/// Automatically cycle the carousel on load. Default is <c>false</c>.
	/// Autoplay pauses on hover (see <see cref="Pause"/>) and stops for good once the visitor takes control.
	/// </summary>
	[Parameter] public bool Autoplay { get; set; }

	/// <summary>
	/// What happens at the first and last slide. Default is <see cref="CarouselEnds.Loop"/>.
	/// </summary>
	[Parameter] public CarouselEnds Ends { get; set; } = CarouselEnds.Loop;

	/// <summary>
	/// Whether the carousel reacts to keyboard events. Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool Keyboard { get; set; } = true;

	/// <summary>
	/// Delay (ms) between automatically cycling to the next item while <see cref="Autoplay"/> is enabled.
	/// When <c>null</c> (default), Bootstrap's default of <c>5000 ms</c> is used. Can be overridden per item via <see cref="HxCarouselItem.Interval"/>.
	/// </summary>
	[Parameter] public int? Interval { get; set; }

	/// <summary>
	/// Carousel pause behavior. Default is <see cref="CarouselPause.Hover"/> (autoplay stops sliding on hover).
	/// </summary>
	[Parameter] public CarouselPause Pause { get; set; } = CarouselPause.Hover;

	/// <summary>
	/// Additional attributes to be splatted onto the root <c>.carousel</c> element
	/// (e.g. <c>data-bs-theme="dark"</c> for a dark carousel).
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

	/// <summary>
	/// Is fired when the current slide is changed (at the very start of the sliding transition).
	/// </summary>
	[Parameter] public EventCallback<CarouselSlideEventArgs> OnSlide { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnSlide"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnSlideAsync(CarouselSlideEventArgs eventArgs) => OnSlide.InvokeAsync(eventArgs);


	/// <summary>
	/// Is fired when the current slide is changed (once the transition is completed).
	/// </summary>
	[Parameter] public EventCallback<CarouselSlideEventArgs> OnSlid { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnSlid"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnSlidAsync(CarouselSlideEventArgs eventArgs) => OnSlid.InvokeAsync(eventArgs);

	[Inject] protected IJSRuntime JSRuntime { get; set; }


	private IJSObjectReference _jsModule;
	private string _id = "hx" + Guid.NewGuid().ToString("N");
	private DotNetObjectReference<HxCarousel> _dotnetObjectReference;
	private ElementReference _elementReference;
	private List<HxCarouselItem> _items;
	private CollectionRegistration<HxCarouselItem> _itemsRegistration;
	private bool _disposed = false;

	public HxCarousel()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
		_items = new List<HxCarouselItem>();
		_itemsRegistration = new CollectionRegistration<HxCarouselItem>(_items,
			async () => await InvokeAsync(StateHasChanged),
			() => _disposed);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			var options = new Dictionary<string, object>
			{
				["autoplay"] = Autoplay,
				["keyboard"] = Keyboard,
				["ends"] = Ends switch
				{
					CarouselEnds.Loop => "loop",
					CarouselEnds.Wrap => "wrap",
					CarouselEnds.Stop => "stop",
					_ => throw new InvalidOperationException($"Unknown value of {nameof(CarouselEnds)}: {Ends}.")
				},
				["pause"] = Pause switch
				{
					CarouselPause.Hover => "hover",
					CarouselPause.False => false,
					_ => throw new InvalidOperationException($"Unknown value of {nameof(CarouselPause)}: {Pause}.")
				}
			};
			if (Interval is not null)
			{
				options["interval"] = Interval.Value;
			}

			await EnsureJsModule();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _elementReference, _dotnetObjectReference, options);
		}
	}

	/// <summary>
	/// Builds the inline <c>style</c> for the root element from the CSS-variable layout parameters,
	/// merged with any <c>style</c> supplied through <see cref="AdditionalAttributes"/>.
	/// </summary>
	private string GetStyle()
	{
		var sb = new StringBuilder();
		// Bootstrap uses --bs-carousel-items as a divisor in the slide flex-basis calc,
		// so a non-positive value produces invalid CSS / a broken layout. Ignore it.
		if (VisibleItems is > 0)
		{
			sb.Append("--bs-carousel-items: ").Append(VisibleItems.Value).Append(';');
		}
		if (!string.IsNullOrEmpty(ItemsGap))
		{
			sb.Append("--bs-carousel-items-gap: ").Append(ItemsGap).Append(';');
		}
		if (!string.IsNullOrEmpty(ItemsPeek))
		{
			sb.Append("--bs-carousel-items-peek: ").Append(ItemsPeek).Append(';');
		}
		if (AdditionalAttributes is not null && AdditionalAttributes.TryGetValue("style", out var style) && style is not null)
		{
			sb.Append(style);
		}
		return sb.Length == 0 ? null : sb.ToString();
	}

	/// <summary>
	/// <see cref="AdditionalAttributes"/> without <c>style</c> (rendered separately by <see cref="GetStyle"/>),
	/// to avoid a duplicate-attribute render error.
	/// </summary>
	private IEnumerable<KeyValuePair<string, object>> GetAttributesExceptStyle()
		=> AdditionalAttributes?.Where(a => !string.Equals(a.Key, "style", StringComparison.OrdinalIgnoreCase))
			?? Enumerable.Empty<KeyValuePair<string, object>>();

	protected async Task EnsureJsModule()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxCarousel));
	}


	[JSInvokable("HxCarousel_HandleSlide")]
	public async Task HandleSlide(int from, int to, string direction)
	{
		CarouselSlideEventArgs eventArgs = new()
		{
			From = from,
			To = to,
			Direction = direction
		};

		await InvokeOnSlideAsync(eventArgs);
	}

	[JSInvokable("HxCarousel_HandleSlid")]
	public async Task HandleSlid(int from, int to, string direction)
	{
		CarouselSlideEventArgs eventArgs = new()
		{
			From = from,
			To = to,
			Direction = direction
		};

		await InvokeOnSlidAsync(eventArgs);
	}

	/// <summary>
	/// Slides to an element with the corresponding index.
	/// </summary>
	public async Task SlideToAsync(int index)
	{
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("slideTo", _elementReference, index);
	}

	/// <summary>
	/// Slides to the previous item (to the left).
	/// </summary>
	public async Task SlideToPreviousItemAsync()
	{
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("previous", _elementReference);
	}

	/// <summary>
	/// Slides to the next item (to the right).
	/// </summary>
	public async Task SlideToNextItemAsync()
	{
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("next", _elementReference);
	}

	/// <summary>
	/// Start cycling between slides.
	/// </summary>
	public async Task CycleAsync()
	{
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("cycle", _elementReference);
	}

	/// <summary>
	/// Pause cycling.
	/// </summary>
	public async Task PauseAsync()
	{
		await EnsureJsModule();
		await _jsModule.InvokeVoidAsync("pause", _elementReference);
	}

	/// <summary>
	/// Dispose the carousel.
	/// </summary>
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();

		//Dispose(disposing: false);
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule is not null)
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
