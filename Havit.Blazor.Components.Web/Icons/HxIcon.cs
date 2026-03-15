namespace Havit.Blazor.Components.Web;

/// <summary>
/// Displays an icon.
/// Currently supports <see href="https://icons.getbootstrap.com/" target="_blank">Bootstrap icons</see> through the <c>BootstrapIcon</c> class.<br />
/// Full documentation and demos can be found at <see href="https://havit.blazor.eu/components/HxIcon">https://havit.blazor.eu/components/HxIcon</see>.
/// You can easily add your own icon set.
/// </summary>
public class HxIcon : ComponentBase
{
	/// <summary>
	/// The icon to display.
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Accessibility label for screen readers. Hides the icon from screen readers if not set.
	/// </summary>
	[Parameter] public string AriaLabel { get; set; }

	/// <summary>
	/// The CSS class to combine with the basic icon CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	/// <inheritdoc cref="ComponentBase.BuildRenderTree(RenderTreeBuilder)" />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent(1, Icon.RendererComponentType);
		builder.AddAttribute(2, "Icon", Icon);
		builder.AddAttribute(3, "CssClass", CssClass);
		if (!string.IsNullOrEmpty(AriaLabel))
		{
			builder.AddAttribute(4, "aria-label", AriaLabel);
		}
		else
		{
			builder.AddAttribute(5, "aria-hidden", true);
		}
		builder.AddMultipleAttributes(3, AdditionalAttributes);
		builder.CloseComponent();
	}
}
