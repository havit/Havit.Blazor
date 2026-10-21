using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Tests.Services;

public class ApiDocModelBuilderTests
{
	[Fact]
	public void GenericRangeComponent_IncludesSharedDefaults()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(HxInputDateRange<>));
		var propertyNames = model.StaticProperties.Select(property => property.PropertyInfo.Name).ToArray();

		Assert.Contains(nameof(HxInputDateRange.Defaults), propertyNames);
		Assert.DoesNotContain("DateOnlyDefaults", propertyNames);
	}

	[Fact]
	public void GenericRangeComponent_MarkdownPreservesInheritedCommentReferences()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(HxInputDateRange<>));
		var markdown = new DocMarkdownRenderer().RenderTypeDoc(model);

		Assert.Contains("the cascading `FormState`", markdown);
		Assert.Contains("use HxFormState", markdown);
		Assert.Contains("When `true`, `HxChipGenerator` is used to generate chip item(s). The default is `true`.", markdown);
	}
}
