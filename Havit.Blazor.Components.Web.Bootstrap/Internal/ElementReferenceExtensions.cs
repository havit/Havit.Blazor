namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

internal static class ElementReferenceExtensions
{
	public static async ValueTask FocusOrThrowAsync(this ElementReference inputElement, ComponentBase caller)
	{
		if (EqualityComparer<ElementReference>.Default.Equals(inputElement, default))
		{
			throw new InvalidOperationException($"[{caller.GetType().Name}] Unable to focus, {nameof(inputElement)} reference not available. You are most likely calling the method too early. The first render must complete before calling this method.");
		}

		await inputElement.FocusAsync();
	}
}
