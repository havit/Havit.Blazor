using System.Net;
using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Standalone markdown-to-HTML parser. Converts markdown text to HTML using Bootstrap typography classes.
/// </summary>
internal static class MarkdownParser
{
	/// <summary>
	/// Converts markdown text to HTML.
	/// </summary>
	/// <param name="markdown">The markdown text to convert.</param>
	/// <param name="options">Rendering options.</param>
	/// <returns>HTML string.</returns>
	internal static string ToHtml(string markdown, MarkdownRenderOptions options)
	{
		if (string.IsNullOrEmpty(markdown))
		{
			return string.Empty;
		}

		var lines = NormalizeLineEndings(markdown);
		var blocks = ParseBlocks(lines, options.SanitizeHtml);
		return RenderBlocks(blocks, options);
	}

	private static List<string> NormalizeLineEndings(string text)
	{
		return text.ReplaceLineEndings("\n").Split('\n').ToList();
	}

	#region Block-level parsing

	private static List<MarkdownBlock> ParseBlocks(List<string> lines, bool sanitizeHtml)
	{
		var blocks = new List<MarkdownBlock>();
		int i = 0;

		while (i < lines.Count)
		{
			// Blank line – skip
			if (string.IsNullOrWhiteSpace(lines[i]))
			{
				i++;
				continue;
			}

			// Fenced code block (``` or ~~~)
			if (TryParseCodeBlock(lines, ref i, out var codeBlock))
			{
				blocks.Add(codeBlock);
				continue;
			}

			// Horizontal rule (---, ***, ___)
			if (TryParseHorizontalRule(lines[i], out var hrBlock))
			{
				blocks.Add(hrBlock);
				i++;
				continue;
			}

			// Heading (# ... ######)
			if (TryParseHeading(lines[i], out var headingBlock))
			{
				blocks.Add(headingBlock);
				i++;
				continue;
			}

			// Blockquote (> ...)
			if (lines[i].TrimStart().StartsWith(">"))
			{
				var bqBlock = ParseBlockquote(lines, ref i);
				blocks.Add(bqBlock);
				continue;
			}

			// Table (| ... |)
			if (TryParseTable(lines, ref i, out var tableBlock))
			{
				blocks.Add(tableBlock);
				continue;
			}

			// Unordered list (- , * , + )
			if (IsUnorderedListItem(lines[i]))
			{
				var listBlock = ParseUnorderedList(lines, ref i);
				blocks.Add(listBlock);
				continue;
			}

			// Ordered list (1. , 2. , etc.)
			if (IsOrderedListItem(lines[i]))
			{
				var listBlock = ParseOrderedList(lines, ref i);
				blocks.Add(listBlock);
				continue;
			}

			// Paragraph (default)
			var paraBlock = ParseParagraph(lines, ref i, sanitizeHtml);
			blocks.Add(paraBlock);
		}

		return blocks;
	}

	private static bool TryParseCodeBlock(List<string> lines, ref int i, out MarkdownBlock block)
	{
		block = null;
		var trimmed = lines[i].TrimStart();
		string fence = null;

		if (trimmed.StartsWith("```"))
		{
			fence = "```";
		}
		else if (trimmed.StartsWith("~~~"))
		{
			fence = "~~~";
		}

		if (fence == null)
		{
			return false;
		}

		var language = trimmed.Substring(fence.Length).Trim();
		block = new MarkdownBlock
		{
			Type = MarkdownBlockType.CodeBlock,
			CodeLanguage = string.IsNullOrEmpty(language) ? null : language
		};

		i++; // skip opening fence
		while (i < lines.Count)
		{
			var currentTrimmed = lines[i].TrimStart();
			if (currentTrimmed.StartsWith(fence.Substring(0, 3)) && currentTrimmed.Trim().Length <= fence.Length + 3 && currentTrimmed.Trim().All(c => c == fence[0]))
			{
				i++; // skip closing fence
				break;
			}
			block.Lines.Add(lines[i]);
			i++;
		}

		return true;
	}

	private static bool TryParseHorizontalRule(string line, out MarkdownBlock block)
	{
		block = null;
		var trimmed = line.Trim();

		if (trimmed.Length < 3)
		{
			return false;
		}

		// Must be 3+ of the same char (-, *, _) with optional spaces
		var withoutSpaces = trimmed.Replace(" ", "");
		if (withoutSpaces.Length >= 3 && withoutSpaces.All(c => c == '-'))
		{
			block = new MarkdownBlock { Type = MarkdownBlockType.HorizontalRule };
			return true;
		}
		if (withoutSpaces.Length >= 3 && withoutSpaces.All(c => c == '*'))
		{
			block = new MarkdownBlock { Type = MarkdownBlockType.HorizontalRule };
			return true;
		}
		if (withoutSpaces.Length >= 3 && withoutSpaces.All(c => c == '_'))
		{
			block = new MarkdownBlock { Type = MarkdownBlockType.HorizontalRule };
			return true;
		}

		return false;
	}

	private static bool TryParseHeading(string line, out MarkdownBlock block)
	{
		block = null;
		var trimmed = line.TrimStart();

		if (!trimmed.StartsWith("#"))
		{
			return false;
		}

		int level = 0;
		while (level < trimmed.Length && level < 6 && trimmed[level] == '#')
		{
			level++;
		}

		// Must have a space after # (or be just #'s followed by nothing)
		if (level < trimmed.Length && trimmed[level] != ' ')
		{
			return false;
		}

		var content = trimmed.Substring(level).Trim();
		// Remove trailing #'s (optional closing)
		content = content.TrimEnd('#').TrimEnd();

		block = new MarkdownBlock
		{
			Type = MarkdownBlockType.Heading,
			HeadingLevel = level,
			Lines = { content }
		};
		return true;
	}

	private static MarkdownBlock ParseBlockquote(List<string> lines, ref int i)
	{
		var block = new MarkdownBlock { Type = MarkdownBlockType.Blockquote };

		while (i < lines.Count)
		{
			var trimmed = lines[i].TrimStart();
			if (trimmed.StartsWith(">"))
			{
				// Remove leading > and optional space
				var content = trimmed.Substring(1);
				if (content.StartsWith(" "))
				{
					content = content.Substring(1);
				}
				block.Lines.Add(content);
				i++;
			}
			else if (string.IsNullOrWhiteSpace(lines[i]))
			{
				break;
			}
			else
			{
				// continuation line in same blockquote
				block.Lines.Add(lines[i]);
				i++;
			}
		}

		return block;
	}

	private static bool TryParseTable(List<string> lines, ref int i, out MarkdownBlock block)
	{
		block = null;

		// Need at least header + separator + one data row
		if (i + 1 >= lines.Count)
		{
			return false;
		}

		var headerLine = lines[i].Trim();
		if (!headerLine.StartsWith("|") || !headerLine.EndsWith("|"))
		{
			return false;
		}

		// Check second line is separator (|---|---|)
		var separatorLine = lines[i + 1].Trim();
		if (!IsTableSeparator(separatorLine))
		{
			return false;
		}

		block = new MarkdownBlock { Type = MarkdownBlockType.Table };
		block.Lines.Add(headerLine);
		block.Lines.Add(separatorLine);
		i += 2;

		while (i < lines.Count)
		{
			var trimmed = lines[i].Trim();
			if (trimmed.StartsWith("|"))
			{
				block.Lines.Add(trimmed);
				i++;
			}
			else
			{
				break;
			}
		}

		return true;
	}

	private static bool IsTableSeparator(string line)
	{
		if (!line.StartsWith("|"))
		{
			return false;
		}
		// Each cell should be like ---  or :--- or ---: or :---:
		var cells = SplitTableRow(line);
		return cells.All(c => Regex.IsMatch(c.Trim(), @"^:?-{1,}:?$"));
	}

	private static bool IsUnorderedListItem(string line)
	{
		var trimmed = line.TrimStart();
		return (trimmed.StartsWith("- ") || trimmed.StartsWith("* ") || trimmed.StartsWith("+ "));
	}

	private static bool IsOrderedListItem(string line)
	{
		var trimmed = line.TrimStart();
		return Regex.IsMatch(trimmed, @"^\d+\.\s");
	}

	private static MarkdownBlock ParseUnorderedList(List<string> lines, ref int i)
	{
		var block = new MarkdownBlock
		{
			Type = MarkdownBlockType.UnorderedList,
			Children = new List<MarkdownBlock>()
		};

		while (i < lines.Count && IsUnorderedListItem(lines[i]))
		{
			var trimmed = lines[i].TrimStart();
			var content = trimmed.Substring(2); // skip "- ", "* ", "+ "
			block.Children.Add(new MarkdownBlock
			{
				Type = MarkdownBlockType.Paragraph,
				Lines = { content }
			});
			i++;
		}

		return block;
	}

	private static MarkdownBlock ParseOrderedList(List<string> lines, ref int i)
	{
		var block = new MarkdownBlock
		{
			Type = MarkdownBlockType.OrderedList,
			Children = new List<MarkdownBlock>()
		};

		while (i < lines.Count && IsOrderedListItem(lines[i]))
		{
			var trimmed = lines[i].TrimStart();
			var match = Regex.Match(trimmed, @"^\d+\.\s(.*)$");
			var content = match.Success ? match.Groups[1].Value : trimmed;
			block.Children.Add(new MarkdownBlock
			{
				Type = MarkdownBlockType.Paragraph,
				Lines = { content }
			});
			i++;
		}

		return block;
	}

	private static MarkdownBlock ParseParagraph(List<string> lines, ref int i, bool sanitizeHtml)
	{
		var block = new MarkdownBlock { Type = MarkdownBlockType.Paragraph };

		while (i < lines.Count && !string.IsNullOrWhiteSpace(lines[i]))
		{
			// Stop if next line looks like a different block type
			if (lines[i].TrimStart().StartsWith("#")
				|| lines[i].TrimStart().StartsWith(">")
				|| lines[i].TrimStart().StartsWith("```")
				|| lines[i].TrimStart().StartsWith("~~~")
				|| IsUnorderedListItem(lines[i])
				|| IsOrderedListItem(lines[i]))
			{
				// Check for HR
				if (TryParseHorizontalRule(lines[i], out _))
				{
					break;
				}
				break;
			}

			// Check for HR within paragraph
			if (TryParseHorizontalRule(lines[i], out _))
			{
				break;
			}

			block.Lines.Add(lines[i]);
			i++;
		}

		return block;
	}

	#endregion

	#region Rendering

	private static string RenderBlocks(List<MarkdownBlock> blocks, MarkdownRenderOptions options)
	{
		var sb = new StringBuilder();

		for (int i = 0; i < blocks.Count; i++)
		{
			RenderBlock(sb, blocks[i], options);
		}

		return sb.ToString().TrimEnd('\n');
	}

	private static void RenderBlock(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		switch (block.Type)
		{
			case MarkdownBlockType.Heading:
				RenderHeading(sb, block, options);
				break;
			case MarkdownBlockType.CodeBlock:
				RenderCodeBlock(sb, block, options);
				break;
			case MarkdownBlockType.Blockquote:
				RenderBlockquote(sb, block, options);
				break;
			case MarkdownBlockType.UnorderedList:
				RenderUnorderedList(sb, block, options);
				break;
			case MarkdownBlockType.OrderedList:
				RenderOrderedList(sb, block, options);
				break;
			case MarkdownBlockType.HorizontalRule:
				sb.Append("<hr />");
				break;
			case MarkdownBlockType.Table:
				RenderTable(sb, block, options);
				break;
			case MarkdownBlockType.Paragraph:
				RenderParagraph(sb, block, options);
				break;
		}
	}

	private static void RenderHeading(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var tag = $"h{block.HeadingLevel}";
		var content = MarkdownInlineParser.ProcessInlineElements(JoinLines(block.Lines), options);
		sb.Append($"<{tag}>{content}</{tag}>");
	}

	private static void RenderCodeBlock(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var code = WebUtility.HtmlEncode(string.Join("\n", block.Lines));
		if (!string.IsNullOrEmpty(block.CodeLanguage))
		{
			sb.Append($"<pre><code class=\"language-{WebUtility.HtmlEncode(block.CodeLanguage)}\">{code}</code></pre>");
		}
		else
		{
			sb.Append($"<pre><code>{code}</code></pre>");
		}
	}

	private static void RenderBlockquote(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var cssClass = options.BlockquoteCssClass;
		var classAttr = !string.IsNullOrEmpty(cssClass) ? $" class=\"{cssClass}\"" : "";
		var innerContent = MarkdownInlineParser.ProcessInlineElements(JoinLines(block.Lines), options);
		sb.Append($"<blockquote{classAttr}><p>{innerContent}</p></blockquote>");
	}

	private static void RenderUnorderedList(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		sb.Append("<ul>");
		foreach (var child in block.Children)
		{
			var content = MarkdownInlineParser.ProcessInlineElements(JoinLines(child.Lines), options);
			sb.Append($"<li>{content}</li>");
		}
		sb.Append("</ul>");
	}

	private static void RenderOrderedList(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		sb.Append("<ol>");
		foreach (var child in block.Children)
		{
			var content = MarkdownInlineParser.ProcessInlineElements(JoinLines(child.Lines), options);
			sb.Append($"<li>{content}</li>");
		}
		sb.Append("</ol>");
	}

	private static void RenderTable(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		if (block.Lines.Count < 2)
		{
			return;
		}

		var cssClass = options.TableCssClass;
		var classAttr = !string.IsNullOrEmpty(cssClass) ? $" class=\"{cssClass}\"" : "";

		sb.Append($"<table{classAttr}>");

		// Header row
		var headerCells = SplitTableRow(block.Lines[0]);
		sb.Append("<thead><tr>");
		foreach (var cell in headerCells)
		{
			var content = MarkdownInlineParser.ProcessInlineElements(cell.Trim(), options);
			sb.Append($"<th>{content}</th>");
		}
		sb.Append("</tr></thead>");

		// Data rows (skip line 0 = header, line 1 = separator)
		if (block.Lines.Count > 2)
		{
			sb.Append("<tbody>");
			for (int r = 2; r < block.Lines.Count; r++)
			{
				var cells = SplitTableRow(block.Lines[r]);
				sb.Append("<tr>");
				foreach (var cell in cells)
				{
					var content = MarkdownInlineParser.ProcessInlineElements(cell.Trim(), options);
					sb.Append($"<td>{content}</td>");
				}
				sb.Append("</tr>");
			}
			sb.Append("</tbody>");
		}

		sb.Append("</table>");
	}

	private static void RenderParagraph(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var content = MarkdownInlineParser.ProcessInlineElements(JoinLines(block.Lines), options);
		sb.Append($"<p>{content}</p>");
	}

	private static string JoinLines(List<string> lines)
	{
		return string.Join("\n", lines);
	}

	private static List<string> SplitTableRow(string row)
	{
		// Remove leading/trailing |
		var trimmed = row.Trim();
		if (trimmed.StartsWith("|"))
		{
			trimmed = trimmed.Substring(1);
		}
		if (trimmed.EndsWith("|"))
		{
			trimmed = trimmed.Substring(0, trimmed.Length - 1);
		}
		return trimmed.Split('|').ToList();
	}

	#endregion
}
