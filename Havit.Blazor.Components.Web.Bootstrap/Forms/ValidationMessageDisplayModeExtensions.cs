namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Extension methods to <see cref="ValidationMessageDisplayMode" />.
/// </summary>
public static class ValidationMessageDisplayModeExtensions
{
	internal static string AsCssClass(this ValidationMessageDisplayMode mode)
	{
		return mode switch
		{
			ValidationMessageDisplayMode.Regular => "invalid-feedback",
			ValidationMessageDisplayMode.Tooltip => "invalid-tooltip",
			ValidationMessageDisplayMode.KeepSpace => "invalid-keepspace",
			ValidationMessageDisplayMode.None => throw new InvalidOperationException($"Invalid {nameof(ValidationMessageDisplayMode)} value {nameof(ValidationMessageDisplayMode.None)}."),
			_ => throw new InvalidOperationException($"Unknown {nameof(ValidationMessageDisplayMode)} value {mode}.")
		};
	}
}