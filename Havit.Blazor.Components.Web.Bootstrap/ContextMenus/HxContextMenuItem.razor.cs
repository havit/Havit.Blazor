using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
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
		/// Item icon (use <see cref="BootstrapIcon" />).
		/// </summary>
		[Parameter] public IconBase Icon { get; set; }

		/// <summary>
		/// Displays <see cref="HxMessageBox" /> to get a confirmation.
		/// </summary>
		[Parameter] public string ConfirmationQuestion { get; set; }

		/// <summary>
		/// Skin of the menu item. Simplifies reuse of item properties.
		/// </summary>
		[Parameter] public ContextMenuItemSkin Skin { get; set; }

		/// <summary>
		/// Item clicked event.
		/// </summary>
		[Parameter] public EventCallback OnClick { get; set; }

		/// <summary>
		/// Stop onClick-event propagation. Deafult is <c>true</c>.
		/// </summary>
		[Parameter] public bool OnClickStopPropagation { get; set; } = true;

		[Inject] protected IStringLocalizerFactory StringLocalizerFactory { get; set; }
		[Inject] protected IJSRuntime JSRuntime { get; set; }
		[Inject] protected IServiceProvider ServiceProvider { get; set; } // optional IHxMessageBoxService, fallback to JS-confirm

		protected IHxMessageBoxService MessageBox { get; set; }

		/// <inheritdoc />
		[CascadingParameter] protected FormState FormState { get; set; }
		FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

		/// <inheritdoc />
		[Parameter] public bool? Enabled { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

			MessageBox = ServiceProvider.GetService<IHxMessageBoxService>(); // conditional, does not have to be registered
		}

		public async Task HandleClick()
		{
			if (!CascadeEnabledComponent.EnabledEffective(this))
			{
				return;
			}

			if (!String.IsNullOrEmpty(GetConfirmationQuestion()))
			{
				if (MessageBox is not null)
				{
					if (!await MessageBox.ConfirmAsync(this.ConfirmationQuestion))
					{
						return; // No action
					}
				}
				else if (!await JSRuntime.InvokeAsync<bool>("confirm", GetConfirmationQuestion()))
				{
					return; // No Action
				}
			}
			await OnClick.InvokeAsync(null);
		}

		protected string GetText()
		{
			if (!String.IsNullOrEmpty(Text))
			{
				return this.Text;
			}
			else if (!String.IsNullOrEmpty(Skin?.Text))
			{
				return StringLocalizerFactory.GetLocalizedValue(Skin.Text, Skin.ResourceType);
			}
			return null;
		}

		protected string GetConfirmationQuestion()
		{
			if (!String.IsNullOrEmpty(ConfirmationQuestion))
			{
				return this.ConfirmationQuestion;
			}
			else if (!String.IsNullOrEmpty(Skin?.ConfirmationQuestion))
			{
				return StringLocalizerFactory.GetLocalizedValue(Skin.ConfirmationQuestion, Skin.ResourceType);
			}
			return null;
		}

		protected IconBase GetIcon()
		{
			return Icon ?? Skin?.Icon;
		}

	}
}