using Havit.Blazor.Documentation.Services;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Documentation.Tests.Services;

public class ApiRendererTests
{
	[Theory]
	[InlineData(typeof(string[]), "string[]")]
	[InlineData(typeof(FieldIdentifier[]), "FieldIdentifier[]")]
	[InlineData(typeof(List<string>), "List&lt;string&gt;")]
	[InlineData(typeof(List<string[]>), "List&lt;string[]&gt;")]
	[InlineData(typeof(Dictionary<string, int[]>), "Dictionary&lt;string, int[]&gt;")]
	public void FormatType_RendersArrayAndGenericTypes(Type type, string expected)
	{
		// Act
		var result = ApiRenderer.FormatType(type, asLink: false);

		// Assert
		Assert.Equal(expected, result);
	}
}
