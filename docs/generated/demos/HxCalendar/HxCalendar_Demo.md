# HxCalendar_Demo.razor

```razor
<div class="calendar-demo">
	<HxCalendar @bind-Value="value" />
</div>

@code
{
	private DateTime? value = DateTime.Today;
}

```
