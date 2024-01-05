namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Extension methods for <see cref="ValidationMessageMode" />.
/// </summary>
public static class ValidationMessageModeExtensions
{
	internal static string AsCssClass(this ValidationMessageMode mode)
	{
		return mode switch
		{
			ValidationMessageMode.Regular => "invalid-feedback",
			ValidationMessageMode.Tooltip => "invalid-tooltip text-truncate",
			ValidationMessageMode.Floating => "invalid-feedback feedback-floating text-truncate",
			ValidationMessageMode.None => throw new InvalidOperationException($"{nameof(ValidationMessageMode.None)} cannot be used."),
			_ => throw new InvalidOperationException($"Unknown value {nameof(ValidationMessageMode)}: {mode}.")
		};
	}
}
