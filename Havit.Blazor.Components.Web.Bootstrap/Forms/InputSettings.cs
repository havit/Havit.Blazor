namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the components derived from <see cref="HxInputBase"/>.
/// </summary>
public record InputSettings
{
	/// <summary>
	/// Specifies how the validation message should be displayed.
	/// </summary>
	public ValidationMessageMode? ValidationMessageMode { get; set; }
	/// <summary>
	/// Specifies how the label should be displayed.
	/// </summary>
	public LabelType? LabelType { get; set; }
}
