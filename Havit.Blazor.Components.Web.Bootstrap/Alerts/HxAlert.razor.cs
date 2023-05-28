namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.2/components/alerts/">Bootstrap alert</see> component.<br />
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
	/// Shows the Close button and allows dismissing of the alert.
	/// </summary>
	[Parameter] public bool Dismissible { get; set; }

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxBadge)} is required.");
	}

	public string GetColorCss()
	{
		return this.Color switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as {nameof(HxAlert)} color."),
			_ => "alert-" + this.Color.ToString("f").ToLower()
		};
	}

}
