using System.Text;
using System.Xml;
using System.Xml.Linq;
using Havit.SourceGenerators.StrongApiStringLocalizers.Helpers;
using Havit.SourceGenerators.StrongApiStringLocalizers.Model;
using Havit.SourceGenerators.StrongApiStringLocalizers.SourceBuilders;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace Havit.SourceGenerators.StrongApiStringLocalizers;

[Generator]
public class StrongApiStringLocalizersGenerator : IIncrementalGenerator
{
	private static readonly DiagnosticDescriptor s_xmlParseWarning = new DiagnosticDescriptor(id: "HLG1002", title: "Cannot parse RESX file", messageFormat: "Cannot parse RESX file '{0}'", category: "Usage", DiagnosticSeverity.Warning, isEnabledByDefault: true);

	public void Initialize(IncrementalGeneratorInitializationContext initializationContext)
	{
		// read project configuration
		IncrementalValueProvider<BuildConfiguration> buildConfigurationProvider = initializationContext.AnalyzerConfigOptionsProvider
			.Combine(initializationContext.CompilationProvider)
			.Select((item, _) =>
			{
				AnalyzerConfigOptionsProvider analyzerConfig = item.Left;
				Compilation compilation = item.Right;
				analyzerConfig.GlobalOptions.TryGetValue("build_property.RootNamespace", out string rootNamespace);
				analyzerConfig.GlobalOptions.TryGetValue("build_property.ProjectDir", out string projectDir);

				return new BuildConfiguration
				{
					RootNamespace = rootNamespace,
					ProjectDirectory = projectDir,
					AssemblyName = compilation.AssemblyName
				};
			});

		// get resx files (with all required data to generate localizers one by one)
		// (resx files are available in initializationContext.AdditionalTextsProvider only when library references
		// nuget package Microsoft.CodeAnalysis)
		IncrementalValuesProvider<ResourceData> resxDataProvider = initializationContext.AdditionalTextsProvider
			.Where(static file => string.Equals(Path.GetExtension(file.Path), ".resx", StringComparison.OrdinalIgnoreCase)) // .resx
			.Where(static file => !Path.GetFileNameWithoutExtension(file.Path).Contains(".")) // skip language-specific files - take Resource.resx, skip Resource.cs.resx
			.Combine(buildConfigurationProvider) // "join" with build configuration
			.Select(static (item, cancellationToken) =>
			{
				AdditionalText additionalText = item.Left;
				BuildConfiguration buildConfiguration = item.Right;
				return new ResourceData
				{
					ResxFilePath = additionalText.Path,
					AssemblyName = buildConfiguration.AssemblyName,
					TargetLocalizerNamespace = GetTargetNamespace(buildConfiguration, additionalText.Path),
					ResourceNamespace = GetResourceNamespace(buildConfiguration, additionalText.Path),
					ResourceName = Path.GetFileNameWithoutExtension(additionalText.Path), // ie. Homepage, Glossary, ...
					Properties = GetResxPropertiesSafe(additionalText, cancellationToken) // ie.. Yes, No, OK, Cancel, ...
				};
			});

		// get list of resx files (with all required data to generate service collection extension)
		IncrementalValueProvider<ServiceCollectionExtensionsData> serviceCollectionExtensionsDataProvider = resxDataProvider
			.Collect()
			.Combine(buildConfigurationProvider)
			.Select(static (item, cancellationToken) => new ServiceCollectionExtensionsData
			{
				RootNamespace = item.Right.RootNamespace,
				Resources = item.Left.OrderBy(item => item.TargetLocalizerNamespace).ThenBy(item => item.ResourceName).ToList()
			});

		initializationContext.RegisterSourceOutput(resxDataProvider, static (sourceContext, resxBuildData) =>
		{
			if (resxBuildData.Properties == null)
			{
				// XML could not be parsed
				sourceContext.ReportDiagnostic(Diagnostic.Create(s_xmlParseWarning, Location.None, resxBuildData.ResxFilePath));
			}
			else
			{
				// generate localizer interface (ie. Homepage.resx => IHomepageLocalizer)
				LocalizerInterfaceSourceBuilder localizerInterfaceSourceBuilder = new LocalizerInterfaceSourceBuilder(resxBuildData);
				sourceContext.AddSource($"{resxBuildData.TargetLocalizerNamespace}.I{resxBuildData.ResourceName}Localizer.g.cs", SourceText.From(localizerInterfaceSourceBuilder.BuildSource(), Encoding.UTF8));

				// generate localizer implementation (ie. Homepage.resx => HomepageLocalizer)
				LocalizerImplementationSourceBuilder localizerImplementationSourceBuilder = new LocalizerImplementationSourceBuilder(resxBuildData);
				sourceContext.AddSource($"{resxBuildData.TargetLocalizerNamespace}.{resxBuildData.ResourceName}Localizer.g.cs", SourceText.From(localizerImplementationSourceBuilder.BuildSource(), Encoding.UTF8));
			}
		});

		initializationContext.RegisterSourceOutput(serviceCollectionExtensionsDataProvider, static (sourceContext, serviceCollectionExtensionsData) =>
		{
			if (serviceCollectionExtensionsData.Resources.Count > 0)
			{
				// generate service collection externsion to register all generated localizers
				var serviceRegistrationsSourceBuilder = new ServiceRegistrationsSourceBuilder(serviceCollectionExtensionsData);
				sourceContext.AddSource($"{serviceCollectionExtensionsData.RootNamespace}.ResourcesServiceCollectionExtensions.g.cs", SourceText.From(serviceRegistrationsSourceBuilder.BuildSource(), Encoding.UTF8));
			}
		});
	}

	private static string GetTargetNamespace(BuildConfiguration buildConfiguration, string path)
	{
		return (buildConfiguration.RootNamespace + "." + GetResourceNamespace(buildConfiguration, path)).Trim('.');
	}

	private static string GetResourceNamespace(BuildConfiguration buildConfiguration, string path)
	{
		string localPath = path.StartsWith(buildConfiguration.ProjectDirectory, StringComparison.InvariantCultureIgnoreCase)
			? path.Substring(buildConfiguration.ProjectDirectory.Length).Trim(Path.DirectorySeparatorChar)
			: path;

		return Path.GetDirectoryName(localPath).Replace(Path.DirectorySeparatorChar, '.');
	}


	private static List<ResourcePropertyItem> GetResxPropertiesSafe(AdditionalText resx, CancellationToken cancellationToken)
	{
		try
		{
			SourceText resxContent = resx.GetText(cancellationToken);
			var resxXmlDoc = XDocument.Load(new SourceTextReader(resxContent));

			var result = new List<ResourcePropertyItem>();
			foreach (var item in resxXmlDoc.Root.Elements("data"))
			{
				var nameAttribute = item.Attribute("name");
				if (nameAttribute == null)
				{
					continue;
				}

				string comment = item.Element("comment")?.Value;
				string value = item.Element("value")?.Value;

				result.Add(new ResourcePropertyItem
				{
					Name = nameAttribute.Value,
					Comment = comment ?? value,
				});
			}
			return result.OrderBy(item => item.Name, StringComparer.InvariantCultureIgnoreCase).ToList();
		}
		catch (XmlException)
		{
			return null;
		}
	}
}
