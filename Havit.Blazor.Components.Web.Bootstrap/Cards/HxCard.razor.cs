using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap <a href="https://getbootstrap.com/docs/5.1/components/card/">Card</a> component.
	/// </summary>
	public partial class HxCard
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxCard"/>.
		/// </summary>
		public static CardDefaults Defaults { get; set; } = new CardDefaults();

		/// <summary>
		/// Image to be placed in the card. For image position see <see cref="ImagePlacement"/>.
		/// </summary>
		[Parameter] public string ImageSrc { get; set; }

		/// <summary>
		/// Placement of the image. Default is <see cref="CardImagePlacement.Top"/>.
		/// </summary>
		[Parameter] public CardImagePlacement ImagePlacement { get; set; }

		/// <summary>
		/// Image <c>alt</c> attribute value.
		/// </summary>
		[Parameter] public string ImageAlt { get; set; }

		/// <summary>
		/// Image <c>width</c> attribute value.
		/// </summary>
		[Parameter] public int? ImageWidth { get; set; }

		/// <summary>
		/// Image <c>height</c> attribute value.
		/// </summary>
		[Parameter] public int? ImageHeight { get; set; }

		/// <summary>
		/// Header content.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Body content.
		/// </summary>
		[Parameter] public RenderFragment BodyTemplate { get; set; }

		/// <summary>
		/// Footer content.
		/// </summary>
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Generic card content (outside <c>.card-body</c>).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS classes for the card-container.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the header.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the body.
		/// </summary>
		[Parameter] public string BodyCssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the footer.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		/// <summary>
		/// Return <see cref="HxCard"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual CardDefaults GetDefaults() => Defaults;
	}
}
