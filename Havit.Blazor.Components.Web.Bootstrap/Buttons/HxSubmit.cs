namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Button <c>&lt;button type="submit"&gt;</c>.
	/// Direct ancestor of <see cref="HxButton"/> with the same API.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxSubmit#HxSubmit">https://havit.blazor.eu/components/HxSubmit#HxSubmit</see>
	/// </summary>
	public class HxSubmit : HxButton
	{
		private protected override string GetButtonType() => "submit";
	}
}
