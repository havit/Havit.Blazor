# HxInputDateRange_Demo_DateOnly.razor

```razor
<EditForm Model="model">
	<DataAnnotationsValidator />
	<HxInputDateRange Label="Absence" @bind-Value="model.Period" Settings="settings" />
</EditForm>

<p>Selected: @model.Period.StartDate?.ToShortDateString() – @model.Period.EndDate?.ToShortDateString()</p>

@code {
	private readonly FormModel model = new();
	private readonly InputDateRangeSettings settings = new()
	{
		PredefinedDateRanges =
		[
			new() { Label = "Year 2026", DateRange = new(new DateTime(2026, 1, 1), new DateTime(2026, 12, 31)) }
		]
	};

	private class FormModel
	{
		public DateOnlyRange Period { get; set; }
	}
}

```
