namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Temporary (?) NavLink component to be used with <see cref="HxScrollspy"/> where <c>#id</c> anchors are required and <c>page-route#id</c> cannot be used.
/// </summary>
public partial class HxScrollspyNavLink
{
	/// <summary>
	/// The navigation target in <c>#id</c> form.
	/// </summary>
	[Parameter, EditorRequired] public string Href { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Raised when the item is clicked (before the navigation location is changed to <see cref="Href"/>).
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <c>&lt;a&gt;</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>((Href is not null) && (Href.StartsWith("#")), $"{nameof(HxScrollspyNavLink)}.{nameof(Href)} has to start with #. Use only for local elements.");
	}

	private async Task HandleClick(MouseEventArgs args)
	{
		await InvokeOnClickAsync(args);
		var targetUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path) + Href;
		NavigationManager.NavigateTo(targetUri);
	}
}
