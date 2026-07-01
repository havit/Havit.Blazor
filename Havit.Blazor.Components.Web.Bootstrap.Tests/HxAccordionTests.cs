using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAccordionTests : BunitTestBase
{
	private static void AddItem(Bunit.ComponentParameterCollectionBuilder<HxAccordion> p, string id, string header, string body)
	{
		p.AddChildContent<HxAccordionItem>(item => item
			.Add(i => i.Id, id)
			.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, header)))
			.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, body))));
	}

	[Fact]
	public void HxAccordion_Render_HasAccordionStructure()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => AddItem(p, "item1", "Header 1", "Body 1"));

		// Assert
		var accordion = cut.Find("div.accordion");
		Assert.True(accordion.ClassList.Contains("hx-accordion"));
	}

	[Fact]
	public void HxAccordion_MultipleItems_RendersAllItems()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			AddItem(p, "item1", "Header 1", "Body 1");
			AddItem(p, "item2", "Header 2", "Body 2");
			AddItem(p, "item3", "Header 3", "Body 3");
		});

		// Assert
		Assert.Equal(3, cut.FindAll("details.accordion-item").Count);
	}

	[Fact]
	public void HxAccordionItem_Render_HasNativeDetailsStructure()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => AddItem(p, "item1", "Header Text", "Body Text"));

		// Assert — Bootstrap 6 accordion is built on native <details>/<summary>
		var item = cut.Find("details.hx-accordion-item");
		Assert.True(item.ClassList.Contains("accordion-item"));
		Assert.False(item.HasAttribute("open"));

		var summary = cut.Find("summary.accordion-header");
		Assert.Contains("Header Text", summary.TextContent);

		var icon = summary.QuerySelector("svg.accordion-icon");
		Assert.NotNull(icon);

		var body = cut.Find("div.accordion-body");
		Assert.Contains("Body Text", body.TextContent);
	}

	[Fact]
	public void HxAccordion_StayOpenFalse_RendersSharedNameAttribute()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.StayOpen, false);
			AddItem(p, "item1", "Header 1", "Body 1");
			AddItem(p, "item2", "Header 2", "Body 2");
		});

		// Assert — exclusive accordions share the name attribute (replaces v5 data-bs-parent)
		var accordionId = cut.Find("div.accordion").Id;
		var items = cut.FindAll("details.accordion-item");
		Assert.All(items, item => Assert.Equal(accordionId, item.GetAttribute("name")));
	}

	[Fact]
	public void HxAccordion_StayOpenTrue_NoNameAttribute()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.StayOpen, true);
			AddItem(p, "item1", "Header 1", "Body 1");
		});

		// Assert
		var item = cut.Find("details.accordion-item");
		Assert.False(item.HasAttribute("name"));
	}

	[Fact]
	public void HxAccordion_InitialExpandedItemId_RendersOpenAttribute()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.InitialExpandedItemId, "item2");
			AddItem(p, "item1", "Header 1", "Body 1");
			AddItem(p, "item2", "Header 2", "Body 2");
		});

		// Assert
		var items = cut.FindAll("details.accordion-item");
		Assert.False(items[0].HasAttribute("open"));
		Assert.True(items[1].HasAttribute("open"));
	}

	[Fact]
	public void HxAccordion_HeaderClick_ExpandsItemAndRaisesExpandedItemIdChanged()
	{
		string expandedItemId = null;

		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.ExpandedItemIdChanged, (string id) => expandedItemId = id);
			AddItem(p, "item1", "Header 1", "Body 1");
		});
		cut.Find("summary.accordion-header").Click();

		// Assert
		Assert.Equal("item1", expandedItemId);
		Assert.True(cut.Find("details.accordion-item").HasAttribute("open"));
	}

	[Fact]
	public void HxAccordion_ExclusiveMode_ExpandingOneItemCollapsesTheOther()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.InitialExpandedItemId, "item1");
			AddItem(p, "item1", "Header 1", "Body 1");
			AddItem(p, "item2", "Header 2", "Body 2");
		});
		cut.FindAll("summary.accordion-header")[1].Click();

		// Assert — exclusivity is replicated Blazor-side (state is Blazor-managed)
		var items = cut.FindAll("details.accordion-item");
		Assert.False(items[0].HasAttribute("open"));
		Assert.True(items[1].HasAttribute("open"));
	}

	[Fact]
	public void HxAccordion_StayOpen_ExpandingOneItemKeepsTheOtherOpen()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.StayOpen, true);
			p.Add(a => a.InitialExpandedItemIds, new List<string> { "item1" });
			AddItem(p, "item1", "Header 1", "Body 1");
			AddItem(p, "item2", "Header 2", "Body 2");
		});
		cut.FindAll("summary.accordion-header")[1].Click();

		// Assert
		var items = cut.FindAll("details.accordion-item");
		Assert.True(items[0].HasAttribute("open"));
		Assert.True(items[1].HasAttribute("open"));
	}

	[Fact]
	public void HxAccordion_Color_RendersThemeCssClass()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p =>
		{
			p.Add(a => a.Color, ThemeColor.Primary);
			AddItem(p, "item1", "Header 1", "Body 1");
		});

		// Assert
		Assert.True(cut.Find("div.accordion").ClassList.Contains("theme-primary"));
	}
}
