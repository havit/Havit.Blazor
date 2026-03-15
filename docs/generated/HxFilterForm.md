# HxFilterForm

Edit form derived from HxModelEditForm with support for chip generators.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| ChildContent | `RenderFragment<TModel>` | Content of the component. |
| Id | `string` |  |
| Model | `TModel` |  |
| ModelChanged | `EventCallback<TModel>` |  |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnChipsUpdated | `EventCallback<ChipItem<>>` |  |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| RemoveChipAsync(ChipItem chipToRemove) | `Task` | Tries to remove a chip. Execution is postponed to OnAfterRender, so this method cannot have a return value. |
| UpdateModelAsync() | `Task` |  |

## Available demo samples

- HxFilterForm_Demo.razor

