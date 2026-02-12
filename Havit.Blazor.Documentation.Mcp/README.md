# HAVIT Blazor Documentation MCP Server

An MCP (Model Context Protocol) server that provides API documentation for HAVIT Blazor components.
AI assistants (GitHub Copilot, etc.) can use this server to look up component parameters, properties, events, and methods.

## Available tools

### `get_component_docs`

Returns the API documentation for a HAVIT Blazor component or type in Markdown format.

**Parameters:**
- `componentName` (string) – Name of the component or type, e.g. `HxButton`, `HxGrid`, `ThemeColor`.

## Developing locally

To test this MCP server from source code (locally), you can configure your IDE to connect to the server using localhost.

```json
{
  "servers": {
    "Havit.Blazor.Documentation.Mcp": {
      "type": "http",
      "url": "https://localhost:5247"
    }
  }
}
```

Refer to the VS Code or Visual Studio documentation for more information on configuring and using MCP servers:

- [Use MCP servers in VS Code (Preview)](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [Use MCP servers in Visual Studio (Preview)](https://learn.microsoft.com/visualstudio/ide/mcp-servers)

## Testing the MCP Server

Once configured, you can ask Copilot Chat about a component, for example, `What parameters does HxButton have?`. It should use the `get_component_docs` tool on the `Havit.Blazor.Documentation.Mcp` MCP server and return the API documentation.

You can also use the `.http` file (`Havit.Blazor.Documentation.Mcp.http`) to test the MCP endpoints directly.
- [MCP C# SDK](https://modelcontextprotocol.github.io/csharp-sdk)
