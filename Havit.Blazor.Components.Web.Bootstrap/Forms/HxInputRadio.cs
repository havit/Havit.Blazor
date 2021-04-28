using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxInputRadio<TValue> : ComponentBase, ICascadeEnabledComponent
	{
		[Parameter] public TValue Value { get; set; }

		[Parameter] public string Label { get; set; }

		[Parameter] public RenderFragment LabelTemplate { get; set; }

		[CascadingParameter] public FormState FormState { get; set; }

		[Parameter] public bool? Enabled { get; set; }

		// TODO: Group + povinná!

		public bool Inline { get; set; } = true; // TODO: Enum, resp. spíš přesunout do HxInputRadioGroup

		private string inputId = "hx" + Guid.NewGuid().ToString("N");

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(101, "div");
			builder.AddAttribute(102, "class", CssClassHelper.Combine("form-check", Inline ? "form-check-inline" : null));

			builder.OpenElement(201, "input");
			builder.AddAttribute(202, "class", "form-check-input");
			builder.AddAttribute(203, "type", "radio");
			builder.AddAttribute(204, "name", "TODO"); // přijmout z groupy!
			builder.AddAttribute(205, "id", inputId);
			builder.AddAttribute(206, "disabled", !CascadeEnabledComponent.EnabledEffective(this));
			builder.AddAttribute(207, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, (args) => HandleInputChange(args)));
			builder.CloseElement(); // input
			// TODO: Checked!

			builder.OpenElement(301, "label");
			builder.AddAttribute(302, "class", "form-check-label");
			builder.AddAttribute(303, "for", inputId);
			builder.AddContent(304, Label);
			builder.AddContent(305, LabelTemplate);
			builder.CloseElement(); // label

			builder.CloseElement(); // div
		}

		private void HandleInputChange(ChangeEventArgs args)
		{

		}
	}
}
