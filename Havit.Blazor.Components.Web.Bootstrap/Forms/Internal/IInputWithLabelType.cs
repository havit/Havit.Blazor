namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Input with label type.
	/// </summary>
	public interface IInputWithLabelType
	{
		/// <summary>
		/// Label type.
		/// </summary>
		LabelType? LabelType { get; }

		/// <summary>
		/// Effective label type.
		/// </summary>
		LabelType LabelTypeEffective => this.LabelType ?? Bootstrap.LabelType.Regular;
	}
}
