using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap Dropdown toggle (the button) which triggers the <see cref="HxDropdown"/> to open.
	/// </summary>
	public class HxDropdownToggle : HxButton
	{
		[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }
		[CascadingParameter] protected HxNav NavContainer { get; set; }

		protected override void OnParametersSet()
		{
			if ((Color is null) && (NavContainer is not null))
			{
				Color = ThemeColor.Link;
			}

			base.OnParametersSet();

			AdditionalAttributes ??= new Dictionary<string, object>();
			AdditionalAttributes["data-bs-toggle"] = "dropdown";
			AdditionalAttributes["aria-expanded"] = "false";
			AdditionalAttributes["data-bs-auto-close"] = DropdownContainer.AutoClose switch
			{
				DropdownAutoClose.True => "true",
				DropdownAutoClose.False => "false",
				DropdownAutoClose.Inside => "inside",
				DropdownAutoClose.Outside => "outside",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {DropdownContainer.AutoClose}.")
			};
		}

		protected override string CoreCssClass => CssClassHelper.Combine(base.CoreCssClass, "dropdown-toggle", DropdownContainer.Split ? "dropdown-toggle-split" : null, (NavContainer is not null) ? "nav-link" : null);
	}
}
