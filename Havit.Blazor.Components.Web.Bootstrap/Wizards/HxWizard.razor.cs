using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Havit.Linq;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Wizard Component
	/// </summary>
	public partial class HxWizard : ComponentBase, IDisposable
	{
		/// <summary>
		/// StepsRegistration cascading value name.
		/// </summary>
		internal const string StepsRegistrationCascadingValueName = "StepsRegistration";

		/// <summary>
		/// The ChildContent container for <see cref="HxWizardStep"/>
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public string ActiveStepId { get; set; }
		[Parameter] public EventCallback<string> ActiveStepIdChanged { get; set; }

		[Parameter] public bool ShowNavigator { get; set; } = true;

		private List<HxWizardStep> steps;
		protected CollectionRegistration<HxWizardStep> stepsRegistration;
		private bool isDisposed = false;

		public HxWizard()
		{
			steps = new List<HxWizardStep>();
			stepsRegistration = new CollectionRegistration<HxWizardStep>(steps, StateHasChanged, () => isDisposed);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				// when no step is active after initial render, activate the first
				if (!steps.Any(tab => IsActiveStep(tab)) && (steps.Count > 0))
				{
					HxWizardStep stepToActivate = steps.FirstOrDefault();
					if (stepToActivate != null)
					{
						await SetActiveStepIdAsync(stepToActivate.Id);
					}
				}
			}
		}

		internal async Task SetActiveStepIdAsync(string newId)
		{
			if (this.ActiveStepId != newId)
			{
				ActiveStepId = newId;
				if (!ActiveStepIdChanged.HasDelegate)
				{
					StateHasChanged(); // Otherwise, the steps gets changed only if there is external @bind-ActiveStepId
				}
				await ActiveStepIdChanged.InvokeAsync(newId);
			}
		}

		protected internal bool CanGoBack()
		{
			return CanGoOffset(-1);
		}

		protected internal bool CanGoNext()
		{
			return CanGoOffset(1);
		}

		private bool CanGoOffset(int offset)
		{
			var activeStep = steps.FirstOrDefault(step => IsActiveStep(step));
			if (activeStep == null)
			{
				return false;
			}

			var targetStepIndex = steps.IndexOf(activeStep) + offset;
			return (targetStepIndex >= 0) && (targetStepIndex < steps.Count);
		}

		/// <summary>
		/// Sets the active step to the previous one.
		/// </summary>
		protected internal async Task GoBackClick()
		{
			await GoOffset(-1);
		}

		/// <summary>
		/// Sets the active step to the next one.
		/// </summary>
		protected internal async Task GoNextClick()
		{
			await GoOffset(1);
		}

		private async Task GoOffset(int offset)
		{
			var activeStep = steps.FirstOrDefault(step => IsActiveStep(step));
			var stepToActivate = steps[steps.IndexOf(activeStep) + offset];

			await SetActiveStepIdAsync(stepToActivate.Id);
		}

		private bool IsActiveStep(HxWizardStep step)
		{
			return ActiveStepId == step.Id;
		}

		public void Dispose()
		{
			isDisposed = true;
		}
	}
}