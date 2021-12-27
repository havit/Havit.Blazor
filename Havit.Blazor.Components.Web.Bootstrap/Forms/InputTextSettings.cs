using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxInputText"/> and derived components.
	/// </summary>
	public record InputTextSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize? InputSize { get; set; }
	}
}