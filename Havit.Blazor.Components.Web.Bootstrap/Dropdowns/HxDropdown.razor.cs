using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap 5 Dropdown</see> component.
	/// </summary>
	public partial class HxDropdown : IAsyncDisposable
	{
		[Parameter] public DropdownDirection Direction { get; set; }

		/// <summary>
		/// Set <c>true</c> to create a <a href="https://getbootstrap.com/docs/5.1/components/dropdowns/#split-button">split dropdown</a>
		/// (using a <c>btn-group</c>).
		/// </summary>
		[Parameter] public bool Split { get; set; }

		/// <summary>
		/// By default, the dropdown menu is closed when clicking inside or outside the dropdown menu (<see cref="DropdownAutoClose.True"/>).
		/// You can use the AutoClose parameter to change this behavior of the dropdown.
		/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/#auto-close-behavior"/>.
		/// </summary>
		[Parameter] public DropdownAutoClose AutoClose { get; set; } = DropdownAutoClose.True;

		/// <summary>
		/// Any additional CSS class to apply.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[CascadingParameter] protected HxNavbar NavbarContainer { get; set; }

		/// <summary>
		/// Raised after the dropdown is shown.
		/// </summary>
		[Parameter] public EventCallback<string> OnShown { get; set; }

		/// <summary>
		/// Raised after the dropdown is hidden.
		/// </summary>
		[Parameter] public EventCallback<string> OnHidden { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }
		[Inject] protected ILogger<HxDropdown> Logger { get; set; }

		private ElementReference dropdownElement;
		private DotNetObjectReference<HxDropdown> dotnetObjectReference;
		private IJSObjectReference jsModule;

		public bool IsOpen { get; private set; }

		public HxDropdown()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			if (firstRender)
			{
				Logger.LogDebug($"OnAfterRenderAsync_create");
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("create", dropdownElement, dotnetObjectReference);
			}
		}

		/// <summary>
		/// Receives notification from javascript when dropdown is shown.
		/// </summary>
		/// <remarks>
		/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
		/// </remarks>
		[JSInvokable("HxDropdown_HandleJsShown")]
		public async Task HandleJsShown()
		{
			Logger.LogDebug($"HandleJsShown");
			IsOpen = true;
			await OnShown.InvokeAsync();
			StateHasChanged();
		}

		/// <summary>
		/// Receives notification from javascript when item is hidden.
		/// </summary>
		[JSInvokable("HxDropdown_HandleJsHidden")]
		public async Task HandleJsHidden()
		{
			Logger.LogDebug($"HandleJsHidden");
			IsOpen = false;
			await OnHidden.InvokeAsync();
			StateHasChanged();
		}

		protected string GetDropdownDirectionCssClass()
		{
			return this.Direction switch
			{
				DropdownDirection.Down => "dropdown",
				DropdownDirection.Up => "dropup",
				DropdownDirection.Start => "dropstart",
				DropdownDirection.End => "dropend",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownDirection)} value {Direction}.")
			};
		}

		protected string GetCssClass()
		{
			/*
				.btn-group
				The basic.dropdown class brings just position:relative requirement(+ directions within other variations).
				.btn-group is needed for Split buttons AND brings default in-row positioning to behave like regular buttons.
				It is used in almost all Bootstrap samples. Might be replaced with more appropriate class if needed.
				.btn-group cannot be used in Navbar as it breaks responsiveness.
			*/
			return CssClassHelper.Combine(
				GetDropdownDirectionCssClass(),
				((this.NavbarContainer is null) || this.Split) ? "btn-group" : null,
				this.CssClass);
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/Havit.Blazor.Components.Web.Bootstrap/{nameof(HxDropdown)}.js");
		}

		/// <inheritdoc/>
		public async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", dropdownElement);
				await jsModule.DisposeAsync();
			}

			dotnetObjectReference.Dispose();
		}

	}
}
