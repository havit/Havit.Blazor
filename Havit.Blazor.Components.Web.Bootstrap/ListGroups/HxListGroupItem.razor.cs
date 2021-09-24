using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxListGroupItem
	{
		/// <summary>
		/// Content of the <code>HxListGroupItem</code>.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Indicates the current active selection.
		/// </summary>
		[Parameter] public bool Active { get; set; }

		/// <summary>
		/// Make the item appear disabled.
		/// </summary>
		[Parameter] public bool Disabled { get; set; }

		/// <summary>
		/// Whether the list item should act as a button.
		/// </summary>
		[Parameter] public bool Button { get; set; }

		/// <summary>
		/// Color.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// An event that is fired when the <code>HxListGroupItem</code> is clicked.
		/// </summary>
		[Parameter] public EventCallback OnClick { get; set; }

		[CascadingParameter] private HxListGroup listGroup { get; set; }

		private bool flexFill;

		protected override void OnParametersSet()
		{
			flexFill = listGroup.EqualWidthItems;
		}

		private void InvokeOnClick()
		{
			OnClick.InvokeAsync();
		}

		private string GetClasses()
		{
			return CssClassHelper.Combine(
				"list-group-item",
				Active ? "active" : null,
				Disabled ? "disabled" : null,
				Button ? "list-group-item-action" : null,
				Color is not null ? GetColorCss() : null,
				flexFill ? "flex-fill" : null);
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
