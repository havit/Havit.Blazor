using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Wizards
{
	public interface IHxWizardStep
	{
		bool IsActive { get; }
		Task Activate();
		Task Deactivate();
	}
}
