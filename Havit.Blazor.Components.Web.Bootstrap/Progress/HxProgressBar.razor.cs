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
		[Parameter] public int Value { get; set; }

		/// <summary>
		/// Lowest possible value. Default is <c>0</c>.
		/// </summary>
		[Parameter] public int MinValue { get; set; } = 0;

		/// <summary>
		/// Highest possible value. Default is <c>100</c>.
		/// </summary>
		[Parameter] public int MaxValue { get; set; } = 100;

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
		[Parameter] public bool Striped { get; set; }

		/// <summary>
		/// If <c>true</c>, stripes are animated right to left via CSS3 animations, stripes are automatically switched on.
		/// </summary>
		[Parameter] public bool Animated { get; set; }

		private string GetStripedCssClass()
		{
			string classes = "";

			if (Striped)
			{
				classes = "progress-bar-striped";
			}

			if (Animated)
			{
				classes = "progress-bar-striped progress-bar-animated";
			}

			return classes;
		}

		private string GetColorCssClass()
		{
			return Color switch
			{
				null => null,
				ThemeColor.None => null,
				_ => "bg-" + this.Color.Value.ToString("f").ToLower()
			};
		}
	}
}
