using Havit.Blazor.TestApp.StandaloneServer.Components;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.TestApp.StandaloneServer.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddCircuitOptions(options =>
	{
		options.DetailedErrors = true;
	});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHxServices();
builder.Services.AddHxMessenger();
builder.Services.AddHxMessageBoxHost();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMediaPipelineEndpoint();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
