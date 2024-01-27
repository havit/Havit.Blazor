using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Pages;

public partial class InternalTypeDoc
{
	[Parameter] public string TypeText { get; set; }

	private Type _type;

	protected override void OnParametersSet()
	{
		try
		{
			_type = ApiTypeHelper.GetType(TypeText, true);
		}
		catch
		{
			// NOOP
		}
	}
}
