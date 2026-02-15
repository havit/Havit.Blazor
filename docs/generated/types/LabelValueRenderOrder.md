# LabelValueRenderOrder

Specifies the render order of label and value/input.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| LabelValue | 0 | Renders the label first, followed by the value/input (used by the majority of components). |
| ValueLabel | 1 | Renders the value/input first, followed by the label (used by the former HxInputCheckbox). Obsolete, should not be needed anymore. |
| ValueOnly | 2 | Renders only the value/input. The label is not rendered (used by HxAutosuggest{TItem, TValue} with floating label which renders the label internally). |

