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

		/// <summary>
		/// If true, the first suggestion is highlighted until another is chosen by the user.
		/// </summary>
		public bool? HighlightFirstSuggestion { get; set; } = true;
	}
}
