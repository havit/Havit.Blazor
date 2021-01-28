using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalizerGenerator
{
	internal class RegistrationsBuilder
	{
		public string Namespace { get; set; }
		public List<LocalizerBuilder> Localizers { get; } = new List<LocalizerBuilder>();
		public string MethodName => "AddGeneratedResourceWrappers";

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
			BuildClass(builder);
			builder.AppendLine("}");
		}

		private void BuildUsings(StringBuilder builder)
		{
			builder.AppendLine("using Microsoft.Extensions.DependencyInjection;");
			builder.AppendLine("using Microsoft.Extensions.Localization;");
			foreach (var @namespace in Localizers.Select(x => x.Namespace).Distinct(StringComparer.Ordinal))
			{
				builder.Append("using ").Append(@namespace).Append(";").AppendLine();
			}
		}

		private void BuildClass(StringBuilder builder)
		{
			builder.Append("partial class ").Append(LocalizerGenerator.ServiceCollectionInstallerMarker).AppendLine();
			builder.AppendLine("{");
			builder.Append("public static void ").Append(MethodName).Append("(this IServiceCollection services)").AppendLine();
			builder.AppendLine("{");
			BuildRegistrations(builder);
			builder.AppendLine("}");
			builder.AppendLine("}");
		}

		private void BuildRegistrations(StringBuilder builder)
		{
			foreach (var localizer in Localizers)
			{
				builder.Append("services.AddScoped<").Append(localizer.LocalizerInterfaceName).Append(", ").Append(localizer.LocalizerClassName).Append(">();").AppendLine();
			}
		}
	}
}
