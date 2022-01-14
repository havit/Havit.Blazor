using System.Reflection;
using System.Text;

namespace Havit.SourceGenerators.StrongApiStringLocalizers
{
	internal static class BuilderExtensions
	{
		public static StringBuilder AppendGeneratedCodeAttribute(this StringBuilder builder)
		{
			builder.Append($"[GeneratedCode(\"{nameof(Havit)}.{nameof(SourceGenerators)}.{nameof(StrongApiStringLocalizers)}.{nameof(LocalizerGenerator)}\", \"{Assembly.GetExecutingAssembly().GetName().Version}\")]");

			return builder;
		}
	}
}
