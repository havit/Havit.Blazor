using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxPasswordStrengthTests : BunitTestBase
{
	[Fact]
	public void HxPasswordStrength_Render_RendersSegmentedMeter()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>();

		// Assert
		var meter = cut.Find(".hx-password-strength > .strength");
		Assert.NotNull(meter);
		Assert.False(meter.HasAttribute("data-bs-strength"));

		var segments = cut.FindAll(".strength > .strength-segment");
		Assert.Equal(4, segments.Count);
		Assert.All(segments, segment => Assert.False(segment.ClassList.Contains("active")));
	}

	[Fact]
	public void HxPasswordStrength_VariantProgressBar_RendersStrengthBarWithoutSegments()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Variant, PasswordStrengthVariant.ProgressBar));

		// Assert
		var meter = cut.Find(".strength-bar");
		Assert.NotNull(meter);
		Assert.Empty(cut.FindAll(".strength-segment"));
	}

	[Fact]
	public void HxPasswordStrength_ChildContent_RendersContentBeforeMeter()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.AddChildContent("<input type=\"password\" id=\"my-password\" />"));

		// Assert
		var wrapper = cut.Find(".hx-password-strength");
		var children = wrapper.Children;

		// the input precedes the meter element
		Assert.Equal(2, children.Length);
		Assert.Equal("my-password", children[0].Id);
		Assert.True(children[1].ClassList.Contains("strength"));
	}

	[Fact]
	public void HxPasswordStrength_ShowText_RendersEmptyTextElement()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.ShowText, true));

		// Assert
		var text = cut.Find("span.strength-text");
		Assert.Equal(string.Empty, text.TextContent);
	}

	[Fact]
	public void HxPasswordStrength_NoShowText_DoesNotRenderTextElement()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>();

		// Assert
		Assert.Empty(cut.FindAll(".strength-text"));
	}

	[Theory]
	[InlineData(PasswordStrengthLevel.Weak, "weak", 1)]
	[InlineData(PasswordStrengthLevel.Fair, "fair", 2)]
	[InlineData(PasswordStrengthLevel.Good, "good", 3)]
	[InlineData(PasswordStrengthLevel.Strong, "strong", 4)]
	public void HxPasswordStrength_StaticStrength_RendersDataAttributeAndActiveSegments(PasswordStrengthLevel level, string expectedAttributeValue, int expectedActiveSegments)
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Strength, level));

		// Assert
		var meter = cut.Find(".strength");
		Assert.Equal(expectedAttributeValue, meter.GetAttribute("data-bs-strength"));

		var activeSegments = cut.FindAll(".strength-segment.active");
		Assert.Equal(expectedActiveSegments, activeSegments.Count);
	}

	[Fact]
	public void HxPasswordStrength_StaticStrengthWithShowText_RendersTextMessage()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Strength, PasswordStrengthLevel.Good)
			.Add(p => p.ShowText, true)
			.Add(p => p.GoodText, "Good password"));

		// Assert
		var text = cut.Find("span.strength-text");
		Assert.Equal("Good password", text.TextContent);
		Assert.Equal("good", text.GetAttribute("data-bs-strength"));
	}

	[Fact]
	public void HxPasswordStrength_StaticStrengthProgressBar_RendersDataAttribute()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Strength, PasswordStrengthLevel.Strong)
			.Add(p => p.Variant, PasswordStrengthVariant.ProgressBar));

		// Assert
		var meter = cut.Find(".strength-bar");
		Assert.Equal("strong", meter.GetAttribute("data-bs-strength"));
	}

	[Fact]
	public void HxPasswordStrength_CssClassAndAdditionalAttributes_RenderedOnMeterElement()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.CssClass, "my-class")
			.AddUnmatched("data-custom", "value"));

		// Assert
		var meter = cut.Find(".strength");
		Assert.True(meter.ClassList.Contains("my-class"));
		Assert.Equal("value", meter.GetAttribute("data-custom"));
	}

	[Fact]
	public void HxPasswordStrength_Settings_AppliedWhenParameterNotSet()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Settings, new PasswordStrengthSettings { Variant = PasswordStrengthVariant.ProgressBar, ShowText = true }));

		// Assert
		Assert.NotNull(cut.Find(".strength-bar"));
		Assert.NotNull(cut.Find(".strength-text"));
	}

	[Fact]
	public void HxPasswordStrength_FirstRender_InitializesJsPlugin()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>();

		// Assert
		var importInvocation = JSInterop.Invocations.Single(i => i.Identifier == "import");
		Assert.Contains("HxPasswordStrength.js", (string)importInvocation.Arguments[0]);
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "initialize");
	}

	[Fact]
	public void HxPasswordStrength_StaticStrength_DoesNotInitializeJsPlugin()
	{
		// Act
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.Strength, PasswordStrengthLevel.Weak));

		// Assert
		Assert.Empty(JSInterop.Invocations);
	}

	[Fact]
	public async Task HxPasswordStrength_DisposeAsync_DisposesJsPlugin()
	{
		// Arrange
		var cut = RenderComponent<HxPasswordStrength>();

		// Act
		await cut.Instance.DisposeAsync();

		// Assert
		Assert.Single(JSInterop.Invocations, i => i.Identifier == "dispose");
	}

	[Fact]
	public void HxPasswordStrength_HandleStrengthChanged_RaisesOnStrengthChanged()
	{
		// Arrange
		PasswordStrengthChangedEventArgs eventArgs = null;
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.OnStrengthChanged, (PasswordStrengthChangedEventArgs args) => eventArgs = args));

		// Act
		cut.InvokeAsync(() => cut.Instance.HandleStrengthChanged("fair", 4));

		// Assert
		Assert.NotNull(eventArgs);
		Assert.Equal(PasswordStrengthLevel.Fair, eventArgs.Strength);
		Assert.Equal(4, eventArgs.Score);
	}

	[Fact]
	public void HxPasswordStrength_HandleStrengthChanged_EmptyPassword_RaisesOnStrengthChangedWithNullStrength()
	{
		// Arrange
		PasswordStrengthChangedEventArgs eventArgs = null;
		var cut = RenderComponent<HxPasswordStrength>(parameters => parameters
			.Add(p => p.OnStrengthChanged, (PasswordStrengthChangedEventArgs args) => eventArgs = args));

		// Act
		cut.InvokeAsync(() => cut.Instance.HandleStrengthChanged(null, 0));

		// Assert
		Assert.NotNull(eventArgs);
		Assert.Null(eventArgs.Strength);
		Assert.Equal(0, eventArgs.Score);
	}

	[Fact]
	public void HxPasswordStrength_InsideEditForm_RendersInputWithoutInterference()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = RenderComponent<EditForm>(parameters => parameters
			.Add(p => p.Model, model)
			.Add(p => p.ChildContent, (RenderFragment<EditContext>)(_ => builder =>
			{
				builder.OpenComponent<HxPasswordStrength>(0);
				builder.AddComponentParameter(1, nameof(HxPasswordStrength.ChildContent), (RenderFragment)(inner =>
				{
					inner.OpenComponent<HxInputText>(0);
					inner.AddComponentParameter(1, nameof(HxInputText.Label), "Password");
					inner.AddComponentParameter(2, nameof(HxInputText.Type), InputType.Password);
					inner.AddComponentParameter(3, nameof(HxInputText.Value), model.Password);
					inner.AddComponentParameter(4, nameof(HxInputText.ValueChanged), EventCallback.Factory.Create<string>(this, (string value) => model.Password = value));
					inner.AddComponentParameter(5, nameof(HxInputText.ValueExpression), (Expression<Func<string>>)(() => model.Password));
					inner.CloseComponent();
				}));
				builder.CloseComponent();
			})));

		// Assert
		var input = cut.Find("input[type=password]");
		Assert.True(input.ClassList.Contains("form-control"));

		// the meter is rendered after the input, within the same wrapper
		Assert.NotNull(cut.Find(".hx-password-strength .strength"));
	}

	private class TestModel
	{
		public string Password { get; set; }
	}
}
