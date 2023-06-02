using System.Text;
using Havit.SourceGenerators.StrongApiStringLocalizers;

namespace LocalizerGenerator;

internal class LocalizerBuilder
{
	public string Name { get; set; }
	public string LocalizerClassName => $"{Name}Localizer";
	public string LocalizerInterfaceName => $"I{Name}Localizer";
	public string Namespace { get; set; }
	public List<string> Properties { get; } = new List<string>();
	public string IStringLocalizerName => $"IStringLocalizer<{Name}>";
	public string BaseClassName => $"DelegatingStringLocalizer<{Name}>";

	public string BuildSource()
	{
		var builder = new StringBuilder();
		BuildNamespace(builder);
		return builder.ToString();
	}

	private void BuildNamespace(StringBuilder builder)
	{
		builder.Append("namespace ").Append(Namespace).AppendLine();
		builder.AppendLine("{");
		BuildUsings(builder);
		BuildInterface(builder);
		BuildLocalizerClass(builder);
		builder.AppendLine("}");
	}

	private void BuildUsings(StringBuilder builder)
	{
		builder.AppendLine("using System.CodeDom.Compiler;");
		builder.AppendLine("using Microsoft.Extensions.Localization;");
		builder.AppendLine("using Havit.Extensions.Localization;");
	}

	private void BuildInterface(StringBuilder builder)
	{
		builder.AppendGeneratedCodeAttribute().AppendLine();
		builder.Append("public interface ").Append(LocalizerInterfaceName).Append(" : ").Append(IStringLocalizerName).AppendLine();
		builder.AppendLine("{");
		foreach (var property in Properties)
		{
			builder.Append("LocalizedString ").Append(property).Append(" { get; }").AppendLine();
		}
		builder.AppendLine("}");
	}

	private void BuildLocalizerClass(StringBuilder builder)
	{
		builder.AppendGeneratedCodeAttribute().AppendLine();
		builder.Append("public class ").Append(LocalizerClassName).Append(" : ").Append(BaseClassName).Append(", ").Append(LocalizerInterfaceName).AppendLine();
		builder.AppendLine("{");
		BuildCtor(builder);
		foreach (var property in Properties)
		{
			builder.Append("public LocalizedString ").Append(property).Append(" => this[\"").Append(property).Append("\"];").AppendLine();
		}
		builder.AppendLine("}");
	}

	private void BuildCtor(StringBuilder builder)
	{
		builder.Append("public ").Append(LocalizerClassName).Append("(").Append(IStringLocalizerName).Append(" innerLocalizer) : base(innerLocalizer)").AppendLine();
		builder.AppendLine("{");
		builder.AppendLine("}");
	}
}
