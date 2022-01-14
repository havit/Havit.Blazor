using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.0/components/badge/">Bootstrap Badge</a> component.
	/// </summary>
	public partial class HxBadge
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Badge color (background). Required.
		/// </summary>
		[Parameter] public ThemeColor Color { get; set; }

		/// <summary>
		/// Color of badge text. Use <see cref="Color"/> for the background color.
		/// Default is <see cref="ThemeColor.None"/> (color automatically selected to work with chosen background color).
		/// </summary>
		[Parameter] public ThemeColor TextColor { get; set; } = ThemeColor.None;

		/// <summary>
		/// Badge type - Regular or rounded-pills.
		/// </summary>
		[Parameter] public BadgeType Type { get; set; } = BadgeType.Regular;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxBadge)} is required.");
		}

		protected string GetTypeCss()
		{
			return Type switch
			{
				BadgeType.Regular => null,
				BadgeType.RoundedPill => "rounded-pill",
				_ => throw new InvalidOperationException($"Unknown {nameof(BadgeType)} value: {Type}.")
			};
		}
	}
}
