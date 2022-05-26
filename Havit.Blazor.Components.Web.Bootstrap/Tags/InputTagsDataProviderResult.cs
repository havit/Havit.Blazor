namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Data provider result for <see cref="HxInputTags"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/InputTagsDataProviderResult">https://havit.blazor.eu/types/InputTagsDataProviderResult</see>
	/// </summary>
	public class InputTagsDataProviderResult
	{
		/// <summary>
		/// The provided items by the request.
		/// </summary>
		public IEnumerable<string> Data { get; set; }
	}
}