namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxInputRangeTests : BunitTestBase
{
	[Fact]
	public void HxInputRange_Render_HasCorrectMinMaxAttributes()
	{
		// Arrange
		int currentValue = 50;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var input = cut.Find("input[type=range]");
		Assert.Equal("0", input.GetAttribute("min"));
		Assert.Equal("100", input.GetAttribute("max"));
	}

	[Fact]
	public void HxInputRange_ChangeValue_UpdatesBoundValue()
	{
		// Arrange
		int currentValue = 25;

		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Act
		cut.Find("input[type=range]").Change("75");

		// Assert
		Assert.Equal(75, currentValue);
	}

	[Fact]
	public void HxInputRange_WithLabel_RendersLabelAndCorrectValueAttribute()
	{
		// Arrange
		int currentValue = 42;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Add(p => p.Label, "Volume")
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		var label = cut.Find("label");
		Assert.Equal("Volume", label.TextContent);

		var input = cut.Find("input[type=range]");
		Assert.Equal("42", input.GetAttribute("value"));
	}

	[Fact]
	public void HxInputRange_Render_WrapsInputInFormRange()
	{
		// Arrange
		int currentValue = 50;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert - Bootstrap 6 structure: .form-range wrapper around input.form-range-input
		var wrapper = cut.Find("div.form-range");
		var input = wrapper.QuerySelector("input.form-range-input");
		Assert.NotNull(input);
		Assert.Equal("range", input.GetAttribute("type"));
	}

	[Fact]
	public void HxInputRange_WithTicks_RendersLinkedDatalist()
	{
		// Arrange
		int currentValue = 50;
		var ticks = new List<InputRangeTick>
		{
			new(0, "Cold"),
			new(50, "Mild"),
			new(100, "Hot"),
		};

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Add(p => p.Ticks, ticks)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert - the input is linked to a datalist with one option per tick
		var input = cut.Find("input[type=range]");
		string listId = input.GetAttribute("list");
		Assert.False(string.IsNullOrEmpty(listId));

		var datalist = cut.Find($"datalist#{listId}");
		var options = datalist.QuerySelectorAll("option");
		Assert.Equal(3, options.Length);
		Assert.Equal("0", options[0].GetAttribute("value"));
		Assert.Equal("Cold", options[0].GetAttribute("label"));
	}

	[Fact]
	public void HxInputRange_WithoutTicks_RendersNoDatalist()
	{
		// Arrange
		int currentValue = 50;

		// Act
		var cut = RenderComponent<HxInputRange<int>>(parameters => parameters
			.Add(p => p.Min, 0)
			.Add(p => p.Max, 100)
			.Bind(p => p.Value, currentValue, newValue => currentValue = newValue));

		// Assert
		Assert.Empty(cut.FindAll("datalist"));
		Assert.Null(cut.Find("input[type=range]").GetAttribute("list"));
	}
}
