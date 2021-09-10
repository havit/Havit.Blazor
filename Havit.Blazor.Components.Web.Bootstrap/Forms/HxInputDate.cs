using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date input component.
	/// </summary>
	public class HxInputDate<TValue> : HxInputBase<TValue>, IInputWithPlaceholder, IInputWithSize
	{
		// DO NOT FORGET TO MAINTAIN DOCUMENTATION!
		private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(DateTime), typeof(DateTimeOffset) };

		/// <summary>
		/// Default dates offered in associated menu (e.g. Today, Yesterday, etc.).
		/// </summary>
		public static List<DateItem> DefaultDates { get; set; }

		/// <summary>
		/// When <c>true</c>, uses default date ranges (this month, last month, this year, last year).
		/// </summary>
		[Parameter] public bool UseDefaultDates { get; set; } = true;

		/// <summary>
		/// Custom date ranges. When <see cref="UseDefaultDates"/> is <c>true</c>, these items are used with default items.
		/// </summary>
		[Parameter] public IEnumerable<DateItem> CustomDates { get; set; }

		/// <summary>
		/// Gets or sets the error message used when displaying a parsing error.
		/// Used with String.Format(...), {0} is replaced by Label property, {1} name of bounded property.
		/// </summary>
		[Parameter] public string ParsingErrorMessage { get; set; }

		/// <inheritdoc cref="IInputWithPlaceholder.Placeholder" />
		[Parameter] public string Placeholder { get; set; }

		/// <inheritdoc cref="Bootstrap.InputSize" />
		[Parameter] public InputSize? InputSize { get; set; }

		/// <summary>
		/// Optional icon to display within the input. Use <see cref="HxInputDate.Defaults"/> to set the icon for the whole project.
		/// </summary>
		[Parameter] public IconBase CalendarIcon { get; set; }

		[Inject] private IStringLocalizer<HxInputDate> StringLocalizer { get; set; }

		/// <summary>
		/// Returns <see cref="HxInputDate{TValue}"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual InputDateDefaults GetDefaults() => HxInputDate.Defaults;
		IInputDefaultsWithSize IInputWithSize.GetDefaults() => GetDefaults(); // might be replaced with C# vNext convariant return types on interfaces

		public HxInputDate()
		{
			Type undelyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
			if (!supportedTypes.Contains(undelyingType))
			{
				throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
			}
		}

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenComponent(1, typeof(HxInputDateInternal<TValue>));

			builder.AddAttribute(100, nameof(HxInputDateInternal<TValue>.Value), Value);
			builder.AddAttribute(101, nameof(HxInputDateInternal<TValue>.ValueChanged), ValueChanged);
			builder.AddAttribute(102, nameof(HxInputDateInternal<TValue>.ValueExpression), ValueExpression);

			builder.AddAttribute(200, nameof(HxInputDateInternal<TValue>.InputId), InputId);
			builder.AddAttribute(201, nameof(HxInputDateInternal<TValue>.InputCssClass), InputCssClass);
			builder.AddAttribute(202, nameof(HxInputDateInternal<TValue>.EnabledEffective), EnabledEffective);
			builder.AddAttribute(203, nameof(HxInputDateInternal<TValue>.ParsingErrorMessageEffective), GetParsingErrorMessage());
			builder.AddAttribute(204, nameof(HxInputDateInternal<TValue>.Placeholder), Placeholder);
			builder.AddAttribute(205, nameof(HxInputDateInternal<TValue>.InputSize), ((IInputWithSize)this).InputSizeEffective);
			builder.AddAttribute(206, nameof(HxInputDateInternal<TValue>.CalendarIcon), CalendarIcon ?? HxInputDate.Defaults.CalendarIcon);
			builder.AddAttribute(207, nameof(HxInputDateInternal<TValue>.CustomDates), GetCustomDates().ToList());

			builder.CloseComponent();
		}

		// For generating chips
		/// <inheritdocs />
		protected override string FormatValueAsString(TValue value) => FormatValue(value);

		private protected override void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
		{
			throw new NotSupportedException();
		}

		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private IEnumerable<DateItem> GetCustomDates()
		{
			if (CustomDates != null)
			{
				foreach (DateItem dateItem in CustomDates)
				{
					yield return dateItem;
				}
			}

			if (UseDefaultDates)
			{
				if (DefaultDates != null)
				{
					foreach (DateItem defaultDateItem in DefaultDates)
					{
						yield return defaultDateItem;
					}
				}
				else
				{
					DateTime today = DateTime.Today;

					yield return new DateItem { Label = StringLocalizer["Today"], Date = today };
				}
			}
		}

		/// <summary>
		/// Returns message for a parsing error.
		/// </summary>
		protected virtual string GetParsingErrorMessage()
		{
			var message = !String.IsNullOrEmpty(ParsingErrorMessage)
				? ParsingErrorMessage
				: StringLocalizer["ParsingErrorMessage"];
			return String.Format(message, Label, FieldIdentifier.FieldName);
		}

		internal static string FormatValue(TValue value)
		{
			// no 1.1.0001, etc.
			if (EqualityComparer<TValue>.Default.Equals(value, default))
			{
				return null;
			}

			switch (value)
			{
				case DateTime dateTimeValue:
					return dateTimeValue.ToShortDateString();
				case DateTimeOffset dateTimeOffsetValue:
					return dateTimeOffsetValue.DateTime.ToShortDateString();
				default:
					throw new InvalidOperationException("Unsupported type.");
			}
		}

		// for easy testability
		internal static bool TryParseDateTimeOffsetFromString(string value, CultureInfo culture, out DateTimeOffset? result)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				result = null;
				return true;
			}

			// it also works for "invalid dates" (with dots, commas, spaces...)
			// ie. 09,09,2020 --> 9.9.2020
			// ie. 09 09 2020 --> 9.9.2020
			// ie. 06,05, --> 6.5.ThisYear
			// ie. 06 05 --> 6.5.ThisYear
			bool success = DateTimeOffset.TryParse(value, culture, DateTimeStyles.AllowWhiteSpaces, out DateTimeOffset parsedValue) && (parsedValue.TimeOfDay == TimeSpan.Zero);
			if (success)
			{
				result = parsedValue;
				return true;
			}

			Match match;

			// 0105 --> 01.05.ThisYear
			match = Regex.Match(value, "^(\\d{2})(\\d{2})$");
			if (match.Success)
			{
				if (int.TryParse(match.Groups[1].Value, out int day)
					&& int.TryParse(match.Groups[2].Value, out int month))
				{
					try
					{
						result = new DateTimeOffset(new DateTime(DateTime.Now.Year, month, day));
						return true;
					}
					catch (ArgumentOutOfRangeException)
					{
						// NOOP
					}
				}
				result = null;
				return false;
			}

			// 01 --> 01.ThisMonth.ThisYear
			match = Regex.Match(value, "^(\\d{2})$");
			if (match.Success)
			{
				if (int.TryParse(match.Groups[1].Value, out int day))
				{
					try
					{
						result = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, day));
						return true;
					}
					catch (ArgumentOutOfRangeException)
					{
						// NOOP
					}
				}
				result = null;
				return false;
			}

			result = null;
			return false;
		}
	}
}