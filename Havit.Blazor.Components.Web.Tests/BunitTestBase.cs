using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public abstract class BunitTestBase : TestContextWrapper
{
	[TestInitialize]
	public void Setup()
	{
		TestContext = new Bunit.TestContext();
		Services.AddSingleton(TimeProvider.System);
		Services.AddLocalization();
		Services.AddLogging();
		Services.AddHxServices();
		Services.AddHxMessenger();
		JSInterop.Mode = JSRuntimeMode.Loose;
	}

	[TestCleanup]
	public void TearDown()
	{
		TestContext?.Dispose();
	}
}
