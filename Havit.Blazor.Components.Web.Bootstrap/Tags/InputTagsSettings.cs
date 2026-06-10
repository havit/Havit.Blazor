using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputTags"/> component.
/// </summary>
public record InputTagsSettings : InputSettings
{
	/// <summary>
	/// Minimum number of characters to start suggesting.
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
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

	/// <summary>
	/// Characters that, when typed, divide the current input into separate tags.
	/// </summary>
	public List<char> Delimiters { get; set; }

	/// <summary>
	/// Indicates whether the add-icon (+) should be displayed.
	/// </summary>
	public bool? ShowAddButton { get; set; }

	/// <summary>
	/// The theme color of the tag chips (applied as a Bootstrap <c>theme-*</c> class).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Settings for the <see cref="HxBadge"/> previously used to render tags.
	/// </summary>
	[Obsolete("Tags are no longer rendered as badges since Bootstrap 6 (the native Chip component is used). Use the Color property (mapped to a theme-* class) instead. Only the Color and CssClass values of these settings are honored.")]
	public BadgeSettings TagBadgeSettings { get; set; }

	/// <summary>
	/// Defines whether the input for new tag may be checked for spelling errors.
	/// </summary>
	public bool? Spellcheck { get; set; }
}
