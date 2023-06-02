using Bunit;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.DemoData;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Tests;

[TestClass]
public class DemosSmokeTests
{
	[DataTestMethod]
	[DynamicData(nameof(GetDemos), DynamicDataSourceType.Method)]
	public void DocumentationDemo_SmokeTest(Type demoComponent)
	{
		// Arrange
		var ctx = new Bunit.TestContext();
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		ctx.Services.AddLogging();
		ctx.Services.AddHxServices();
		ctx.Services.AddHxMessenger();
		ctx.Services.AddHxMessageBoxHost();

		ctx.Services.AddTransient<IDemoDataService, DemoDataService>();

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent(1, demoComponent);
			builder.CloseComponent();
		};

		// Act
		ctx.Render(componentRenderer);

		// Assert			
		// Smoke test - no exception should occur
	}

	public static IEnumerable<object[]> GetDemos()
	{
		return typeof(Demo).Assembly.GetTypes()
			.Where(t => t.Name.Contains("_Demo"))
			.Select(t => new object[] { t });
	}
}
