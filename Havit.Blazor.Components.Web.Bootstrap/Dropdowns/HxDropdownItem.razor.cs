using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item of the <see cref="HxDropdownMenu"/>.
	/// </summary>
	public partial class HxDropdownItem : ICascadeEnabledComponent
	{
		[CascadingParameter] protected FormState FormState { get; set; }
		FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// The navigation target.
		/// </summary>
		[Parameter] public string Href { get; set; }

		/// <summary>
		/// Raised when the item is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		/// <inheritdoc />
		[Parameter] public bool? Enabled { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
