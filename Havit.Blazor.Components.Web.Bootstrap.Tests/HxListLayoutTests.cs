using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxListLayoutTests : BunitTestBase
{
	private class TestFilterModel
	{
		public string Name { get; set; }
	}

	[Fact]
	public void HxListLayout_Render_DisplaysGridAndFilterArea()
	{
		// Arrange & Act
		var cut = RenderComponent<HxListLayout<TestFilterModel>>(parameters => parameters
			.Add(p => p.Title, "Test Title")
			.Add(p => p.FilterModel, new TestFilterModel())
			.Add(p => p.DataTemplate, (RenderFragment)(builder =>
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "class", "test-data-area");
				builder.CloseElement();
			}))
			.Add(p => p.FilterTemplate, (RenderFragment<TestFilterModel>)(model => builder =>
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "class", "test-filter-area");
				builder.CloseElement();
			})));

		// Assert — outer container and title are rendered
		cut.Find(".hx-list-layout");
		var titleElement = cut.Find(".hx-list-layout-title");
		Assert.Contains("Test Title", titleElement.TextContent);

		// Assert — data template content is rendered
		cut.Find(".test-data-area");

		// Assert — filter button area is rendered (because FilterTemplate is set)
		cut.Find(".hx-list-layout-header-buttons");

		// Assert — filter template content is rendered
		cut.Find(".test-filter-area");
	}

	[Fact]
	public async Task HxListLayout_ApplyFilter_RefreshesGrid()
	{
		// Arrange
		var filterModel = new TestFilterModel { Name = "Initial" };
		TestFilterModel appliedFilter = null;

		var cut = RenderComponent<HxListLayout<TestFilterModel>>(parameters => parameters
			.Add(p => p.FilterModel, filterModel)
			.Add(p => p.FilterModelChanged, newFilter => appliedFilter = newFilter)
			.Add(p => p.FilterTemplate, (RenderFragment<TestFilterModel>)(model => builder => { })));

		// Act — submit the filter form (simulates the user clicking the Apply/submit button)
		await cut.InvokeAsync(() => cut.Find("form.hx-form").Submit());

		// Assert — FilterModelChanged was raised, allowing the grid to refresh
		Assert.NotNull(appliedFilter);
		Assert.Equal("Initial", appliedFilter.Name);
	}

	[Fact]
	public void HxListLayout_Chips_ReflectActiveFilters()
	{
		// Arrange
		var filterModel = new TestFilterModel { Name = "ActiveFilter" };

		var cut = RenderComponent<HxListLayout<TestFilterModel>>(parameters => parameters
			.Add(p => p.FilterModel, filterModel)
			.Add(p => p.FilterTemplate, (RenderFragment<TestFilterModel>)(model => builder =>
			{
				builder.OpenComponent<HxChipGenerator>(0);
				builder.AddAttribute(1, nameof(HxChipGenerator.ChildContent),
					(RenderFragment)(b => b.AddContent(0, model.Name)));
				builder.CloseComponent();
			})));

		// Assert — chip list is rendered and contains the active filter value
		cut.WaitForAssertion(() =>
		{
			var chipList = cut.Find(".hx-chip-list");
			Assert.Contains("ActiveFilter", chipList.TextContent);
		});
	}

	[Fact]
	public async Task HxListLayout_RemoveChip_UpdatesFilterAndGrid()
	{
		// Arrange
		var filterModel = new TestFilterModel { Name = "FilterToRemove" };
		TestFilterModel updatedFilter = null;

		var cut = RenderComponent<HxListLayout<TestFilterModel>>(parameters => parameters
			.Add(p => p.FilterModel, filterModel)
			.Add(p => p.FilterModelChanged, newFilter => updatedFilter = newFilter)
			.Add(p => p.FilterTemplate, (RenderFragment<TestFilterModel>)(model => builder =>
			{
				builder.OpenComponent<HxChipGenerator>(0);
				builder.AddAttribute(1, nameof(HxChipGenerator.ChildContent),
					(RenderFragment)(b => b.AddContent(0, model.Name)));
				builder.AddAttribute(2, nameof(HxChipGenerator.ChipRemoveAction),
					(Action<object>)(obj =>
					{
						if (obj is TestFilterModel m)
						{
							m.Name = null;
						}
					}));
				builder.CloseComponent();
			})));

		// Wait for the chip remove button to appear (chips are generated in OnAfterRenderAsync)
		cut.WaitForAssertion(() => cut.Find(".hx-chip-list-remove-btn"));

		// Act — click the chip remove button
		await cut.InvokeAsync(() => cut.Find(".hx-chip-list-remove-btn").Click());

		// Assert — FilterModelChanged was raised and the filter property was cleared
		cut.WaitForAssertion(() => Assert.NotNull(updatedFilter));
		Assert.Null(updatedFilter.Name);
	}

	[Fact]
	public void HxListLayout_FilterButton_HasAriaLabel()
	{
		// Arrange — regression for #1190: filter button must have aria-label for accessibility
		var cut = RenderComponent<HxListLayout<TestFilterModel>>(parameters => parameters
			.Add(p => p.FilterModel, new TestFilterModel())
			.Add(p => p.FilterTemplate, (RenderFragment<TestFilterModel>)(model => builder => { }))
			.Add(p => p.DataTemplate, (RenderFragment)(builder => { })));

		// Assert — filter button should have a non-empty aria-label attribute
		var filterButtons = cut.FindAll("button[aria-label]");
		Assert.NotEmpty(filterButtons);
		var ariaLabel = filterButtons[0].GetAttribute("aria-label");
		Assert.NotNull(ariaLabel);
		Assert.NotEqual(string.Empty, ariaLabel);
	}
}
