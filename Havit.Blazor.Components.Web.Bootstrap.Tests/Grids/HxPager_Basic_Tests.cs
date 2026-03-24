namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxPager_Basic_Tests : BunitTestBase
{
	[TestMethod]
	public void HxPager_Render_ShowsCorrectPageCount()
	{
		// Arrange & Act
		var cut = RenderComponent<HxPager>(parameters => parameters
			.Add(p => p.TotalPages, 5)
			.Add(p => p.CurrentPageIndex, 0));

		// Assert: 5 numeric page buttons are rendered (page-item with page-link containing a number)
		var pageItems = cut.FindAll("li.page-item");
		// 5 numeric pages + First + Previous + Next + Last = 9 items
		// (all pages fit within NumericButtonsCount default of 10)
		int numericPageCount = pageItems.Count(li =>
		{
			var link = li.QuerySelector("a.page-link");
			return link != null && int.TryParse(link.TextContent.Trim(), out _);
		});
		Assert.AreEqual(5, numericPageCount, "Pager should show 5 numeric page buttons for TotalPages=5.");
	}

	[TestMethod]
	public async Task HxPager_ClickPage_ShowsRequestedPage()
	{
		// Arrange
		int currentPageIndex = 0;
		var cut = RenderComponent<HxPager>(parameters => parameters
			.Add(p => p.TotalPages, 3)
			.Add(p => p.CurrentPageIndex, currentPageIndex)
			.Add(p => p.CurrentPageIndexChanged, (int newIndex) => currentPageIndex = newIndex));

		// Act: click page 2 (index 1) — find the "2" numeric page button and click it
		var page2Link = cut.FindAll("li.page-item a.page-link")
			.First(a => a.TextContent.Trim() == "2");
		await cut.InvokeAsync(() => page2Link.Click());

		// Assert: page index changed to 1 (second page, zero-based)
		Assert.AreEqual(1, currentPageIndex, "Clicking page 2 should set page index to 1.");
	}
}
