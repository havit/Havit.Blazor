namespace Havit.Blazor.E2ETests;

/// <summary>
/// Assembly-level initialization and cleanup for E2E tests using MSTest Platform.
/// </summary>
[TestClass]
public static class TestAppAssemblyInitializer
{
	private static TestAppWebApplicationFactory _factory;
	private static string _baseUrl;
	private static bool _isInitialized;
	private static readonly object _lock = new object();

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
		lock (_lock)
		{
			if (_isInitialized)
			{
				return;
			}

			Console.WriteLine("===== TestApp Initialization START =====");

			_factory = new TestAppWebApplicationFactory();

			// Trigger host creation by creating a client
			var client = _factory.CreateClient();

			// Get the actual server address from the factory
			_baseUrl = _factory.GetServerAddress();

			Console.WriteLine($"TestApp started at: {_baseUrl}");
			Console.WriteLine("===== TestApp Initialization END =====");

			_isInitialized = true;
		}
	}

	[AssemblyCleanup]
	public static void AssemblyCleanup()
	{
		_factory?.Dispose();
		_factory = null;
		_baseUrl = null;
		_isInitialized = false;
	}
}
