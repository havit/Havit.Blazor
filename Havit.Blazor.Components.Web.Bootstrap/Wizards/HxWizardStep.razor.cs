using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Wizard Step component
	/// </summary>
	public partial class HxWizardStep : ComponentBase, IDisposable, IHxWizardStep
	{
		/*/// <summary>
		/// The Name of the step
		///</summary>
		[Parameter]
		public string Name { get; set; }*/

		/// <summary>
		/// The Child Content of the current <see cref="WizardStep" />
		/// </summary>
		[Parameter]
		public RenderFragment Content { get; set; }

		[CascadingParameter(Name = "WizardStepsRegistration")] // TODO Konstanta
		public CollectionRegistration<IHxWizardStep> StepsRegistration { get; set; }

		[Parameter]
		public bool IsActive { get; set; } // TODO: nebo activestep? Nedělal bych obojí. Ale jak zvenku nastavit aktuální krok?

		[Parameter]
		public EventCallback<bool> IsActiveChanged { get; set; }

		public async Task Activate()
		{
			if (!IsActive)
			{
				IsActive = true;
				await IsActiveChanged.InvokeAsync(true);
			}
		}

		public async Task Deactivate()
		{
			if (IsActive)
			{
				IsActive = false;
				await IsActiveChanged.InvokeAsync(false);
			}
		}

		protected override void OnInitialized()
		{
			StepsRegistration.Register(this);
		}

		public void Dispose()
		{
			StepsRegistration.Unregister(this);
		}
	}
}