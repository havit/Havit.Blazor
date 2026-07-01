using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Forms;

public class HxOtpInputTests : BunitTestBase
{
	private static RenderFragment CreateOtpInputRenderer(TestModel model, Action<RenderTreeBuilder> additionalParameters = null)
	{
		return (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<HxOtpInput>(0);
			builder.AddAttribute(1, "Value", model.Value);
			builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create<string>(model, (value) => { model.Value = value; }));
			builder.AddAttribute(3, "ValueExpression", (Expression<Func<string>>)(() => model.Value));
			additionalParameters?.Invoke(builder);
			builder.CloseComponent();
		};
	}

	[Fact]
	public void HxOtpInput_Render_BasicMarkup()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model));

		// Assert
		var container = cut.Find("div.otp");
		var inputs = cut.FindAll("div.otp input");
		Assert.Equal(6, inputs.Count); // default Length
		Assert.DoesNotContain("input-group", container.ClassList);

		foreach (var input in inputs)
		{
			Assert.Equal("text", input.GetAttribute("type"));
			Assert.Contains("form-control", input.ClassList);
			Assert.Equal("1", input.GetAttribute("maxlength"));
			Assert.Equal("numeric", input.GetAttribute("inputmode"));
			Assert.Equal("\\d*", input.GetAttribute("pattern"));
			Assert.False(input.HasAttribute("disabled"));
		}

		// first input gets autocomplete for browser OTP autofill
		Assert.Equal("one-time-code", inputs[0].GetAttribute("autocomplete"));
		Assert.All(inputs.Skip(1), input => Assert.Equal("off", input.GetAttribute("autocomplete")));

		for (int i = 0; i < inputs.Count; i++)
		{
			Assert.Equal($"Digit {i + 1}", inputs[i].GetAttribute("aria-label"));
		}
	}

	[Fact]
	public void HxOtpInput_Render_RespectsLength()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Length), (int?)4);
		}));

		// Assert
		Assert.Equal(4, cut.FindAll("div.otp input").Count);
	}

	[Fact]
	public void HxOtpInput_Render_Connected_AddsInputGroupClass()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Connected), (bool?)true);
		}));

		// Assert
		Assert.Contains("input-group", cut.Find("div.otp").ClassList);
	}

	[Fact]
	public void HxOtpInput_Render_GroupSize_RendersSeparators()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.GroupSize), (int?)3);
		}));

		// Assert
		var separators = cut.FindAll("div.otp span.otp-separator");
		Assert.Single(separators);
		Assert.Equal("–", separators[0].TextContent);
		Assert.Equal(6, cut.FindAll("div.otp input").Count);
	}

	[Fact]
	public void HxOtpInput_Render_GroupSizeWithConnected_RendersNestedInputGroups()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Connected), (bool?)true);
			builder.AddAttribute(11, nameof(HxOtpInput.GroupSize), (int?)3);
			builder.AddAttribute(12, nameof(HxOtpInput.Separator), "-");
		}));

		// Assert
		var container = cut.Find("div.otp");
		Assert.DoesNotContain("input-group", container.ClassList); // input-group goes to the nested group wrappers
		Assert.Equal(2, cut.FindAll("div.otp div.input-group").Count);
		Assert.Equal(3, cut.FindAll("div.otp div.input-group:first-of-type input").Count);
		Assert.Equal("-", cut.Find("div.otp span.otp-separator").TextContent);
	}

	[Fact]
	public void HxOtpInput_Render_Mask_RendersPasswordInputs()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Mask), (bool?)true);
		}));

		// Assert
		Assert.All(cut.FindAll("div.otp input"), input => Assert.Equal("password", input.GetAttribute("type")));
	}

	[Fact]
	public void HxOtpInput_Render_EnabledFalse_RendersDisabledInputs()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Enabled), (bool?)false);
		}));

		// Assert
		var inputs = cut.FindAll("div.otp input");
		Assert.Equal(6, inputs.Count);
		Assert.All(inputs, input => Assert.True(input.HasAttribute("disabled")));
	}

	[Fact]
	public void HxOtpInput_Render_Value_RendersDigitsToInputs()
	{
		// Arrange
		var model = new TestModel { Value = "123" };

		// Act
		var cut = Render(CreateOtpInputRenderer(model));

		// Assert
		var inputs = cut.FindAll("div.otp input");
		Assert.Equal("1", inputs[0].GetAttribute("value"));
		Assert.Equal("2", inputs[1].GetAttribute("value"));
		Assert.Equal("3", inputs[2].GetAttribute("value"));
		Assert.False(inputs[3].HasAttribute("value"));
	}

	[Fact]
	public void HxOtpInput_Render_Label_PointsToFirstInput()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Label), "Verification code");
		}));

		// Assert
		var label = cut.Find("label");
		Assert.Equal("Verification code", label.TextContent);
		Assert.Equal(label.GetAttribute("for"), cut.FindAll("div.otp input")[0].GetAttribute("id"));
	}

	[Fact]
	public async Task HxOtpInput_HandleJsInput_UpdatesValueBinding()
	{
		// Arrange
		var model = new TestModel();
		var cut = Render(CreateOtpInputRenderer(model));
		var otpInput = cut.FindComponent<HxOtpInput>();

		// Act
		await otpInput.InvokeAsync(() => otpInput.Instance.HandleJsInput("12"));

		// Assert
		Assert.Equal("12", model.Value);
	}

	[Fact]
	public async Task HxOtpInput_HandleJsComplete_UpdatesValueBindingAndRaisesOnCompleted()
	{
		// Arrange
		var model = new TestModel();
		string completedValue = null;

		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.OnCompleted), EventCallback.Factory.Create<string>(new object(), (v) => completedValue = v));
		}));
		var otpInput = cut.FindComponent<HxOtpInput>();

		// Act
		await otpInput.InvokeAsync(() => otpInput.Instance.HandleJsComplete("123456"));

		// Assert
		Assert.Equal("123456", model.Value);
		Assert.Equal("123456", completedValue);
	}

	[Fact]
	public async Task HxOtpInput_RequiredEmpty_RendersIsInvalid()
	{
		// Arrange
		var model = new OtpFormData { Code = "123456" };

		RenderFragment componentRenderer = (RenderTreeBuilder builder) =>
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, "Model", model);
			builder.AddAttribute(2, "ChildContent", (RenderFragment<EditContext>)((context) => (builder2) =>
			{
				builder2.OpenComponent<DataAnnotationsValidator>(0);
				builder2.CloseComponent();

				builder2.OpenComponent<HxOtpInput>(1);
				builder2.AddAttribute(2, "Value", model.Code);
				builder2.AddAttribute(3, "ValueChanged", EventCallback.Factory.Create<string>(this, (value) => { model.Code = value; }));
				builder2.AddAttribute(4, "ValueExpression", (Expression<Func<string>>)(() => model.Code));
				builder2.CloseComponent();
			}));
			builder.CloseComponent();
		};

		var cut = Render(componentRenderer);
		var otpInput = cut.FindComponent<HxOtpInput>();
		Assert.DoesNotContain("is-invalid", cut.Find("div.otp").ClassList);

		// Act
		await otpInput.InvokeAsync(() => otpInput.Instance.HandleJsInput(""));

		// Assert
		Assert.Equal("", model.Code);
		Assert.Contains("is-invalid", cut.Find("div.otp").ClassList);
		Assert.Contains("invalid-feedback", cut.Markup);
		Assert.Contains("Code is required.", cut.Markup);
	}

	private class TestModel
	{
		public string Value { get; set; }
	}

	private class OtpFormData
	{
		[Required(ErrorMessage = "Code is required.")]
		public string Code { get; set; }
	}
}
