using Xunit;

[assembly: AssemblyFixture(typeof(Havit.Blazor.Storage.E2ETests.TestAppAssemblyInitializer))]

namespace Havit.Blazor.Storage.E2ETests;

public sealed class TestAppAssemblyInitializer : IDisposable
{
	private static TestAppWebApplicationFactory s_factory;
	private static string s_baseUrl;

	/// <summary>
	/// Gets the base URL of the running TestApp instance.
	/// </summary>
	public static string BaseUrl
	{
		get
		{
			if (s_baseUrl is null)
			{
				throw new InvalidOperationException("TestApp has not been started yet.");
			}

			return s_baseUrl;
		}
	}

	public TestAppAssemblyInitializer()
	{
		s_factory = new TestAppWebApplicationFactory();

		// Trigger host creation by creating a HttpClient
		_ = s_factory.CreateClient();
		s_baseUrl = s_factory.GetServerAddress();
	}

	public void Dispose()
	{
		s_factory?.Dispose();
	}
}
