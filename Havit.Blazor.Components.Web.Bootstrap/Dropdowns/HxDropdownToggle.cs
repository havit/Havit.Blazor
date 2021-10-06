using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap Dropdown</see> toggle button which triggers the <see cref="HxDropdown"/> to open.
	/// </summary>
	public class HxDropdownToggleButton : HxButton
	{
		/// <summary>
		/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
		/// of the dropdown relative to its target.  Default is <c>(0, 2)</c>.
		/// </summary>
		[Parameter] public (int Skidding, int Distance)? DropdownOffset { get; set; }

		/// <summary>
		/// Reference element of the dropdown menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
		/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
		/// For more information refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
		/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
		/// </summary>
		[Parameter] public string DropdownReference { get; set; }

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
			AdditionalAttributes["data-bs-auto-close"] = (DropdownContainer?.AutoClose ?? DropdownAutoClose.True) switch
			{
				DropdownAutoClose.True => "true",
				DropdownAutoClose.False => "false",
				DropdownAutoClose.Inside => "inside",
				DropdownAutoClose.Outside => "outside",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {DropdownContainer.AutoClose}.")
			};

			if (this.DropdownOffset is not null)
			{
				AdditionalAttributes["data-bs-offset"] = $"{DropdownOffset.Value.Skidding},{DropdownOffset.Value.Distance}";
			}

			if (!String.IsNullOrWhiteSpace(this.DropdownReference))
			{
				AdditionalAttributes["data-bs-reference"] = this.DropdownReference;
			}
		}

		protected override string CoreCssClass =>
			CssClassHelper.Combine(
				base.CoreCssClass,
				"dropdown-toggle",
				(DropdownContainer?.Split ?? false) ? "dropdown-toggle-split" : null,
				(NavContainer is not null) ? "nav-link" : null);
	}
}
