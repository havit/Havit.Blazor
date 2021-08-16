using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Single tag visual. Building block for <see cref="HxInputTags"/>.
	/// </summary>
	public partial class HxTag
	{
		/// <summary>
		/// Tag background color. Default is <see cref="ThemeColor.Light"/>.
		/// </summary>
		[Parameter] public ThemeColor BackgroundColor { get; set; } = ThemeColor.Light;

		/// <summary>
		/// Tag text (value) color. Default is <see cref="ThemeColor.Dark"/>.
		/// </summary>
		[Parameter] public ThemeColor TextColor { get; set; } = ThemeColor.Dark;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Tag text (value).
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Raised when the remove button is clicked.
		/// </summary>
		[Parameter] public EventCallback OnRemove { get; set; }
	}
}
