# HxMarkdown – implementační plán

## Problém
Potřebujeme komponentu `HxMarkdown`, která převede Markdown text na HTML a vykreslí ho s využitím Bootstrap typografie. Bez externích závislostí – standalone parser.

## Navržené API

```razor
@* Primární API – Content parametr *@
<HxMarkdown Content="@markdownString" />

@* S CSS třídou *@
<HxMarkdown Content="@markdownString" CssClass="my-custom-class" />
```

### Proč nedoporučuji ChildContent pro markdown
`<HxMarkdown>` **text** `</HxMarkdown>` by vyžadovalo extrakci stringu z `RenderFragment`, což je v Blazoru netriviální a křehké. Navíc Razor syntaxe koliduje s markdown (`@` znaky, `<` tagy vyžadují escaping). Parametr `Content` je čistější a spolehlivější.

### Wrapper element
**Doporučuji volitelný wrapper** – komponenta bude standardně renderovat bez wrapper elementu (holý HTML výstup z markdown). Pokud uživatel zadá `CssClass` nebo `AdditionalAttributes`, automaticky se obalí do `<div>`. Důvody:
- Bez wrapperu je výstup čistší a lépe se integruje do existujícího layoutu
- Wrapper je potřeba jen když chceme na výstup aplikovat CSS třídu nebo atributy
- Implementace přes `BuildRenderTree` umožňuje podmíněné renderování wrapperu

### Parametry komponenty

| Parametr | Typ | Default | Popis |
|----------|-----|---------|-------|
| `Content` | `string` | `null` | Markdown text k vykreslení |
| `SanitizeHtml` | `bool?` | `true` | Pokud `true`, HTML tagy ve vstupu budou escaped. Pokud `false`, raw HTML ve vstupu projde do výstupu. |
| `TableCssClass` | `string` | `null` | CSS třída pro `<table>` (default z Settings: `"table"`) |
| `BlockquoteCssClass` | `string` | `null` | CSS třída pro `<blockquote>` (default z Settings: `"blockquote"`) |
| `ImageCssClass` | `string` | `null` | CSS třída pro `<img>` (default z Settings: `"img-fluid"`) |
| `CssClass` | `string` | `null` | CSS třída pro wrapper `<div>` (volitelný wrapper) |
| `AdditionalAttributes` | `Dictionary<string, object>` | `null` | Další HTML atributy (aktivuje wrapper) |
| `Settings` | `MarkdownSettings` | `null` | Instance-level nastavení |

### MarkdownSettings
```csharp
public class MarkdownSettings
{
    public bool? SanitizeHtml { get; set; }
    public string CssClass { get; set; }
    public string TableCssClass { get; set; }          // default "table"
    public string BlockquoteCssClass { get; set; }     // default "blockquote"
    public string ImageCssClass { get; set; }           // default "img-fluid"
}
```

### Statické defaults
```csharp
public static MarkdownSettings Defaults { get; set; }

// Globální konfigurace:
// HxMarkdown.Defaults.SanitizeHtml = false;  // povolí raw HTML globálně
```

## Architektura

### Standalone Markdown Parser
Dvou-fázový parser (block-level → inline-level), inspirovaný CommonMark specifikací:

**Block-level elementy (fáze 1):**
1. Headings (`# ` až `###### `)
2. Code blocks (``` fenced code blocks ```)
3. Blockquotes (`> `)
4. Unordered lists (`- `, `* `, `+ `)
5. Ordered lists (`1. `)
6. Horizontal rules (`---`, `***`, `___`)
7. **Tabulky** (`| col1 | col2 |` s `|---|---|` oddělovačem)
8. Paragraphs (vše ostatní)

**Inline elementy (fáze 2):**
1. Bold (`**text**` a `__text__`)
2. Italic (`*text*` a `_text_`)
3. **Strikethrough** (`~~text~~`)
4. Inline code (`` `code` ``)
5. Links (`[text](url)`)
6. Images (`![alt](url)`)
7. Line breaks (dvojitá mezera na konci řádku / `<br>`)

### Bezpečnost
- Vstupní text je standardně HTML-encoded před zpracováním (`SanitizeHtml = true`)
- Markdown syntaxe generuje bezpečné HTML tagy
- Parametr `SanitizeHtml` (default `true`) řídí, zda se HTML tagy ve vstupu escapují:
  - `true` (default): HTML tagy escaped → bezpečný výstup
  - `false`: raw HTML ve vstupu projde do výstupu (pro důvěryhodný obsah)
- Konfigurovatelné globálně přes `HxMarkdown.Defaults.SanitizeHtml = false`

### Bootstrap typografie mapování
| Markdown | HTML výstup | Bootstrap styl |
|----------|------------|----------------|
| `# Heading` | `<h1>` - `<h6>` | Nativní Bootstrap headings |
| `**bold**` | `<strong>` | Nativní |
| `*italic*` | `<em>` | Nativní |
| `` `code` `` | `<code>` | Bootstrap `<code>` |
| Code block | `<pre><code>` | Bootstrap `<pre>` |
| `> quote` | `<blockquote class="blockquote">` | Bootstrap blockquote |
| Lists | `<ul>/<ol>` + `<li>` | Nativní |
| `~~strike~~` | `<s>` | Nativní |
| Table | `<table class="table">` | Bootstrap tabulka |
| `---` | `<hr>` | Nativní |
| `[text](url)` | `<a href="url">` | Nativní |
| `![alt](url)` | `<img class="img-fluid">` | Bootstrap responsive image |
| Paragraph | `<p>` | Nativní |

## Struktura souborů

```
Havit.Blazor.Components.Web.Bootstrap/
  Markdown/
    HxMarkdown.razor             # Šablona (nebo BuildRenderTree)
    HxMarkdown.razor.cs          # Code-behind s parametry a logikou
    MarkdownSettings.cs           # Settings třída
    Internal/
      MarkdownParser.cs           # Hlavní parser (block-level)
      MarkdownInlineParser.cs     # Inline parser
      MarkdownBlock.cs            # Model pro block elementy

Havit.Blazor.Components.Web.Bootstrap.Tests/
  Markdown/
    HxMarkdownTests.cs            # Testy komponenty
    MarkdownParserTests.cs        # Unit testy parseru
```

## Implementační kroky

### 1. Markdown parser – block-level
Implementace parseru pro block-level elementy (headings, code blocks, paragraphs, lists, blockquotes, HR).

### 2. Markdown parser – inline
Implementace inline parseru (bold, italic, code, links, images, line breaks).

### 3. Komponenta HxMarkdown
Blazor komponenta s parametry, settings pattern, volitelný wrapper, `BuildRenderTree`.

### 4. Unit testy parseru
Pokrytí všech markdown elementů unit testy.

### 5. Testy komponenty
Bunit testy pro renderování komponenty.

### 6. Dokumentace
Dokumentační stránka s demo ukázkami.

## Poznámky a rozhodnutí
- **Bez externích závislostí** – parser je součástí knihovny
- **SanitizeHtml** – volitelný parametr (default `true`), konfigurovatelný globálně přes `MarkdownSettings`. Pokud `false`, raw HTML ve vstupu projde do výstupu.
- **Volitelný wrapper** – čistý výstup bez zbytečného `<div>`
- Parser bude `internal static` třída – není součástí veřejného API
- Nested seznamy (vnořené odrážky) v první verzi nepodporujeme pro jednoduchost
