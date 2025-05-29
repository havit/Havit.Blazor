using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.InternalSourceGenerators;

[Generator]
public class BootstrapIconsGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
	{
		// Get a provider that provides the content of the selection.json file
		var selectionJsonContentProvider = initializationContext.AdditionalTextsProvider
			.Where(static file => file.Path.EndsWith(Path.Combine("Icons", "bootstrap-icons.json"))) // We are only interested in the bootstrap-icons.json file
			.Select((file, cancellationToken) => file.GetText(cancellationToken)) // Read its content
			.Collect(); // Instead of calling GenerateSourceCode in a loop for each bootstrap.json, we want a single call with all bootstrap.json files (if there are multiple)

		// Generate the code
		initializationContext.RegisterSourceOutput(selectionJsonContentProvider, static (sourceContext, source) =>
		{
			GenerateSourceCode(source, sourceContext);
		});
	}

	private static void GenerateSourceCode(ImmutableArray<SourceText> source, SourceProductionContext sourceContext)
	{
		// If there is no file, generate nothing
		if (source.Length == 0)
		{
			return;
		}

		// Assume the existence of a single file, generate code from it
		sourceContext.AddSource("BootstrapIcon.generated.cs", SourceText.From(GenerateSourceCode(source[0]), Encoding.UTF8));
	}

	private static string GenerateSourceCode(SourceText jsonSourceText)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("namespace Havit.Blazor.Components.Web.Bootstrap");
		sb.AppendLine("{");
		sb.AppendLine("	public partial class BootstrapIcon");
		sb.AppendLine("	{");
		foreach (string iconName in EnumerateIconNames(jsonSourceText))
		{
			string propertyName = GetPropertyNameFromIconName(iconName);
			string fieldName = GetFieldNameFromPropertyName(propertyName);
			sb.AppendLine($"		private static BootstrapIcon {fieldName};");
			sb.AppendLine($"		public static BootstrapIcon {propertyName} => {fieldName} ??= new BootstrapIcon(\"{iconName}\");");
		}
		sb.AppendLine("	}");
		sb.AppendLine("}");

		return sb.ToString();
	}

	private static IEnumerable<string> EnumerateIconNames(SourceText jsonSourceText)
	{
		foreach (TextLine textLine in jsonSourceText.Lines)
		{
			string line = textLine.ToString();

			if (!line.Contains("\"") || line.Contains("//"))
			{
				continue;
			}

			int s = line.IndexOf("\"");
			int e = line.LastIndexOf("\"");
			yield return line.Substring(s + 1, e - s - 1);
		}
	}

	private static string GetPropertyNameFromIconName(string iconName)
	{
		string[] segments = iconName.Split('-');
		var propertyName = String.Join("", segments.Select(segment => segment.Substring(0, 1).ToUpper() + segment.Substring(1)));
		if (!SyntaxFacts.IsValidIdentifier(propertyName))
		{
			// e.g. "123" => "_123"
			propertyName = "_" + propertyName;
		}
		return propertyName;
	}

	private static string GetFieldNameFromPropertyName(string propertyName)
	{
		// One of the icons is "new" which is a C# keyword. We are adding "_" to not conflict the field name with a keyword.
		return "_" + propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
	}
}