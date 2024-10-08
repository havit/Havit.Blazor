﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputFile"/> component.
/// </summary>
public record InputFileSettings
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }
}
