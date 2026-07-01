namespace Havit.Blazor.Components.Web.Bootstrap;

public static class MenuPlacementExtensions
{
	/// <summary>
	/// Gets the <c>data-bs-placement</c> attribute value for the specified <see cref="MenuPlacement"/>.
	/// </summary>
	public static string ToDataBsPlacement(this MenuPlacement placement)
	{
		return placement switch
		{
			MenuPlacement.BottomStart => "bottom-start",
			MenuPlacement.Bottom => "bottom",
			MenuPlacement.BottomEnd => "bottom-end",
			MenuPlacement.Top => "top",
			MenuPlacement.TopStart => "top-start",
			MenuPlacement.TopEnd => "top-end",
			MenuPlacement.Left => "left",
			MenuPlacement.LeftStart => "left-start",
			MenuPlacement.LeftEnd => "left-end",
			MenuPlacement.Right => "right",
			MenuPlacement.RightStart => "right-start",
			MenuPlacement.RightEnd => "right-end",
			MenuPlacement.Start => "start",
			MenuPlacement.StartStart => "start-start",
			MenuPlacement.StartEnd => "start-end",
			MenuPlacement.End => "end",
			MenuPlacement.EndStart => "end-start",
			MenuPlacement.EndEnd => "end-end",
			_ => throw new InvalidOperationException($"Unknown {nameof(MenuPlacement)} value {placement}.")
		};
	}
}
