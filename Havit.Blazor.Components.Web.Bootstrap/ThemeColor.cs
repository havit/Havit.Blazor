namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap theme colors. In Bootstrap 6, theme colors are applied to components via composable <c>.theme-*</c> classes
/// (e.g. <c>.btn-solid .theme-primary</c>). For more information, see <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/customize/color/">Bootstrap 6 color documentation</see>.
/// </summary>
public enum ThemeColor
{
	None = 0,
	Primary,
	Secondary,
	Success,
	Danger,
	Warning,
	Info,
	/// <summary>
	/// This color is intended to be used for buttons only.
	/// </summary>
	Link,
	/// <summary>
	/// Accent theme color (new in Bootstrap 6).
	/// </summary>
	Accent,
	/// <summary>
	/// Inverse theme color (new in Bootstrap 6) — dark in light color mode, light in dark color mode.
	/// Closest replacement for the former <c>Dark</c> theme color.
	/// </summary>
	Inverse
}
