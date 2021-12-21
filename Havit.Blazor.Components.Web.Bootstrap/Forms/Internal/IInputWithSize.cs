using Havit.Blazor.Components.Web.Bootstrap.Forms;

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
		InputSize? InputSize { get; set; }

		IInputSettingsWithSize GetDefaults();

		InputSize InputSizeEffective => this.InputSize ?? GetDefaults().InputSize;

		string GetInputSizeCssClass() => this.InputSizeEffective.AsFormControlCssClass();

		string GetInputGroupSizeCssClass() => this.InputSizeEffective.AsInputGroupCssClass();
	}
}
