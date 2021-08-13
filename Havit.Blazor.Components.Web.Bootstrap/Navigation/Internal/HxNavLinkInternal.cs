using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// <see cref="NavLink"/> variation which adds <see cref="OnClick"/> and related stuff.
	/// </summary>
	/// <remarks>
	/// <a href="https://github.com/dotnet/aspnetcore/issues/18460#issuecomment-577175682">https://github.com/dotnet/aspnetcore/issues/18460#issuecomment-577175682</a>.
	/// </remarks>
	public class HxNavLinkInternal : NavLink
	{
		/// <summary>
		/// Raised when the item is clicked.
		/// </summary>
		[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "a");

			builder.AddMultipleAttributes(1, AdditionalAttributes);
			builder.AddAttribute(2, "class", CssClass);

			if (this.OnClick.HasDelegate)
			{
				builder.AddAttribute(3, "onclick", this.OnClick);
				builder.AddEventPreventDefaultAttribute(4, "onclick", true);
				builder.AddEventStopPropagationAttribute(5, "onclick", true);
			}

			builder.AddContent(6, ChildContent);

			builder.CloseElement();
		}


	}
}
