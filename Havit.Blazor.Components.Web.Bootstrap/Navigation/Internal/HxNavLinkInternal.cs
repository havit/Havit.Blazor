using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Variation of <see cref="NavLink"/> that adds <see cref="OnClick"/> and related functionality.
/// </summary>
/// <remarks>
/// See <see href="https://github.com/dotnet/aspnetcore/issues/18460#issuecomment-577175682">https://github.com/dotnet/aspnetcore/issues/18460#issuecomment-577175682</see> for more information.
/// </remarks>
public class HxNavLinkInternal : NavLink
{
	/// <summary>
	/// Raised when the item is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	/// <summary>
	/// Stops event propagation when the item is clicked. Default is <c>null</c>, which means <c>true</c> when <see cref="OnClick"/> is set.
	/// </summary>
	[Parameter] public bool? OnClickStopPropagation { get; set; }

	/// <summary>
	/// Prevents the default action for the onclick event. Default is <c>null</c>, which means <c>true</c> when <see cref="OnClick"/> is set.
	/// </summary>
	[Parameter] public bool? OnClickPreventDefault { get; set; }

	[Parameter] public string Text { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "a");

		builder.AddMultipleAttributes(1, AdditionalAttributes);
		builder.AddAttribute(2, "class", CssClass);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(3, "onclick", OnClick);
			builder.AddEventPreventDefaultAttribute(4, "onclick", this.OnClickPreventDefault ?? true);
			builder.AddEventStopPropagationAttribute(5, "onclick", this.OnClickStopPropagation ?? true);
		}

		builder.AddContent(6, Text);
		builder.AddContent(7, ChildContent);

		builder.CloseElement();
	}
}
