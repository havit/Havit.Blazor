using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Tests.Services;

public class ApiDocModelBuilderTests
{
	[Fact]
	public void GenericRangeSettings_DoesNotDuplicateCompatibilitySpecializationProperties()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(InputDateRangeSettings<>));
		var propertyNames = model.Properties.Select(property => property.PropertyInfo.Name).ToArray();

		Assert.Equal(propertyNames.Length, propertyNames.Distinct().Count());
		Assert.Contains(nameof(InputDateRangeSettings.PredefinedDateRanges), propertyNames);
	}

	[Fact]
	public void GenericRangeComponent_IncludesBothDefaultSets()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(HxInputDateRange<>));
		var propertyNames = model.StaticProperties.Select(property => property.PropertyInfo.Name).ToArray();

		Assert.Contains(nameof(HxInputDateRange.Defaults), propertyNames);
		Assert.Contains(nameof(HxInputDateRange.DateOnlyDefaults), propertyNames);
	}
}
