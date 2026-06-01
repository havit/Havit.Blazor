using Bunit;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.Documentation.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Documentation.Tests;

public class DemosSmokeTests
{
	[Theory]
	[MemberData(nameof(GetDemos))]
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

	public static TheoryData<Type> GetDemos()
	{
		var data = new TheoryData<Type>();
		foreach (var demoType in typeof(Demo).Assembly.GetTypes().Where(t => t.Name.Contains("_Demo")))
		{
			data.Add(demoType);
		}
		return data;
	}
}
