﻿namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Defaults for <see cref="HxMessengerServiceExtensions"/>.
/// </summary>
public record MessengerServiceExtensionsSettings
{
	/// <summary>
	/// Color scheme of informational messages.
	/// </summary>
	public ThemeColor InformationColor { get; set; } = ThemeColor.Primary;

	/// <summary>
	/// CssClass of informational messages.
	/// </summary>
	public string InformationCssClass { get; set; }

	/// <summary>
	/// Default autohide delay for information (in milliseconds). Default is <c>5000</c> ms.
	/// </summary>
	public int? InformationAutohideDelay { get; set; } = 5000;

	/// <summary>
	/// Color scheme of warning messages.
	/// </summary>
	public ThemeColor WarningColor { get; set; } = ThemeColor.Warning;

	/// <summary>
	/// CssClass of warning messages.
	/// </summary>
	public string WarningCssClass { get; set; }

	/// <summary>
	/// Default autohide delay for warnings (in milliseconds). Default is <c>null</c> (do not autohide).
	/// </summary>
	public int? WarningAutohideDelay { get; set; }

	/// <summary>
	/// Color scheme of error messages.
	/// </summary>
	public ThemeColor ErrorColor { get; set; } = ThemeColor.Danger;

	/// <summary>
	/// CssClass of error messages.
	/// </summary>
	public string ErrorCssClass { get; set; }

	/// <summary>
	/// Default autohide delay for errors (in milliseconds). Default is <c>null</c> (do not autohide).
	/// </summary>
	public int? ErrorAutohideDelay { get; set; }
}
