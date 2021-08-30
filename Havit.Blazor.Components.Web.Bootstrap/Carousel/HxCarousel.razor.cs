using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCarousel
	{
		[Inject] private IJSRuntime JSRuntime { get; set; }
		private IJSObjectReference jsModule;
		[Parameter] public RenderFragment ChildContent { get; set; }
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
		[Parameter] public bool Dark { get; set; }
		/// <summary>
		/// Animate slides with a fade transition instead of slide.
		/// </summary>
		[Parameter] public bool Crossfade { get; set; }
		[Parameter] public int Interval { get; set; }
		/// <summary>
		/// Enable or disable swiping left/right on touchscreen devices to move between slides.
		/// </summary>
		[Parameter] public bool TouchSwiping { get; set; }
		/// <summary>
		/// Is fired when the current slide is changed (at the very start of the sliding transition).
		/// </summary>
		[Parameter] public EventCallback OnSlide { get; set; }
		/// <summary>
		/// Is fired when the current slide is changed (once the transition is completed).
		/// </summary>
		[Parameter] public EventCallback OnSlid { get; set; }

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

		public void SlideTo(int index)
		{
			jsModule.InvokeVoidAsync("SlideTo", id, index);
		}

		public void SlideToLastItem()
		{
			SlideTo(Items.Count - 1);
		}

		public void SlideToPreviousItem()
		{
			jsModule.InvokeVoidAsync("Previous", id);
		}

		public void SlideToNextItem()
		{
			jsModule.InvokeVoidAsync("Next", id);
		}

		public void Cycle()
		{
			jsModule.InvokeVoidAsync("Cycle", id);
		}

		public void Pause()
		{
			jsModule.InvokeVoidAsync("Pause", id);
		}

		public void Dispose()
		{
			jsModule.InvokeVoidAsync("Dispose", id);
		}
	}
}
