# HxDynamicElement

Renders an element with the specified name, attributes, and child content.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IDictionary<string, object>` | A collection of additional attributes that will be applied to the created element. |
| ElementName | `string` | Gets or sets the name of the element to render. |
| ElementRef | `ElementReference` | Element reference. |
| ElementRefChanged | `Action<ElementReference>` | Action (synchronous, not an EventCallback) called when the element's reference is captured. |
| ChildContent | `RenderFragment` | Content of the component. |
| OnClickPreventDefault | `bool` | Prevents the default action for the onclick event. Default is `false`. |
| OnClickStopPropagation | `bool` | Stops onClick event propagation. Default is `false`. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClick | `EventCallback<MouseEventArgs>` | Raised after the element is clicked. |

## Available demo samples

- HxDynamicElement_Demo_BasicUsage.razor

