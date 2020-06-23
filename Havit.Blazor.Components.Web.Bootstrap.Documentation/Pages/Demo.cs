using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages
{
	public class Demo : ComponentBase
	{
		[Parameter]
		public string Title { get; set; }

		[Parameter]
		public Type Type { get; set; }

		private string GetLinkUrl()
		{
			string shortTypeName = Type.FullName.Replace("Havit.Blazor.Components.Web.Bootstrap.Documentation.", "");
			string urlSegment = shortTypeName.Replace(".", "/");
			return "https://dev.azure.com/havit/DEV/_git/002.HFW-HavitBlazor?path=" + System.Net.WebUtility.UrlEncode("/Havit.Blazor.Components.Web.Bootstrap.Documentation/" + urlSegment + ".razor");
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// no base call
			builder.OpenElement(100, "div");
			builder.AddAttribute(101, "class", "card");
			builder.OpenElement(200, "div");
			builder.AddAttribute(201, "class", "card-header");
			builder.AddContent(202, Title);

			builder.OpenElement(300, "a");
			builder.AddAttribute(301, "href", GetLinkUrl());
			builder.AddAttribute(302, "target", "_blank");
			builder.AddAttribute(303, "style", "float: right");
			builder.AddContent(304, "source");
			builder.CloseElement(); // a
			builder.CloseElement(); // card-header

			builder.OpenElement(400, "div");
			builder.AddAttribute(401, "class", "card-body");

			builder.OpenComponent(500, Type);
			builder.CloseComponent();

			builder.CloseElement(); // card-body
			builder.CloseElement(); // card
		}
	}
}
