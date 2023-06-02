using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Tests;

[TestClass]
public class CssClassHelperTests
{
	[TestMethod]
	public void CssClassHelper_Combine_IgnoresNull()
	{
		// act
		var result = CssClassHelper.Combine("btn btn-primary", null, "bt-lrg");

		// assert
		Assert.AreEqual("btn btn-primary bt-lrg", result);
	}

	[TestMethod]
	public void CssClassHelper_Combine_StandaloneNull()
	{
		// act
		var result = CssClassHelper.Combine(null);

		// assert
		Assert.AreEqual(String.Empty, result);
	}

	[TestMethod]
	public void CssClassHelper_Combine_MultipleNulls()
	{
		// act
		var result = CssClassHelper.Combine(null, null);

		// assert
		Assert.AreEqual(String.Empty, result);
	}
}
