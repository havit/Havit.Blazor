# HxInputText_Demo_SelectOnFocus.razor

```razor
<HxInputText @bind-Value="inputTextContent" Label='SelectOnFocus="false" (default)' SelectOnFocus="false" />
<HxInputText @bind-Value="inputTextContent" Label='SelectOnFocus="true"' SelectOnFocus="true" />

@code {
    private string inputTextContent = "John Doe";
}

```
