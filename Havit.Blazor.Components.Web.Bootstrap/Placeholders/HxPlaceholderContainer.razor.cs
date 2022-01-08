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
		/// Application-wide defaults for <see cref="HxPlaceholderContainer"/> and derived components.
		/// </summary>
		public static PlaceholderContainerSettings Defaults { get; set; }

		static HxPlaceholderContainer()
		{
			Defaults = new PlaceholderContainerSettings()
			{
				Animation = PlaceholderAnimation.None,
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual PlaceholderContainerSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public PlaceholderContainerSettings Settings { get; set; }

		/// <summary>
		/// Animation of the placeholders in container.
		/// </summary>
		[Parameter] public PlaceholderAnimation? Animation { get; set; }
		protected PlaceholderAnimation AnimationEffective => this.Animation ?? this.Settings?.Animation ?? GetDefaults().Animation ?? throw new InvalidOperationException(nameof(Animation) + " default for " + nameof(HxPlaceholderContainer) + " has to be set.");

		/// <summary>
		/// Size of the placeholders (propagated to the child <see cref="HxPlaceholder"/>s).
		/// </summary>
		[Parameter] public PlaceholderSize? Size { get; set; }

		/// <summary>
		/// Color of the placeholders (propagated to the child <see cref="HxPlaceholder"/>s).
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
		protected string CssClassEffective => this.CssClass ?? this.Settings?.CssClass ?? GetDefaults().CssClass;

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		protected virtual string GetCssClass()
		{
			return CssClassHelper.Combine(
				GetAnimationCssClass(),
				this.CssClassEffective);
		}

		private string GetAnimationCssClass()
		{
			return (this.AnimationEffective) switch
			{
				PlaceholderAnimation.None => null,
				PlaceholderAnimation.Glow => "placeholder-glow",
				PlaceholderAnimation.Wave => "placeholder-wave",
				_ => throw new InvalidOperationException($"Unknown {nameof(PlaceholderAnimation)} value {this.AnimationEffective}.")
			};
		}
	}
}
