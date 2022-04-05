using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxMultiSelect{TValue, TItem}"/> component.
	/// </summary>
	public record MultiSelectSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize? InputSize { get; set; }
	}
}