using Havit.Blazor.Documentation.Services;
namespace Havit.Blazor.Documentation.Tests.Services;

public class ComponentNameHelperTests
{
	private readonly IDocumentationCatalogService _catalogService = new DocumentationCatalogService();

	[Theory]
	[InlineData("PxButton", "HxButton")]
	[InlineData("BtPager", "HxPager")]
	[InlineData("MedTabPanel", "HxTabPanel")]
	[InlineData("AppGrid", "HxGrid")]
	[InlineData("MyAutosuggest", "HxAutosuggest")]
	public void TryResolveHxComponentName_WhenDerivedComponentName_ReturnsMatchingHxComponent(string componentName, string expectedHxName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.Equal(expectedHxName, result);
	}

	[Theory]
	[InlineData("HxButton")]
	[InlineData("HxGrid")]
	[InlineData("hxButton")]
	public void TryResolveHxComponentName_WhenAlreadyHxComponent_ReturnsNull(string componentName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.Null(result);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("   ")]
	public void TryResolveHxComponentName_WhenNullOrWhitespace_ReturnsNull(string componentName)
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName(componentName, _catalogService);

		// assert
		Assert.Null(result);
	}

	[Fact]
	public void TryResolveHxComponentName_WhenNoMatchingSuffix_ReturnsNull()
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("CompletelyUnknownComponent", _catalogService);

		// assert
		Assert.Null(result);
	}

	[Fact]
	public void TryResolveHxComponentName_WhenGenericDerivedComponent_StripsTypeArgsAndResolves()
	{
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("BtGrid<TItem>", _catalogService);

		// assert
		Assert.Equal("HxGrid", result);
	}

	[Fact]
	public void TryResolveHxComponentName_PrefersLongestSuffixMatch()
	{
		// "MyInputDateRange" should match "HxInputDateRange" (suffix "InputDateRange")
		// rather than "HxInputDate" (suffix "InputDate") because it's a longer match
		// act
		string result = ComponentNameHelper.TryResolveHxComponentName("MyInputDateRange", _catalogService);

		// assert
		Assert.Equal("HxInputDateRange", result);
	}
}
