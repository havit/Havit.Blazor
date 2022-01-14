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

		/// <summary>
		/// Hint to browsers as to the type of virtual keyboard configuration to use when editing.
		/// </summary>
		public InputMode? InputMode { get; set; }
	}
}