using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public abstract class BunitTestBase : TestContextWrapper, IDisposable
{
	protected BunitTestBase()
	{
		TestContext = new Bunit.TestContext();
		Services.AddSingleton(TimeProvider.System);
		Services.AddLocalization();
		Services.AddLogging();
		Services.AddHxServices();
		Services.AddHxMessenger();
		Services.AddHxMessageBoxHost();
		JSInterop.Mode = JSRuntimeMode.Loose;
	}

	public void Dispose()
	{
		TestContext?.Dispose();
	}
}
