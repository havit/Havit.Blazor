using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap Dropdown</a> component.
	/// </summary>
	/// <remarks>
	/// For now HxDropdown does not implement Show/Hide methods nor OnXy events. If we need them, we will switch the implementation to JS-interop.
	/// </remarks>
	public partial class HxDropdown
	{
		[Parameter] public DropdownDirection Direction { get; set; }

		/// <summary>
		/// Set <c>true</c> to create a <a href="https://getbootstrap.com/docs/5.1/components/dropdowns/#split-button">split dropdown</a>
		/// (using a <c>btn-group</c>).
		/// </summary>
		[Parameter] public bool Split { get; set; }

		/// <summary>
		/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
		/// You can use the AutoClose parameter to change this behavior of the dropdown.
		/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/#auto-close-behavior"/>.
		/// </summary>
		[Parameter] public DropdownAutoClose AutoClose { get; set; } = DropdownAutoClose.True;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		private string GetDropdownDirectionCssClass()
		{
			return this.Direction switch
			{
				DropdownDirection.Down => "dropdown",
				DropdownDirection.Up => "dropup",
				DropdownDirection.Start => "dropstart",
				DropdownDirection.End => "dropend",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownDirection)} value {Direction}.")
			};
		}
	}
}
