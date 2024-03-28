using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Numeric input.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputNumber">https://havit.blazor.eu/components/HxInputNumber</see>
/// </summary>
/// <remarks>
/// Defaults located in separate non-generic type <see cref="HxInputNumber"/>.
/// </remarks>
/// <typeparam name="TValue">Supported values: <c>byte (Byte), sbyte (SByte), short (Int16), ushort(UInt16), int (Int32), uint(UInt32), long (Int64), ulong(UInt64), float (Single), double (Double) and decimal (Decimal)</c>.</typeparam>
public class HxInputNumber<TValue> : HxInputBaseWithInputGroups<TValue>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
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
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected override InputNumberSettings GetDefaults() => HxInputNumber.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputNumber.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputNumberSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputNumberSettings GetSettings() => Settings;


	/// <summary>
	/// Gets or sets the error message used when displaying an a parsing error.
	/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
	/// </summary>
	[Parameter] public string ParsingErrorMessage { get; set; }

	/// <summary>
	/// Hint to browsers as to the type of virtual keyboard configuration to use when editing.<br/>
	/// If not set (neither with <see cref="Settings"/> nor <see cref="HxInputNumber.Defaults"/>, i.e. <c>null</c>),
	/// the <see cref="InputMode.Numeric"/>	will be used for <see cref="Decimals"/> equal to <c>0</c>.
	/// </summary>
	/// <remarks>
	/// We cannot set <see cref="InputMode.Decimal"/> for <see cref="Decimals"/> greater that <c>0</c>
	/// as the users with keyboard locale not matching the application locale won't be able to enter decimal point
	/// (is <kbd>,</kbd> in some locales and <kbd>.</kbd> in others).<br />
	/// Feel free to set the InputMode on your own as needed.
	/// </remarks>
	[Parameter] public InputMode? InputMode { get; set; }
	protected InputMode? InputModeEffective => InputMode ?? GetSettings()?.InputMode ?? GetDefaults()?.InputMode;

	/// <summary>
	/// Placeholder for the input.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxInputNumber) + " has to be set.");
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;

	/// <inheritdoc cref="Bootstrap.LabelType" />
	[Parameter] public LabelType? LabelType { get; set; }

#pragma warning disable BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxInputNumber<TValue>.Decimals' should be auto property
	/// <summary>
	/// Gets or sets the number of decimal digits.
	/// Can be used only for floating point types, for integer types throws exception (for values other than 0).
	/// When not set, 2 decimal digits are used.
	/// </summary>
	[Parameter]
	public int? Decimals
	{
		get
		{
			return _decimals;
		}
		set
		{
			// TODO Move validation to OnParametersSet
			if ((value != 0) && IsTValueIntegerType)
			{
				throw new InvalidOperationException($"{nameof(Decimals)} can be set only on floating point types (not on integer types).");
			}
			_decimals = value;
		}
	}
	private int? _decimals;
#pragma warning restore BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxInputNumber<TValue>.Decimals' should be auto property

	/// <summary>
	/// Gets effective value for Decimals (when not set gets 0 for integer types and 2 for floating point types.
	/// </summary>
	protected virtual int DecimalsEffective => Decimals ?? (IsTValueIntegerType ? 0 : 2);

	[Inject] private protected IStringLocalizer<HxInputNumber> StringLocalizer { get; set; }

	/// <summary>
	/// Returns <c>true</c> for integer types (<c>false</c> for floating point types).
	/// </summary>
	private bool IsTValueIntegerType
	{
		get
		{
			Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
			return (underlyingType == typeof(byte))
				|| (underlyingType == typeof(sbyte))
				|| (underlyingType == typeof(short))
				|| (underlyingType == typeof(ushort))
				|| (underlyingType == typeof(int))
				|| (underlyingType == typeof(uint))
				|| (underlyingType == typeof(long))
				|| (underlyingType == typeof(ulong));
		}
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	public HxInputNumber()
	{
		Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
		if (!s_supportedTypes.Contains(underlyingType))
		{
			throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
		}
	}

	protected bool forceRenderValue = false;
	private int _valueSequenceOffset = 0;

	/// <inheritdoc />
	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "input");
		BuildRenderInput_AddCommonAttributes(builder, "text");

		var inputMode = InputModeEffective;
		if ((inputMode is null) && (DecimalsEffective == 0))
		{
			inputMode = Web.InputMode.Numeric;
		}
		if (inputMode is not null)
		{
			builder.AddAttribute(1001, "inputmode", inputMode.Value.ToString("f").ToLower());
		}

		builder.AddAttribute(1002, "onfocus", "this.select();"); // source: https://stackoverflow.com/questions/4067469/selecting-all-text-in-html-text-input-when-clicked
		builder.AddAttribute(1003, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
#if NET8_0_OR_GREATER
		builder.SetUpdatesAttributeName("value");
#endif

		// Normalization of pasted value (applied only if the pasted value differs from the normalized value)
		// - If we cancel the original paste event, Blazor does not recognize the change, so we fire change event manually.
		// - This is kind of hack and causes the input to behave slightly differently when normalizing the value (the value is accepted immediately, not after the user leaves the input)
		// - If this turns out to be a problem, we will have to implement the normalization in the Blazor-handled @onpaste event
		builder.AddAttribute(1004, "onpaste", @"var clipboardValue = event.clipboardData.getData('text/plain'); var normalizedValue = clipboardValue.replace(/[^\d.,\-eE]/g, ''); if (+clipboardValue != +normalizedValue) { this.value = normalizedValue; this.dispatchEvent(new Event('change')); return false; }");
#if NET8_0_OR_GREATER
		builder.SetUpdatesAttributeName("value");
#endif

		builder.AddEventStopPropagationAttribute(1004, "onclick", true);

		// The counting of sequence values violates all general recommendations.
		// We want the value of HxInputNumber to be updated (re-rendered) when the user input changes, even if FormatValueAsString(Value) hasn't changed.
		// For instance, if a value like "1.00" is displayed and a user modifies it to "1.0", FormatValueAsString(Value) won't change,
		// and the attribute won't be re-rendered, so the user input stays at "1.0".
		// To address this issue, we use sequence values starting from 1006. This forces Blazor to update the value anyway (due to the sequence change).
		// However, we adjust the sequence only if we want to enforce the re-rendering. Otherwise, the component would update continuously.
		checked
		{
			if (forceRenderValue)
			{
				_valueSequenceOffset++;
				forceRenderValue = false;
			}
			builder.AddAttribute(1006 + _valueSequenceOffset, "value", CurrentValueAsString);
		}
		builder.AddElementReferenceCapture(Int32.MaxValue, elementReference => InputElement = elementReference);

		builder.CloseElement();
	}

	/// <inheritdoc />
	protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
	{
		CultureInfo culture = CultureInfo.CurrentCulture;

		string workingValue = value;
		bool success = true;
		result = default;

		// replace . with ,
		if ((culture.NumberFormat.NumberDecimalSeparator == ",") // when decimal separator is ,
			&& (culture.NumberFormat.NumberGroupSeparator != ".")) // and . is NOT used as group separator)
		{
			workingValue = workingValue.Replace(".", ",");
		}

		// limitation of the number of decimal places
		// for complications with the fact that we have TValue and it is quite difficult to work with, we will limit ourselves to solving and modifying the input data before converting it to the target type
		if (Decimal.TryParse(workingValue, IsTValueIntegerType ? NumberStyles.Integer : NumberStyles.Number, culture, out decimal parsedValue))
		{
			workingValue = Math.Round(parsedValue, DecimalsEffective, MidpointRounding.AwayFromZero).ToString(culture);
		}
		else
		{
			success = false;
			result = default;
		}

		if (success)
		{
			// conversion to the target type
			// BindConverter has a first-class citizen support for a lot of types (byte, short, int, long) but not for sbyte, ushort, uint, ulong.
			// These second-citizen types fallback to TypeConverter (TypeDescriptor.GetConverter(...))).
			// This converter throws ArgumentException when the value cannot be converted to the target type despite the method is "Try...".
			try
			{
				success = BindConverter.TryConvertTo<TValue>(workingValue, culture, out result);
			}
			catch (ArgumentException)
			{
				result = default;
				success = false;
			}
		}

		if (success)
		{
			// if there is only a change in format without changing the numeric value (for example, from 5.50 to 5.5), we want to convert the value back to the correct format (5.5 to 5.50).
			// StateHasChanged is not enough, see the comment in BuildRenderInput.
			if (FormatValueAsString(result) != value)
			{
				forceRenderValue = true;
			}

			validationErrorMessage = null;
			return true;
		}
		else
		{
			validationErrorMessage = GetParsingErrorMessage();
			return false;
		}
	}

	/// <summary>
	/// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
	/// </summary>
	/// <param name="value">The value to format.</param>
	/// <returns>A string representation of the value.</returns>
	protected override string FormatValueAsString(TValue value)
	{
		// integer types
		switch (value)
		{
			case null:
				return null;

			// mostly used first
			case int @int:
				return @int.ToString(CultureInfo.CurrentCulture);

			case long @long:
				return @long.ToString(CultureInfo.CurrentCulture);

			case short @short:
				return @short.ToString(CultureInfo.CurrentCulture);

			case byte @byte:
				return @byte.ToString(CultureInfo.CurrentCulture);

			// signed/unsigned integer variants
			case sbyte @sbyte:
				return @sbyte.ToString(CultureInfo.CurrentCulture);

			case ushort @ushort:
				return @ushort.ToString(CultureInfo.CurrentCulture);

			case uint @uint:
				return @uint.ToString(CultureInfo.CurrentCulture);

			case ulong @ulong:
				return @ulong.ToString(CultureInfo.CurrentCulture);
		}

		// floating-point types
		string format = (DecimalsEffective > 0)
			? "0." + String.Join("", Enumerable.Repeat('0', DecimalsEffective))
			: "0";

		switch (value)
		{
			case float @float:
				return @float.ToString(format, CultureInfo.CurrentCulture);

			case double @double:
				return @double.ToString(format, CultureInfo.CurrentCulture);

			case decimal @decimal:
				return @decimal.ToString(format, CultureInfo.CurrentCulture);
		}

		throw new InvalidOperationException($"Unsupported type {value.GetType()}.");

	}

	/// <summary>
	/// Returns the message for a parsing error.
	/// </summary>
	protected virtual string GetParsingErrorMessage()
	{
		var message = ParsingErrorMessage;
		if (ParsingErrorMessage is null)
		{
			if (IsTValueIntegerType)
			{
				message = StringLocalizer["ParsingErrorMessage_Integer"];
			}
			else
			{
				message = StringLocalizer["ParsingErrorMessage"];
			}
		}
		return String.Format(message, DisplayName ?? Label ?? FieldIdentifier.FieldName);
	}
}
