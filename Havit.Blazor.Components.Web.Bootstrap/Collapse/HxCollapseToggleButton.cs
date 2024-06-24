namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap Collapse toggle (in the form of a button) which triggers the <see cref="HxCollapse"/> to toggle.
/// Derived from <see cref="HxButton"/> (including <see cref="HxButton.Defaults"/> inheritance).
/// </summary>
public class HxCollapseToggleButton : HxButton, IHxCollapseToggle
{
	/// <summary>
	/// Target selector of the toggle.
	/// Use <c>#id</c> to reference a single <see cref="HxCollapse"/> or <c>.class</c> for multiple <see cref="HxCollapse"/>s.
	/// </summary>
	[Parameter] public string CollapseTarget { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// This code cannot be in OnParametersSet() as there is a SetParametersAsync() override in HxNavbarToggler
		// which manipulates some of the parameters
		AdditionalAttributes ??= new Dictionary<string, object>();
		AdditionalAttributes["data-bs-toggle"] = "collapse";
		AdditionalAttributes["aria-expanded"] = false;

		if (!String.IsNullOrWhiteSpace(CollapseTarget))
		{
			AdditionalAttributes["data-bs-target"] = CollapseTarget;

			if (CollapseTarget.StartsWith("#"))
			{
				AdditionalAttributes["aria-controls"] = CollapseTarget.Substring(1);
			}
		}

		base.BuildRenderTree(builder);
	}
}
