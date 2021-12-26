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
		IInputSettingsWithSize GetSettings() => null; // TODO Remove as all components will have their Settings implemented

		InputSize InputSizeEffective => this.InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default has to be set.");

		string GetInputSizeCssClass() => this.InputSizeEffective.AsFormControlCssClass();

		string GetInputGroupSizeCssClass() => this.InputSizeEffective.AsInputGroupCssClass();
	}
}
