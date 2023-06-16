namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/placeholders/">Bootstrap 5 placeholder</see> in form of a button.<br/>
/// Derives from <see cref="HxButton"/> (adds <c>placeholder</c> class and changes <c>Enabled</c> default to <c>false</c>).<br/>
/// Set the size of the button by using the <see cref="Columns"/> parameter.
/// </summary>
public class HxPlaceholderButton : HxButton, ILayoutColumnComponent
{
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

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		await base.SetParametersAsync(parameters);

		this.Enabled = parameters.GetValueOrDefault(nameof(Enabled), false);
	}

	protected override string GetButtonCssClass()
	{
		return CssClassHelper.Combine(
			base.GetButtonCssClass(),
			"placeholder",
			this.GetColumnsCssClasses());
	}
}
