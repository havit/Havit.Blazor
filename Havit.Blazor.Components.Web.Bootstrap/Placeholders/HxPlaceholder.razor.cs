using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Layouts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/placeholders/">Bootstrap 5 Placeholder</see> component, aka Skeleton.<br/>
	/// Use loading placeholders for your components or pages to indicate something may still be loading.
	/// </summary>
	public partial class HxPlaceholder : ILayoutColumnComponent
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxPlaceholder"/>.
		/// </summary>
		public static PlaceholderDefaults Defaults { get; set; } = new();

		/// <inheritdoc cref="ILayoutColumnComponent.Columns"/>
		[Parameter] public string Columns { get; set; }

		/// <inheritdoc cref="ILayoutColumnComponent.ColumnsSmallUp"/>
		[Parameter] public string ColumnsSmallUp { get; set; }

		/// <inheritdoc cref="ILayoutColumnComponent.ColumnsMediumUp"/>
		[Parameter] public string ColumnsMediumUp { get; set; }

		/// <inheritdoc cref="ILayoutColumnComponent.ColumnsLargeUp"/>
		[Parameter] public string ColumnsLargeUp { get; set; }

		/// <inheritdoc cref="ILayoutColumnComponent.ColumnsExtraLargeUp"/>
		[Parameter] public string ColumnsExtraLargeUp { get; set; }

		/// <inheritdoc cref="ILayoutColumnComponent.ColumnsXxlUp"/>
		[Parameter] public string ColumnsXxlUp { get; set; }

		/// <summary>
		/// Size of the placeholder.
		/// </summary>
		[Parameter] public PlaceholderSize? Size { get; set; }

		/// <summary>
		/// Color of the placeholder.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }

		/// <summary>
		/// Optional content of the placeholder (usualy not used).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		[CascadingParameter] protected HxPlaceholderContainer PlaceholderContainer { get; set; }

		protected PlaceholderSize SizeEffective => Size ?? PlaceholderContainer?.Size ?? GetDefaults().Size;
		protected ThemeColor ColorEffective => Color ?? PlaceholderContainer?.Color ?? GetDefaults().Color;
		protected string CssClassEffective => CssClass ?? GetDefaults().CssClass;

		/// <summary>
		/// Return <see cref="HxPlaceholder"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual PlaceholderDefaults GetDefaults() => Defaults;

		protected virtual string GetCssClass()
		{
			return CssClassHelper.Combine(
				"placeholder",
				this.GetColumnsCssClasses(),
				ThemeColorExtensions.ToBackgroundColorCss(ColorEffective),
				GetSizeCssClass(),
				this.CssClassEffective);
		}

		private string GetSizeCssClass()
		{
			return this.SizeEffective switch
			{
				PlaceholderSize.Regular => null,
				PlaceholderSize.Small => "placeholder-sm",
				PlaceholderSize.ExtraSmall => "placeholder-xs",
				PlaceholderSize.Large => "placeholder-lg",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxPlaceholder)}.{nameof(Size)} value {this.Size:g}.")
			};
		}
	}
}
