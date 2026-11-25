namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxStepper"/>.
/// </summary>
public record StepperSettings
{
	/// <summary>
	/// Layout of the stepper items (vertical/horizontal, including the responsive breakpoint).
	/// </summary>
	public StepperHorizontal? Horizontal { get; set; }

	/// <summary>
	/// Theme color of the stepper (renders the <c>theme-*</c> class).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// When <c>true</c>, the stepper is wrapped in a scrollable <c>stepper-overflow</c> container.
	/// </summary>
	public bool? Overflow { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the stepper.
	/// </summary>
	public string CssClass { get; set; }
}
