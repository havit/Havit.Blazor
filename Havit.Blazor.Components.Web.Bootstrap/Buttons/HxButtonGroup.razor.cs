namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Groups related <see cref="HxButton"/> controls into a single visual block, with support for sizing, nesting, and vertical stacking (<see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/button-group/">Bootstrap Button groups</see>).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxButtonGroup">https://havit.blazor.eu/components/HxButtonGroup</see>
/// </summary>
public partial class HxButtonGroup
{
	internal const string InButtonGroupCascadingValueName = nameof(HxButtonGroup) + "_InButtonGroup_CascadingValue";

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Size. The default value is <see cref="ButtonGroupSize.Regular"/>.
	/// </summary>
	[Parameter] public ButtonGroupSize Size { get; set; } = ButtonGroupSize.Regular;

	/// <summary>
	/// Orientation. The default value is <see cref="ButtonGroupOrientation.Horizontal"/>.
	/// </summary>
	[Parameter] public ButtonGroupOrientation Orientation { get; set; } = ButtonGroupOrientation.Horizontal;

	/// <summary>
	/// When <c>true</c>, renders a thin divider between the buttons (<c>.btn-group-divider</c>), typically used with solid buttons of the same color.
	/// The default value is <c>false</c>.
	/// </summary>
	[Parameter] public bool Divider { get; set; }

	/// <summary>
	/// Groups should be given an explicit label, as most assistive technologies will otherwise not announce them, despite the presence of the correct role attribute. 
	/// </summary>
	[Parameter] public string AriaLabel { get; set; }

	/// <summary>
	/// Additional CSS class(es) to be added.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	protected string GetCoreCssClass()
	{
		if (Orientation == ButtonGroupOrientation.Vertical)
		{
			return "btn-group-vertical";
		}
		return "btn-group";
	}

	protected string GetSizeCssClass()
	{
		return Size switch
		{
			ButtonGroupSize.Regular => null,
			ButtonGroupSize.Small => "btn-group-sm",
			ButtonGroupSize.Large => "btn-group-lg",
			ButtonGroupSize.ExtraSmall => "btn-group-xs",
			_ => throw new InvalidOperationException($"Unknown {nameof(ButtonGroupSize)}: {Size}.")
		};
	}
}
