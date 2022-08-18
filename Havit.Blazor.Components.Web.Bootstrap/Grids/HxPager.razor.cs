using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Pager.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxPager">https://havit.blazor.eu/components/HxPager</see>
	/// </summary>
	public partial class HxPager : ComponentBase
	{
		/// <summary>
		/// Total pages of data items.
		/// </summary>
		[Parameter] public int TotalPages { get; set; }  // TODO RH: Divide to na TotalItems a PageSize?

		/// <summary>
		/// Current page index. Zero based.
		/// Displayed numbers start with 1 which is page index 0.
		/// </summary>
		[Parameter] public int CurrentPageIndex { get; set; }  // TODO RH: ActivePageIndex?

		/// <summary>
		/// Event raised where page index is changed.
		/// </summary>
		[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }
		/// <summary>
		/// Triggers the <see cref="CurrentPageIndexChanged"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeCurrentPageIndexChangedAsync(int newPageIndex) => CurrentPageIndexChanged.InvokeAsync(newPageIndex);

		/// <summary>
		/// Count of numbers to display. Default value is 10.
		/// </summary>
		[Parameter] public int DisplayNumberCount { get; set; } = 10; // TODO RH: Rename NumberCount? or NumericButtonsCount #57515 Doc - Titles?

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Changes current page index and fires event.
		/// </summary>
		protected async Task SetCurrentPageIndexTo(int newPageIndex)
		{
			Contract.Requires(newPageIndex >= 0, nameof(newPageIndex));
			Contract.Requires(newPageIndex < TotalPages, nameof(newPageIndex));

			CurrentPageIndex = newPageIndex;
			await InvokeCurrentPageIndexChangedAsync(CurrentPageIndex);
		}
	}
}
