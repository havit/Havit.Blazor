namespace Havit.Blazor.Components.Web.Bootstrap;

public static class InputSizeExtensions
{
	/// <summary>
	/// Returns CSS class to render input (form-control) with the desired size.
	/// </summary>
	public static string AsFormControlCssClass(this InputSize inputSize)
	{
		return inputSize switch
		{
			InputSize.Regular => null,
			InputSize.Small => "form-control-sm",
			InputSize.Large => "form-control-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {inputSize}.")
		};
	}

	/// <summary>
	/// Returns CSS class to render input-group with the desired size.
	/// </summary>
	public static string AsInputGroupCssClass(this InputSize inputSize)
	{
		return inputSize switch
		{
			InputSize.Regular => null,
			InputSize.Small => "input-group-sm",
			InputSize.Large => "input-group-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {inputSize}.")
		};
	}

	/// <summary>
	/// Returns CSS class to render a checkbox (<c>check</c>), radio (<c>radio</c>) or switch (<c>switch</c>) with the desired size
	/// (<c>check-sm</c>, <c>radio-lg</c>, ...).
	/// </summary>
	internal static string AsCheckRadioSwitchCssClass(this InputSize inputSize, string baseCssClass)
	{
		return inputSize switch
		{
			InputSize.Regular => null,
			InputSize.Small => baseCssClass + "-sm",
			InputSize.Large => baseCssClass + "-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {inputSize}.")
		};
	}

	/// <summary>
	/// Returns CSS class to render select with the desired size (Bootstrap 6 consolidated .form-select into .form-control).
	/// </summary>
	public static string AsFormSelectCssClass(this InputSize inputSize)
	{
		return inputSize switch
		{
			InputSize.Regular => null,
			InputSize.Small => "form-control-sm",
			InputSize.Large => "form-control-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {inputSize}.")
		};
	}
}
