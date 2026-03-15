using Havit.Blazor.Documentation.RepoDumpGenerator.Services;
using Havit.Blazor.Documentation.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IDocXmlProvider, DocXmlProvider>();
builder.Services.AddSingleton<IApiDocModelBuilder, ApiDocModelBuilder>();
builder.Services.AddSingleton<IApiDocModelProvider, ApiDocModelProvider>();
builder.Services.AddSingleton<IDocMarkdownRenderer, DocMarkdownRenderer>();
builder.Services.AddSingleton<IComponentDemosProvider, ComponentDemosProvider>();
builder.Services.AddSingleton<IDocumentationCatalogService, DocumentationCatalogService>();
builder.Services.AddSingleton<DocDumpService>();

using IHost host = builder.Build();

string outputRoot = Path.Combine(FindRepoRoot(AppContext.BaseDirectory), "docs", "generated");

DocDumpService dumpService = host.Services.GetRequiredService<DocDumpService>();
dumpService.Run(outputRoot);

// --- Helper methods ---

static string FindRepoRoot(string startDir)
{
	string dir = startDir;
	while (dir is not null)
	{
		if (File.Exists(Path.Combine(dir, "Havit.Blazor.sln")))
		{
			return dir;
		}
		dir = Path.GetDirectoryName(dir);
	}
	throw new InvalidOperationException("Could not find repository root (Havit.Blazor.sln).");
}
