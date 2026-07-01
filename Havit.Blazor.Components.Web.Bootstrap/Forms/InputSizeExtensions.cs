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
	/// Returns CSS class to render select (form-select) with the desired size.
	/// </summary>
	public static string AsFormSelectCssClass(this InputSize inputSize)
	{
		return inputSize switch
		{
			InputSize.Regular => null,
			InputSize.Small => "form-select-sm",
			InputSize.Large => "form-select-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(InputSize)} value {inputSize}.")
		};
	}
}
