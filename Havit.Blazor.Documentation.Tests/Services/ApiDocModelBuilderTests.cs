using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Tests.Services;

public class ApiDocModelBuilderTests
{
	[Fact]
	public void GenericPresetItem_DoesNotDuplicateSpecializationProperties()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(InputDateRangePredefinedRangesItem<>));
		var propertyNames = model.Properties.Select(property => property.PropertyInfo.Name).ToArray();

		Assert.Equal(3, propertyNames.Length);
		Assert.Single(propertyNames, name => name == nameof(InputDateRangePredefinedRangesItem.Label));
		Assert.Single(propertyNames, name => name == nameof(InputDateRangePredefinedRangesItem.ResourceType));
		Assert.Single(propertyNames, name => name == nameof(InputDateRangePredefinedRangesItem.DateRange));
	}

	[Fact]
	public void GenericRangeComponent_IncludesSharedDefaults()
	{
		var builder = new ApiDocModelBuilder(new DocXmlProvider());
		var model = builder.BuildModel(typeof(HxInputDateRange<>));
		var propertyNames = model.StaticProperties.Select(property => property.PropertyInfo.Name).ToArray();

		Assert.Contains(nameof(HxInputDateRange.Defaults), propertyNames);
		Assert.DoesNotContain("DateOnlyDefaults", propertyNames);
	}
}
