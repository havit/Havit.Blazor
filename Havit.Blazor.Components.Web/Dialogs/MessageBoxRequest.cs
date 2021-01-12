using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web
{
	public struct MessageBoxRequest
	{
		/// <summary>
		/// Title text (Header).
		/// </summary>
		[Parameter] public string Title { get; set; }

		/// <summary>
		/// Title template (Header).
		/// </summary>
		[Parameter] public RenderFragment TitleTemplate { get; set; }

		/// <summary>
		/// Content (body) text.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Content (body) template.
		/// </summary>
		[Parameter] public RenderFragment ContentTemplate { get; set; }

		/// <summary>
		/// Indicates whether to show close button.
		/// </summary>
		[Parameter] public bool ShowCloseButton { get; set; }

		/// <summary>
		/// Buttons to show.
		/// </summary>
		[Parameter] public MessageBoxButtons Buttons { get; set; }

		/// <summary>
		/// Primary button (if you want to override the default).
		/// </summary>
		[Parameter] public MessageBoxButtons? PrimaryButton { get; set; }

		/// <summary>
		/// Text for <see cref="MessageBoxButtons.Custom"/>.
		/// </summary>
		[Parameter] public string CustomButtonText { get; set; }

		[Parameter] public string CssClass { get; set; }
	}
}
