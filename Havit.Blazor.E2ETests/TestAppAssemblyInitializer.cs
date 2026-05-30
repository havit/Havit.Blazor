using System.Text;

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

		lock (TestAppTestBase.Lock)
		{
			var filePath = Path.Combine(
				Path.GetTempPath(),
				$"axe-results-{DateTime.UtcNow:yyyyMMdd-HHmmss}.md"
			);

			if (TestAppTestBase.AxeReport.Length == 0)
			{
				File.WriteAllText(filePath, "No Errors Found");
				return;
			}

			var sb = new StringBuilder();
			sb.AppendLine("| Testname | Rule | Description | Impact | Target |");
			sb.AppendLine("|----------|------|-------------|--------|--------|");
			sb.Append(TestAppTestBase.AxeReport);


			File.WriteAllText(filePath, sb.ToString());

			Console.WriteLine($"Axe report written to: {filePath}");
		}
	}
}
