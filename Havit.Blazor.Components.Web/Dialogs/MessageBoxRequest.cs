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
		/// Header text.
		/// </summary>
		[Parameter] public string HeaderText { get; set; }

		/// <summary>
		/// Header template.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

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
		/// Buttons to show. Default is <see cref="MessageBoxButtons.Ok"/>.
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
