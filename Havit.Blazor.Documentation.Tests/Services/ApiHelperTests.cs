using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Documentation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Documentation.Tests.Services;

[TestClass]
public class ApiHelperTests
{
	[TestMethod]
	[DataRow("AutosuggestDataProviderDelegate", true)]
	[DataRow("GridDataProviderDelegate", true)]
	[DataRow("InputTagsDataProviderDelegate", true)]
	[DataRow("CalendarDateCustomizationProviderDelegate", true)]
	[DataRow("SearchBoxDataProviderDelegate", true)]
	[DataRow("HxButton", false)]
	[DataRow("ButtonSize", false)]
	public void ApiHelper_IsDelegate_ReturnsExpected(string typeName, bool expected)
	{
		// arrange
		Type type = ApiTypeHelper.GetType(typeName);

		// act
		bool actual = ApiTypeHelper.IsDelegate(type);

		// assert
		Assert.AreEqual(expected, actual);
	}

	[TestMethod]
	public void ApiHelper_GetType_ReturnsHxButton()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxButton");

		// assert
		Assert.AreEqual(typeof(HxButton), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_ReturnsButtonSizeEnum()
	{
		// act
		Type result = ApiTypeHelper.GetType("ButtonSize");

		// assert
		Assert.AreEqual(typeof(ButtonSize), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_ReturnsGenericHxGrid()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid");

		// assert
		Assert.AreEqual(typeof(HxGrid<>), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_ReturnsDelegateFromDictionary()
	{
		// act
		Type result = ApiTypeHelper.GetType("AutosuggestDataProviderDelegate");

		// assert
		Assert.AreEqual(typeof(AutosuggestDataProviderDelegate<>), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_ReturnsDelegateFromDictionary_NonGeneric()
	{
		// act
		Type result = ApiTypeHelper.GetType("CalendarDateCustomizationProviderDelegate");

		// assert
		Assert.AreEqual(typeof(CalendarDateCustomizationProviderDelegate), result);
	}

	[TestMethod]
	[DataRow("NonExistentType123")]
	[DataRow("")]
	public void ApiHelper_GetType_ReturnsNullForUnknownTypeName(string typeName)
	{
		// act
		Type result = ApiTypeHelper.GetType(typeName);

		// assert
		Assert.IsNull(result);
	}

	[TestMethod]
	public void ApiHelper_GetType_StripsGenericBrackets()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid<TItem>");

		// assert
		Assert.AreEqual(typeof(HxGrid<>), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_StripsMultipleGenericBrackets()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxAutosuggest<TItem, TValue>");

		// assert
		Assert.AreEqual(typeof(HxAutosuggest<,>), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_PrefersGenericTypeByDefault()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid");

		// assert
		Assert.AreEqual(typeof(HxGrid<>), result);
	}

	[TestMethod]
	public void ApiHelper_GetType_PrefersNonGenericTypeWhenRequested()
	{
		// act
		Type result = ApiTypeHelper.GetType("HxGrid", preferGenericTypes: false);

		// assert
		Assert.AreEqual(typeof(HxGrid), result);
	}

	[TestMethod]
	[DataRow("HxButton", true)]
	[DataRow("HxGrid", true)]
	[DataRow("ButtonSize", true)]
	[DataRow("GridDataProviderDelegate", true)]
	[DataRow("NonExistentType123", false)]
	[DataRow("", false)]
	public void ApiHelper_IsLibraryType_ReturnsExpected(string typeName, bool expected)
	{
		// act
		bool actual = ApiTypeHelper.IsLibraryType(typeName);

		// assert
		Assert.AreEqual(expected, actual);
	}

	[TestMethod]
	public void ApiHelper_GetType_WithIncludeTypesContainingTypeName_FindsType()
	{
		// act
		Type result = ApiTypeHelper.GetType("GridSettings", includeTypesContainingTypeName: true);

		// assert
		Assert.AreEqual(typeof(GridSettings), result);
	}
}
