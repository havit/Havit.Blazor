namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxSelect{TValue, TItem}"/> component.
/// </summary>
public record SelectSettings : InputSettings
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

}
