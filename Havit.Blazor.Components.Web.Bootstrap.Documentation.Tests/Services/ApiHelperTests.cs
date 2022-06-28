using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Tests.Services;

[TestClass]
public class ApiHelperTests
{
	[DataTestMethod]
	[DataRow("AutosuggestDataProviderDelegate", true)]
	[DataRow("GridDataProviderDelegate", true)]
	[DataRow("InputTagsDataProviderDelegate", true)]
	[DataRow("CalendarDateCustomizationProviderDelegate", true)]
	[DataRow("HxButton", false)]
	[DataRow("ButtonSize", false)]
	public void ApiHelper_GetType_IsDelegate(string typeName, bool expected)
	{
		// arrange
		var type = ApiHelper.GetType(typeName);

		// act
		var actual = ApiHelper.IsDelegate(type);

		// assert
		Assert.AreEqual(expected, actual);
	}

}
