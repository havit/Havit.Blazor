namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Represents an input with a label type.
/// </summary>
public interface IInputWithLabelType
{
	/// <summary>
	/// Gets or sets the label type.
	/// </summary>
	LabelType? LabelType { get; }

	/// <summary>
	/// Gets the effective label type.
	/// </summary>
	LabelType LabelTypeEffective => LabelType ?? Bootstrap.LabelType.Regular;
}
