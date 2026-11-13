namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Extension methods for <see cref="IFormValueComponent" />.
/// </summary>
public static class FormValueComponentExtensions
{
	/// <summary>
	/// Returns <c>true</c> if <see cref="IFormValueComponent" /> should render input groups (at least one input group).
	/// </summary>
	public static bool ShouldRenderInputGroups(this IFormValueComponent formValueComponent)
	{
		return (formValueComponent is IFormValueComponentWithInputGroups formValueComponentWithInputGroups)
			&& (!String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupStartText)
				|| (formValueComponentWithInputGroups.InputGroupStartTemplate != null)
				|| !String.IsNullOrEmpty(formValueComponentWithInputGroups.InputGroupEndText)
				|| (formValueComponentWithInputGroups.InputGroupEndTemplate != null));
	}

	/// <summary>
	/// Returns <c>true</c> if <see cref="IFormValueComponent" /> should render the <c>form-adorn</c> wrapper (at least one adornment).
	/// </summary>
	public static bool ShouldRenderAdorn(this IFormValueComponent formValueComponent)
	{
		return (formValueComponent is IFormValueComponentWithAdorns formValueComponentWithAdorns)
			&& (!String.IsNullOrEmpty(formValueComponentWithAdorns.AdornStartText)
				|| (formValueComponentWithAdorns.AdornStartTemplate != null)
				|| !String.IsNullOrEmpty(formValueComponentWithAdorns.AdornEndText)
				|| (formValueComponentWithAdorns.AdornEndTemplate != null));
	}
}
