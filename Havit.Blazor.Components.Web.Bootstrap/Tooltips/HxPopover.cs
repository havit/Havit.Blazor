using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/popovers">Bootstrap Popover</see> component.<br />
	/// Rendered as a <c>span</c> wrapper to fully support popovers on disabled elements (see example in <see href="https://getbootstrap.com/docs/5.1/components/popovers/#disabled-elements">Disabled elements</see> in the Bootstrap popover documentation).<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPopover">https://havit.blazor.eu/components/HxPopover</see>
	/// </summary>
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
		protected override string DataBsToggle => "popover";

		public HxPopover()
		{
			this.Placement = PopoverPlacement.Right;
			this.Trigger = PopoverTrigger.Click;
		}
	}
}
