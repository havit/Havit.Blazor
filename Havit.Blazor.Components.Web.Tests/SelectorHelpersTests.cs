namespace Havit.Blazor.Components.Web.Tests;

public class SelectorHelpersTests
{
	[Fact]
	public void SelectorHelpers_GetText()
	{
		Assert.Equal("X1", SelectorHelpers.GetText<int>(i => "X" + i, 1)); // use text selector
		Assert.Equal("1", SelectorHelpers.GetText<int>(null, 1)); // text selector is null, use fallback (1.ToString())
		Assert.Equal("", SelectorHelpers.GetText<int?>(null, null)); // no text selector, no item, use empty string
		Assert.Equal("", SelectorHelpers.GetText<int>(i => null, 1)); // text selector is not null, do not use fallback (1.ToString())
	}

	[Fact]
	public void SelectorHelpers_GetValue()
	{
		Assert.Null(SelectorHelpers.GetValue<int?, string>(i => "X" + i, null)); // use default
		Assert.Equal("X1", SelectorHelpers.GetValue<int?, string>(i => "X" + i, 1)); // use value selector
		Assert.Equal(1, SelectorHelpers.GetValue<int?, int?>(null, 1)); // use value selector
	}
}
