using Microsoft.CodeAnalysis.Text;

namespace Havit.SourceGenerators.StrongApiStringLocalizers.Helpers;

// source: https://github.com/dotnet/roslyn-analyzers/blob/8fe7aeb135c64e095f43292c427453858d937184/src/Microsoft.CodeAnalysis.ResxSourceGenerator/Microsoft.CodeAnalysis.ResxSourceGenerator/AbstractResxGenerator.cs#L888
internal sealed class SourceTextReader : TextReader
{
	private readonly SourceText _text;
	private int _position;

	public SourceTextReader(SourceText text)
	{
		_text = text;
	}

	public override int Read(char[] buffer, int index, int count)
	{
		var remaining = _text.Length - _position;
		var charactersToRead = Math.Min(remaining, count);
		_text.CopyTo(_position, buffer, index, charactersToRead);
		_position += charactersToRead;
		return charactersToRead;
	}
}
