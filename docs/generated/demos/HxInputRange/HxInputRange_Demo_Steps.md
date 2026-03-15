# HxInputRange_Demo_Steps.razor

```razor
<HxInputRange Min="0" Max="100" @bind-Value="value" Label="Example slider" Step="10" />

@value

@code {
    private float value = 50;
}


```
