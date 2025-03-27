using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Displays a read-only value in the form control visual (as <c>.form-control</c>, with label, border, etc.).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxFormValue">https://havit.blazor.eu/components/HxFormValue</see>
/// </summary>
public class HxFormValue : ComponentBase, IFormValueComponent, IFormValueComponentWithInputGroups, IInputWithSize
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxFormValue"/> and derived components.
	/// </summary>
	public static FormValueSettings Defaults { get; set; }

	static HxFormValue()
	{
		Defaults = new FormValueSettings();
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual FormValueSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public FormValueSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual FormValueSettings GetSettings() => Settings;


	/// <inheritdoc cref="IFormValueComponent.CssClass" />
	[Parameter] public string CssClass { get; set; }

	/// <inheritdoc cref="IFormValueComponent.Label" />
	[Parameter] public string Label { get; set; }

	/// <inheritdoc cref="IFormValueComponent.LabelTemplate" />
	[Parameter] public RenderFragment LabelTemplate { get; set; }

	/// <inheritdoc cref="IFormValueComponent.LabelCssClass" />
	[Parameter] public string LabelCssClass { get; set; }

	/// <inheritdoc cref="IFormValueComponent.Hint" />
	[Parameter] public string Hint { get; set; }

	/// <inheritdoc cref="IFormValueComponent.HintTemplate" />
	[Parameter] public RenderFragment HintTemplate { get; set; }

	/// <summary>
	/// Value to be presented.
	/// </summary>
	[Parameter] public string Value { get; set; }

	/// <summary>
	/// Template to render the value.
	/// </summary>
	[Parameter] public RenderFragment ValueTemplate { get; set; }

	/// <summary>
	/// Custom CSS class to render with the value.
	/// </summary>
	[Parameter] public string ValueCssClass { get; set; }

	/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupStartText" />
	[Parameter] public string InputGroupStartText { get; set; }

	/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupStartTemplate" />
	[Parameter] public RenderFragment InputGroupStartTemplate { get; set; }

	/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupEndText"/>
	[Parameter] public string InputGroupEndText { get; set; }

	/// <inheritdoc cref="IFormValueComponentWithInputGroups.InputGroupEndTemplate" />
	[Parameter] public RenderFragment InputGroupEndTemplate { get; set; }

	/// <summary>
	/// Size of the input.
	/// </summary>
	[Parameter] public InputSize? InputSize { get; set; }
	protected InputSize InputSizeEffective => InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? HxSetup.Defaults.InputSize;
	InputSize IInputWithSize.InputSizeEffective => InputSizeEffective;


	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenRegion(0);
		base.BuildRenderTree(builder);
		builder.CloseRegion();

		FormValueRenderer.Current.Render(1, builder, this);
	}

	/// <inheritdoc />
	public void RenderValue(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClassHelper.Combine("form-control", ((IInputWithSize)this).GetInputSizeCssClass(), ValueCssClass));
		builder.AddAttribute(2, "aria-readonly", "true");
		builder.AddAttribute(3, "tabindex", 0);
		builder.AddContent(4, Value);
		builder.AddContent(5, ValueTemplate);
		if (String.IsNullOrWhiteSpace(Value) && (ValueTemplate == null))
		{
			// workaround for [HxFormValue] Shrunk when displaying null/empty value #208
			builder.AddMarkupContent(4, "&nbsp;");
		}
		builder.CloseElement();
	}
}
