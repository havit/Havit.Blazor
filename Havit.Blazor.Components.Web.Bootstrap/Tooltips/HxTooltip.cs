using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.0/components/tooltips/">Bootstrap Tooltip</a> component, activates on hover.<br />
	/// Rendered as a <c>span</c> wrapper to fully support tooltips on disabled elements (see example in <a href="https://getbootstrap.com/docs/5.0/components/tooltips/#disabled-elements">Disabled elements</a> in the Bootstrap tooltip documentation).
	/// </summary>
	public class HxTooltip : HxTooltipInternalBase
	{
		/// <summary>
		/// Tooltip text.
		/// </summary>
		[Parameter]
		public string Text
		{
			get => base.TitleInternal;
			set => base.TitleInternal = value;
		}

		/// <summary>
		/// Tooltip placement. Default is <see cref="TooltipPlacement.Top"/>.
		/// </summary>
		[Parameter]
		public TooltipPlacement Placement
		{
			get => base.PlacementInternal;
			set => base.PlacementInternal = value;
		}

		/// <summary>
		/// Tooltip trigger(s). Default is <c><see cref="TooltipTrigger.Hover"/> | <see cref="TooltipTrigger.Focus"/></c>.
		/// </summary>
		[Parameter]
		public TooltipTrigger Trigger
		{
			get => base.TriggerInternal;
			set => base.TriggerInternal = value;
		}


		protected override string JsModuleName => nameof(HxTooltip);

		public HxTooltip()
		{
			this.Placement = TooltipPlacement.Top;
			this.Trigger = TooltipTrigger.Hover | TooltipTrigger.Focus;
		}
	}
}
