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
	/// Default values for <see cref="HxInputTags"/>.
	/// </summary>
	public class InputTagsDefaults : IInputDefaultsWithSize
	{
		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		public int SuggestMinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		public int SuggestDelay { get; set; } = 300;

		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize InputSize { get; set; } = InputSize.Regular;

		/// <summary>
		/// Characters, when typed, divide the current input into separate tags.
		/// Default is <c>comma, semicolon and space</c>.
		/// </summary>
		public List<char> Delimiters = new() { ',', ';', ' ' };

		/// <summary>
		/// Indicates whether the add-icon (+) should be displayed.
		/// Default is <c>false</c>.
		/// </summary>
		public bool ShowAddButton { get; set; } = false;

		/// <summary>
		/// Background color of the tag (also used for the AddButton).
		/// Default is <see cref="ThemeColor.Light"/>.
		/// </summary>
		[Parameter] public ThemeColor TagBackgroundColor { get; set; } = ThemeColor.Light;

		/// <summary>
		/// Color of the tag text (also used for the AddButtonText and icons).
		/// Default is <see cref="ThemeColor.Dark"/>.
		/// </summary>
		[Parameter] public ThemeColor TagTextColor { get; set; } = ThemeColor.Dark;
	}
}
