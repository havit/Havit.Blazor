using System.Text;
using System.Text.RegularExpressions;
using Havit.Blazor.Documentation.Model;
using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Mcp.Services;

/// <summary>
/// Renders a <see cref="ComponentApiDocModel"/> into a Markdown string suitable for MCP tool responses.
/// </summary>
internal class McpDocMarkdownRenderer
{
	/// <summary>
	/// Renders the type API documentation as markdown (parameters, properties, events, methods).
	/// </summary>
	public string RenderTypeDoc(ComponentApiDocModel model)
	{
		StringBuilder sb = new StringBuilder();
		RenderTypeDocCore(sb, model);
		return sb.ToString();
	}

	/// <summary>
	/// Renders the component API documentation as markdown, including a list of available demo samples.
	/// </summary>
	public string RenderComponentDoc(ComponentApiDocModel model, IReadOnlyList<string> sampleNames)
	{
		StringBuilder sb = new StringBuilder();
		RenderTypeDocCore(sb, model);
		RenderSampleList(sb, sampleNames);
		return sb.ToString();
	}

	private static void RenderTypeDocCore(StringBuilder sb, ComponentApiDocModel model)
	{
		string plainTypeName = ApiRenderer.RemoveSpecialCharacters(model.Type.Name);

		sb.AppendLine($"# {plainTypeName}");
		sb.AppendLine();

		if (!string.IsNullOrWhiteSpace(model.Class?.Comments?.Summary))
		{
			sb.AppendLine(StripHtml(model.Class.Comments.Summary));
			sb.AppendLine();
		}

		if (model.IsDelegate)
		{
			sb.AppendLine($"**Delegate signature:** `{StripHtml(model.DelegateSignature)}`");
			sb.AppendLine();
			return;
		}

		if (model.IsEnum)
		{
			RenderEnumMembers(sb, model);
			return;
		}

		RenderParameters(sb, model);
		RenderProperties(sb, model);
		RenderEvents(sb, model);
		RenderMethods(sb, model);
		RenderStaticProperties(sb, model);
		RenderStaticMethods(sb, model);
	}

	private static void RenderEnumMembers(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.EnumMembers.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Enum values");
		sb.AppendLine();
		sb.AppendLine("| Name | Value | Description |");
		sb.AppendLine("|------|-------|-------------|");

		foreach (EnumModel enumMember in model.EnumMembers)
		{
			string summary = StripHtml(enumMember.Summary ?? string.Empty);
			sb.AppendLine($"| {enumMember.Name} | {enumMember.Value} | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderParameters(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.Parameters.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Parameters");
		sb.AppendLine();
		sb.AppendLine("| Name | Type | Description |");
		sb.AppendLine("|------|------|-------------|");

		foreach (PropertyModel property in model.Parameters.OrderByDescending(p => p.EditorRequired).ThenBy(p => p.PropertyInfo.Name))
		{
			string name = property.PropertyInfo.Name;
			if (property.EditorRequired)
			{
				name += " **[REQUIRED]**";
			}
			if (property.IsStatic)
			{
				name = "*(static)* " + name;
			}

			string type = StripHtml(ApiRenderer.FormatType(property.PropertyInfo.PropertyType));
			string summary = StripHtml(property.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {name} | `{type}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderProperties(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.Properties.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Properties");
		sb.AppendLine();
		sb.AppendLine("| Name | Type | Description |");
		sb.AppendLine("|------|------|-------------|");

		foreach (PropertyModel property in model.Properties.OrderBy(p => p.PropertyInfo.Name))
		{
			string type = StripHtml(ApiRenderer.FormatType(property.PropertyInfo.PropertyType));
			string summary = StripHtml(property.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {property.PropertyInfo.Name} | `{type}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderEvents(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.Events.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Event callbacks");
		sb.AppendLine();
		sb.AppendLine("| Name | Type | Description |");
		sb.AppendLine("|------|------|-------------|");

		foreach (PropertyModel currentEvent in model.Events.OrderBy(e => e.PropertyInfo.Name))
		{
			string type = StripHtml(ApiRenderer.FormatType(currentEvent.PropertyInfo.PropertyType));
			string summary = StripHtml(currentEvent.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {currentEvent.PropertyInfo.Name} | `{type}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderMethods(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.IsEnum || model.Methods.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Methods");
		sb.AppendLine();
		sb.AppendLine("| Method | Returns | Description |");
		sb.AppendLine("|--------|---------|-------------|");

		foreach (MethodModel method in model.Methods.OrderBy(m => m.MethodInfo.Name))
		{
			string parameters = StripHtml(method.GetParameters());
			string returnType = StripHtml(ApiRenderer.FormatMethodReturnType(method.MethodInfo.ReturnType, model));
			string summary = StripHtml(method.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {method.MethodInfo.Name}{parameters} | `{returnType}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderStaticProperties(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.StaticProperties.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Static properties");
		sb.AppendLine();
		sb.AppendLine("| Property | Type | Description |");
		sb.AppendLine("|----------|------|-------------|");

		foreach (PropertyModel property in model.StaticProperties.OrderBy(p => p.PropertyInfo.Name))
		{
			string type = StripHtml(ApiRenderer.FormatType(property.PropertyInfo.PropertyType));
			string summary = StripHtml(property.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {property.PropertyInfo.Name} | `{type}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderStaticMethods(StringBuilder sb, ComponentApiDocModel model)
	{
		if (model.IsEnum || model.StaticMethods.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Static methods");
		sb.AppendLine();
		sb.AppendLine("| Method | Returns | Description |");
		sb.AppendLine("|--------|---------|-------------|");

		foreach (MethodModel method in model.StaticMethods.OrderBy(m => m.MethodInfo.Name))
		{
			string parameters = StripHtml(method.GetParameters());
			string returnType = StripHtml(ApiRenderer.FormatType(method.MethodInfo.ReturnType));
			string summary = StripHtml(method.Comments?.Summary ?? string.Empty);
			sb.AppendLine($"| {method.MethodInfo.Name}{parameters} | `{returnType}` | {summary} |");
		}

		sb.AppendLine();
	}

	private static void RenderSampleList(StringBuilder sb, IReadOnlyList<string> sampleNames)
	{
		if (sampleNames is null || sampleNames.Count == 0)
		{
			return;
		}

		sb.AppendLine("## Available demo samples");
		sb.AppendLine();
		sb.AppendLine("Use the `get_component_samples` tool to retrieve the full source code.");
		sb.AppendLine();

		foreach (string name in sampleNames)
		{
			sb.AppendLine($"- {name}");
		}

		sb.AppendLine();
	}

	/// <summary>
	/// Strips HTML tags and decodes HTML entities to produce plain text suitable for markdown.
	/// </summary>
	private static string StripHtml(string html)
	{
		if (string.IsNullOrEmpty(html))
		{
			return string.Empty;
		}

		// Replace <code> tags with backticks
		string result = Regex.Replace(html, @"<code>(.*?)</code>", "`$1`");

		// Replace <br/> and <br> with space
		result = Regex.Replace(result, @"<br\s*/?>", " ");

		// Remove remaining HTML tags
		result = Regex.Replace(result, @"<[^>]+>", string.Empty);

		// Decode HTML entities
		result = System.Net.WebUtility.HtmlDecode(result);

		// Normalize whitespace (collapse multiple spaces, trim)
		result = Regex.Replace(result, @"\s+", " ").Trim();

		// Escape pipe characters for markdown tables
		result = result.Replace("|", "\\|");

		return result;
	}
}
