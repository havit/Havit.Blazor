# HxRedirectTo_Demo_BasicUsage.razor

```razor
<HxButton Text="Redirect" OnClick="() => redirectCondition = true" Color="ThemeColor.Primary" />
@if (redirectCondition)
{
    <HxRedirectTo Uri="put_your_URI_here" />
}

@code
{
    private bool redirectCondition;
}                                                                                   
```
