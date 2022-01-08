using Havit.Blazor.Components.Web.Bootstrap.Forms;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Displays a read-only value in the form control visual (as <c>.form-control</c>, with label, border, etc.).
	/// </summary>
	public class HxFormValue : ComponentBase, IFormValueComponent, IFormValueComponentWithInputGroups, IInputWithSize
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxFormValue"/> and derived components.
		/// </summary>
		public static FormValueSettings Defaults { get; set; }

		static HxFormValue()
		{
			Defaults = new FormValueSettings()
			{
				InputSize = Bootstrap.InputSize.Regular,
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual FormValueSettings GetDefaults() => Defaults;
		IInputSettingsWithSize IInputWithSize.GetDefaults() => GetDefaults(); // might be replaced with C# vNext convariant return types on interfaces
		IInputSettingsWithSize IInputWithSize.GetSettings() => this.Settings;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public FormValueSettings Settings { get; set; }

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
		/// Template to render value.
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

		/// <inheritdoc cref="IInputWithSize.InputSize" />
		[Parameter] public InputSize? InputSize { get; set; }


		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenRegion(0);
			base.BuildRenderTree(builder);
			builder.CloseRegion();

			HxFormValueRenderer.Current.Render(1, builder, this);
		}

		/// <inheritdoc />
		public void RenderValue(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "div");
			builder.AddAttribute(1, "class", CssClassHelper.Combine("form-control", ((IInputWithSize)this).GetInputSizeCssClass(), ValueCssClass));
			builder.AddContent(2, Value);
			builder.AddContent(3, ValueTemplate);
			builder.CloseElement();
		}
	}
}
