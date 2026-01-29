using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Navigation;

[TestClass]
public class HxSidebarItem_ShouldMatch_Tests
{
	[TestMethod]
	public void HxSidebarItem_ShouldMatch_NullHref_ReturnsFalse()
	{
		// Arrange
		var testItem = new TestHxSidebarItem();

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test");

		// Assert
		Assert.IsFalse(result, "ShouldMatch should return false when _hrefAbsolute is null");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_ExactMatch_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test"
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true for exact URI match");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_CaseInsensitiveMatch_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/Test"
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true for case-insensitive match");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_TrailingSlashInHref_WithoutTrailingSlashInUri_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test/"
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true when href has trailing slash but current URI doesn't");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_PrefixMatch_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.Prefix
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test/page");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true for prefix match");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_PrefixMatch_WithSeparator_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test/",
			Match = NavLinkMatch.Prefix
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test/page");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true for prefix match with separator");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_PrefixMatch_PartialWordMatch_ReturnsFalse()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.Prefix
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/testing");

		// Assert
		Assert.IsFalse(result, "ShouldMatch should return false when prefix matches partial word");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_MatchAll_WithQueryString_IgnoresQueryString()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.All
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test?param=value");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should ignore query string for NavLinkMatch.All by default");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_MatchAll_WithFragment_IgnoresFragment()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.All
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test#section");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should ignore fragment for NavLinkMatch.All by default");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_MatchAll_WithQueryStringAndFragment_IgnoresBoth()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.All
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test?param=value#section");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should ignore both query string and fragment for NavLinkMatch.All by default");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_MatchAll_HrefWithQueryString_UriWithQueryString_IgnoresBoth()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test?param1=value1",
			Match = NavLinkMatch.All
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test?param2=value2");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should ignore query strings in both href and URI for NavLinkMatch.All");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_MatchAll_HrefWithFragment_UriWithFragment_IgnoresBoth()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test#section1",
			Match = NavLinkMatch.All
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test#section2");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should ignore fragments in both href and URI for NavLinkMatch.All");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_PrefixMatch_WithQueryString_DoesNotIgnoreQueryString()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test",
			Match = NavLinkMatch.Prefix
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test?param=value");

		// Assert
		Assert.IsTrue(result, "ShouldMatch with Prefix should still match when URI has query string");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_DifferentPath_ReturnsFalse()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/test"
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/other");

		// Assert
		Assert.IsFalse(result, "ShouldMatch should return false for different paths");
	}

	[TestMethod]
	public void HxSidebarItem_ShouldMatch_EmptyPrefix_ReturnsTrue()
	{
		// Arrange
		var testItem = new TestHxSidebarItem
		{
			HrefAbsolute = "https://example.com/",
			Match = NavLinkMatch.Prefix
		};

		// Act
		var result = testItem.TestShouldMatch("https://example.com/test");

		// Assert
		Assert.IsTrue(result, "ShouldMatch should return true for empty prefix (root match)");
	}

	// Helper class to expose protected ShouldMatch method for testing
	private class TestHxSidebarItem : HxSidebarItem
	{
		private string _testHrefAbsolute;

		public string HrefAbsolute
		{
			get => _testHrefAbsolute;
			set
			{
				_testHrefAbsolute = value;
				// Use reflection to set the private field
				var fieldInfo = typeof(HxSidebarItem).GetField("_hrefAbsolute", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				fieldInfo?.SetValue(this, value);
			}
		}

		public bool TestShouldMatch(string uriAbsolute)
		{
			return ShouldMatch(uriAbsolute);
		}
	}
}
