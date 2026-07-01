using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

public static class ButtonSizeExtensions
{
	public static string ToButtonSizeCssClass(this ButtonSize size)
	{
		return size switch
		{
			ButtonSize.Regular => null,
			ButtonSize.Small => "btn-sm",
			ButtonSize.Large => "btn-lg",
			_ => throw new InvalidOperationException($"Unknown button size: {size:g}.")
		};
	}
}
