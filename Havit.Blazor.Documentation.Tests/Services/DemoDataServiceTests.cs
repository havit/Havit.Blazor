using Havit.Blazor.Documentation.DemoData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Documentation.Tests.Services;

[TestClass]
public class DemoDataServiceTests
{
	[TestMethod]
	public async Task GetEmployeesDataFragmentAsync_WithFilter_ReturnsCorrectTotalCount()
	{
		// Arrange
		ILogger<DemoDataService> logger = new NullLogger<DemoDataService>();
		DemoDataService service = new DemoDataService(logger);

		// Create a filter that will match multiple employees (more than page size)
		// Using "a" should match many employees
		EmployeesFilterDto filter = new EmployeesFilterDto
		{
			Name = "a" // This will match employees whose names contain "a"
		};

		// Act - Get first page (5 items) of filtered results
		DataFragmentResult<EmployeeDto> firstPageResult = await service.GetEmployeesDataFragmentAsync(filter, startIndex: 0, count: 5);

		// Also get second page to verify pagination works
		DataFragmentResult<EmployeeDto> secondPageResult = await service.GetEmployeesDataFragmentAsync(filter, startIndex: 5, count: 5);

		// Also get the total count separately for comparison
		int expectedTotalCount = await service.GetEmployeesCountAsync(filter);

		// Assert
		// The TotalCount should be the same for both pages and match the filtered count
		Assert.AreEqual(expectedTotalCount, firstPageResult.TotalCount, "First page TotalCount should match filtered employee count");
		Assert.AreEqual(expectedTotalCount, secondPageResult.TotalCount, "Second page TotalCount should match filtered employee count");

		// The TotalCount should be greater than the page size (5) to verify pagination
		Assert.IsTrue(firstPageResult.TotalCount > 5, $"TotalCount should be greater than page size. TotalCount was {firstPageResult.TotalCount}");

		// The data returned should be at most the page size
		Assert.IsTrue(firstPageResult.Data.Count <= 5, "First page should have at most 5 items");
		Assert.IsTrue(secondPageResult.Data.Count <= 5, "Second page should have at most 5 items");

		// All returned items should match the filter
		Assert.IsTrue(firstPageResult.Data.All(e => e.Name.Contains("a", StringComparison.CurrentCultureIgnoreCase)),
			"All first page items should match the filter");
		Assert.IsTrue(secondPageResult.Data.All(e => e.Name.Contains("a", StringComparison.CurrentCultureIgnoreCase)),
			"All second page items should match the filter");
	}

	[TestMethod]
	public async Task GetEmployeesDataFragmentAsync_WithNoFilter_ReturnsAllEmployees()
	{
		// Arrange
		ILogger<DemoDataService> logger = new NullLogger<DemoDataService>();
		DemoDataService service = new DemoDataService(logger);

		EmployeesFilterDto emptyFilter = new EmployeesFilterDto();

		// Act
		DataFragmentResult<EmployeeDto> result = await service.GetEmployeesDataFragmentAsync(emptyFilter, startIndex: 0, count: 10);
		int totalCount = await service.GetEmployeesCountAsync();

		// Assert
		Assert.AreEqual(totalCount, result.TotalCount, "TotalCount should match total employee count when no filter is applied");
		Assert.AreEqual(10, result.Data.Count, "Should return 10 items as requested");
	}

	[TestMethod]
	public async Task GetEmployeesDataFragmentAsync_WithFilterThatMatchesNoEmployees_ReturnsZeroTotalCount()
	{
		// Arrange
		ILogger<DemoDataService> logger = new NullLogger<DemoDataService>();
		DemoDataService service = new DemoDataService(logger);

		EmployeesFilterDto filter = new EmployeesFilterDto
		{
			Name = "XyZzZzNonExistentName123" // This should match no employees
		};

		// Act
		DataFragmentResult<EmployeeDto> result = await service.GetEmployeesDataFragmentAsync(filter, startIndex: 0, count: 10);

		// Assert
		Assert.AreEqual(0, result.TotalCount, "TotalCount should be 0 when filter matches no employees");
		Assert.AreEqual(0, result.Data.Count, "Data should be empty when filter matches no employees");
	}
}
