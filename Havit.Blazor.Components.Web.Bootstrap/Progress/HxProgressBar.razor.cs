namespace Havit.Blazor.Components.Web.Bootstrap;

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
	/// Current value (proportion of the progress bar that is filled).
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

	protected float MinValueEffective => MinValue ?? ProgressBarContainer.MinValue;
	protected float MaxValueEffective => MaxValue ?? ProgressBarContainer.MaxValue;
	protected bool StripedEffective => Striped ?? ProgressBarContainer.Striped;
	protected bool AnimatedEffective => Animated ?? ProgressBarContainer.Animated;

	private string GetStripedCssClass()
	{
		string classes = "";

		if (AnimatedEffective)
		{
			classes = "progress-bar-striped progress-bar-animated";
		}
		else if (StripedEffective)
		{
			classes = "progress-bar-striped";
		}

		return classes;
	}

	private int GetNormalizedValue()
	{
		if (MinValueEffective == 0 && MaxValueEffective == 100)
		{
			return (int)Value;
		}

		float normalized = (float)((100f / (MaxValueEffective - MinValueEffective) * (Value - MaxValueEffective)) + 100f);
		return (int)normalized;
	}
}
