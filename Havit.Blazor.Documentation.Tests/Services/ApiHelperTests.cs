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
	[DataRow("HxButton", false)]
	[DataRow("ButtonSize", false)]
	public void ApiHelper_GetType_IsDelegate(string typeName, bool expected)
	{
		// arrange
		var type = ApiTypeHelper.GetType(typeName);

		// act
		var actual = ApiTypeHelper.IsDelegate(type);

		// assert
		Assert.AreEqual(expected, actual);
	}

}
