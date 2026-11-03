# GridStacked

Stacked layout of the `HxGrid` (Bootstrap stacked tables). In the stacked layout, each row is rendered as a vertical block: the first cell acts as a bold title, the remaining cells are displayed as label–value pairs (labels are taken from the column headers, see `HxGridColumn.StackedLabel`).

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| Never | 0 | Never stacked, the grid is always rendered as a regular table. |
| Always | 1 | Always stacked (`table-stacked`). |
| BelowSmall | 2 | Stacked when the grid container is narrower than the "small" breakpoint (`576px`, exclusive; `sm:table-stacked`). |
| BelowMedium | 3 | Stacked when the grid container is narrower than the "medium" breakpoint (`768px`, exclusive; `md:table-stacked`). |
| BelowLarge | 4 | Stacked when the grid container is narrower than the "large" breakpoint (`1024px`, exclusive; `lg:table-stacked`). |
| BelowExtraLarge | 5 | Stacked when the grid container is narrower than the "extra-large" breakpoint (`1280px`, exclusive; `xl:table-stacked`). |
| BelowXxl | 6 | Stacked when the grid container is narrower than the "2xl" breakpoint (`1536px`, exclusive; `2xl:table-stacked`). |

