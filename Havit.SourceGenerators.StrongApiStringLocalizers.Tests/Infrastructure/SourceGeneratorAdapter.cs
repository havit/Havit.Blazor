using Microsoft.CodeAnalysis;

namespace Havit.SourceGenerators.StrongApiStringLocalizers.Tests.Infrastructure;

public class SourceGeneratorAdapter<TIncrementalGenerator> : ISourceGenerator, IIncrementalGenerator
	where TIncrementalGenerator : IIncrementalGenerator, new()
{
	private readonly TIncrementalGenerator _internalGenerator;

	public SourceGeneratorAdapter()
	{
		_internalGenerator = new TIncrementalGenerator();
	}

	public void Execute(GeneratorExecutionContext context)
	{
		throw new System.NotImplementedException();
	}

	public void Initialize(GeneratorInitializationContext context)
	{
		throw new System.NotImplementedException();
	}

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		_internalGenerator.Initialize(context);
	}
}