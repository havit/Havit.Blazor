﻿using System.Text;
using Havit.SourceGenerators.StrongApiStringLocalizers.Model;

namespace Havit.SourceGenerators.StrongApiStringLocalizers.SourceBuilders;

/// <summary>
/// Generates source code for the localizer interface.
/// </summary>
internal class LocalizerInterfaceSourceBuilder
{
	private readonly ResourceData _resxBuildData;

	public LocalizerInterfaceSourceBuilder(ResourceData resxBuildData)
	{
		_resxBuildData = resxBuildData;
	}

	public string BuildSource()
	{
		var builder = new StringBuilder();
		builder.AppendAutoGeneratedDocumenationCommentLine();
		builder.AppendLine();
		builder.AppendLine($"namespace {_resxBuildData.TargetLocalizerNamespace};");
		builder.AppendLine();
		builder.AppendLine("using System.CodeDom.Compiler;");
		builder.AppendLine("using Microsoft.Extensions.Localization;");
		builder.AppendLine();
		builder.AppendGeneratedCodeAttributeLine();
		builder.AppendLine($"public interface {_resxBuildData.LocalizerInterfaceName} : IStringLocalizer");
		builder.AppendLine("{");
		foreach (var property in _resxBuildData.Properties)
		{
			builder.AppendLine($"\tLocalizedString {property} {{ get; }}");
		}
		builder.AppendLine("}");

		return builder.ToString();
	}
}
