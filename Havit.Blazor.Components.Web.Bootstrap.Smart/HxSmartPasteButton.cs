namespace Havit.Blazor.Components.Web.Bootstrap.Smart;

/// <summary>
/// Smart Paste is an intelligent AI-powered app feature that fills out forms automatically using data
/// from the user's clipboard. You can use this with any existing form in your web app.<br />
/// <code>HxSmartPasteButton</code> derives from <see href="https://github.com/dotnet-smartcomponents/smartcomponents/blob/main/docs/smart-paste.md">SmartPasteButton</see>,
/// a component created by the Microsoft Blazor team.
/// It extends the original component with Bootstrap styling and Hx-component features.
/// </summary>
public class HxSmartPasteButton : SmartPasteButton
{
	// TODO <button title="..." />

	// Used to hide SmartPasteButton.DefaultIcon from parameter-list.
	public new bool DefaultIcon { get; set; }

	/// <summary>
	/// Application-wide defaults for <see cref="HxSmartPasteButton"/>.
	/// Please note, the defaults are not shared with <see cref="HxButton"/>.
	/// </summary>
	public static ButtonSettings Defaults { get; set; }

	static HxSmartPasteButton()
	{
		Defaults = new ButtonSettings()
		{
			Size = ButtonSize.Regular,
			IconPlacement = ButtonIconPlacement.Start,
			Color = ThemeColor.None,
			CssClass = null,
			Outline = false,
			Icon = null
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use a separate set of defaults).
	/// </summary>
	protected virtual ButtonSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public ButtonSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in component descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ButtonSettings GetSettings() => Settings;

	/// <summary>
	/// Text of the button.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Icon to render into the button.
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }
	protected IconBase IconEffective => Icon ?? GetSettings()?.Icon ?? GetDefaults().Icon;

	/// <summary>
	/// Position of the icon within the button. The default is <see cref="ButtonIconPlacement.Start" /> (configurable through <see cref="HxSmartPasteButton.Defaults"/>).
	/// </summary>
	[Parameter] public ButtonIconPlacement? IconPlacement { get; set; }
	protected ButtonIconPlacement IconPlacementEffective => IconPlacement ?? GetSettings()?.IconPlacement ?? GetDefaults()?.IconPlacement ?? throw new InvalidOperationException(nameof(IconPlacement) + " default for " + nameof(HxSmartPasteButton) + " has to be set.");

	/// <summary>
	/// Bootstrap button style - theme color.<br />
	/// The default is taken from <see cref="HxSmartPasteButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }
	protected ThemeColor ColorEffective => Color ?? GetSettings()?.Color ?? GetDefaults().Color ?? throw new InvalidOperationException(nameof(Color) + " default for " + nameof(HxSmartPasteButton) + " has to be set.");

	/// <summary>
	/// Button size. The default is <see cref="ButtonSize.Regular"/>.
	/// </summary>
	[Parameter] public ButtonSize? Size { get; set; }
	protected ButtonSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxSmartPasteButton) + " has to be set.");

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
	/// </summary>
	[Parameter] public bool? Outline { get; set; }
	protected bool OutlineEffective => Outline ?? GetSettings()?.Outline ?? GetDefaults().Outline ?? throw new InvalidOperationException(nameof(Outline) + " default for " + nameof(HxSmartPasteButton) + " has to be set.");

	/// <summary>
	/// Custom CSS class to render with the <c>&lt;button /&gt;</c>.<br />
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// CSS class to be rendered with the button icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }
	protected string IconCssClassEffective => IconCssClass ?? GetSettings()?.IconCssClass ?? GetDefaults().IconCssClass;

	/// <summary>
	/// Enables or disables the spinner. <br/>
	/// Leave <c>null</c> if you want automated spinner.
	/// You can set an explicit <c>false</c> constant to disable (override) the spinner automation.
	/// </summary>
	[Parameter] public bool? Spinner { get; set; }

	public override Task SetParametersAsync(ParameterView parameters)
	{
		RenderFragment childContentParameter = null;
		IReadOnlyDictionary<string, object> additionalAttributesParameter = null;

		foreach (var parameter in parameters)
		{
			switch (parameter.Name)
			{
				case nameof(Settings):
					Settings = (ButtonSettings)parameter.Value;
					break;
				case nameof(Text):
					Text = (string)parameter.Value;
					break;
				case nameof(ChildContent):
					childContentParameter = (RenderFragment)parameter.Value;
					break;
				case nameof(Icon):
					Icon = (IconBase)parameter.Value;
					break;
				case nameof(IconPlacement):
					IconPlacement = (ButtonIconPlacement)parameter.Value;
					break;
				case nameof(Color):
					Color = (ThemeColor)parameter.Value;
					break;
				case nameof(Size):
					Size = (ButtonSize)parameter.Value;
					break;
				case nameof(Outline):
					Outline = (bool)parameter.Value;
					break;
				case nameof(AdditionalAttributes):
					additionalAttributesParameter = (Dictionary<string, object>)parameter.Value;
					break;
				default:
					throw new InvalidOperationException($"Parameter {parameter.Name} not supported.");
			}
		}

		var newAdditionalAttributes = new Dictionary<string, object>();
		if (additionalAttributesParameter != null)
		{
			foreach (var parameter in additionalAttributesParameter)
			{
				switch (parameter.Key)
				{
					case "class":
						// NOOP
						break;
					default:
						newAdditionalAttributes.Add(parameter.Key, parameter.Value);
						break;
				}
			}
		}

		newAdditionalAttributes["class"] = GetButtonCssClass();
		AdditionalAttributes = newAdditionalAttributes;

		base.ChildContent = BuildChildContent(childContentParameter);

		return base.SetParametersAsync(ParameterView.Empty);
	}

	private RenderFragment BuildChildContent(RenderFragment childContent) => (builder) =>
	{
		if ((IconPlacementEffective == ButtonIconPlacement.Start)
			&& ((IconEffective is not null) || (Spinner ?? true)))
		{
			if (IconEffective is not null)
			{
				builder.OpenComponent(0, typeof(HxIcon));
				builder.AddAttribute(1, nameof(HxIcon.Icon), IconEffective);
				builder.AddAttribute(2, nameof(HxIcon.CssClass), CssClassHelper.Combine(IconCssClassEffective, "smart-paste-icon-normal"));
				builder.CloseComponent();
			}

			builder.OpenElement(3, "span");
			builder.AddAttribute(4, "class", "smart-paste-icon-running");
			builder.OpenComponent(5, typeof(HxSpinner));
			builder.AddAttribute(6, nameof(HxSpinner.Size), SpinnerSize.Small);
			builder.CloseComponent();
			builder.CloseElement();


			if (!String.IsNullOrEmpty(Text) || (ChildContent != null))
			{
				builder.OpenElement(10, "span");
				//builder.AddAttribute(11, "class", "hx-button-icon-text-spacer");
				builder.AddMarkupContent(11, "&nbsp;"); // TODO spacer
				builder.CloseElement();
			}
		}

		if (Text != null)
		{
			builder.AddContent(20, Text);
		}
		else if (childContent != null)
		{
			builder.AddContent(21, childContent);
		}

		if ((IconPlacementEffective == ButtonIconPlacement.End)
			&& ((IconEffective is not null) || (Spinner ?? true)))
		{
			if (!String.IsNullOrEmpty(Text) || (ChildContent != null))
			{
				builder.OpenElement(110, "span");
				//builder.AddAttribute(111, "class", "hx-button-icon-text-spacer");
				builder.AddMarkupContent(111, "&nbsp;"); // TODO spacer
				builder.CloseElement();
			}

			if (IconEffective is not null)
			{
				builder.OpenComponent(100, typeof(HxIcon));
				builder.AddAttribute(101, nameof(HxIcon.Icon), IconEffective);
				builder.AddAttribute(102, nameof(HxIcon.CssClass), CssClassHelper.Combine(IconCssClassEffective, "smart-paste-icon-normal"));
				builder.CloseComponent();
			}

			builder.OpenElement(103, "span");
			builder.AddAttribute(104, "class", "smart-paste-icon-running");
			builder.OpenComponent(105, typeof(HxSpinner));
			builder.AddAttribute(106, nameof(HxSpinner.Size), SpinnerSize.Small);
			builder.CloseComponent();
			builder.CloseElement();
		}
	};

	/// <summary>
	/// Gets the basic CSS class(es) which get rendered to every single button. <br/>
	/// The default implementation is <c>"hx-button btn"</c>.
	/// </summary>
	protected virtual string CoreCssClass => "hx-button btn";

	protected virtual string GetButtonCssClass()
	{
		return CssClassHelper.Combine(
			CoreCssClass,
			ColorEffective.ToButtonColorCss(OutlineEffective),
			SizeEffective.ToButtonSizeCssClass(),
			CssClassEffective);
	}
}
