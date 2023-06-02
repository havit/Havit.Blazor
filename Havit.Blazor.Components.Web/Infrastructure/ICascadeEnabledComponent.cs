namespace Havit.Blazor.Components.Web.Infrastructure;

/// <summary>
/// Component which can be enabled/disabled in a cascade.
/// </summary>
public interface ICascadeEnabledComponent
{
	/// <summary>
	/// Form state cascading parameter.
	/// </summary>
	public FormState FormState { get; set; }

	/// <summary>
	/// When <c>null</c> (default), the Enabled value is received from cascading <see cref="FormState" />.
	/// When value is <c>false</c>, input is rendered as disabled.
	/// To set multiple controls as disabled use <seealso cref="HxFormState" />.
	/// </summary>
	public bool? Enabled { get; set; }
}
