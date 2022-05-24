namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/accordion/">Bootstrap accordion</see> component.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAccordion">https://havit.blazor.eu/components/HxAccordion</see>
	/// </summary>
	public partial class HxAccordion
	{
		/// <summary>
		/// Additional CSS classes for the accordion container.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// ID of the expanded item.
		/// Do not use constant value as it reverts the accordion to that item on every roundtrip. Use <see cref="InitialExpandedItemId"/> to set the initial state.
		/// </summary>
		[Parameter] public string ExpandedItemId { get; set; }
		[Parameter] public EventCallback<string> ExpandedItemIdChanged { get; set; }
		protected virtual Task InvokeExpandedItemIdChangedAsync(string newExpandedItemId) => ExpandedItemIdChanged.InvokeAsync(newExpandedItemId);

		/// <summary>
		/// Set to <c>true</c> to make accordion items stay open when another item is opened.
		/// Default is <c>false</c>, openning another item collapses current item.
		/// </summary>
		[Parameter] public bool StayOpen { get; set; }

		/// <summary>
		/// ID of the item you want to expand at the very beginning (overwrites <see cref="ExpandedItemId"/> if set).
		/// </summary>
		[Parameter] public string InitialExpandedItemId { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		protected internal string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (!String.IsNullOrWhiteSpace(InitialExpandedItemId))
			{
				await SetExpandedItemIdAsync(InitialExpandedItemId);
			}
		}

		internal async Task SetExpandedItemIdAsync(string newId)
		{
			if (this.ExpandedItemId != newId)
			{
				ExpandedItemId = newId;
				await InvokeExpandedItemIdChangedAsync(newId);
			}
		}
	}
}
