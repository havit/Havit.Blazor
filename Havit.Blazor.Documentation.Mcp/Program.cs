using Azure.Monitor.OpenTelemetry.AspNetCore;
using Havit.Blazor.Documentation.Mcp.Diagnostics;
using Havit.Blazor.Documentation.Mcp.Services;
using Havit.Blazor.Documentation.Mcp.Tools;
using Havit.Blazor.Documentation.Services;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

OpenTelemetryBuilder openTelemetry = builder.Services
	.AddOpenTelemetry()
	.WithTracing(tracing => tracing.AddSource(McpToolActivitySource.Name));

if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
	openTelemetry.UseAzureMonitor();
}

builder.Services.AddSingleton<IDocXmlProvider, McpDocXmlProvider>();
builder.Services.AddTransient<IComponentApiDocModelBuilder, ComponentApiDocModelBuilder>();
builder.Services.AddSingleton<McpDocMarkdownRenderer>();
builder.Services.AddSingleton<IDocumentationCatalogService, DocumentationCatalogService>();

builder.Services
	.AddMcpServer()
	.WithHttpTransport(options =>
	{
		options.Stateless = true;
	})
	.WithTools<GetComponentDocsTool>()
	.WithTools<GetComponentCatalogTool>()
	.WithTools<GetComponentSamplesTool>()
	.WithTools<GetTypeDocTool>();

var app = builder.Build();
app.MapMcp();

app.Run();
