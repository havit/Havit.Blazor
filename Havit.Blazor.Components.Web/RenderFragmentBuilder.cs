namespace Havit.Blazor.Components.Web;

/// <summary>
/// Build render fragments for specific scenarios.
/// </summary>
public static class RenderFragmentBuilder
{
	/// <summary>
	/// Returns a RenderFragment to render "nothing". The implementation returns <c>null</c>.
	/// </summary>
	public static RenderFragment Empty()
	{
		return null;
	}

	/// <summary>
	/// Returns a RenderFragment that renders content and a template (it is expected that at least one of the arguments is null).		
	/// If both are <c>null</c>, it returns <see cref="Empty"/>.
	/// </summary>
	public static RenderFragment CreateFrom(string content, RenderFragment template)
	{
		if (content is null && template is null)
		{
			return Empty();
		}

		return (RenderTreeBuilder builder) =>
		{
			builder.AddContent(0, content); // null check: if the string is null, use String.Empty
			builder.AddContent(1, template); // null check: used inside 
		};
	}
}
