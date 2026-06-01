using Xunit;

[assembly: AssemblyFixture(typeof(Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure.PlaywrightFixture))]

namespace Havit.Blazor.ApplicationInsights.E2ETests.Infrastructure;

public sealed class PlaywrightFixture : IDisposable
{
	internal static BlazorWebApplicationFactory Factory = null!;

	public PlaywrightFixture()
	{
		Factory = new BlazorWebApplicationFactory();
		Factory.CreateClient(); // triggers CreateHost → starts Kestrel
	}

	public void Dispose() => Factory.Dispose();
}
