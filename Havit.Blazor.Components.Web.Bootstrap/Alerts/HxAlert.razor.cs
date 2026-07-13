namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Contextual feedback message (success, warning, danger, info, and more) with an optional icon, links, and a dismiss button.<br />
/// Renders the <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/alert/">Bootstrap alert</see> markup.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAlert">https://havit.blazor.eu/components/HxAlert</see>
/// </summary>
public partial class HxAlert
{
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Alert color (background). Required.
	/// </summary>
	[Parameter, EditorRequired] public ThemeColor Color { get; set; }

	/// <summary>
	/// Shows the Close button and allows dismissing the alert.
	/// </summary>
	[Parameter] public bool Dismissible { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxAlert)} is required.");
	}

	public string GetColorCss()
	{
		return Color switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as {nameof(HxAlert)} color."),
			_ => Color.ToThemeCss()
		};
	}

}
