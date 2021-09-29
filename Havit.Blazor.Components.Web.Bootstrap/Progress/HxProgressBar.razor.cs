using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// A progress bar to be placed inside <see cref="HxProgress" />.
	/// </summary>
	public partial class HxProgressBar
	{
		/// <summary>
		/// Inner content of the progress bar.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS classes to be applied to the progress bar.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Current value (proportion of the progress bar that is taken up).
		/// </summary>
		[Parameter] public float Value { get; set; }

		/// <summary>
		/// Lowest possible value. Default is <c>0</c>.
		/// </summary>
		[Parameter] public float? MinValue { get; set; }

		/// <summary>
		/// Highest possible value. Default is <c>100</c>.
		/// </summary>
		[Parameter] public float? MaxValue { get; set; }

		/// <summary>
		/// Text to be displayed on the progress bar.
		/// </summary>
		[Parameter] public string Label { get; set; }

		/// <summary>
		/// Fill (background) color of the progress bar.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// If <c>true</c>, applies a stripe via CSS gradient over the progress bar's background color.
		/// </summary>
		[Parameter] public bool? Striped { get; set; }

		/// <summary>
		/// If <c>true</c>, stripes are animated right to left via CSS3 animations, stripes are automatically switched on.
		/// </summary>
		[Parameter] public bool? Animated { get; set; }

		[CascadingParameter] protected HxProgress ProgressBarContainer { get; set; }

		protected override void OnParametersSet()
		{
			if (ProgressBarContainer is not null)
			{
				if (!MinValue.HasValue)
				{
					MinValue = ProgressBarContainer.MinValue;
				}
				if (!MaxValue.HasValue)
				{
					MaxValue = ProgressBarContainer.MaxValue;
				}
				if (!Striped.HasValue)
				{
					Striped = ProgressBarContainer.Striped;
				}
				if (!Animated.HasValue)
				{
					Animated = ProgressBarContainer.Animated;
				}
			}
		}

		private string GetStripedCssClass()
		{
			string classes = "";

			if (Animated.GetValueOrDefault())
			{
				classes = "progress-bar-striped progress-bar-animated";
			}
			else if (Striped.GetValueOrDefault())
			{
				classes = "progress-bar-striped";
			}

			return classes;
		}

		private int GetNormalizedValue()
		{
			if (MinValue == 0 && MaxValue == 100)
			{
				return (int)Value;
			}

			float normalized = (float)((100f / (MaxValue - MinValue) * (Value - MaxValue)) + 100f);
			return (int)normalized;
		}
	}
}
