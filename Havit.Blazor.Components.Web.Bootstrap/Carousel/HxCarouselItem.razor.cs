using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCarouselItem : IAsyncDisposable
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public string CssClass { get; set; }

		[Parameter] public bool Active { get; set; }

		/// <summary>
		/// Time before automatically cycling to the next item.
		/// </summary>
		[Parameter] public int? Interval { get; set; }

		/// <summary>
		/// Cascading parameter to register the tab.
		/// </summary>
		[CascadingParameter(Name = HxCarousel.ItemsRegistrationCascadingValueName)]
		protected CollectionRegistration<HxCarouselItem> ItemsRegistration { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Contract.Requires<InvalidOperationException>(ItemsRegistration != null, $"{nameof(HxCarouselItem)} has to be inside {nameof(HxCarousel)}.");
			ItemsRegistration.Register(this);
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();
		}

		protected virtual async Task DisposeAsyncCore()
		{
			await ItemsRegistration.UnregisterAsync(this);
		}
	}
}
