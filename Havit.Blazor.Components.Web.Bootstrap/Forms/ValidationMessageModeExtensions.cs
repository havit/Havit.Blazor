namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Extension methods to <see cref="ValidationMessageMode" />.
/// </summary>
public static class ValidationMessageModeExtensions
{
	internal static string AsCssClass(this ValidationMessageMode mode)
	{
		return mode switch
		{
			ValidationMessageMode.Regular => "invalid-feedback",
			ValidationMessageMode.Tooltip => "invalid-tooltip",
			ValidationMessageMode.Floating => "invalid-feedback feedback-floating",
			ValidationMessageMode.None => throw new InvalidOperationException($"Cannot use for {nameof(ValidationMessageMode.None)} value."),
			_ => throw new InvalidOperationException($"Unknown {nameof(ValidationMessageMode)} value {mode}.")
		};
	}
}