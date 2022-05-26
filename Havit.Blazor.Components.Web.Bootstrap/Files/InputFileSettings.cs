using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxInputFile"/> component.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/InputFileSettings">https://havit.blazor.eu/types/InputFileSettings</see>
	/// </summary>
	public record InputFileSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize? InputSize { get; set; }
	}
}
