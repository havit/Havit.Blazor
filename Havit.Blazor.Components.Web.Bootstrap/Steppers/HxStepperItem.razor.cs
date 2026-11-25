namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A single step for <see cref="HxStepper"/>.
/// The step number is rendered automatically by Bootstrap (CSS counter).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxStepper">https://havit.blazor.eu/components/HxStepper</see>
/// </summary>
public partial class HxStepperItem
{
	/// <summary>
	/// Step label.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Content of the step (an alternative to <see cref="Text"/> which allows complex content,
	/// such as descriptions, helper messages, status indicators, badges, or other decorative elements).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Link of the step. When set, the item renders as an anchor element
	/// (and the whole stepper renders as a <c>div</c> instead of an ordered list).
	/// </summary>
	[Parameter] public string Href { get; set; }

	/// <summary>
	/// Set to <c>true</c> for completed steps and the current step (highlights the step number and the connecting line).
	/// Bootstrap automatically renders the connecting line following the last active step in the inactive color.
	/// </summary>
	[Parameter] public bool Active { get; set; }

	/// <summary>
	/// Theme color of the step (renders the <c>theme-*</c> class).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the step
	/// (e.g. use <c>align-items-start</c> to align multi-line content with the step number).
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[CascadingParameter] protected HxStepper StepperContainer { get; set; }

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (Href is not null)
		{
			StepperContainer?.RegisterAnchorItem();
		}
	}

	private string GetClasses()
	{
		return CssClassHelper.Combine(
			"stepper-item",
			Active ? "active" : null,
			GetColorCssClass(),
			CssClass);
	}

	private string GetColorCssClass()
	{
		return Color switch
		{
			null => null,
			ThemeColor.None => null,
			_ => Color.Value.ToThemeCss()
		};
	}
}
