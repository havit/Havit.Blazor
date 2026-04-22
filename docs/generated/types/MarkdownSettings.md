# MarkdownSettings

Settings for the `HxMarkdown` and derived components.

## Properties

| Name | Type | Description |
|------|------|-------------|
| BlockquoteCssClass | `string` | CSS class for `<blockquote>` elements rendered from markdown blockquotes. Default is `"blockquote"` (Bootstrap blockquote). |
| CssClass | `string` | CSS class for the wrapper `<div>` element. |
| ImageCssClass | `string` | CSS class for `<img>` elements rendered from markdown images. Default is `"img-fluid"` (Bootstrap responsive image). |
| SanitizeHtml | `bool?` | When `true` (default), HTML tags in the input are escaped. When `false`, raw HTML in the input passes through to the output (use only with trusted content). |
| TableCssClass | `string` | CSS class for `<table>` elements rendered from markdown tables. Default is `"table"` (Bootstrap table). |

