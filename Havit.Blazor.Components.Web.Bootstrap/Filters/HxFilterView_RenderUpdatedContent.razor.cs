using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public partial class HxFilterView_RenderUpdatedContent : ComponentBase, IDisposable
	{
		[Parameter]
		public IHxFilter Filter { get; set; }

		[Parameter]
		public RenderFragment Content { get; set; }

		private Action detachAction;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			DetachIfPossible();
			if (Filter != null)
			{
				var filter = Filter;
				filter.Rendered += FilterRenderedHandler;
				detachAction = () => { filter.Rendered -= FilterRenderedHandler; };
			}
		}

		private void FilterRenderedHandler(IHxFilter sender, bool firstRender)
		{
			Debug.Assert(sender == Filter);

			if (!firstRender)
			{
				StateHasChanged();
			}
		}

		private void DetachIfPossible()
		{
			if (detachAction != null)
			{
				detachAction();
				detachAction = null;
			}
		}

		public void Dispose()
		{
			DetachIfPossible();
		}
	}
}
