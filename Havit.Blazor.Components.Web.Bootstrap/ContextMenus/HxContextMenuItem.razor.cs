using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

public partial class HxContextMenuItem : ComponentBase, ICascadeEnabledComponent
{
	/// <summary>
	/// Item text.
	/// </summary>
	[Parameter] public string Text { get; set; }

	/// <summary>
	/// Custom item content to be rendered.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the menu item.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Item icon (use <see cref="BootstrapIcon" />).
	/// </summary>
	[Parameter] public IconBase Icon { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the context menu item icon.
	/// </summary>
	[Parameter] public string IconCssClass { get; set; }

	/// <summary>
	/// Displays <see cref="HxMessageBox" /> to get a confirmation.
	/// </summary>
	[Parameter] public string ConfirmationQuestion { get; set; }

	/// <summary>
	/// Item clicked event.
	/// </summary>
	[Parameter] public EventCallback OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Stop onClick-event propagation. Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool OnClickStopPropagation { get; set; } = true;

	[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }
	[Inject] protected IJSRuntime JSRuntime { get; set; }
	[Inject] protected IServiceProvider ServiceProvider { get; set; } // optional IHxMessageBoxService, fallback to JS-confirm

	protected IHxMessageBoxService MessageBox { get; set; }

	/// <inheritdoc cref="Web.FormState" />
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	protected override void OnInitialized()
	{
		base.OnInitialized();

		MessageBox = ServiceProvider.GetService<IHxMessageBoxService>(); // conditional, does not have to be registered
	}

	public async Task HandleClick(MouseEventArgs args)
	{
		if (!CascadeEnabledComponent.EnabledEffective(this))
		{
			return;
		}

		if (!String.IsNullOrEmpty(this.ConfirmationQuestion))
		{
			if (MessageBox is not null)
			{
				if (!await MessageBox.ConfirmAsync(this.ConfirmationQuestion))
				{
					return; // No action
				}
			}
			else if (!await JSRuntime.InvokeAsync<bool>("confirm", this.ConfirmationQuestion))
			{
				return; // No Action
			}
		}

		await InvokeOnClickAsync(args);
	}
}