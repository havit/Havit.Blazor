using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Tests
{
	[TestClass]
	public class TextSelectorHelperTests
	{
		[TestMethod]
		public void TextSelectorHelper_GetText()
		{
			Assert.AreEqual("X1", TextSelectorHelper.GetText<int>(i => "X" + i, 1)); // use text selector
			Assert.AreEqual("1", TextSelectorHelper.GetText<int>(null, 1)); // text selector is null, use fallback (1.ToString())
			Assert.AreEqual("", TextSelectorHelper.GetText<int?>(null, null)); // no text selector, no item, use empty string
			Assert.AreEqual("", TextSelectorHelper.GetText<int>(i => null, 1)); // text selector is not null, do not use fallback (1.ToString())
		}
	}
}
