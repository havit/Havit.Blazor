namespace Havit.Blazor.Components.Web.Infrastructure;

/// <summary>
/// <see cref="ICascadeEnabledComponent"/> helper method.
/// </summary>
public static class CascadeEnabledComponent
{
	/// <summary>
	/// Effective value of Enabled. When Enabled is not set, it receives the value from FormState or defaults to true.
	/// </summary>
	public static bool EnabledEffective(ICascadeEnabledComponent component)
	{
		return component.Enabled ?? component.FormState?.Enabled ?? true;
	}
}
