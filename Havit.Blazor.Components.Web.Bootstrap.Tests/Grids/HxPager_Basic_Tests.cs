using System.Globalization;
using Havit;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

public class HxPager_Basic_Tests : BunitTestBase
{
	[Fact]
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
			var link = li.QuerySelector(".page-link");
			return link != null && int.TryParse(link.TextContent.Trim(), out _);
		});
		Assert.Equal(5, numericPageCount);
	}

	[Fact]
	public async Task HxPager_ClickPage_ShowsRequestedPage()
	{
		// Arrange
		int currentPageIndex = 0;
		var cut = RenderComponent<HxPager>(parameters => parameters
			.Add(p => p.TotalPages, 3)
			.Add(p => p.CurrentPageIndex, currentPageIndex)
			.Add(p => p.CurrentPageIndexChanged, (int newIndex) => currentPageIndex = newIndex));

		// Act: click page 2 (index 1) — find the "2" numeric page button and click it
		var page2Link = cut.FindAll("li.page-item button.page-link")
			.First(button => button.TextContent.Trim() == "2");
		await cut.InvokeAsync(() => page2Link.Click());

		// Assert: page index changed to 1 (second page, zero-based)
		Assert.Equal(1, currentPageIndex);
	}

	[Fact]
	public void HxPager_Render_DefaultIconButtons_HaveAccessibleLabels()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange & Act
			var cut = RenderComponent<HxPager>(parameters => parameters
				.Add(p => p.TotalPages, 5)
				.Add(p => p.CurrentPageIndex, 0));

			// Assert
			var labeledButtons = cut.FindAll("button.page-link[aria-label]");
			Assert.Equal(
				new[] { "First page", "Previous page", "Next page", "Last page" },
				labeledButtons.Select(button => button.GetAttribute("aria-label")).ToArray());
		}
	}

	[Fact]
	public void HxPager_Render_EllipsisButtons_HaveAccessibleLabels()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange & Act
			var cut = RenderComponent<HxPager>(parameters => parameters
				.Add(p => p.TotalPages, 25)
				.Add(p => p.CurrentPageIndex, 10));

			// Assert
			var labeledButtons = cut.FindAll("button.page-link[aria-label]")
				.Select(button => button.GetAttribute("aria-label"))
				.ToArray();

			Assert.Equivalent(
				new[] { "First page", "Previous page", "Previous pages", "Next pages", "Next page", "Last page" },
				labeledButtons);
		}
	}
}
