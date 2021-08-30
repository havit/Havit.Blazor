using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCarouselItem
	{
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string CssClass { get; set; }
		[Parameter] public bool Active { get; set; }
		[CascadingParameter] protected HxCarousel ParentCarousel { get; set; }
		/// <summary>
		/// Time before automatically cycling to the next item.
		/// </summary>
		[Parameter] public int Interval { get; set; }

		protected override void OnParametersSet()
		{
			if (ParentCarousel is null)
			{
				throw new NullReferenceException();
			}

			//Contract.Requires<InvalidOperationException>(ParentCarousel is not null, "<HxCarouselItem /> has to be placed inside <HxCarousel />.");

			if (!ParentCarousel.Items.Contains(this))
			{
				ParentCarousel.Items.Add(this);
			}
		}
	}
}
