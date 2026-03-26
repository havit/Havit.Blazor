using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxAccordionTests : BunitTestBase
{
	[TestMethod]
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
		Assert.IsNotNull(accordion);
		Assert.IsTrue(accordion.ClassList.Contains("hx-accordion"));
	}

	[TestMethod]
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
		Assert.HasCount(3, items);
	}

	[TestMethod]
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
		Assert.IsNotNull(item);
		Assert.IsTrue(item.ClassList.Contains("accordion-item"));

		var header = cut.Find("h2.accordion-header");
		Assert.IsNotNull(header);

		var button = cut.Find("button.accordion-button");
		Assert.IsNotNull(button);
		Assert.AreEqual("button", button.GetAttribute("type"));
		Assert.AreEqual("collapse", button.GetAttribute("data-bs-toggle"));
		Assert.Contains("Header Text", button.TextContent);

		var body = cut.Find("div.accordion-body");
		Assert.IsNotNull(body);
		Assert.Contains("Body Text", body.TextContent);
	}

	[TestMethod]
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
		Assert.IsNotNull(dataBsTarget, "data-bs-target should be set.");
		Assert.StartsWith("#collapse-", dataBsTarget, "data-bs-target should reference a collapse element.");

		// The aria-controls should match the collapse id (without #)
		var ariaControls = button.GetAttribute("aria-controls");
		Assert.AreEqual(dataBsTarget.TrimStart('#'), ariaControls);
	}

	[TestMethod]
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
		Assert.AreEqual($"#{accordionId}", dataBsParent, "data-bs-parent should reference the accordion element.");
	}

	[TestMethod]
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
		Assert.IsTrue(string.IsNullOrEmpty(dataBsParent), "data-bs-parent should not be set when StayOpen is true.");
	}

	[TestMethod]
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
		Assert.IsTrue(accordion.ClassList.Contains("custom-accordion"));
	}
}
