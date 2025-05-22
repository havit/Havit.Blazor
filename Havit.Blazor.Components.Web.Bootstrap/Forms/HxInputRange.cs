using System.Diagnostics.CodeAnalysis;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Allows the user to select a number in a specified range using a slider.
/// </summary>
/// <typeparam name="TValue">Supported values: <c>byte (Byte), sbyte (SByte), short (Int16), ushort(UInt16), int (Int32), uint(UInt32), long (Int64), ulong(UInt64), float (Single), double (Double) and decimal (Decimal)</c>.</typeparam>
public class HxInputRange<TValue> : HxInputBase<TValue>
{
	// DO NOT FORGET TO MAINTAIN DOCUMENTATION! (documentation comment of this class)
	private static HashSet<Type> s_supportedTypes = new HashSet<Type>
	{
		typeof(byte),
		typeof(sbyte),
		typeof(short),
		typeof(ushort),
		typeof(int),
		typeof(uint),
		typeof(long),
		typeof(ulong),
		typeof(float),
		typeof(double),
		typeof(decimal)
	};

	/// <summary>
	/// Returns <see cref="HxInputRange"/> defaults.
	/// Enables not sharing defaults in descendants with base classes.
	/// Enables having multiple descendants which differ in the default values.
	/// </summary>
	protected override InputRangeSettings GetDefaults() => HxInputRange.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputRange.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputRangeSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> for component descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputRangeSettings GetSettings() => Settings;

	/// <summary>
	/// By default, <code>HxInputRange</code> snaps to integer values. To change this, you can specify a step value.
	/// </summary>
	[Parameter] public TValue Step { get; set; }

	/// <summary>
	/// Minimum value.
	/// </summary>
	[Parameter, EditorRequired] public TValue Min { get; set; }

	/// <summary>
	/// Maximum value.
	/// </summary>
	[Parameter, EditorRequired] public TValue Max { get; set; }

	/// <summary>
	/// Instructs whether the <c>Value</c> is going to be updated <c>oninput</c> (immediately), or <c>onchange</c> (usually <c>onmouseup</c>).<br />
	/// Default is <see cref="BindEvent.OnChange"/>.
	/// </summary>
	[Parameter] public BindEvent? BindEvent { get; set; }
	protected virtual BindEvent BindEventEffective => BindEvent ?? GetSettings()?.BindEvent ?? GetDefaults()?.BindEvent ?? throw new InvalidOperationException(nameof(BindEvent) + " default for " + nameof(HxInputRange<TValue>) + " has to be set.");

	private protected override string CoreInputCssClass => "form-range";

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

	public HxInputRange()
	{
		Type underlyingType = typeof(TValue);

		if (!s_supportedTypes.Contains(underlyingType))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unsupported type {typeof(TValue)}.");
		}
	}

	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenElement(1, "input");

		builder.AddAttribute(2, "class", GetInputCssClassToRender());
		builder.AddAttribute(3, "type", "range");

		builder.AddAttribute(4, "value", BindConverter.FormatValue(Value));
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
		// TODO VSTHRD101 via RuntimeHelpers.CreateInferredBindSetter?
		builder.AddAttribute(5, BindEventEffective.ToEventName(), EventCallback.Factory.CreateBinder(this, async value => await HandleValueChanged(value), Value));
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
		builder.SetUpdatesAttributeName("value");
		builder.AddAttribute(10, "min", Min);
		builder.AddAttribute(11, "max", Max);

		builder.AddAttribute(15, "step", Step);

		builder.AddAttribute(20, "disabled", !EnabledEffective);

		builder.AddAttribute(30, "id", InputId);
		if (!String.IsNullOrEmpty(NameAttributeValue))
		{
			builder.AddAttribute(31, "name", NameAttributeValue);
		}

		// Capture ElementReference to the input to make focusing it programmatically possible.
		builder.AddElementReferenceCapture(40, value => InputElement = value);

		builder.CloseElement();
	}

	protected async Task HandleValueChanged(TValue value)
	{
		Value = value;
		await ValueChanged.InvokeAsync(Value);
	}

	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new InvalidOperationException("HxInputRange displays no text value and receives the initial value as float, therefore, this method must not be called.");
	}

	/// <summary>
	/// Focuses the input range.
	/// </summary>
	public async ValueTask FocusAsync() => await InputElement.FocusOrThrowAsync(this);

}
