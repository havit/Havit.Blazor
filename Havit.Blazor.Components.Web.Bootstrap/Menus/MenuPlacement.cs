namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Placement of the <see cref="HxMenu"/> relative to its toggle.
/// Physical placements (<c>Top</c>, <c>Bottom</c>, <c>Left</c>, <c>Right</c>) are fixed directions regardless of text direction.
/// Logical placements (<c>Start</c>, <c>End</c>) automatically flip based on RTL.
/// All placements support <c>-Start</c> and <c>-End</c> alignment modifiers.
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/menu/#placement"/>.
/// </summary>
public enum MenuPlacement
{
	/// <summary>
	/// Below the toggle, aligned to the start. Bootstrap default.
	/// </summary>
	BottomStart = 0,
	Bottom,
	BottomEnd,
	Top,
	TopStart,
	TopEnd,
	Left,
	LeftStart,
	LeftEnd,
	Right,
	RightStart,
	RightEnd,
	/// <summary>
	/// Left in LTR, right in RTL.
	/// </summary>
	Start,
	StartStart,
	StartEnd,
	/// <summary>
	/// Right in LTR, left in RTL.
	/// </summary>
	End,
	EndStart,
	EndEnd
}
