﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for <see cref="HxInputNumber{TValue}"/>.
/// Marker for resources for <see cref="HxInputNumber{TValue}"/>.
/// </summary>
public class HxInputNumber
{
	/// <summary>
	/// Application-wide defaults for the <see cref="HxAutosuggest{TItem, TValue}"/> and derived components.
	/// </summary>
	public static InputNumberSettings Defaults { get; set; }

	static HxInputNumber()
	{
		Defaults = new InputNumberSettings()
		{
			Type = InputType.Text,
			SelectOnFocus = true
		};
	}
}
