# HxRepeater

A data-bound list component that allows custom layout by repeating a specified template for each item displayed in the list. Analogous to the ASP.NET WebForms Repeater control.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Data **[REQUIRED]** | `IEnumerable<TItem>` | The items to be rendered. |
| ItemTemplate **[REQUIRED]** | `RenderFragment<TItem>` | The template that defines how items in the Repeater component are displayed. |
| EmptyTemplate | `RenderFragment` | The template that defines what should be rendered in case of empty Items. |
| FooterTemplate | `RenderFragment` | The template that defines how the footer section of the Repeater component is displayed. Renders only if there are any items to display. |
| HeaderTemplate | `RenderFragment` | The template that defines how the header section of the Repeater component is displayed. Renders only if there are any items to display. |
| NullTemplate | `RenderFragment` | The template that defines what should be rendered in case of Items being null. |

## Available demo samples

- HxRepeater_Demo.razor

