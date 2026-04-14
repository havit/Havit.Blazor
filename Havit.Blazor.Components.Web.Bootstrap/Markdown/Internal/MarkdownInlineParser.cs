using System.Net;
using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Processes inline markdown elements (bold, italic, code, links, images, strikethrough, line breaks).
/// </summary>
internal static class MarkdownInlineParser
{
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
		if (options.SanitizeHtml)
		{
			text = text.Replace("&", "&amp;").Replace("<", "&lt;");
		}

		// Process inline elements in correct order:
		// 1. Inline code first (to protect content inside backticks from further processing)
		// 2. Images before links (image syntax contains link syntax)
		// 3. Bold before italic (** before *)
		// 4. Strikethrough
		// 5. Line breaks

		text = ProcessInlineCode(text);
		text = ProcessImages(text, options);
		text = ProcessLinks(text);
		text = ProcessBold(text);
		text = ProcessItalic(text);
		text = ProcessStrikethrough(text);
		text = ProcessLineBreaks(text);

		return text;
	}

	private static string ProcessInlineCode(string text)
	{
		// Match `code` - backtick-delimited inline code
		return Regex.Replace(text, @"`([^`]+)`", match =>
		{
			var code = match.Groups[1].Value;
			return $"<code>{code}</code>";
		});
	}

	private static string ProcessImages(string text, MarkdownRenderOptions options)
	{
		// Match ![alt](url) or ![alt](url "title")
		return Regex.Replace(text, @"!\[([^\]]*)\]\(([^\s\)]+)(?:\s+""([^""]*)"")?\)", match =>
		{
			var alt = match.Groups[1].Value;
			var url = match.Groups[2].Value;
			var title = match.Groups[3].Value;

			var cssClass = options.ImageCssClass;
			var classAttr = !string.IsNullOrEmpty(cssClass) ? $" class=\"{cssClass}\"" : "";
			var titleAttr = !string.IsNullOrEmpty(title) ? $" title=\"{title}\"" : "";

			return $"<img src=\"{url}\" alt=\"{alt}\"{titleAttr}{classAttr} />";
		});
	}

	private static string ProcessLinks(string text)
	{
		// Match [text](url) or [text](url "title")
		return Regex.Replace(text, @"\[([^\]]+)\]\(([^\s\)]+)(?:\s+""([^""]*)"")?\)", match =>
		{
			var linkText = match.Groups[1].Value;
			var url = match.Groups[2].Value;
			var title = match.Groups[3].Value;

			var titleAttr = !string.IsNullOrEmpty(title) ? $" title=\"{title}\"" : "";

			return $"<a href=\"{url}\"{titleAttr}>{linkText}</a>";
		});
	}

	private static string ProcessBold(string text)
	{
		// **bold** and __bold__
		text = Regex.Replace(text, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
		text = Regex.Replace(text, @"__(.+?)__", "<strong>$1</strong>");
		return text;
	}

	private static string ProcessItalic(string text)
	{
		// *italic* and _italic_ (but not inside words for _)
		text = Regex.Replace(text, @"\*(.+?)\*", "<em>$1</em>");
		text = Regex.Replace(text, @"(?<!\w)_(.+?)_(?!\w)", "<em>$1</em>");
		return text;
	}

	private static string ProcessStrikethrough(string text)
	{
		// ~~strikethrough~~
		return Regex.Replace(text, @"~~(.+?)~~", "<s>$1</s>");
	}

	private static string ProcessLineBreaks(string text)
	{
		// Two spaces at end of line followed by newline = <br />
		text = Regex.Replace(text, @"  \n", "<br />");
		// Single newline within a paragraph = space (soft break)
		text = text.Replace("\n", " ");
		return text;
	}
}
