using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A base class for form input components. This base class automatically integrates
/// with a Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
/// as a cascading parameter.
/// Extends the <seealso cref="HxInputBase{TValue}" /> class.
/// Adds support for input groups, <see href="https://getbootstrap.com/docs/5.3/forms/input-group/">https://getbootstrap.com/docs/5.3/forms/input-group/</see>.
/// </summary>
public abstract class HxInputBaseWithInputGroups<TValue> : HxInputBase<TValue>, IFormValueComponentWithInputGroups
{
	/// <summary>
	/// Input group at the beginning of the input.
	/// </summary>
	[Parameter] public string InputGroupStartText { get; set; }

	/// <summary>
	/// Input group at the beginning of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <summary>
	/// Input group at the end of the input.
	/// </summary>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <summary>
	/// Input group at the end of the input.
	/// </summary>
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }
}
