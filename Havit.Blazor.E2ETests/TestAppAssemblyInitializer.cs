namespace Havit.Blazor.E2ETests;

[TestClass]
public static class TestAppAssemblyInitializer
{
	private static TestAppWebApplicationFactory _factory;
	private static string _baseUrl;

	/// <summary>
	/// Gets the base URL of the running TestApp instance.
	/// </summary>
	public static string BaseUrl
	{
		get
		{
			if (_baseUrl == null)
			{
				throw new InvalidOperationException("TestApp has not been started yet.");
			}
			return _baseUrl;
		}
	}

	[AssemblyInitialize]
	public static void Initialize(TestContext testContext)
	{
		_factory = new TestAppWebApplicationFactory();

		// Trigger host creation by creating a HttpClient
		_ = _factory.CreateClient();
		_baseUrl = _factory.GetServerAddress();
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		_factory?.Dispose();
	}
}
