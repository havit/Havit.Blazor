using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.Tests;

[TestClass]
public class TelemetryInitializerTests
{
	[TestMethod]
	public void TelemetryInitializer_Tags_SetToNull_KeepsTagsUsable()
	{
		// Arrange
		var initializer = new TelemetryInitializer { Tags = null };

		// Act
		initializer.CloudRoleName = "TestRole";
		initializer.ApplicationVersion = "1.2.3";

		// Assert
		Assert.AreEqual("TestRole", initializer.CloudRoleName);
		Assert.AreEqual("1.2.3", initializer.ApplicationVersion);
	}
}
