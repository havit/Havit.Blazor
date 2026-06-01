using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.Tests;

public class TelemetryInitializerTests
{
	[Fact]
	public void TelemetryInitializer_Tags_SetToNull_KeepsTagsUsable()
	{
		// Arrange
		var initializer = new TelemetryInitializer { Tags = null };

		// Act
		initializer.CloudRoleName = "TestRole";
		initializer.ApplicationVersion = "1.2.3";

		// Assert
		Assert.Equal("TestRole", initializer.CloudRoleName);
		Assert.Equal("1.2.3", initializer.ApplicationVersion);
	}
}
