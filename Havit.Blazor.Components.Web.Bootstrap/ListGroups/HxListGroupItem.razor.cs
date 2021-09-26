using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Item for <see cref="HxListGroup"/>.
	/// </summary>
	public partial class HxListGroupItem
	{
		/// <summary>
		/// Content of the item.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Indicates the current active selection.
		/// </summary>
		[Parameter] public bool Active { get; set; }

		/// <summary>
		/// Make the item appear disabled by setting to <c>false</c>.
		/// Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool Enabled { get; set; } = true;

		/// <summary>
		/// Color.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// An event that is fired when the <code>HxListGroupItem</code> is clicked.
		/// </summary>
		[Parameter] public EventCallback OnClick { get; set; }

		[CascadingParameter] protected HxListGroup ListGroupContainer { get; set; }

		private string GetClasses()
		{
			return CssClassHelper.Combine(
				"list-group-item",
				Active ? "active" : null,
				Enabled ? null : "disabled",
				OnClick.HasDelegate ? "list-group-item-action" : null,
				Color is not null ? GetColorCss() : null,
				ListGroupContainer.EqualWidthItems ? "flex-fill" : null);
		}

		private string GetColorCss()
		{
			return Color switch
			{
				ThemeColor.None => null,
				_ => "list-group-item-" + this.Color.Value.ToString("f").ToLower()
			};
		}
	}
}
