using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class HxGrid_PreserveSelection_Tests : BunitTestBase
{
	[TestMethod]
	public void HxGrid_PreserveSelection_false_SelectedItem_ShouldResetWhenItemNoLongerVisible()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		object selectedItem = items[7];

		GridDataProviderDelegate<object> dataProvider = (GridDataProviderRequest<object> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10) // selectedItem visible
			.Bind(p => p.SelectedDataItem, selectedItem, newValue => selectedItem = newValue, () => selectedItem)
			.Add(p => p.PreserveSelection, false));

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.PageSize, 5)); // selectedItem no longer visible

		// Assert
		Assert.IsNull(selectedItem);
	}

	[TestMethod]
	public void HxGrid_PreserveSelection_false_SelectedItems_ShouldRemoveInvisibleItemsFromSelection()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		var selectedItems = items[3..7].ToHashSet<object>();

		GridDataProviderDelegate<object> dataProvider = (GridDataProviderRequest<object> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10) // selectedItem visible
			.Bind(p => p.SelectedDataItems, selectedItems, newValue => selectedItems = newValue, () => selectedItems)
			.Add(p => p.PreserveSelection, false));

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.PageSize, 5)); // someItems no longer visible

		// Assert
		CollectionAssert.AreEquivalent(items[3..5], selectedItems.ToList());
	}

	[TestMethod]
	public async Task HxGrid_PreserveSelection_false_SelectedItem_ShouldPreserveWhenItemVisible()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		object selectedItem = items[7];

		GridDataProviderDelegate<object> dataProvider = (GridDataProviderRequest<object> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10) // selectedItem visible
			.Bind(p => p.SelectedDataItem, selectedItem, newValue => selectedItem = newValue, () => selectedItem)
			.Add(p => p.PreserveSelection, false));

		// Act
		await cut.InvokeAsync(async () => await cut.Instance.RefreshDataAsync());

		// Assert
		Assert.AreSame(items[7], selectedItem);
	}

	[TestMethod]
	public void HxGrid_PreserveSelection_true_SelectedItem_ShouldPreserveWhenItemNoLongerVisible()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		object selectedItem = items[7];

		GridDataProviderDelegate<object> dataProvider = (GridDataProviderRequest<object> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10) // selectedItem visible
			.Bind(p => p.SelectedDataItem, selectedItem, newValue => selectedItem = newValue, () => selectedItem)
			.Add(p => p.PreserveSelection, true));

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.PageSize, 5)); // selectedItem no longer visible

		// Assert
		Assert.AreSame(items[7], selectedItem);
	}

	[TestMethod]
	public void HxGrid_PreserveSelection_true_SelectedItems_ShouldPreserveWhenItemsNoLongerVisible()
	{
		// Arrange
		var items = Enumerable.Range(1, 100).Select(i => new { Id = i, Name = $"Item {i}" }).ToList();
		var selectedItems = items[3..7].ToHashSet<object>();

		GridDataProviderDelegate<object> dataProvider = (GridDataProviderRequest<object> request) => Task.FromResult(request.ApplyTo(items));

		var cut = RenderComponent<HxGrid<object>>(parameters => parameters
			.Add(p => p.DataProvider, dataProvider)
			.Add(p => p.PageSize, 10) // selectedItem visible
			.Bind(p => p.SelectedDataItems, selectedItems, newValue => selectedItems = newValue, () => selectedItems)
			.Add(p => p.PreserveSelection, true));

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.PageSize, 5)); // someItems no longer visible

		// Assert
		CollectionAssert.AreEquivalent(items[3..7], selectedItems.ToList());
	}
}
