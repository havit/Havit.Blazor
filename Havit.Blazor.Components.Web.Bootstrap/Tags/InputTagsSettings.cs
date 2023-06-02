using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputTags"/> component.
/// </summary>
public record InputTagsSettings : InputSettings, IInputSettingsWithSize
{
	/// <summary>
	/// Minimal number of characters to start suggesting.
	/// </summary>
	public int? SuggestMinimumLength { get; set; }

	/// <summary>
	/// Debounce delay in milliseconds.
	/// </summary>
	public int? SuggestDelay { get; set; }

	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Characters, when typed, divide the current input into separate tags.
	/// </summary>
	public List<char> Delimiters { get; set; }

	/// <summary>
	/// Indicates whether the add-icon (+) should be displayed.
	/// </summary>
	public bool? ShowAddButton { get; set; }

	/// <summary>
	/// Settings for the <see cref="HxBadge"/> used to render tags.
	/// </summary>
	public BadgeSettings TagBadgeSettings { get; set; }
}
