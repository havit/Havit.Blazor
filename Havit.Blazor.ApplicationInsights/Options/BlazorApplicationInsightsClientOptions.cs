namespace Havit.Blazor.ApplicationInsights.Options;

public record class BlazorApplicationInsightsClientOptions : ApplicationInsightsConfig // record for easy cloning by with { }
{
	/// <summary>
	/// Merges properties from this instance into <paramref name="target"/>.
	/// Only null properties on <paramref name="target"/> are filled; already-set values are never overwritten.
	/// </summary>
	internal void MergeTo(BlazorApplicationInsightsClientOptions target)
	{
		ArgumentNullException.ThrowIfNull(target);

		base.MergeTo(target);
	}
}