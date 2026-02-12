using Havit.Blazor.Documentation.Mcp.Services;
using Havit.Blazor.Documentation.Mcp.Tools;
using Havit.Blazor.Documentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDocXmlProvider, McpDocXmlProvider>();
builder.Services.AddTransient<IComponentApiDocModelBuilder, ComponentApiDocModelBuilder>();
builder.Services.AddSingleton<ComponentDocMarkdownRenderer>();

builder.Services
	.AddMcpServer()
	.WithHttpTransport()
	.WithTools<GetComponentDocsTool>();

var app = builder.Build();
app.MapMcp();

app.Run();
