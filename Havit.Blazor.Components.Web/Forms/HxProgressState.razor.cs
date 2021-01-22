using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Forms
{
	/// <summary>
	/// Propagates progress state as a cascading parementer to child components.
	/// </summary>
	public partial class HxProgressState
	{
		/// <summary>
		/// Received form state.
		/// </summary>
		[CascadingParameter] protected ProgressState CascadingFormState { get; set; }

		/// <summary>
		/// Indicated enabled/disabled section. Value to propagate.
		/// </summary>
		[Parameter] public bool? InProgress { get; set; }

		/// <summary>
		/// Child content.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// Create form state to propagate.
		/// </summary>
		private ProgressState CreateNewCascadingProgressState()
		{
			return new ProgressState
			{
				InProgress = InProgress ?? CascadingFormState?.InProgress,
			};
		}

	}

}
