using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A base class for form input components. This base class automatically integrates
/// with a Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
/// as a cascading parameter.
/// Extends the <seealso cref="HxInputBaseWithInputGroups{TValue}" /> class.
/// Adds support for adorns (icons/text rendered inside the field chrome),
/// see <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/form-adorn/">form-adorn</see>.
/// </summary>
public abstract class HxInputBaseWithAdorns<TValue> : HxInputBaseWithInputGroups<TValue>, IFormValueComponentWithAdorns
{
	/// <summary>
	/// Custom CSS class to be rendered with the <c>form-adorn</c> wrapper.
	/// </summary>
	[Parameter] public string AdornCssClass { get; set; }

	/// <summary>
	/// Text adornment at the beginning of the input.
	/// </summary>
	[Parameter] public string AdornStartText { get; set; }

	/// <summary>
	/// Adornment (typically an icon) at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment AdornStartTemplate { get; set; }

	/// <summary>
	/// Text adornment at the end of the input.
	/// </summary>
	[Parameter] public string AdornEndText { get; set; }

	/// <summary>
	/// Adornment (typically an icon) at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment AdornEndTemplate { get; set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if ((this is IInputWithLabelType inputWithLabelType)
			&& (inputWithLabelType.LabelTypeEffective == LabelType.Floating)
			&& this.ShouldRenderAdorn())
		{
			throw new InvalidOperationException($"[{GetType().Name}] LabelType.Floating cannot be combined with adorns (AdornStartText/AdornStartTemplate/AdornEndText/AdornEndTemplate) — the form-adorn wrapper owns the visual chrome and cannot host a floating label. Use LabelType.Regular.");
		}
	}
}
