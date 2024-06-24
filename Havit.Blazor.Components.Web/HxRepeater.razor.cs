namespace Havit.Blazor.Components.Web;

/// <summary>
/// A data-bound list component that allows custom layout by repeating a specified template for each item displayed in the list.
/// Analogous to the ASP.NET WebForms Repeater control.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxRepeater">https://havit.blazor.eu/components/HxRepeater</see>
/// </summary>
/// <typeparam name="TItem">The type of the item</typeparam>
public partial class HxRepeater<TItem> : ComponentBase
{
	/// <summary>
	/// The template that defines how the header section of the Repeater component is displayed.
	/// Renders only if there are any items to display.
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// The template that defines how items in the Repeater component are displayed.
	/// </summary>
	[Parameter, EditorRequired] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// The template that defines how the footer section of the Repeater component is displayed.
	/// Renders only if there are any items to display.
	/// </summary>
	[Parameter] public RenderFragment FooterTemplate { get; set; }

	/// <summary>
	/// The template that defines what should be rendered in case of Items being null.
	/// </summary>
	[Parameter] public RenderFragment NullTemplate { get; set; }

	/// <summary>
	/// The template that defines what should be rendered in case of empty Items.
	/// </summary>
	[Parameter] public RenderFragment EmptyTemplate { get; set; }

	/// <summary>
	/// The items to be rendered.
	/// </summary>
	[Parameter, EditorRequired] public IEnumerable<TItem> Data { get; set; }
}
