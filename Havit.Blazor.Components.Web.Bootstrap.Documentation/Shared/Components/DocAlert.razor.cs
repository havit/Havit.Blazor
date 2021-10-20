using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	/// <summary>
	/// An alert for the documentation with 3 available configurations. Wraps the <c>HxAlert</c> component.
	/// </summary>
	public partial class DocAlert
	{
		/// <summary>
		/// Type of the alert. Default is <c>DocAlertType.Info</c>
		/// </summary>
		[Parameter] public DocAlertType Type { get; set; } = DocAlertType.Info;

		/// <summary>
		/// Content of the alert.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Css classes for the contained <c>HxIcon</c> component. Default is <c>fs-5 me-2</c>.
		/// </summary>
		[Parameter] public string IconCssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying <c>HxAlert</c> component.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		/// <summary>
		/// Use this parameter to override alert color determined by the <c>DocAlert.Type</c> property.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Use this parameter to override the icon determined by the <c>DocAlert.Type</c> property.
		/// </summary>
		[Parameter] public BootstrapIcon Icon { get; set; }

		/// <summary>
		/// If <c>false</c>, no <c>HxIcon</c> component is rendered. Default is <c>true</c>.
		/// </summary>
		[Parameter] public bool IconEnabled { get; set; } = true;

		public DocAlert()
		{
			IconCssClass = "fs-5 me-2";
		}

		private RenderFragment GetIcon()
		{
			if (!IconEnabled)
			{
				return null;
			}

			return builder =>
			{
				builder.OpenComponent<HxIcon>(1);
				builder.AddAttribute(2, nameof(HxIcon.Icon), Icon ?? GetBootstrapIcon());
				builder.AddAttribute(3, nameof(HxIcon.CssClass), IconCssClass);
				builder.CloseComponent();
			};
		}

		private BootstrapIcon GetBootstrapIcon()
		{
			return Type switch
			{
				DocAlertType.Info => BootstrapIcon.InfoCircle,
				DocAlertType.Warning => BootstrapIcon.ExclamationTriangle,
				DocAlertType.Danger => BootstrapIcon.ExclamationTriangle,
				_ => BootstrapIcon.InfoCircle,
			};
		}

		private ThemeColor GetColor()
		{
			if (Color is not null)
			{
				return Color.Value;
			}

			return Type switch
			{
				DocAlertType.Info => ThemeColor.Info,
				DocAlertType.Warning => ThemeColor.Warning,
				DocAlertType.Danger => ThemeColor.Danger,
				_ => ThemeColor.Info,
			};
		}
	}
}
