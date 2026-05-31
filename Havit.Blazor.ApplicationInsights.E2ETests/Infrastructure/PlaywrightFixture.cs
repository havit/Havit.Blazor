namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

[TestClass]
public static class PlaywrightFixture
{
	internal static BlazorWebApplicationFactory Factory = null!;

	[AssemblyInitialize]
	public static void AssemblyInitialize(TestContext _)
	{
		Factory = new BlazorWebApplicationFactory();
		Factory.CreateClient(); // triggers CreateHost → starts Kestrel
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup() => Factory.Dispose();
}
