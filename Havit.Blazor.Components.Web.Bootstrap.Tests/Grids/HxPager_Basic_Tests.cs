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

			// Assert - numeric page buttons carry their own "Go to page N" label, so they are excluded here
			var labeledButtons = cut.FindAll("button.page-link[aria-label]")
				.Where(button => !int.TryParse(button.TextContent.Trim(), out _));
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

			// Assert - numeric page buttons carry their own "Go to page N" label, so they are excluded here
			var labeledButtons = cut.FindAll("button.page-link[aria-label]")
				.Where(button => !int.TryParse(button.TextContent.Trim(), out _))
				.Select(button => button.GetAttribute("aria-label"))
				.ToArray();

			Assert.Equivalent(
				new[] { "First page", "Previous page", "Previous pages", "Next pages", "Next page", "Last page" },
				labeledButtons);
		}
	}

	[Fact]
	public void HxPager_Render_NumericButtons_HaveGoToPageLabels()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange & Act
			var cut = RenderComponent<HxPager>(parameters => parameters
				.Add(p => p.TotalPages, 5)
				.Add(p => p.CurrentPageIndex, 0));

			// Assert - the current page (1) renders as a non-interactive <span>, so labels start at page 2
			var numericLabels = cut.FindAll("li.page-item button.page-link")
				.Where(button => int.TryParse(button.TextContent.Trim(), out _))
				.Select(button => button.GetAttribute("aria-label"))
				.ToArray();

			Assert.Equal(
				new[] { "Go to page 2", "Go to page 3", "Go to page 4", "Go to page 5" },
				numericLabels);
		}
	}

	[Theory]
	[InlineData(PagerSize.Regular, null)]
	[InlineData(PagerSize.Small, "pagination-sm")]
	[InlineData(PagerSize.Large, "pagination-lg")]
	public void HxPager_Render_Size_AppliesPaginationSizeCssClass(PagerSize size, string expectedCssClass)
	{
		// Arrange & Act
		var cut = RenderComponent<HxPager>(parameters => parameters
			.Add(p => p.TotalPages, 5)
			.Add(p => p.CurrentPageIndex, 0)
			.Add(p => p.Size, size));

		// Assert
		var pagination = cut.Find("ul.pagination");
		if (expectedCssClass is null)
		{
			Assert.DoesNotContain("pagination-sm", pagination.ClassList);
			Assert.DoesNotContain("pagination-lg", pagination.ClassList);
		}
		else
		{
			Assert.Contains(expectedCssClass, pagination.ClassList);
		}
	}

	[Fact]
	public void HxPager_Render_DefaultSize_DoesNotApplyPaginationSizeCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxPager>(parameters => parameters
			.Add(p => p.TotalPages, 5)
			.Add(p => p.CurrentPageIndex, 0));

		// Assert - the default size (Regular) renders no pagination-sm/pagination-lg modifier
		var pagination = cut.Find("ul.pagination");
		Assert.DoesNotContain("pagination-sm", pagination.ClassList);
		Assert.DoesNotContain("pagination-lg", pagination.ClassList);
	}

	[Fact]
	public void HxPager_Render_WrappedInLabeledNavLandmark()
	{
		using (CultureInfoExt.EnterScope(CultureInfo.GetCultureInfo("en-US")))
		{
			// Arrange & Act
			var cut = RenderComponent<HxPager>(parameters => parameters
				.Add(p => p.TotalPages, 5)
				.Add(p => p.CurrentPageIndex, 0));

			// Assert - the pagination list is exposed as a labeled navigation landmark
			var nav = cut.Find("nav");
			Assert.Equal("Pagination", nav.GetAttribute("aria-label"));
			Assert.NotNull(nav.QuerySelector("ul.pagination"));
		}
	}
}
