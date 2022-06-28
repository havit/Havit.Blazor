using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages
{
	public partial class InternalTypeDoc
	{
		[Parameter] public string TypeText { get; set; }
		[Parameter] public Type Type { get; set; }

		private bool isDelegate;

		protected override void OnParametersSet()
		{
			try
			{
				var typeInfo = ApiHelper.GetType(TypeText);
				Type = typeInfo.type;
				isDelegate = typeInfo.isDelegate;
			}
			catch { }
			InvokeAsync(StateHasChanged);
		}
	}
}
