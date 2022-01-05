using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxInputNumber{TValue}"/> and derived components.
	/// </summary>
	public record InputNumberSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize? InputSize { get; set; }
	}
}