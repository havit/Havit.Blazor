namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for <see cref="HxListGroup"/>.
/// </summary>
public partial class HxListGroupItem
{
	/// <summary>
	/// Content of the item.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Indicates the currently active selection.
	/// </summary>
	[Parameter] public bool Active { get; set; }

	/// <summary>
	/// Make the item appear disabled by setting it to <c>false</c>.
	/// The default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool Enabled { get; set; } = true;

	/// <summary>
	/// Color.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// An event that is fired when the <c>HxListGroupItem</c> is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	[CascadingParameter] protected HxListGroup ListGroupContainer { get; set; }

	private string GetClasses()
	{
		return CssClassHelper.Combine(
			"list-group-item",
			Active ? "active" : null,
			Enabled ? null : "disabled",
			OnClick.HasDelegate ? "list-group-item-action" : null,
			GetColorCssClass(),
			CssClass);
	}

	private string GetColorCssClass()
	{
		return Color switch
		{
			null => null,
			ThemeColor.None => null,
			_ => "list-group-item-" + Color.Value.ToString("f").ToLower()
		};
	}
}
