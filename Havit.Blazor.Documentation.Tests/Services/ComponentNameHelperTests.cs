using Havit.Blazor.Documentation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Documentation.Tests.Services;

[TestClass]
public class ComponentNameHelperTests
{
	private readonly IDocumentationCatalogService _catalogService = new DocumentationCatalogService();

	[TestMethod]
	[DataRow("PxButton", "HxButton")]
	[DataRow("BtPager", "HxPager")]
	[DataRow("MedTabPanel", "HxTabPanel")]
	[DataRow("AppGrid", "HxGrid")]
	[DataRow("MyAutosuggest", "HxAutosuggest")]
	public void TryResolveHxComponentName_WhenDerivedComponentName_ReturnsMatchingHxComponent(string componentName, string expectedHxName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.AreEqual(expectedHxName, result);
	}

	[TestMethod]
	[DataRow("HxButton")]
	[DataRow("HxGrid")]
	[DataRow("hxButton")]
	public void TryResolveHxComponentName_WhenAlreadyHxComponent_ReturnsNull(string componentName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.IsNull(result);
	}

	[TestMethod]
	[DataRow(null)]
	[DataRow("")]
	[DataRow("   ")]
	public void TryResolveHxComponentName_WhenNullOrWhitespace_ReturnsNull(string componentName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public void TryResolveHxComponentName_WhenNoMatchingSuffix_ReturnsNull()
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("CompletelyUnknownComponent", _catalogService);

		// assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public void TryResolveHxComponentName_WhenGenericDerivedComponent_StripsTypeArgsAndResolves()
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("BtGrid<TItem>", _catalogService);

		// assert
		Assert.AreEqual("HxGrid", result);
	}

	[TestMethod]
	public void TryResolveHxComponentName_PrefersLongestSuffixMatch()
	{
		// "MyInputDateRange" should match "HxInputDateRange" (suffix "InputDateRange")
		// rather than "HxInputDate" (suffix "InputDate") because it's a longer match
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("MyInputDateRange", _catalogService);

		// assert
		Assert.AreEqual("HxInputDateRange", result);
	}
}
