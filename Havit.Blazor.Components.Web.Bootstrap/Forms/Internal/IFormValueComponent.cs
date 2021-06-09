using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public interface IFormValueComponent
	{
		string CssClass => null;

		string Label => null;
		RenderFragment LabelTemplate => null;
		string LabelFor => null;
		string LabelCssClass => null;

		void RenderValue(RenderTreeBuilder builder); // no default implementation!
		void RenderValidationMessage() { }

		string Hint => null;
		RenderFragment HintTemplate => null;

		/// <summary>
		/// CSS class to be rendered with the wrapping div.
		/// </summary>
		string CoreCssClass => "hx-form-group position-relative";

		/// <summary>
		/// CSS class to be rendered with the label.
		/// </summary>
		string CoreLabelCssClass => "form-label";

		/// <summary>
		/// CSS class to be rendered with the hint.
		/// </summary>
		string CoreHintCssClass => "form-text";

		LabelValueRenderOrder RenderOrder => LabelValueRenderOrder.LabelValue;
	}
}
