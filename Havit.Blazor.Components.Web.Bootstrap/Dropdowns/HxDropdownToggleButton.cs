using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap Dropdown</see> toggle button which triggers the <see cref="HxDropdown"/> to open.
	/// </summary>
	public class HxDropdownToggleButton : HxButton, IAsyncDisposable, IHxDropdownToggle
	{
		/// <summary>
		/// Application-wide defaults for <see cref="HxDropdownToggleButton"/> and derived components.
		/// </summary>
		public static new ButtonSettings Defaults { get; set; }

		static HxDropdownToggleButton()
		{
			Defaults = HxButton.Defaults with
			{
				Color = ThemeColor.Link
			};
		}

		/// <inheritdoc cref="HxButton.GetDefaults"/>
		protected override ButtonSettings GetDefaults() => Defaults;

		/// <summary>
		/// Offset <c>(<see href="https://popper.js.org/docs/v2/modifiers/offset/#skidding-1">skidding</see>, <see href="https://popper.js.org/docs/v2/modifiers/offset/#distance-1">distance</see>)</c>
		/// of the dropdown relative to its target.  Default is <c>(0, 2)</c>.
		/// </summary>
		[Parameter] public (int Skidding, int Distance)? DropdownOffset { get; set; }

		/// <summary>
		/// Reference element of the dropdown menu. Accepts the values of <c>toggle</c> (default), <c>parent</c>,
		/// an HTMLElement reference (e.g. <c>#id</c>) or an object providing <c>getBoundingClientRect</c>.
		/// For more information refer to Popper's <see href="https://popper.js.org/docs/v2/constructors/#createpopper">constructor docs</see>
		/// and <see href="https://popper.js.org/docs/v2/virtual-elements/">virtual element docs</see>.
		/// </summary>
		[Parameter] public string DropdownReference { get; set; }

		/// <summary>
		/// Fired when the dropdown has been made visible to the user and CSS transitions have completed.
		/// </summary>
		[Parameter] public EventCallback OnShown { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

		/// <summary>
		/// Fired when the dropdown has finished being hidden from the user and CSS transitions have completed.
		/// </summary>
		[Parameter] public EventCallback OnHidden { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnHidden"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnHiddenAsync() => OnHidden.InvokeAsync();

		[CascadingParameter] protected HxDropdown DropdownContainer { get; set; }
		[CascadingParameter] protected HxNav NavContainer { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private DotNetObjectReference<HxDropdownToggleButton> dotnetObjectReference;
		private IJSObjectReference jsModule;
		private bool disposed;

		public HxDropdownToggleButton()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		protected override void OnParametersSet()
		{
			if ((Color is null) && (NavContainer is not null))
			{
				Color = ThemeColor.Link;
			}

			base.OnParametersSet();

			AdditionalAttributes ??= new Dictionary<string, object>();
			AdditionalAttributes["data-bs-toggle"] = "dropdown";
			AdditionalAttributes["aria-expanded"] = "false";
			AdditionalAttributes["data-bs-auto-close"] = (DropdownContainer?.AutoClose ?? DropdownAutoClose.True) switch
			{
				DropdownAutoClose.True => "true",
				DropdownAutoClose.False => "false",
				DropdownAutoClose.Inside => "inside",
				DropdownAutoClose.Outside => "outside",
				_ => throw new InvalidOperationException($"Unknown {nameof(DropdownAutoClose)} value {DropdownContainer.AutoClose}.")
			};

			if (this.DropdownOffset is not null)
			{
				AdditionalAttributes["data-bs-offset"] = $"{DropdownOffset.Value.Skidding},{DropdownOffset.Value.Distance}";
			}

			if (!String.IsNullOrWhiteSpace(this.DropdownReference))
			{
				AdditionalAttributes["data-bs-reference"] = this.DropdownReference;
			}
		}

		protected override string CoreCssClass =>
			CssClassHelper.Combine(
				base.CoreCssClass,
				"dropdown-toggle",
				((DropdownContainer as IDropdownContainer)?.IsOpen ?? false) ? "show" : null,
				(DropdownContainer?.Split ?? false) ? "dropdown-toggle-split" : null,
				(NavContainer is not null) ? "nav-link" : null);


		/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			if (firstRender)
			{
				await EnsureJsModuleAsync();
				if (disposed)
				{
					return;
				}
				await jsModule.InvokeVoidAsync("create", buttonElementReference, dotnetObjectReference);
			}
		}

		/// <summary>
		/// Shows the dropdown.
		/// </summary>
		public async Task ShowAsync()
		{
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("show", buttonElementReference);
		}

		/// <summary>
		/// Hides the dropdown.
		/// </summary>
		public async Task HideAsync()
		{
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("hide", buttonElementReference);
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
			((IDropdownContainer)DropdownContainer).IsOpen = true;
			await InvokeOnShownAsync();
		}

		/// <summary>
		/// Receives notification from javascript when item is hidden.
		/// </summary>
		[JSInvokable("HxDropdown_HandleJsHidden")]
		public async Task HandleJsHidden()
		{
			((IDropdownContainer)DropdownContainer).IsOpen = false;
			await InvokeOnHiddenAsync();
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", $"./_content/Havit.Blazor.Components.Web.Bootstrap/{nameof(HxDropdown)}.js");
		}

		/// <inheritdoc/>
		public virtual async ValueTask DisposeAsync()
		{
			disposed = true;

			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", buttonElementReference);
				await jsModule.DisposeAsync();
			}

			dotnetObjectReference.Dispose();
		}
	}
}
