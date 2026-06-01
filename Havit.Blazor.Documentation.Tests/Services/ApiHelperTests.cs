using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Documentation.Services;
namespace Havit.Blazor.Documentation.Tests.Services;

public class ApiHelperTests
{
	[Theory]
	[InlineData("AutosuggestDataProviderDelegate", true)]
	[InlineData("GridDataProviderDelegate", true)]
	[InlineData("InputTagsDataProviderDelegate", true)]
	[InlineData("CalendarDateCustomizationProviderDelegate", true)]
	[InlineData("SearchBoxDataProviderDelegate", true)]
	[InlineData("HxButton", false)]
	[InlineData("ButtonSize", false)]
	public void ApiHelper_IsDelegate_ReturnsExpected(string typeName, bool expected)
	{
		// arrange
		Type type = ApiTypeHelper.GetType(typeName);

		// act
		bool actual = ApiTypeHelper.IsDelegate(type);

		// assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ApiHelper_GetType_ReturnsHxButton()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxButton");

		// assert
		Assert.Equal(typeof(HxButton), result);
	}

	[Fact]
	public void ApiHelper_GetType_ReturnsButtonSizeEnum()
	{
		// act
		Type result = ApiTypeHelper.GetType("ButtonSize");

		// assert
		Assert.Equal(typeof(ButtonSize), result);
	}

	[Fact]
	public void ApiHelper_GetType_ReturnsGenericHxGrid()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid");

		// assert
		Assert.Equal(typeof(HxGrid<>), result);
	}

	[Fact]
	public void ApiHelper_GetType_ReturnsDelegateFromDictionary()
	{
		// act
		Type result = ApiTypeHelper.GetType("AutosuggestDataProviderDelegate");

		// assert
		Assert.Equal(typeof(AutosuggestDataProviderDelegate<>), result);
	}

	[Fact]
	public void ApiHelper_GetType_ReturnsDelegateFromDictionary_NonGeneric()
	{
		// act
		Type result = ApiTypeHelper.GetType("CalendarDateCustomizationProviderDelegate");

		// assert
		Assert.Equal(typeof(CalendarDateCustomizationProviderDelegate), result);
	}

	[Theory]
	[InlineData("NonExistentType123")]
	[InlineData("")]
	public void ApiHelper_GetType_ReturnsNullForUnknownTypeName(string typeName)
	{
		// act
		Type result = ApiTypeHelper.GetType(typeName);

		// assert
		Assert.Null(result);
	}

	[Fact]
	public void ApiHelper_GetType_StripsGenericBrackets()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid<TItem>");

		// assert
		Assert.Equal(typeof(HxGrid<>), result);
	}

	[Fact]
	public void ApiHelper_GetType_StripsMultipleGenericBrackets()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxAutosuggest<TItem, TValue>");

		// assert
		Assert.Equal(typeof(HxAutosuggest<,>), result);
	}

	[Fact]
	public void ApiHelper_GetType_PrefersGenericTypeByDefault()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid");

		// assert
		Assert.Equal(typeof(HxGrid<>), result);
	}

	[Fact]
	public void ApiHelper_GetType_PrefersNonGenericTypeWhenRequested()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid", preferGenericTypes: false);

		// assert
		Assert.Equal(typeof(HxGrid), result);
	}

	[Theory]
	[InlineData("HxButton", true)]
	[InlineData("HxGrid", true)]
	[InlineData("ButtonSize", true)]
	[InlineData("GridDataProviderDelegate", true)]
	[InlineData("NonExistentType123", false)]
	[InlineData("", false)]
	public void ApiHelper_IsLibraryType_ReturnsExpected(string typeName, bool expected)
	{
		// act
		bool actual = ApiTypeHelper.IsLibraryType(typeName);

		// assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void ApiHelper_GetType_WithIncludeTypesContainingTypeName_FindsType()
	{
		// act
		Type result = ApiTypeHelper.GetType("GridSettings", includeTypesContainingTypeName: true);

		// assert
		Assert.Equal(typeof(GridSettings), result);
	}
}
