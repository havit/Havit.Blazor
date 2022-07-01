﻿using Havit.Blazor.Components.Web.Bootstrap.Layouts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/placeholders">Bootstrap 5 Placeholder</see> component, aka Skeleton.<br/>
	/// Use loading placeholders for your components or pages to indicate something may still be loading.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPlaceholder">https://havit.blazor.eu/components/HxPlaceholder</see>
	/// </summary>
	public partial class HxPlaceholder : ILayoutColumnComponent
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxPlaceholder"/> and derived components.
		/// </summary>
		public static PlaceholderSettings Defaults { get; set; }

		static HxPlaceholder()
		{
			Defaults = new PlaceholderSettings()
			{
				Color = ThemeColor.None,
				Size = PlaceholderSize.Regular,
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual PlaceholderSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public PlaceholderSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual PlaceholderSettings GetSettings() => this.Settings;


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
		protected PlaceholderSize SizeEffective => this.Size ?? this.GetSettings()?.Size ?? PlaceholderContainer?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxPlaceholder) + " has to be set.");

		/// <summary>
		/// Color of the placeholder.
		/// </summary>
		[Parameter] public ThemeColor? Color { get; set; }
		protected ThemeColor ColorEffective => this.Color ?? this.GetSettings()?.Color ?? PlaceholderContainer?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxPlaceholder) + " has to be set.");

		/// <summary>
		/// Optional content of the placeholder (usualy not used).
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }
		protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

		/// <summary>
		/// Additional attributes to be splatted onto an underlying HTML element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		[CascadingParameter] protected HxPlaceholderContainer PlaceholderContainer { get; set; }

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
				_ => throw new InvalidOperationException($"Unknown {nameof(HxPlaceholder)}.{nameof(Size)} value {this.SizeEffective:g}.")
			};
		}
	}
}
