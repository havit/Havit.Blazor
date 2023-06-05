namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/progress/">Bootstrap 5 Progress</see> component.<br/>
/// A wrapping component for the <see cref="HxProgressBar" />.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxProgress">https://havit.blazor.eu/components/HxProgress</see>
/// </summary>
public partial class HxProgress
{
	/// <summary>
	/// Content consisting of one or multiple <c>HxProgressBar</c> components.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS classes for the progress.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Height of all inner progress bars. Default is <c>15px</c>.
	/// </summary>
	[Parameter] public int Height { get; set; } = 15;

	/// <summary>
	/// Lowest possible value. Default is <c>0</c>.
	/// </summary>
	[Parameter] public float MinValue { get; set; } = 0;

	/// <summary>
	/// Highest possible value. Default is <c>100</c>.
	/// </summary>
	[Parameter] public float MaxValue { get; set; } = 100;

	/// <summary>
	/// If <c>true</c>, applies a stripe via CSS gradient over the progress bar's background color.
	/// </summary>
	[Parameter] public bool Striped { get; set; }

	/// <summary>
	/// If <c>true</c>, stripes are animated right to left via CSS3 animations, stripes are automatically switched on.
	/// </summary>
	[Parameter] public bool Animated { get; set; }
}
