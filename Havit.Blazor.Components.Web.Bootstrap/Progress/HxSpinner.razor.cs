using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/spinners/">Spinner</see> (usually indicates operation in progress).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSpinner">https://havit.blazor.eu/components/HxSpinner</see>
/// </summary>
public partial class HxSpinner
{
	/// <summary>
	/// Spinner type. Spinner type - Border (default) or Grow.
	/// </summary>
	[Parameter] public SpinnerType Type { get; set; } = SpinnerType.Border;

	/// <summary>
	/// Spinner size. You can also use custom <see cref="CssClass"/> to change the dimensions as needed.
	/// </summary>
	[Parameter] public SpinnerSize Size { get; set; } = SpinnerSize.Regular;

	/// <summary>
	/// Spinner color.
	/// </summary>
	[Parameter] public ThemeColor Color { get; set; } = ThemeColor.None;

	/// <summary>
	/// Additional CSS class to be applied.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[Inject] protected IStringLocalizer<HxSpinner> Localizer { get; set; }

	protected string GetSpinnerTypeCssClass()
	{
		return this.Type switch
		{
			SpinnerType.Border => "spinner-border",
			SpinnerType.Grow => "spinner-grow",
			_ => throw new InvalidOperationException($"Unknown {nameof(SpinnerType)}: {Type}.")
		};
	}

	protected string GetSpinnerSizeCssClass()
	{
		return this.Size switch
		{
			SpinnerSize.Regular => null,
			SpinnerSize.Small => GetSpinnerTypeCssClass() + "-sm",
			_ => throw new InvalidOperationException($"Unknown {nameof(SpinnerSize)}: {Size}.")
		};
	}
}
