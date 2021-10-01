using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Optional container for the <see cref="HxPlaceholder"/> components where you can set the animation and some common properties
	/// for all placeholders contained.
	/// </summary>
	public partial class HxPlaceholderContainer
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxPlaceholderContainer"/>.
		/// </summary>
		public static PlaceholderContainerDefaults Defaults { get; set; } = new();

		/// <summary>
		/// Animation of the placeholders in container.
		/// </summary>
		[Parameter] public PlaceholderAnimation? Animation { get; set; }

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

		/// <summary>
		/// Return <see cref="HxPlaceholderContainer"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual PlaceholderContainerDefaults GetDefaults() => Defaults;

		protected virtual string GetCssClass()
		{
			return CssClassHelper.Combine(
				GetAnimationCssClass(),
				this.CssClass ?? GetDefaults().CssClass);
		}

		private string GetAnimationCssClass()
		{
			return (this.Animation ?? GetDefaults().Animation) switch
			{
				PlaceholderAnimation.None => null,
				PlaceholderAnimation.Glow => "placeholder-glow",
				PlaceholderAnimation.Wave => "placeholder-wave",
				_ => throw new InvalidOperationException($"Unknown {nameof(PlaceholderAnimation)} value {this.Animation}.")
			};
		}
	}
}
