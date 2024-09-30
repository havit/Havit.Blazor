namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Represents an input with a label type.
/// </summary>
public interface IInputWithLabelType
{
	// We do not want to force the LabelType property here as only LabelTypeEffective is needed
	// to provide necessary information for the rendering.
	// The Internal implementation components do not have the LabelType, they use just LabelTypeEffective.

	/// <summary>
	/// Gets the effective label type.
	/// </summary>
	LabelType LabelTypeEffective { get; }
}
