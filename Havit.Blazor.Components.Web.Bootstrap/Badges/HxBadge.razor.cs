using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap Badge component. https://getbootstrap.com/docs/5.0/components/badge/
	/// </summary>
	public partial class HxBadge
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Badge color (background). Required.
		/// </summary>
		[Parameter] public ThemeColor Color { get; set; }

		[Parameter] public ThemeColor TextColor { get; set; } = ThemeColor.None;

		/// <summary>
		/// Badge type - Regular or rounded-pills.
		/// </summary>
		[Parameter] public BadgeType Type { get; set; } = BadgeType.Regular;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxBadge)} is required.");
		}

		protected string GetTypeCss()
		{
			return Type switch
			{
				BadgeType.Regular => null,
				BadgeType.RoundedPill => "rounded-pill",
				_ => throw new InvalidOperationException($"Unknown {nameof(BadgeType)} value: {Type}.")
			};
		}
	}
}
