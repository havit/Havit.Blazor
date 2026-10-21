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
		if (IsActive())
		{
			// The base NavLink.BuildRenderTree renders aria-current="page" for the active link, keep it.
			builder.AddAttribute(3, "aria-current", "page");
		}

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(4, "onclick", OnClick);
			builder.AddEventPreventDefaultAttribute(5, "onclick", this.OnClickPreventDefault ?? true);
			builder.AddEventStopPropagationAttribute(6, "onclick", this.OnClickStopPropagation ?? true);
		}

		builder.AddContent(7, Text);
		builder.AddContent(8, ChildContent);

		builder.CloseElement();
	}

	/// <summary>
	/// The active state of the base <see cref="NavLink"/> is private, it is reflected only by the active class in <see cref="NavLink.CssClass"/>.
	/// An empty <see cref="NavLink.ActiveClass"/> means the active state is not rendered at all.
	/// </summary>
	private bool IsActive()
	{
		string activeClass = ActiveClass ?? "active";
		return !String.IsNullOrWhiteSpace(activeClass)
			&& (CssClass?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(activeClass) ?? false);
	}
}
