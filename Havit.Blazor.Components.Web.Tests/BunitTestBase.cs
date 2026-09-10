using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public abstract class BunitTestBase : BunitContext
{
	protected BunitTestBase()
	{
		Services.AddSingleton(TimeProvider.System);
		Services.AddLocalization();
		Services.AddLogging();
		Services.AddHxServices();
		Services.AddHxMessenger();
		JSInterop.Mode = JSRuntimeMode.Loose;
	}
}
