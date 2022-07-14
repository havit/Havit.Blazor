using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Allows the user to select a number in a specified range using a slider.
/// </summary>
public class HxInputRange : HxInputBase<float>
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxInputRange"/>.
	/// </summary>
	public static InputRangeSettings Defaults { get; set; }

	/// <summary>
	/// Return <see cref="HxInputRange"/> defaults.
	/// Enables to not share defaults in descandants with base classes.
	/// Enables to have multiple descendants which differs in the default values.
	/// </summary>
	protected virtual InputRangeSettings GetDefaults() => HxInputRange.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
	/// </summary>
	[Parameter] public InputRangeSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> for components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual InputRangeSettings GetSettings() => this.Settings;

	/// <summary>
	/// Minimum value.
	/// </summary>
	[Parameter] public float? Min { get; set; }
	protected float MinEffective => Min ?? GetSettings()?.Min ?? GetDefaults()?.Min ?? throw new InvalidOperationException(nameof(Min) + " default for " + nameof(HxInputRange) + " has to be set.");

	/// <summary>
	/// Maximum value.
	/// </summary>
	[Parameter] public float? Max { get; set; }
	protected float MaxEffective => Max ?? GetSettings()?.Max ?? GetDefaults()?.Max ?? throw new InvalidOperationException(nameof(Max) + " default for " + nameof(HxInputRange) + " has to be set");

	/// <summary>
	/// By default, <code>HxInputRange</code> snaps to integer values. To change this, you can specify a step value.
	/// </summary>
	[Parameter] public float? Step { get; set; }
	protected virtual float? StepEffective => Step ?? GetDefaults()?.Step ?? GetDefaults()?.Step;

	/// <summary>
	/// Instructs whether the <c>Value</c> is going to be updated <c>oninput</c> (immediately), or <c>onchange</c> (usually <c>onmouseup</c>).
	/// </summary>
	[Parameter] public BindEvent? BindEvent { get; set; }
	protected virtual BindEvent BindEventEffective => BindEvent ?? GetSettings()?.BindEvent ?? GetDefaults()?.BindEvent ?? throw new InvalidOperationException(nameof(BindEvent) + " default for " + nameof(HxInputRange) + " has to be set.");

	private protected override string CoreInputCssClass => "form-range";

	static HxInputRange()
	{
		Defaults = new InputRangeSettings()
		{
			Min = 0,
			Max = 100,
			BindEvent = Bootstrap.BindEvent.OnChange
		};
	}

	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenElement(1, "input");

		builder.AddAttribute(2, "class", GetInputCssClassToRender());
		builder.AddAttribute(3, "type", "range");

		builder.AddAttribute(4, "value", BindConverter.FormatValue(Value));
		builder.AddAttribute(5, BindEventEffective.ToEventName(), EventCallback.Factory.CreateBinder(this, async value => await HandleValueChanged(value), Value));

		builder.AddAttribute(10, "min", MinEffective);
		builder.AddAttribute(11, "max", MaxEffective);

		builder.AddAttribute(15, "step", StepEffective);

		builder.AddAttribute(20, "disabled", !EnabledEffective);

		builder.AddAttribute(30, "id", InputId);

		// Capture ElementReference to the input to make focusing it programmatically possible.
		builder.AddElementReferenceCapture(40, value => InputElement = value);

		builder.CloseElement();
	}

	protected async Task HandleValueChanged(float value)
	{
		Value = value;
		await ValueChanged.InvokeAsync(Value);
	}

	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out float result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new InvalidOperationException("HxInputRange displays no text value and receives the initial value as float, therefore, this method must not be called.");
	}
}
