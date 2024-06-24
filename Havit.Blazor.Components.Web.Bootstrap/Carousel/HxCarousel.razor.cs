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
	/// Display controls to switch between slides.
	/// </summary>
	[Parameter] public bool Controls { get; set; }

	/// <summary>
	/// Display indicators showing which slide is active. Can also be used to switch between slides.
	/// </summary>
	[Parameter] public bool Indicators { get; set; }

	/// <summary>
	/// Show controls, captions, etc. with dark colors.
	/// </summary>
	[Parameter] public bool Dark { get; set; }

	/// <summary>
	/// Animate slides with a fade transition instead of sliding.
	/// </summary>
	[Parameter] public bool Crossfade { get; set; }

	/// <summary>
	/// Delay for automatically switching slides. Default is <c>3000 ms</c>.
	/// </summary>
	[Parameter] public int? Interval { get; set; } = 3000;

	/// <summary>
	/// Enable or disable swiping left/right on touchscreen devices to move between slides.
	/// Default is <c>true</c> (enabled).
	/// </summary>
	[Parameter] public bool TouchSwiping { get; set; } = true;

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

	/// <summary>
	/// Carousel ride (autoplay) behavior. Default is <see cref="CarouselRide.Carousel"/> (autoplays the carousel on load).
	/// </summary>
	[Parameter] public CarouselRide Ride { get; set; } = CarouselRide.Carousel;

	/// <summary>
	/// Carousel pause behavior. Default is <see cref="CarouselPause.Hover"/> (carousel will stop sliding on hover).
	/// </summary>
	[Parameter] public CarouselPause Pause { get; set; } = CarouselPause.Hover;

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
			var options = new Dictionary<string, object>();
			options["ride"] = Ride switch
			{
				CarouselRide.Carousel => "carousel",
				CarouselRide.False => false,
				CarouselRide.True => true,
				_ => throw new InvalidOperationException($"Unknown value of {nameof(CarouselRide)}: {Ride}.")
			};
			options["pause"] = Pause switch
			{
				CarouselPause.Hover => "hover",
				CarouselPause.False => false,
				_ => throw new InvalidOperationException($"Unknown value of {nameof(CarouselPause)}: {Pause}.")
			};
			await EnsureJsModule();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _elementReference, _dotnetObjectReference, options);
		}
	}

	private object GetRideJavaScriptValue()
	{
		throw new NotImplementedException();
	}

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
