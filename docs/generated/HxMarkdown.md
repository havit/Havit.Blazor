# HxMarkdown

Renders Markdown content as HTML using Bootstrap typography.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Content **[REQUIRED]** | `string` | Markdown text to render as HTML. |
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the wrapper `<div>` element. When set, the output is wrapped in a `<div>`. |
| BlockquoteCssClass | `string` | CSS class for `<blockquote>` elements rendered from markdown blockquotes. Default is `"blockquote"` (Bootstrap blockquote). |
| CssClass | `string` | Any additional CSS class to apply to the wrapper element. When set (or when `AdditionalAttributes` is used), the output is wrapped in a `<div>`. |
| ImageCssClass | `string` | CSS class for `<img>` elements rendered from markdown images. Default is `"img-fluid"` (Bootstrap responsive image). |
| SanitizeHtml | `bool?` | When `true` (default), HTML tags in the input are escaped. When `false`, raw HTML in the input passes through to the output (use only with trusted content). |
| Settings | `MarkdownSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| TableCssClass | `string` | CSS class for `<table>` elements rendered from markdown tables. Default is `"table"` (Bootstrap table). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `MarkdownSettings` | Application-wide defaults for `HxMarkdown` and derived components. |

## Available demo samples

- HxMarkdown_Demo_Basic.razor
- HxMarkdown_Demo_CodeBlocks.razor
- HxMarkdown_Demo_CustomCss.razor
- HxMarkdown_Demo_RichContent.razor
- HxMarkdown_Demo_Sanitization.razor
- HxMarkdown_Demo_Tables.razor
- HxMarkdown_Demo_Wrapper.razor

