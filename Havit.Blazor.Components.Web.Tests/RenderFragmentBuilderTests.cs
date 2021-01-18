using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Tests
{
	[TestClass]
	public class RenderFragmentBuilderTests
	{
		[TestMethod]
		public void RenderFragmentBuilder_Empty_ReturnsNull()
		{
			// act
			var result = RenderFragmentBuilder.Empty();

			// assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void RenderFragmentBuilder_CreateFrom_BothNull_ReturnsNull()
		{
			// act
			var result = RenderFragmentBuilder.CreateFrom(null, null);

			// assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void RenderFragmentBuilder_CreateFrom_BothSet_RendersContentFirst()
		{
			// assert
			var ctx = new Bunit.TestContext();

			// act
			var result = ctx.Render(RenderFragmentBuilder.CreateFrom("content", (RenderTreeBuilder builder) => builder.AddContent(0, "template")));

			// assert
			result.MarkupMatches("contenttemplate");
		}

		[TestMethod]
		public void RenderFragmentBuilder_CreateFrom_OnlyContentSet_RendersContent()
		{
			// arrange
			var ctx = new Bunit.TestContext();

			// act
			var result = ctx.Render(RenderFragmentBuilder.CreateFrom("content", null));

			// assert
			result.MarkupMatches("content");
		}

		[TestMethod]
		public void RenderFragmentBuilder_CreateFrom_OnlyTemplateSet_RendersTemplate()
		{
			// arrange
			var ctx = new Bunit.TestContext();

			// act
			var result = ctx.Render(RenderFragmentBuilder.CreateFrom(null, (RenderTreeBuilder builder) => builder.AddContent(0, "template")));

			// assert
			result.MarkupMatches("template");
		}

		[TestMethod]
		public void RenderFragmentBuilder_CreateFrom_EmptyStringContent_ReturnsFragmentWhichRendersStringEmpty()
		{
			// arrange
			var ctx = new Bunit.TestContext();

			// act
			var result = ctx.Render(RenderFragmentBuilder.CreateFrom(String.Empty, null));

			// assert
			result.MarkupMatches(String.Empty);
		}
	}
}
