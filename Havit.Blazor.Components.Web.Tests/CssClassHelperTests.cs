namespace Havit.Blazor.Components.Web.Tests;

public class CssClassHelperTests
{
	[Fact]
	public void CssClassHelper_Combine_IgnoresNull()
	{
		// act
		var result = CssClassHelper.Combine("btn btn-primary", null, "bt-lrg");

		// assert
		Assert.Equal("btn btn-primary bt-lrg", result);
	}

	[Fact]
	public void CssClassHelper_Combine_StandaloneNull()
	{
		// act
		var result = CssClassHelper.Combine(null);

		// assert
		Assert.Equal(String.Empty, result);
	}

	[Fact]
	public void CssClassHelper_Combine_MultipleNulls()
	{
		// act
		var result = CssClassHelper.Combine(null, null);

		// assert
		Assert.Equal(String.Empty, result);
	}
}
