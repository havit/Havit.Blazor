using System.Globalization;
using System.Text.RegularExpressions;

// source: https://www.codeproject.com/Articles/1135909/String-parsing-with-custom-patterns

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

internal class StringPatternizer
{
	#region Private members

	#endregion

	#region Constructor

	public StringPatternizer()
	{
		Markers = new Dictionary<string, Type>();
		Patterns = new List<string>();
	}

	#endregion

	#region Properties

	public Dictionary<string, Type> Markers
	{
		get;
		private set;
	}

	public List<string> Patterns
	{
		get;
		private set;
	}

	#endregion

	#region Public methods

	public PatternizationResult Match(string value)
	{
		foreach (var pattern in Patterns)
		{
			Dictionary<string, object> matchResult;
			if (Match(value, pattern, out matchResult))
			{
				return new PatternizationResult(pattern, matchResult);
			}
		}

		return new PatternizationResult(new FormatException(string.Format("No pattern that matches value '{0}'.", value)));
	}

	public List<PatternizationResult> MatchAll(string value)
	{
		List<PatternizationResult> result = new List<PatternizationResult>();

		foreach (var pattern in Patterns)
		{
			Dictionary<string, object> matchResult;
			if (Match(value, pattern, out matchResult))
			{
				result.Add(new PatternizationResult(pattern, matchResult));
			}
		}

		return result;
	}

	public void MarkersAddRange(Dictionary<string, Type> markers)
	{
		foreach (var marker in markers)
		{
			Markers.Add(marker.Key, marker.Value);
		}
	}

	#endregion

	#region Private methods

	private bool Match(string str, string originalPattern, out Dictionary<string, object> matchResult)
	{
		matchResult = new Dictionary<string, object>();

		var normalizedPattern = TranslateInlineCharCodes(originalPattern);
		string regexPattern = TranslateToRegexPattern(normalizedPattern);


		Regex regex = new Regex(regexPattern);
		var match = regex.Match(str);
		if (!match.Success)
		{
			return false;
		}


		for (int g = 0; g < match.Groups.Count; g++)
		{
			var groupName = regex.GroupNameFromNumber(g);
			if (!Markers.ContainsKey(groupName))
			{
				continue;
			}
			var groupValue = match.Groups[g].Value;


			try
			{
				var convertedValue = ConvertToType(groupValue, Markers[groupName]);
				matchResult.Add(groupName, convertedValue);
			}
			catch
			{
				return false; //pattern marker value was found in processing String, but value has incorrect type format - means pattern doesn't match
			}
		}

		return true;
	}

	private string TranslateToRegexPattern(string pattern)
	{
		var regexPattern = new StringBuilder(Regex.Escape(pattern)); //Escape - treats any character as literal (needed to avoid RegEx injections from original pattern)

		foreach (var marker in Markers)
		{
			if (!regexPattern.ToString().Contains(marker.Key.ToString()))
			{
				continue;
			}

			var regexMarkerPattern = new StringBuilder();
			regexMarkerPattern.Append("(?<");
			regexMarkerPattern.Append(marker.Key); //specify group name
			regexMarkerPattern.Append(">.+)"); //'.+' - means one or unlimited number of any characters(except line terminators)
			regexPattern = regexPattern.Replace(marker.Key.ToString(), regexMarkerPattern.ToString());
		}

		return regexPattern.ToString();
	}

	private object ConvertToType(string value, Type type)
	{
		try
		{
			if (type == typeof(int))
			{
				return Convert.ToInt32(value);
			}
			else if (type == typeof(double))
			{
				value = value.Replace(',', '.');
				return double.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
			}
			else if (type == typeof(decimal))
			{
				value = value.Replace(',', '.');
				return decimal.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
			}
			else if (type == typeof(float))
			{
				value = value.Replace(',', '.');
				return float.Parse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
			}
			else if (type == typeof(bool))
			{
				return Convert.ToBoolean(value);
			}
			else if (type == typeof(string))
			{
				return value;
			}
			else
			{
				throw new NotSupportedException(string.Format("Marker type '{0}' is not supported.", type));
			}
		}
		catch (Exception ex)
		{
			throw new FormatException(string.Format("Failed to convert value '{0}' to type '{1}'.", value, type), ex);
		}
	}

	/// <summary>
	/// Searches for character codes surrounded with '{' and '}' and replaces them with corresponding symbol.
	/// </summary>
	private string TranslateInlineCharCodes(string pattern)
	{
		var result = pattern;
		var inlineCharParts = pattern.Split(new char[] { '{' }, StringSplitOptions.RemoveEmptyEntries);
		foreach (var inlineCharPart in inlineCharParts)
		{
			try
			{
				var inlineCharCode = inlineCharPart.Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries)[0];

				var inlineChar = (char)Convert.ToUInt16(inlineCharCode);

				result = result.Replace("{" + inlineCharCode + "}", inlineChar.ToString());
			}
			catch
			{
				continue;
			}
		}

		return result;
	}

	#endregion
}
