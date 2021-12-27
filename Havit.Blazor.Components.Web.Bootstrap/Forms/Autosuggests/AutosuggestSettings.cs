using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxAutosuggest{TItem, TValue} "/> and derived components.
	/// </summary>
	public record AutosuggestSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Icon displayed in input when no item is selected.
		/// </summary>
		public IconBase SearchIcon { get; set; }

		/// <summary>
		/// Icon displayed in input on selection clear button when item is selected.
		/// </summary>
		public IconBase ClearIcon { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting.
		/// </summary>
		public int? MinimumLength { get; set; }

		/// <summary>
		/// Debounce delay in miliseconds.
		/// </summary>
		public int? Delay { get; set; }

		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize? InputSize { get; set; }
	}
}
