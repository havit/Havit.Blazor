using System.Net;
using System.Text.RegularExpressions;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Standalone markdown-to-HTML parser. Converts markdown text to HTML using Bootstrap typography classes.
/// </summary>
internal static partial class MarkdownParser
{
	[GeneratedRegex(@"^:?-{1,}:?$")]
	private static partial Regex TableSeparatorCellRegex();

	[GeneratedRegex(@"^\d+\.\s")]
	private static partial Regex OrderedListItemRegex();

	[GeneratedRegex(@"^\d+\.\s(.*)$")]
	private static partial Regex OrderedListItemContentRegex();
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
		var blocks = ParseBlocks(lines);
		return RenderBlocks(blocks, options);
	}

	private static string[] NormalizeLineEndings(string text)
	{
		return text.ReplaceLineEndings("\n").Split('\n');
	}

	#region Block-level parsing

	private static List<MarkdownBlock> ParseBlocks(string[] lines)
	{
		var blocks = new List<MarkdownBlock>();
		int i = 0;

		while (i < lines.Length)
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
			if (lines[i].AsSpan().TrimStart().StartsWith(">"))
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
			var paraBlock = ParseParagraph(lines, ref i);
			blocks.Add(paraBlock);
		}

		return blocks;
	}

	private static bool TryParseCodeBlock(string[] lines, ref int i, out MarkdownBlock block)
	{
		block = null;
		var trimmed = lines[i].TrimStart();
		char fenceChar;

		if (trimmed.StartsWith("```"))
		{
			fenceChar = '`';
		}
		else if (trimmed.StartsWith("~~~"))
		{
			fenceChar = '~';
		}
		else
		{
			return false;
		}

		// Count the actual opening fence length (CommonMark: >= 3 of the same char)
		int fenceLength = 0;
		while (fenceLength < trimmed.Length && trimmed[fenceLength] == fenceChar)
		{
			fenceLength++;
		}

		var language = trimmed.Substring(fenceLength).Trim();
		block = new MarkdownBlock
		{
			Type = MarkdownBlockType.CodeBlock,
			CodeLanguage = string.IsNullOrEmpty(language) ? null : language
		};

		i++; // skip opening fence
		while (i < lines.Length)
		{
			var currentTrimmed = lines[i].Trim();
			// Closing fence: same char only, length >= opening fence length (CommonMark)
			if (currentTrimmed.Length >= fenceLength && currentTrimmed.All(c => c == fenceChar))
			{
				i++; // skip closing fence
				break;
			}
			block.Lines.Add(lines[i]);
			i++;
		}

		return true;
	}

	private static bool TryParseHorizontalRule(ReadOnlySpan<char> line, out MarkdownBlock block)
	{
		block = null;
		var trimmed = line.Trim();

		if (trimmed.Length < 3)
		{
			return false;
		}

		// Must be 3+ of the same char (-, *, _) with optional spaces between them
		char ruleChar = '\0';
		int charCount = 0;
		foreach (var c in trimmed)
		{
			if (c == ' ')
			{
				continue;
			}
			if (ruleChar == '\0')
			{
				if (c is not ('-' or '*' or '_'))
				{
					return false;
				}
				ruleChar = c;
			}
			if (c != ruleChar)
			{
				return false;
			}
			charCount++;
		}

		if (charCount >= 3)
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

	private static MarkdownBlock ParseBlockquote(string[] lines, ref int i)
	{
		var block = new MarkdownBlock { Type = MarkdownBlockType.Blockquote };

		while (i < lines.Length)
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

	private static bool TryParseTable(string[] lines, ref int i, out MarkdownBlock block)
	{
		block = null;

		// Need at least header + separator; data rows are optional
		if (i + 1 >= lines.Length)
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

		while (i < lines.Length)
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
		return cells.All(c => TableSeparatorCellRegex().IsMatch(c.Trim()));
	}

	private static bool IsUnorderedListItem(ReadOnlySpan<char> line)
	{
		var trimmed = line.TrimStart();
		return trimmed.StartsWith("- ") || trimmed.StartsWith("* ") || trimmed.StartsWith("+ ");
	}

	private static bool IsHeadingStart(ReadOnlySpan<char> line)
	{
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
		// Valid heading: # followed by space or end of line
		return level >= trimmed.Length || trimmed[level] == ' ';
	}

	private static bool IsOrderedListItem(ReadOnlySpan<char> line)
	{
		var trimmed = line.TrimStart();
		return OrderedListItemRegex().IsMatch(trimmed);
	}

	private static MarkdownBlock ParseUnorderedList(string[] lines, ref int i)
	{
		var block = new MarkdownBlock
		{
			Type = MarkdownBlockType.UnorderedList,
			Children = new List<MarkdownBlock>()
		};

		while (i < lines.Length && IsUnorderedListItem(lines[i]))
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

	private static MarkdownBlock ParseOrderedList(string[] lines, ref int i)
	{
		var block = new MarkdownBlock
		{
			Type = MarkdownBlockType.OrderedList,
			Children = new List<MarkdownBlock>()
		};

		while (i < lines.Length && IsOrderedListItem(lines[i]))
		{
			var trimmed = lines[i].TrimStart();
			var match = OrderedListItemContentRegex().Match(trimmed);
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

	private static MarkdownBlock ParseParagraph(string[] lines, ref int i)
	{
		var block = new MarkdownBlock { Type = MarkdownBlockType.Paragraph };

		// The first line is guaranteed by ParseBlocks to not match any other block parser,
		// so we always consume it. Break checks only apply to continuation lines (2nd+).
		bool isFirstLine = true;

		while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
		{
			if (!isFirstLine)
			{
				var trimmedSpan = lines[i].AsSpan().TrimStart();

				// Stop if continuation line looks like a different block type
				if (trimmedSpan.StartsWith(">")
					|| trimmedSpan.StartsWith("```")
					|| trimmedSpan.StartsWith("~~~")
					|| IsUnorderedListItem(trimmedSpan)
					|| IsOrderedListItem(trimmedSpan)
					|| IsHeadingStart(trimmedSpan))
				{
					break;
				}

				// Check for HR within paragraph
				if (TryParseHorizontalRule(lines[i], out _))
				{
					break;
				}

				// Check for table start (| ... | followed by separator line).
				// Must require trailing | to match TryParseTable's header-line check.
				// Use TrimEnd() so trailing whitespace doesn't prevent the match (TryParseTable uses Trim()).
				if (trimmedSpan.StartsWith("|") && trimmedSpan.TrimEnd().EndsWith("|") && (i + 1 < lines.Length) && IsTableSeparator(lines[i + 1].Trim()))
				{
					break;
				}
			}

			block.Lines.Add(lines[i]);
			i++;
			isFirstLine = false;
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
		sb.Append("<pre><code");
		if (!string.IsNullOrEmpty(block.CodeLanguage))
		{
			sb.Append(" class=\"language-").Append(WebUtility.HtmlEncode(block.CodeLanguage)).Append('"');
		}
		sb.Append('>');

		for (int i = 0; i < block.Lines.Count; i++)
		{
			if (i > 0)
			{
				sb.Append('\n');
			}
			sb.Append(WebUtility.HtmlEncode(block.Lines[i]));
		}

		sb.Append("</code></pre>");
	}

	private static void RenderBlockquote(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var cssClass = options.BlockquoteCssClass;
		var classAttr = !string.IsNullOrEmpty(cssClass) ? $" class=\"{cssClass}\"" : "";
		// Recursively parse the blockquote's inner lines so nested blockquotes (>> ...) are rendered correctly.
		var innerBlocks = ParseBlocks(block.Lines.ToArray());
		var innerHtml = RenderBlocks(innerBlocks, options);
		sb.Append($"<blockquote{classAttr}>{innerHtml}</blockquote>");
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

		// Parse column alignments from separator row (line 1)
		var alignments = GetColumnAlignments(block.Lines[1]);

		// Header row
		var headerCells = SplitTableRow(block.Lines[0]);
		sb.Append("<thead><tr>");
		for (int c = 0; c < headerCells.Length; c++)
		{
			var content = MarkdownInlineParser.ProcessInlineElements(headerCells[c].Trim(), options);
			var align = c < alignments.Length ? alignments[c] : null;
			var styleAttr = align != null ? $" style=\"text-align:{align}\"" : "";
			sb.Append($"<th{styleAttr}>{content}</th>");
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
				for (int c = 0; c < cells.Length; c++)
				{
					var content = MarkdownInlineParser.ProcessInlineElements(cells[c].Trim(), options);
					var align = c < alignments.Length ? alignments[c] : null;
					var styleAttr = align != null ? $" style=\"text-align:{align}\"" : "";
					sb.Append($"<td{styleAttr}>{content}</td>");
				}
				sb.Append("</tr>");
			}
			sb.Append("</tbody>");
		}

		sb.Append("</table>");
	}

	/// <summary>
	/// Parses the separator row and returns the text-align value for each column, or null when no alignment is specified.
	/// </summary>
	private static string[] GetColumnAlignments(string separatorLine)
	{
		var cells = SplitTableRow(separatorLine);
		var alignments = new string[cells.Length];
		for (int i = 0; i < cells.Length; i++)
		{
			var cell = cells[i].Trim();
			var left = cell.StartsWith(':');
			var right = cell.EndsWith(':');
			if (left && right)
			{
				alignments[i] = "center";
			}
			else if (right)
			{
				alignments[i] = "right";
			}
			else if (left)
			{
				alignments[i] = "left";
			}
			else
			{
				alignments[i] = null;
			}
		}
		return alignments;
	}

	private static void RenderParagraph(StringBuilder sb, MarkdownBlock block, MarkdownRenderOptions options)
	{
		var content = MarkdownInlineParser.ProcessInlineElements(JoinLines(block.Lines), options);
		sb.Append($"<p>{content}</p>");
	}

	private static string JoinLines(List<string> lines)
	{
		if (lines.Count == 0)
		{
			return string.Empty;
		}
		if (lines.Count == 1)
		{
			return lines[0];
		}
		return string.Join('\n', lines);
	}

	private static string[] SplitTableRow(string row)
	{
		// Remove leading/trailing |
		var span = row.AsSpan().Trim();
		if (span.Length > 0 && span[0] == '|')
		{
			span = span.Slice(1);
		}
		if (span.Length > 0 && span[span.Length - 1] == '|')
		{
			span = span.Slice(0, span.Length - 1);
		}
		return span.ToString().Split('|');
	}

	#endregion
}
