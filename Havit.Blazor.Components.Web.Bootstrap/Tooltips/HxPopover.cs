using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxPopover : HxTooltipInternalBase
	{
		/// <summary>
		/// Popover title.
		/// </summary>
		[Parameter]
		public string Title
		{
			get => base.TitleInternal;
			set => base.TitleInternal = value;
		}

		/// <summary>
		/// Popover content.
		/// </summary>
		[Parameter]
		public string Content
		{
			get => base.ContentInternal;
			set => base.ContentInternal = value;
		}

		/// <summary>
		/// Popover placement. Default is <see cref="PopoverPlacement.Right"/>.
		/// </summary>
		[Parameter]
		public PopoverPlacement Placement
		{
			get => (PopoverPlacement)base.PlacementInternal;
			set => base.PlacementInternal = (TooltipPlacement)value;
		}

		/// <summary>
		/// Popover trigger(s). Default is <see cref="PopoverTrigger.Click"/>.
		/// </summary>
		[Parameter]
		public PopoverTrigger Trigger
		{
			get => (PopoverTrigger)base.TriggerInternal;
			set => base.TriggerInternal = (TooltipTrigger)value;
		}

		protected override string JsModuleName => nameof(HxPopover);

		public HxPopover()
		{
			this.Placement = PopoverPlacement.Right;
			this.Trigger = PopoverTrigger.Click;
		}

	}
}
