using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web.Bootstrap.Smart.Tests;

public abstract class BunitTestBase : BunitContext
{
	protected BunitTestBase()
	{
		Services.AddSingleton(TimeProvider.System);
		Services.AddLocalization();
		Services.AddLogging();
		Services.AddHxServices();
		Services.AddHxMessenger();
		Services.AddHxMessageBoxHost();
		JSInterop.Mode = JSRuntimeMode.Loose;
	}
}
