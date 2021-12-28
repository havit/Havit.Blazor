using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxInputTags"/> component.
	/// </summary>
	public record InputTagsSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Minimal number of characters to start suggesting.
		/// </summary>
		public int? SuggestMinimumLength { get; set; }

		/// <summary>
		/// Debounce delay in miliseconds.
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
		/// Background color of the tag (also used for the AddButton).
		/// </summary>
		public ThemeColor? TagBackgroundColor { get; set; }

		/// <summary>
		/// Color of the tag text (also used for the AddButtonText and icons).
		/// </summary>
		public ThemeColor? TagTextColor { get; set; }
	}
}
