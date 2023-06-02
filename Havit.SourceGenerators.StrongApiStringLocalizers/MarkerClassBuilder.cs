using System.Text;
using Havit.SourceGenerators.StrongApiStringLocalizers;

namespace LocalizerGenerator;

internal class MarkerClassBuilder
{
	public string Name { get; set; }
	public string Namespace { get; set; }

	public string BuildSource()
	{
		var builder = new StringBuilder();
		BuildUsings(builder);
		BuildNamespace(builder);
		return builder.ToString();
	}
	private void BuildUsings(StringBuilder builder)
	{
		builder.AppendLine("using System.CodeDom.Compiler;");
		builder.AppendLine("using System.ComponentModel;");
	}

	private void BuildNamespace(StringBuilder builder)
	{
		builder.Append("namespace ").Append(Namespace).AppendLine();
		builder.AppendLine("{");
		BuildMarkerClass(builder);
		builder.AppendLine("}");
	}

	private void BuildMarkerClass(StringBuilder builder)
	{
		builder.AppendGeneratedCodeAttribute().AppendLine();
		builder.Append("[Browsable(false)]").AppendLine();
		builder.Append("[EditorBrowsable(EditorBrowsableState.Never)]").AppendLine();
		builder.Append("public class ").Append(Name).AppendLine();
		builder.AppendLine("{");
		builder.AppendLine("}");
	}
}
