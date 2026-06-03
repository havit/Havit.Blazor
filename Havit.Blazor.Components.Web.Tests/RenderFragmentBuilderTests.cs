using Bunit;
using Microsoft.AspNetCore.Components.Rendering;
namespace Havit.Blazor.Components.Web.Tests;

public class RenderFragmentBuilderTests
{
	[Fact]
	public void RenderFragmentBuilder_Empty_ReturnsNull()
	{
		// act
		var result = RenderFragmentBuilder.Empty();

		// assert
		Assert.Null(result);
	}

	[Fact]
	public void RenderFragmentBuilder_CreateFrom_BothNull_ReturnsNull()
	{
		// act
		var result = RenderFragmentBuilder.CreateFrom(null, null);

		// assert
		Assert.Null(result);
	}

	[Fact]
	public void RenderFragmentBuilder_CreateFrom_BothSet_RendersContentFirst()
	{
		// assert
		var ctx = new Bunit.TestContext();

		// act
		var result = ctx.Render(RenderFragmentBuilder.CreateFrom("content", (RenderTreeBuilder builder) => builder.AddContent(0, "template")));

		// assert
		result.MarkupMatches("contenttemplate");
	}

	[Fact]
	public void RenderFragmentBuilder_CreateFrom_OnlyContentSet_RendersContent()
	{
		// arrange
		var ctx = new Bunit.TestContext();

		// act
		var result = ctx.Render(RenderFragmentBuilder.CreateFrom("content", null));

		// assert
		result.MarkupMatches("content");
	}

	[Fact]
	public void RenderFragmentBuilder_CreateFrom_OnlyTemplateSet_RendersTemplate()
	{
		// arrange
		var ctx = new Bunit.TestContext();

		// act
		var result = ctx.Render(RenderFragmentBuilder.CreateFrom(null, (RenderTreeBuilder builder) => builder.AddContent(0, "template")));

		// assert
		result.MarkupMatches("template");
	}

	[Fact]
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
