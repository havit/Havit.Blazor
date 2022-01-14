using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap alert component <a href="https://getbootstrap.com/docs/5.0/components/alerts/" />
	/// </summary>
	public partial class HxAlert
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Alert color (background). Required.
		/// </summary>
		[Parameter] public ThemeColor Color { get; set; }

		/// <summary>
		/// Shows the Close button and allows dissmissing of the alert.
		/// </summary>
		[Parameter] public bool Dismissible { get; set; }

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>(Color != ThemeColor.None, $"Parameter {nameof(Color)} of {nameof(HxBadge)} is required.");
		}

		public string GetColorCss()
		{
			return this.Color switch
			{
				ThemeColor.None => null,
				ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as {nameof(HxAlert)} color."),
				_ => "alert-" + this.Color.ToString("f").ToLower()
			};
		}

	}
}
