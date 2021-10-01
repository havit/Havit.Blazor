using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxPlaceholderContainer
	{
		/// <summary>
		/// Animation of the placeholders in container.
		/// </summary>
		[Parameter] public PlaceholderAnimation Animation { get; set; }

		/// <summary>
		/// Size of the placeholder.
		/// </summary>
		[Parameter] public PlaceholderSize? Size { get; set; }

		/// <summary>
		/// Color of the placeholder.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Content of the placeholder container (put your <see cref="HxPlaceholder"/>s here).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		protected virtual string GetCssClass()
		{
			return CssClassHelper.Combine(
				GetAnimationCssClass(),
				this.CssClass);
		}

		private string GetAnimationCssClass()
		{
			return this.Animation switch
			{
				PlaceholderAnimation.None => null,
				PlaceholderAnimation.Glow => "placeholder-glow",
				PlaceholderAnimation.Wave => "placeholder-wave",
				_ => throw new InvalidOperationException($"Unknown {nameof(PlaceholderAnimation)} value {this.Animation}.")
			};
		}
	}
}
