using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Tests;

[TestClass]
public class SelectorHelpersTests
{
	[TestMethod]
	public void SelectorHelpers_GetText()
	{
		Assert.AreEqual("X1", SelectorHelpers.GetText<int>(i => "X" + i, 1)); // use text selector
		Assert.AreEqual("1", SelectorHelpers.GetText<int>(null, 1)); // text selector is null, use fallback (1.ToString())
		Assert.AreEqual("", SelectorHelpers.GetText<int?>(null, null)); // no text selector, no item, use empty string
		Assert.AreEqual("", SelectorHelpers.GetText<int>(i => null, 1)); // text selector is not null, do not use fallback (1.ToString())
	}

	[TestMethod]
	public void SelectorHelpers_GetValue()
	{
		Assert.IsNull(SelectorHelpers.GetValue<int?, string>(i => "X" + i, null)); // use default
		Assert.AreEqual("X1", SelectorHelpers.GetValue<int?, string>(i => "X" + i, 1)); // use value selector
		Assert.AreEqual(1, SelectorHelpers.GetValue<int?, int?>(null, 1)); // use value selector
	}
}
