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

	private string _adornStartTextId;
	private string _adornEndTextId;

	// The text adornments are aria-hidden (must not join the accessible name of the input via the wrapping label),
	// but they can be meaningful (currency, unit, domain suffix), so the input references them in aria-describedby.
	string IFormValueComponentWithAdorns.AdornStartTextId => String.IsNullOrEmpty(AdornStartText) ? null : (_adornStartTextId ??= "el" + Guid.NewGuid().ToString("N"));
	string IFormValueComponentWithAdorns.AdornEndTextId => String.IsNullOrEmpty(AdornEndText) ? null : (_adornEndTextId ??= "el" + Guid.NewGuid().ToString("N"));

	/// <inheritdoc />
	private protected override IEnumerable<string> GetAriaDescribedByIds()
	{
		IFormValueComponentWithAdorns adorns = this;
		return new[] { adorns.AdornStartTextId, adorns.AdornEndTextId }.Concat(base.GetAriaDescribedByIds());
	}

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
