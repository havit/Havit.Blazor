using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public class Demo : ComponentBase
	{
		[Parameter]
		public string Title { get; set; }

		[Parameter]
		public Type Type { get; set; }

		[Inject]
		public IJSRuntime JSRuntime { get; set; }

		private bool showingDemo = true;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var resourceName = Type.FullName + ".razor";
			string code;

			using (Stream stream = Type.Assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					code = reader.ReadToEnd();
				}
			}

			// no base call
			builder.AddMarkupContent(0, "<!--googleoff: index-->"); // source: https://perishablepress.com/tell-google-to-not-index-certain-parts-of-your-page/

			builder.OpenElement(100, "div");
			builder.AddAttribute(101, "class", "card card-demo my-3");
			builder.OpenElement(200, "div");
			builder.AddAttribute(201, "class", "card-header");
			builder.AddContent(202, Title);

			builder.OpenElement(300, "button");
			builder.AddAttribute(301, "type", "button");
			builder.AddAttribute(302, "class", "btn btn-info btn-sm");
			builder.AddAttribute(303, "style", "float: right");
			builder.AddAttribute(304, "onclick", EventCallback.Factory.Create(this, () => { showingDemo = !showingDemo; }));
			builder.AddContent(305, showingDemo ? "source" : "demo");
			builder.CloseElement(); // button

			builder.CloseElement(); // card-header

			builder.OpenElement(400, "div");
			builder.AddAttribute(401, "class", "card-body");

			if (showingDemo)
			{
				builder.OpenComponent(500, Type);
				builder.CloseComponent();
			}
			else
			{ 
				builder.OpenElement(600, "pre");
				builder.OpenElement(601, "code");
				builder.AddAttribute(602, "class", "language-html");
				builder.AddContent(603, code.Trim());
				builder.CloseElement();
				builder.CloseElement();
			}

			builder.CloseElement(); // card-body
			builder.CloseElement(); // card

			builder.AddMarkupContent(700, "<!--googleon: index-->"); // source: https://perishablepress.com/tell-google-to-not-index-certain-parts-of-your-page/
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			await JSRuntime.InvokeVoidAsync("highlightCode");
		}
	}
}
