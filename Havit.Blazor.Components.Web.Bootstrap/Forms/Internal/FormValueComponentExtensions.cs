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
}
