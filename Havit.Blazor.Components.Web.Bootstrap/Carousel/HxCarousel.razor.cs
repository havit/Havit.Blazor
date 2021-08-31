using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// A slideshow component for cycling through elements—images or slides of text—like a carousel.
	/// </summary>
	public partial class HxCarousel
	{
		[Inject] private IJSRuntime JSRuntime { get; set; }
		private IJSObjectReference jsModule;
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
		/// Show controls, captions, etc. to dark colors.
		/// </summary>
		[Parameter] public bool Dark { get; set; } = true;
		/// <summary>
		/// Animate slides with a fade transition instead of slide.
		/// </summary>
		[Parameter] public bool Crossfade { get; set; }
		/// <summary>
		/// Delay for automatically switching slides. Default is 3000 ms.
		/// </summary>
		[Parameter] public int? Interval { get; set; } = 3000;
		/// <summary>
		/// Enable or disable swiping left/right on touchscreen devices to move between slides.
		/// </summary>
		[Parameter] public bool TouchSwiping { get; set; } = true;
		/// <summary>
		/// Is fired when the current slide is changed (at the very start of the sliding transition).
		/// </summary>
		[Parameter] public EventCallback OnSlide { get; set; }
		/// <summary>
		/// Is fired when the current slide is changed (once the transition is completed).
		/// </summary>
		[Parameter] public EventCallback OnSlid { get; set; }

		/// <summary>
		/// A list of all <c>HxCarouselItem</c> components contained in the carousel.
		/// </summary>
		public List<HxCarouselItem> Items { get; set; } = new();

		private string id = "hx" + Guid.NewGuid().ToString("N");
		private DotNetObjectReference<HxCarousel> dotnetObjectReference;

		public HxCarousel()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxCarousel) + ".js");
				await jsModule.InvokeVoidAsync("AddEventListeners", id, dotnetObjectReference);

				InitializeCarousel();
				await InvokeAsync(StateHasChanged);
			}
		}

		[JSInvokable("HxCarousel_HandleSlide")]
		public async Task HandleSlide()
		{
			await OnSlide.InvokeAsync();
		}

		[JSInvokable("HxCarousel_HandleSlid")]
		public async Task HandleSlid()
		{
			await OnSlid.InvokeAsync();
		}

		private void InitializeCarousel()
		{
			jsModule.InvokeVoidAsync("InitializeCarousel", id);
		}

		public void SetInterval(int interval)
		{
			jsModule.InvokeVoidAsync("SetInterval", id, interval);
		}

		/// <summary>
		/// Slides to an element with the coresponding index.
		/// </summary>
		/// <param name="index"></param>
		public void SlideTo(int index)
		{
			jsModule.InvokeVoidAsync("SlideTo", id, index);
		}

		/// <summary>
		/// Slides to the last item.
		/// </summary>
		public void SlideToLastItem()
		{
			if (Items.Count <= 0)
			{
				return;
			}

			SlideTo(Items.Count - 1);
		}

		/// <summary>
		/// Slides to the previous item in the collection (to the left).
		/// </summary>
		public void SlideToPreviousItem()
		{
			jsModule.InvokeVoidAsync("Previous", id);
		}

		/// <summary>
		/// Slides to the next item in the collection (to the right).
		/// </summary>
		public void SlideToNextItem()
		{
			jsModule.InvokeVoidAsync("Next", id);
		}

		/// <summary>
		/// Start cycling between slides.
		/// </summary>
		public void Cycle()
		{
			jsModule.InvokeVoidAsync("Cycle", id);
		}

		/// <summary>
		/// Pause cycling.
		/// </summary>
		public void Pause()
		{
			jsModule.InvokeVoidAsync("Pause", id);
		}

		/// <summary>
		/// Dispose the carousel.
		/// </summary>
		public void Dispose()
		{
			jsModule.InvokeVoidAsync("Dispose", id);
		}
	}
}
