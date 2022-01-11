using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCarouselItem : IDisposable
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

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ItemsRegistration.Unregister(this);
			}
		}

	}
}
