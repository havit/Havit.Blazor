using System.Diagnostics;

namespace Havit.Blazor.Components.Web.Infrastructure
{
	public partial class UpdatedContentRenderer : ComponentBase, IDisposable
	{
		[Parameter] public IRenderNotificationComponent Component { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		private Action detachAction;
		private IRenderNotificationComponent attachedComponent;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (attachedComponent != this.Component)
			{
				DetachIfPossible();
				if (this.Component != null)
				{
					var component = this.Component;
					component.Rendered += FilterRenderedHandler;
					detachAction = () => { component.Rendered -= FilterRenderedHandler; };
				}
				attachedComponent = this.Component;
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
