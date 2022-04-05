
namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Input with sizing support.
	/// </summary>
	public interface IInputWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		InputSize InputSizeEffective { get; }

		string GetInputSizeCssClass() => this.InputSizeEffective.AsFormControlCssClass();

		string GetInputGroupSizeCssClass() => this.InputSizeEffective.AsInputGroupCssClass();
	}
}
