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

		// When sanitizing, encode only the HTML-dangerous characters (&lt; and &amp;)
		// that don't conflict with markdown syntax. The " and > characters are safe
		// in HTML text context and are needed for markdown link titles / blockquote syntax.
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

		// Process inline elements in correct order:
		// 1. Inline code first (to protect content inside backticks from further processing)
		// 2. Images before links (image syntax contains link syntax)
		// 3. Bold before italic (** before *)
		// 4. Strikethrough
		// 5. Line breaks

		text = ProcessInlineCode(text);
		text = ProcessImages(text, options);
		text = ProcessLinks(text, options);
		text = ProcessBold(text);
		text = ProcessItalic(text);
		text = ProcessStrikethrough(text);
		text = ProcessLineBreaks(text);

		return text;
	}

	private static string ProcessInlineCode(string text)
	{
		return InlineCodeRegex().Replace(text, match =>
		{
			var code = match.Groups[1].Value;
			return $"<code>{code}</code>";
		});
	}

	private static string ProcessImages(string text, MarkdownRenderOptions options)
	{
		return ImageRegex().Replace(text, match =>
		{
			var alt = match.Groups[1].Value;
			var url = match.Groups[2].Value;
			var title = match.Groups[3].Value;

			if (options.SanitizeHtml)
			{
				alt = alt.Replace("\"", "&quot;");
				title = title.Replace("\"", "&quot;");
				if (!IsSafeUrl(url))
				{
					url = "#";
				}
			}

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

			if (options.SanitizeHtml)
			{
				title = title.Replace("\"", "&quot;");
				if (!IsSafeUrl(url))
				{
					url = "#";
				}
			}

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

		// Relative URLs are always safe (but not protocol-relative //host/path)
		if (url[0] == '#' || (url[0] == '/' && (url.Length < 2 || url[1] != '/')))
		{
			return true;
		}

		if (url.StartsWith("./", StringComparison.Ordinal) || url.StartsWith("../", StringComparison.Ordinal))
		{
			return true;
		}

		// Only allow known-safe absolute URL schemes
		return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase)
			|| url.StartsWith("tel:", StringComparison.OrdinalIgnoreCase);
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
