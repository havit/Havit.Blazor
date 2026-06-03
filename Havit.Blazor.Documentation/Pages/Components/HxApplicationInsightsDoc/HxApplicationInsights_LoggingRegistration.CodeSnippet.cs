// Required in Blazor WebAssembly — the WASM host does NOT read logging
// configuration from appsettings.json automatically (unlike server-side ASP.NET Core).
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

builder.Logging.AddBlazorApplicationInsights();
