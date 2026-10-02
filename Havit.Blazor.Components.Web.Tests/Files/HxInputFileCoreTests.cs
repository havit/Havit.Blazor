using Bunit;
using Havit.Blazor.Components.Web.Bootstrap.Tests;

namespace Havit.Blazor.Components.Web.Tests.Files;

public class HxInputFileCoreTests : BunitTestBase
{
	[Fact]
	public async Task HxInputFileCore_UploadHttpMethod_NotSet_UsesPostFromDefaults()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters
			.Add(p => p.UploadUrl, "/upload"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.StartUploadAsync());

		// Assert
		Assert.Equal("POST", GetUploadHttpMethodPassedToJs());
	}

	[Fact]
	public async Task HxInputFileCore_UploadHttpMethod_SetInSettings_UsesSettings()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters
			.Add(p => p.UploadUrl, "/upload")
			.Add(p => p.Settings, new InputFileCoreSettings { UploadHttpMethod = "PUT" }));

		// Act
		await cut.InvokeAsync(() => cut.Instance.StartUploadAsync());

		// Assert
		Assert.Equal("PUT", GetUploadHttpMethodPassedToJs());
	}

	[Fact]
	public async Task HxInputFileCore_UploadHttpMethod_SetAsParameter_OverridesSettings()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters
			.Add(p => p.UploadUrl, "/upload")
			.Add(p => p.Settings, new InputFileCoreSettings { UploadHttpMethod = "PUT" })
			.Add(p => p.UploadHttpMethod, "PATCH"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.StartUploadAsync());

		// Assert
		Assert.Equal("PATCH", GetUploadHttpMethodPassedToJs());
	}

	private string GetUploadHttpMethodPassedToJs()
	{
		var invocation = JSInterop.Invocations.Single(i => i.Identifier == "upload");
		return (string)invocation.Arguments[6];
	}
}
