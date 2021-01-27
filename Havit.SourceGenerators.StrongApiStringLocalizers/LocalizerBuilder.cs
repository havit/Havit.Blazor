using System.Collections.Generic;
using System.Text;

namespace LocalizerGenerator
{
	internal class LocalizerBuilder
	{
		public string Name { get; set; }
		public string ClassName => $"{Name}Localizer";
		public string InterfaceName => $"I{Name}Localizer";
		public string Namespace { get; set; }
		public List<string> Properties { get; } = new List<string>();
		public string IStringLocalizerName => $"IStringLocalizer<{ClassName}>";
		public string BaseClassName => $"DelegatingStringLocalizer<{ClassName}>";

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
			BuildClass(builder);
			builder.AppendLine("}");
		}

		private void BuildUsings(StringBuilder builder)
		{
			builder.AppendLine("using Microsoft.Extensions.Localization;");
			builder.AppendLine("using Havit.Blazor.Components.Web.Infrastructure;");
		}

		private void BuildInterface(StringBuilder builder)
		{
			builder.Append("public interface ").Append(InterfaceName).Append(" : ").Append(IStringLocalizerName).AppendLine();
			builder.AppendLine("{");
			foreach (var property in Properties)
			{
				builder.Append("LocalizedString ").Append(property).Append(" { get; }").AppendLine();
			}
			builder.AppendLine("}");
		}

		private void BuildClass(StringBuilder builder)
		{
			builder.Append("public class ").Append(ClassName).Append(" : ").Append(BaseClassName).Append(", ").Append(InterfaceName).AppendLine();
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
			builder.Append("public ").Append(ClassName).Append("(").Append(IStringLocalizerName).Append(" innerLocalizer) : base(innerLocalizer)").AppendLine();
			builder.AppendLine("{");
			builder.AppendLine("}");
		}
	}
}
