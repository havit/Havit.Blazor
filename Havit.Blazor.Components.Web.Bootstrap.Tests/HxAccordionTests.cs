using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAccordionTests : BunitTestBase
{
	[Fact]
	public void HxAccordion_Render_HasAccordionStructure()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "item1")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header 1")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body 1")))));

		// Assert
		var accordion = cut.Find("div.accordion");
		Assert.NotNull(accordion);
		Assert.True(accordion.ClassList.Contains("hx-accordion"));
	}

	[Fact]
	public void HxAccordion_MultipleItems_RendersAllItems()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.AddChildContent(builder =>
			{
				builder.OpenComponent<HxAccordionItem>(0);
				builder.AddAttribute(1, nameof(HxAccordionItem.Id), "item1");
				builder.AddAttribute(2, nameof(HxAccordionItem.HeaderTemplate), (RenderFragment)(b => b.AddContent(0, "Header 1")));
				builder.AddAttribute(3, nameof(HxAccordionItem.BodyTemplate), (RenderFragment)(b => b.AddContent(0, "Body 1")));
				builder.CloseComponent();

				builder.OpenComponent<HxAccordionItem>(10);
				builder.AddAttribute(11, nameof(HxAccordionItem.Id), "item2");
				builder.AddAttribute(12, nameof(HxAccordionItem.HeaderTemplate), (RenderFragment)(b => b.AddContent(0, "Header 2")));
				builder.AddAttribute(13, nameof(HxAccordionItem.BodyTemplate), (RenderFragment)(b => b.AddContent(0, "Body 2")));
				builder.CloseComponent();

				builder.OpenComponent<HxAccordionItem>(20);
				builder.AddAttribute(21, nameof(HxAccordionItem.Id), "item3");
				builder.AddAttribute(22, nameof(HxAccordionItem.HeaderTemplate), (RenderFragment)(b => b.AddContent(0, "Header 3")));
				builder.AddAttribute(23, nameof(HxAccordionItem.BodyTemplate), (RenderFragment)(b => b.AddContent(0, "Body 3")));
				builder.CloseComponent();
			}));

		// Assert
		var items = cut.FindAll("div.accordion-item");
		Assert.Equal(3, items.Count());
	}

	[Fact]
	public void HxAccordionItem_Render_HasCorrectStructure()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "item1")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header Text")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body Text")))));

		// Assert
		var item = cut.Find("div.hx-accordion-item");
		Assert.NotNull(item);
		Assert.True(item.ClassList.Contains("accordion-item"));

		var header = cut.Find("h2.accordion-header");
		Assert.NotNull(header);

		var button = cut.Find("button.accordion-button");
		Assert.NotNull(button);
		Assert.Equal("button", button.GetAttribute("type"));
		Assert.Equal("collapse", button.GetAttribute("data-bs-toggle"));
		Assert.Contains("Header Text", button.TextContent);

		var body = cut.Find("div.accordion-body");
		Assert.NotNull(body);
		Assert.Contains("Body Text", body.TextContent);
	}

	[Fact]
	public void HxAccordionItem_DataBsTarget_MatchesCollapseId()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "testitem")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))));

		// Assert - button data-bs-target should match collapse id
		var button = cut.Find("button.accordion-button");
		var dataBsTarget = button.GetAttribute("data-bs-target");
		Assert.NotNull(dataBsTarget);
		Assert.StartsWith("#collapse-", dataBsTarget);

		// The aria-controls should match the collapse id (without #)
		var ariaControls = button.GetAttribute("aria-controls");
		Assert.Equal(dataBsTarget.TrimStart('#'), ariaControls);
	}

	[Fact]
	public void HxAccordion_StayOpenFalse_SetsDataBsParent()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.Add(a => a.StayOpen, false)
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "item1")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))));

		// Assert - the collapse element should have data-bs-parent pointing to accordion
		var accordionDiv = cut.Find("div.accordion");
		var accordionId = accordionDiv.Id;

		var collapseDiv = cut.Find("div.accordion-collapse");
		var dataBsParent = collapseDiv.GetAttribute("data-bs-parent");
		Assert.Equal($"#{accordionId}", dataBsParent);
	}

	[Fact]
	public void HxAccordion_StayOpenTrue_NoDataBsParent()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.Add(a => a.StayOpen, true)
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "item1")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))));

		// Assert - the collapse element should NOT have data-bs-parent
		var collapseDiv = cut.Find("div.accordion-collapse");
		var dataBsParent = collapseDiv.GetAttribute("data-bs-parent");
		Assert.True(string.IsNullOrEmpty(dataBsParent), "data-bs-parent should not be set when StayOpen is true.");
	}

	[Fact]
	public void HxAccordion_CssClass_IsApplied()
	{
		// Act
		var cut = RenderComponent<HxAccordion>(p => p
			.Add(a => a.CssClass, "custom-accordion")
			.AddChildContent<HxAccordionItem>(item => item
				.Add(i => i.Id, "item1")
				.Add(i => i.HeaderTemplate, (RenderFragment)(b => b.AddContent(0, "Header")))
				.Add(i => i.BodyTemplate, (RenderFragment)(b => b.AddContent(0, "Body")))));

		// Assert
		var accordion = cut.Find("div.accordion");
		Assert.True(accordion.ClassList.Contains("custom-accordion"));
	}
}
