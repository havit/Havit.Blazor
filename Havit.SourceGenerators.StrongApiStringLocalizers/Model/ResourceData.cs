namespace Havit.SourceGenerators.StrongApiStringLocalizers.Model;

internal class ResourceData
{
	public string AssemblyName { get; set; }
	public string ResxFilePath { get; set; }
	public string ResourceNamespace { get; set; }
	public string ResourceName { get; set; }

	public string TargetLocalizerNamespace { get; set; }
	public string LocalizerImplementationClassName => $"{ResourceName}Localizer";
	public string LocalizerInterfaceName => $"I{ResourceName}Localizer";

	public List<ResourcePropertyItem> Properties { get; set; }
}