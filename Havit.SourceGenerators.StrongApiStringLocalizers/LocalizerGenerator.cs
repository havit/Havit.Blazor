using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace LocalizerGenerator;

[Generator]
public class LocalizerGenerator : ISourceGenerator
{
	public const string ServiceCollectionInstallerMarker = "ResourcesServiceCollectionInstaller";

#pragma warning disable RS2008
	private static readonly DiagnosticDescriptor xmlParseWarning = new DiagnosticDescriptor(id: "LG0001", title: "Cannot parse XML file", messageFormat: "Cannot parse XML file '{0}'", category: nameof(LocalizerGenerator), DiagnosticSeverity.Warning, isEnabledByDefault: true);
#pragma warning restore RS2008


	public void Initialize(GeneratorInitializationContext context)
	{ }

	public void Execute(GeneratorExecutionContext context)
	{
		foreach (var syntaxTree in context.Compilation.SyntaxTrees)
		{
			var file = Path.GetFileNameWithoutExtension(syntaxTree.FilePath);
			if (!file.Equals(ServiceCollectionInstallerMarker, StringComparison.Ordinal))
			{
				continue;
			}

			var syntaxRoot = syntaxTree.GetRoot();

			var namespaceBase = FindNamespaceName(syntaxRoot);
			if (namespaceBase == null)
			{
				continue;
			}

			var registrationsBuilder = new RegistrationsBuilder();
			registrationsBuilder.Namespace = namespaceBase;

			var rootDir = Path.GetDirectoryName(syntaxTree.FilePath);
#pragma warning disable RS1035 // Do not use APIs banned for analyzers (Directory)
			foreach (var resx in Directory.EnumerateFiles(rootDir, "*.resx", SearchOption.AllDirectories))
			{
				var localizerBuilder = new LocalizerBuilder();
				localizerBuilder.Name = Path.GetFileNameWithoutExtension(resx);
				if (localizerBuilder.Name.Contains("."))
				{
					// language-specific file
					continue;
				}
				var namespaceSuffix = Path.GetDirectoryName(resx).Remove(0, rootDir.Length).Replace(Path.DirectorySeparatorChar, '.');
				localizerBuilder.Namespace = $"{namespaceBase}{namespaceSuffix}";
				var properties = ParseResx(resx);
				if (properties == null)
				{
					context.ReportDiagnostic(Diagnostic.Create(xmlParseWarning, Location.None, resx));
					continue;
				}
				localizerBuilder.Properties.AddRange(properties);
				context.AddSource($"{nameof(LocalizerGenerator)}.{localizerBuilder.Namespace}.{localizerBuilder.LocalizerClassName}.generated.cs", SourceText.From(localizerBuilder.BuildSource(), Encoding.UTF8));

				var markerClassBuilder = new MarkerClassBuilder();
				markerClassBuilder.Namespace = localizerBuilder.Namespace;
				markerClassBuilder.Name = localizerBuilder.Name;
				context.AddSource($"{nameof(LocalizerGenerator)}.{markerClassBuilder.Namespace}.{markerClassBuilder.Name}.generated.cs", SourceText.From(markerClassBuilder.BuildSource(), Encoding.UTF8));

				registrationsBuilder.Localizers.Add(localizerBuilder);
			}
#pragma warning restore RS1035 // Do not use APIs banned for analyzers

			context.AddSource($"{nameof(LocalizerGenerator)}.{registrationsBuilder.Namespace}.{registrationsBuilder.MethodName}.generated.cs", SourceText.From(registrationsBuilder.BuildSource(), Encoding.UTF8));
		}
	}

	private static List<string> ParseResx(string path)
	{
		try
		{
			var result = new List<string>();
			var xdoc = XDocument.Load(path);
			foreach (var item in xdoc.Root.Elements("data"))
			{
				var nameAttribute = item.Attribute("name");
				if (nameAttribute == null)
				{
					continue;
				}

				result.Add(nameAttribute.Value);
			}
			return result;
		}
		catch (XmlException)
		{
			return null;
		}
	}

	private static string FindNamespaceName(SyntaxNode syntaxRoot)
	{
		var namespaceNode = (BaseNamespaceDeclarationSyntax)syntaxRoot.ChildNodes().Where(x => x.IsKind(SyntaxKind.NamespaceDeclaration)).FirstOrDefault();
		if (namespaceNode != null)
		{
			return namespaceNode.Name.ToString();
		}

		var fileScopedNamespaceNode = (BaseNamespaceDeclarationSyntax)syntaxRoot.ChildNodes().Where(x => x.IsKind(SyntaxKind.FileScopedNamespaceDeclaration)).FirstOrDefault();
		if (fileScopedNamespaceNode != null)
		{
			return fileScopedNamespaceNode.Name.ToString();
		}

		return null;
	}
}
