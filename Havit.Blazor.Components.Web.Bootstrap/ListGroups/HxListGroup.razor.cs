namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/list-group/">Bootstrap 5 List group</see> component.<br/>
/// List groups are a flexible and powerful component for displaying a series of content.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxListGroup">https://havit.blazor.eu/components/HxListGroup</see>
/// </summary>
public partial class HxListGroup
{
	/// <summary>
	/// Content of the list group component.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// If set to <c>true</c>, removes borders and rounded corners to render list group items edge-to-edge.
	/// </summary>
	[Parameter] public bool Flush { get; set; }

	/// <summary>
	/// Set to <c>true</c> to opt into numbered list group items. The list group changes from an unordered list to an ordered list.
	/// </summary>
	[Parameter] public bool Numbered { get; set; }

	/// <summary>
	/// Changes the layout of the list group items from vertical to horizontal. Cannot be combined with <see cref="Flush"/>.
	/// </summary>
	[Parameter] public ListGroupHorizontal Horizontal { get; set; } = ListGroupHorizontal.Never;

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <see cref="HxListGroup"/> component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected virtual string GetClasses()
	{
		return CssClassHelper.Combine(
			"list-group",
			this.Flush ? "list-group-flush" : null,
			this.Numbered ? "list-group-numbered" : null,
			GetHorizontalCssClass(),
			this.CssClass);
	}

	protected virtual string GetHorizontalCssClass()
	{
		return this.Horizontal switch
		{
			ListGroupHorizontal.Never => null,
			ListGroupHorizontal.Always => "list-group-horizontal",
			ListGroupHorizontal.SmallUp => "list-group-horizontal-sm",
			ListGroupHorizontal.MediumUp => "list-group-horizontal-md",
			ListGroupHorizontal.LargeUp => "list-group-horizontal-lg",
			ListGroupHorizontal.ExtraLargeUp => "list-group-horizontal-xl",
			ListGroupHorizontal.XxlUp => "list-group-horizontal-xxl",
			_ => throw new InvalidOperationException($"Unknown {nameof(ListGroupHorizontal)} value {this.Horizontal}.")
		};
	}
}
