namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Specifies the placement of the image in the <see cref="HxCard"/> component.
/// </summary>
public enum CardImagePlacement
{
	/// <summary>
	/// Image before the body (<c>card-img-top</c>).
	/// In a horizontal card (<see cref="HxCard.Horizontal"/>), the image is placed at the (inline) start of the card (<c>card-img-start</c>).
	/// </summary>
	Top = 0,

	/// <summary>
	/// Image after the body (<c>card-img-bottom</c>).
	/// In a horizontal card (<see cref="HxCard.Horizontal"/>), the image is placed at the (inline) end of the card (<c>card-img-end</c>).
	/// </summary>
	Bottom = 1,

	/// <summary>
	/// Image as a card background, the body is rendered as an overlay (<c>card-img-overlay</c>).
	/// </summary>
	Overlay = 2
}
