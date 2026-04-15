using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Processes inline markdown elements (bold, italic, code, links, images, strikethrough, line breaks).
/// </summary>
internal static partial class MarkdownInlineParser
{
	[GeneratedRegex(@"`([^`]+)`")]
	private static partial Regex InlineCodeRegex();

	[GeneratedRegex(@"!\[([^\]]*)\]\(([^\s\)]+)(?:\s+""([^""]*)"")?\)")]
	private static partial Regex ImageRegex();

	[GeneratedRegex(@"\[([^\]]+)\]\(([^\s\)]+)(?:\s+""([^""]*)"")?\)")]
	private static partial Regex LinkRegex();

	[GeneratedRegex(@"\*\*(.+?)\*\*")]
	private static partial Regex BoldAsteriskRegex();

	[GeneratedRegex(@"__(.+?)__")]
	private static partial Regex BoldUnderscoreRegex();

	[GeneratedRegex(@"\*(.+?)\*")]
	private static partial Regex ItalicAsteriskRegex();

	[GeneratedRegex(@"(?<!\w)_(.+?)_(?!\w)")]
	private static partial Regex ItalicUnderscoreRegex();

	[GeneratedRegex(@"~~(.+?)~~")]
	private static partial Regex StrikethroughRegex();

	[GeneratedRegex(@"  \n")]
	private static partial Regex LineBreakRegex();

	/// <summary>
	/// Converts inline markdown syntax to HTML within a text block.
	/// </summary>
	internal static string ProcessInlineElements(string text, MarkdownRenderOptions options)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}

		// Step 1: Extract inline code spans and replace with placeholders.
		// Code content is HTML-encoded (always, regardless of SanitizeHtml) and protected
		// from all subsequent inline-element passes (bold, italic, links, etc.).
		var codePlaceholders = new List<string>();
		text = InlineCodeRegex().Replace(text, match =>
		{
			var code = match.Groups[1].Value;
			// Always HTML-encode code content — code spans are always literal text.
			var encoded = code.Replace("&", "&amp;").Replace("<", "&lt;");
			var placeholder = $"\x00CODE{codePlaceholders.Count}\x00";
			codePlaceholders.Add($"<code>{encoded}</code>");
			return placeholder;
		});

		// Step 2: Sanitize remaining text (& and < outside of code spans).
		if (options.SanitizeHtml && text.AsSpan().IndexOfAny('&', '<') >= 0)
		{
			var sanitized = new StringBuilder(text.Length);
			foreach (var c in text)
			{
				switch (c)
				{
					case '&': sanitized.Append("&amp;"); break;
					case '<': sanitized.Append("&lt;"); break;
					default: sanitized.Append(c); break;
				}
			}
			text = sanitized.ToString();
		}

		// Step 3: Process other inline elements in correct order:
		// Images before links (image syntax contains link syntax)
		// Bold before italic (** before *)
		text = ProcessImages(text, options);
		text = ProcessLinks(text, options);
		text = ProcessBold(text);
		text = ProcessItalic(text);
		text = ProcessStrikethrough(text);
		text = ProcessLineBreaks(text);

		// Step 4: Restore code span placeholders.
		if (codePlaceholders.Count > 0)
		{
			for (int i = 0; i < codePlaceholders.Count; i++)
			{
				text = text.Replace($"\x00CODE{i}\x00", codePlaceholders[i]);
			}
		}

		return text;
	}

	private static string ProcessImages(string text, MarkdownRenderOptions options)
	{
		return ImageRegex().Replace(text, match =>
		{
			var alt = match.Groups[1].Value;
			var url = match.Groups[2].Value;
			var title = match.Groups[3].Value;

			if (options.SanitizeHtml && !IsSafeUrl(url))
			{
				url = "#";
			}
			// Always encode quotes in attribute values to produce valid HTML regardless of SanitizeHtml.
			alt = alt.Replace("\"", "&quot;");
			url = url.Replace("\"", "&quot;");
			title = title.Replace("\"", "&quot;");

			var cssClass = options.ImageCssClass;
			var classAttr = !string.IsNullOrEmpty(cssClass) ? $" class=\"{cssClass}\"" : "";
			var titleAttr = !string.IsNullOrEmpty(title) ? $" title=\"{title}\"" : "";

			return $"<img src=\"{url}\" alt=\"{alt}\"{titleAttr}{classAttr} />";
		});
	}

	private static string ProcessLinks(string text, MarkdownRenderOptions options)
	{
		return LinkRegex().Replace(text, match =>
		{
			var linkText = match.Groups[1].Value;
			var url = match.Groups[2].Value;
			var title = match.Groups[3].Value;

			if (options.SanitizeHtml && !IsSafeUrl(url))
			{
				url = "#";
			}
			// Always encode quotes in attribute values to produce valid HTML regardless of SanitizeHtml.
			url = url.Replace("\"", "&quot;");
			title = title.Replace("\"", "&quot;");

			var titleAttr = !string.IsNullOrEmpty(title) ? $" title=\"{title}\"" : "";

			return $"<a href=\"{url}\"{titleAttr}>{linkText}</a>";
		});
	}

	private static bool IsSafeUrl(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return true;
		}

		// Protocol-relative URLs (//host/path) can be used for scheme-hijacking
		if (url.StartsWith("//", StringComparison.Ordinal))
		{
			return false;
		}

		// Find first colon and first slash to distinguish scheme from path
		var colonIndex = url.IndexOf(':');

		// No colon → no scheme → safe relative URL (e.g., "page.html", "images/photo.png")
		if (colonIndex < 0)
		{
			return true;
		}

		// A slash before the colon means the colon is inside a path segment, not a scheme
		// (e.g., "/path/to/page:anchor" or "./dir/file:name")
		var slashIndex = url.IndexOf('/');
		if (slashIndex >= 0 && slashIndex < colonIndex)
		{
			return true;
		}

		// URL has a scheme — allow only the known-safe ones
		var scheme = url.Substring(0, colonIndex);
		return scheme.Equals("http", StringComparison.OrdinalIgnoreCase)
			|| scheme.Equals("https", StringComparison.OrdinalIgnoreCase)
			|| scheme.Equals("ftp", StringComparison.OrdinalIgnoreCase)
			|| scheme.Equals("ftps", StringComparison.OrdinalIgnoreCase)
			|| scheme.Equals("mailto", StringComparison.OrdinalIgnoreCase)
			|| scheme.Equals("tel", StringComparison.OrdinalIgnoreCase);
	}

	private static string ProcessBold(string text)
	{
		text = BoldAsteriskRegex().Replace(text, "<strong>$1</strong>");
		text = BoldUnderscoreRegex().Replace(text, "<strong>$1</strong>");
		return text;
	}

	private static string ProcessItalic(string text)
	{
		text = ItalicAsteriskRegex().Replace(text, "<em>$1</em>");
		text = ItalicUnderscoreRegex().Replace(text, "<em>$1</em>");
		return text;
	}

	private static string ProcessStrikethrough(string text)
	{
		return StrikethroughRegex().Replace(text, "<s>$1</s>");
	}

	private static string ProcessLineBreaks(string text)
	{
		text = LineBreakRegex().Replace(text, "<br />");
		text = text.Replace("\n", " ");
		return text;
	}
}
