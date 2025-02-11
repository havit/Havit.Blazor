using System.Text;

// Copy of HFW HttpUtilityExt.

namespace Havit.SourceGenerators.StrongApiStringLocalizers.Helpers;

internal static partial class HttpUtilityExt
{
	public static string HtmlEncode(string unicodeText, HtmlEncodeOptions options)
	{
		int unicodeValue;
		StringBuilder result = new StringBuilder();

		bool opIgnoreNonASCIICharacters = (options & HtmlEncodeOptions.IgnoreNonASCIICharacters) == HtmlEncodeOptions.IgnoreNonASCIICharacters;
		bool opExtendedHtmlEntities = (options & HtmlEncodeOptions.ExtendedHtmlEntities) == HtmlEncodeOptions.ExtendedHtmlEntities;
		bool opXmlApostropheEntity = (options & HtmlEncodeOptions.XmlApostropheEntity) == HtmlEncodeOptions.XmlApostropheEntity;

		int length = unicodeText.Length;
		for (int i = 0; i < length; i++)
		{
			unicodeValue = unicodeText[i];
			switch (unicodeValue)
			{
				case '&':
					result.Append("&amp;");
					break;
				case '<':
					result.Append("&lt;");
					break;
				case '>':
					result.Append("&gt;");
					break;
				case '"':
					result.Append("&quot;");
					break;
				case '\'':
					if (opXmlApostropheEntity)
					{
						result.Append("&apos;");
						break;
					}
					else
					{
						goto default;
					}
				case 0xA0: // no-break space
					if (opExtendedHtmlEntities)
					{
						result.Append("&nbsp;");
						break;
					}
					else
					{
						goto default;
					}
				case '€':
					if (opExtendedHtmlEntities)
					{
						result.Append("&euro;");
						break;
					}
					else
					{
						goto default;
					}
				case '©':
					if (opExtendedHtmlEntities)
					{
						result.Append("&copy;");
						break;
					}
					else
					{
						goto default;
					}
				case '®':
					if (opExtendedHtmlEntities)
					{
						result.Append("&reg;");
						break;
					}
					else
					{
						goto default;
					}
				case '™': // trade-mark
					if (opExtendedHtmlEntities)
					{
						result.Append("&trade;");
						break;
					}
					else
					{
						goto default;
					}
				default:
					if (((unicodeText[i] >= ' ') && (unicodeText[i] <= 0x007E))
						|| opIgnoreNonASCIICharacters)
					{
						result.Append(unicodeText[i]);
					}
					else
					{
						result.Append("&#");
						result.Append(unicodeValue.ToString(System.Globalization.NumberFormatInfo.InvariantInfo));
						result.Append(";");
					}
					break;
			}
		}
		return result.ToString();
	}
	public static string HtmlEncode(string unicodeText)
	{
		return HtmlEncode(unicodeText, HtmlEncodeOptions.None);
	}
}

[Flags]
public enum HtmlEncodeOptions
{
	/// <summary>
	/// Označuje, že nemají být nastaveny žádné options, použije se default postup.
	/// Default postup převede pouze čtyři základní entity
	/// <list type="bullet">
	///		<item>&lt; --- &amp;lt;</item>
	///		<item>&gt; --- &amp;gt;</item>
	///		<item>&amp; --- &amp;amp;</item>
	///		<item>&quot; --- &amp;quot;</item>
	/// </list>
	/// </summary>
	None = 0,

	/// <summary>
	/// Při konverzi budou ignorovány znaky mimo ASCII hodnoty, nebudou tedy tvořeny číselné entity typu &amp;#123;.
	/// </summary>
	IgnoreNonASCIICharacters = 1,

	/// <summary>
	/// Při konverzi bude použita rozšířená sada HTML-entit, které by se jinak převedly na číselné entity.
	/// Např. bude použito &amp;copy;, &amp;nbsp;, &amp;sect;, atp. 
	/// </summary>
	ExtendedHtmlEntities = 2,

	/// <summary>
	/// Při konverzi převede apostrofy na &amp;apos; entitu.
	/// POZOR! &amp;apos; není standardní HTML entita a třeba IE ji v HTML režimu nepozná!!!
	/// </summary>
	/// <remarks>
	/// V kombinaci se základním <see cref="None"/> dostaneme sadu pěti built-in XML entit:
	/// <list type="bullet">
	///		<item>&lt; --- &amp;lt;</item>
	///		<item>&gt; --- &amp;gt;</item>
	///		<item>&amp; --- &amp;amp;</item>
	///		<item>&quot; --- &amp;quot;</item>
	///		<item>&apos; --- &amp;apos;</item>
	/// </list>
	/// </remarks>
	XmlApostropheEntity = 4
}
