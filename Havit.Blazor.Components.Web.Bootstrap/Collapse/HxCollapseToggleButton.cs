﻿namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Bootstrap Collapse toggle (in form of button) which triggers the <see cref="HxCollapse"/> to toggle.
	/// Derived from <see cref="HxButton"/> (incl. <see cref="HxButton.Defaults"/> inheritance).
	/// </summary>
	public class HxCollapseToggleButton : HxButton, IHxCollapseToggle
	{
		/// <summary>
		/// Target selector of the toggle.
		/// Use <c>#id</c> to reference single <see cref="HxCollapse"/> or <c>.class</c> for multiple <see cref="HxCollapse"/>s.
		/// </summary>
		[Parameter] public string CollapseTarget { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// this code cannot be in OnParametersSet() as there is a SetParametersAsync() override in HxNavbarToggler
			// which manipulates some of the parameters
			AdditionalAttributes ??= new Dictionary<string, object>();
			AdditionalAttributes["data-bs-toggle"] = "collapse";
			AdditionalAttributes["aria-expanded"] = false;

			if (!String.IsNullOrWhiteSpace(this.CollapseTarget))
			{
				AdditionalAttributes["data-bs-target"] = this.CollapseTarget;

				if (this.CollapseTarget.StartsWith("#"))
				{
					AdditionalAttributes["aria-controls"] = this.CollapseTarget.Substring(1);
				}
			}

			base.BuildRenderTree(builder);
		}
	}
}
