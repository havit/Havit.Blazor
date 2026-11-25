using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text.Json;
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
		Assert.DoesNotContain("otp-connected", container.ClassList);

		// a single input backs the whole control (the Bootstrap plugin renders the visual slots)
		var input = Assert.Single(cut.FindAll("div.otp input"));
		Assert.Equal("text", input.GetAttribute("type"));
		Assert.Contains("otp-input", input.ClassList);
		Assert.Equal("6", input.GetAttribute("maxlength")); // default Length
		Assert.Equal("numeric", input.GetAttribute("inputmode"));
		Assert.Equal("[0-9]*", input.GetAttribute("pattern"));
		Assert.Equal("one-time-code", input.GetAttribute("autocomplete"));
		Assert.False(input.HasAttribute("disabled"));
		Assert.False(input.HasAttribute("value"));
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
		Assert.Equal("4", cut.Find("div.otp input").GetAttribute("maxlength"));
		Assert.Equal(4, GetJsInitializeOptions().GetProperty("length").GetInt32());
	}

	[Fact]
	public void HxOtpInput_Render_Connected_AddsOtpConnectedClass()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Connected), (bool?)true);
		}));

		// Assert
		Assert.Contains("otp-connected", cut.Find("div.otp").ClassList);
	}

	[Fact]
	public void HxOtpInput_Render_GroupSize_PassesGroupsToJs()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.GroupSize), (int?)3);
			builder.AddAttribute(11, nameof(HxOtpInput.Separator), "-");
		}));

		// Assert
		Assert.Single(cut.FindAll("div.otp input")); // the separators are rendered by the Bootstrap plugin

		var options = GetJsInitializeOptions();
		Assert.Equal(3, Assert.Single(options.GetProperty("groups").EnumerateArray()).GetInt32());
		Assert.Equal("-", options.GetProperty("separator").GetString());
	}

	[Fact]
	public void HxOtpInput_Render_NullSeparator_PassesEmptySeparatorToJs()
	{
		// Arrange
		var model = new TestModel();

		// Act
		Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Separator), (string)null);
		}));

		// Assert
		Assert.Equal(String.Empty, GetJsInitializeOptions().GetProperty("separator").GetString());
	}

	[Fact]
	public void HxOtpInput_Render_NoGroupSize_PassesNullGroupsToJs()
	{
		// Arrange
		var model = new TestModel();

		// Act
		Render(CreateOtpInputRenderer(model));

		// Assert
		Assert.Equal(JsonValueKind.Null, GetJsInitializeOptions().GetProperty("groups").ValueKind);
	}

	[Fact]
	public void HxOtpInput_Render_Mask_PassesMaskToJsAndKeepsTextInput()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Mask), (bool?)true);
		}));

		// Assert
		Assert.Equal("text", cut.Find("div.otp input").GetAttribute("type")); // the Bootstrap plugin masks the slots, not the input
		Assert.True(GetJsInitializeOptions().GetProperty("mask").GetBoolean());
	}

	[Fact]
	public void HxOtpInput_Render_EnabledFalse_RendersDisabledInput()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Enabled), (bool?)false);
		}));

		// Assert
		Assert.True(cut.Find("div.otp input").HasAttribute("disabled"));
	}

	[Fact]
	public void HxOtpInput_Render_Value_RendersValueToInput()
	{
		// Arrange
		var model = new TestModel { Value = "123" };

		// Act
		var cut = Render(CreateOtpInputRenderer(model));

		// Assert
		Assert.Equal("123", cut.Find("div.otp input").GetAttribute("value"));
	}

	[Fact]
	public void HxOtpInput_Render_Label_PointsToInput()
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
		Assert.Equal(label.GetAttribute("for"), cut.Find("div.otp input").GetAttribute("id"));
	}

	[Fact]
	public void HxOtpInput_Render_Hint_DescribesInput()
	{
		// Arrange
		var model = new TestModel();

		// Act
		var cut = Render(CreateOtpInputRenderer(model, (builder) =>
		{
			builder.AddAttribute(10, nameof(HxOtpInput.Hint), "Enter the code.");
		}));

		// Assert
		var hint = cut.Find(".form-text");
		Assert.Contains(hint.GetAttribute("id"), cut.Find("div.otp input").GetAttribute("aria-describedby"));
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

	private JsonElement GetJsInitializeOptions()
	{
		var initializeInvocation = Assert.Single(JSInterop.Invocations, invocation => invocation.Identifier == "initialize");
		return JsonSerializer.SerializeToElement(initializeInvocation.Arguments[2]);
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
