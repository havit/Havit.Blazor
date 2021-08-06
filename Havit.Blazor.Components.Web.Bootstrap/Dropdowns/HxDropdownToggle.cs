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

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			AdditionalAttributes ??= new Dictionary<string, object>();
			AdditionalAttributes["data-bs-toggle"] = "dropdown";
			AdditionalAttributes["aria-expanded"] = "false";
		}

		protected override string CoreCssClass => CssClassHelper.Combine(base.CoreCssClass, "dropdown-toggle", DropdownContainer.Split ? "dropdown-toggle-split" : null);
	}
}
