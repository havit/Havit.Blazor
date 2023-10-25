using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Pages;

public partial class InternalTypeDoc
{
	[Parameter] public string TypeText { get; set; }

	private Type type;

	protected override void OnParametersSet()
	{
		try
		{
			type = ApiTypeHelper.GetType(TypeText);
		}
		catch
		{
			// NOOP
		}
	}
}
