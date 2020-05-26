using Havit.Linq;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Wizards
{
    /// <summary>
    /// Wizard Component
    /// </summary>
    public partial class HxWizard : ComponentBase, IDisposable
    {
        /// <summary>
        /// The ChildContent container for <see cref="WizardStep"/>
        /// </summary>
        [Parameter]
        public RenderFragment Steps { get; set; }

        private List<IHxWizardStep> steps;
        protected CollectionRegistration<IHxWizardStep> stepsRegistration;
        private bool isDisposed = false;

        public HxWizard()
        {
            steps = new List<IHxWizardStep>();
            stepsRegistration = new CollectionRegistration<IHxWizardStep>(steps, StateHasChanged, () => isDisposed);
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
            var activeStep = steps.FirstOrDefault(step => step.IsActive);
            if (activeStep == null)
			{
                return false;
			}

            var targetStepIndex = steps.IndexOf(activeStep) + offset;
            return (targetStepIndex >= 0) && (targetStepIndex < steps.Count);
        }

        protected internal async Task GoBackClick()
        {
            await GoOffset(-1);
        }

        /// <summary>
        /// Sets the <see cref="ActiveStep"/> to the next Index
        /// </summary>
        protected internal async Task GoNextClick()
        {
            await GoOffset(1);
        }

        private async Task GoOffset(int offset)
		{
            var activeStep = steps.FirstOrDefault(step => step.IsActive);
            var stepToActivate = steps[steps.IndexOf(activeStep) + offset];

            await activeStep.Deactivate();
            await stepToActivate.Activate();
        }

		public void Dispose()
		{
            isDisposed = true;
		}
	}
}