using System.Diagnostics;

namespace Havit.Blazor.Components.Web.Infrastructure
{
	public partial class UpdatedContentRenderer : ComponentBase, IDisposable
	{
		[Parameter] public IRenderNotificationComponent Component { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		private Action detachAction;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			DetachIfPossible();
			if (Component != null)
			{
				var component = Component;
				component.Rendered += FilterRenderedHandler;
				detachAction = () => { component.Rendered -= FilterRenderedHandler; };
			}
		}

		private void FilterRenderedHandler(ComponentBase component, bool firstRender)
		{
			Debug.Assert(component == Component);

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
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				DetachIfPossible();
			}
		}
	}
}
