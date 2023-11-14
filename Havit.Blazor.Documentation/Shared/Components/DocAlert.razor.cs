namespace Havit.Blazor.Documentation.Shared.Components;

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
	/// Additional attributes to be splatted onto an underlying <c>HxAlert</c> component.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

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
		return Type switch
		{
			DocAlertType.Info => ThemeColor.Info,
			DocAlertType.Warning => ThemeColor.Warning,
			DocAlertType.Danger => ThemeColor.Danger,
			_ => ThemeColor.Info,
		};
	}
}
