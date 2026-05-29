namespace Havit.Blazor.Storage.E2ETests;

[TestClass]
public static class TestAppAssemblyInitializer
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

	[AssemblyInitialize]
	public static void Initialize(TestContext testContext)
	{
		s_factory = new TestAppWebApplicationFactory();

		// Trigger host creation by creating a HttpClient
		_ = s_factory.CreateClient();
		s_baseUrl = s_factory.GetServerAddress();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		s_factory?.Dispose();
	}
}