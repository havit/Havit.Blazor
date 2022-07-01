namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Displays an icon.
	/// Currently supports <see href="https://icons.getbootstrap.com" target="_blank">Bootstrap icons</see> through <c>BootstrapIcon</c> class.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxIcon">https://havit.blazor.eu/components/HxIcon</see>
	/// You can add your own icon-set easily.
	/// </summary>
	public class HxIcon : ComponentBase
	{
		/// <summary>
		/// Icon to display.
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// CSS Class to combine with basic icon CSS class.
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
			builder.AddAttribute(2, "CssClass", CssClass);
			builder.AddMultipleAttributes(3, AdditionalAttributes);
			builder.CloseComponent();
		}
	}
}
