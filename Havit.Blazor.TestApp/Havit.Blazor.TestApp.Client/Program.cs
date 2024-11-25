using Havit.Blazor.TestApp.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddClientServices();

await builder.Build().RunAsync();
