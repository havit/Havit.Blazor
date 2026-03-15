namespace Havit.Blazor.TestApp.Client.HxModalTests;

public partial class HxModal_BusinessCardValidation_Test : ComponentBase
{
	private bool _startValidation;

	private void TriggerValidation()
	{
		_startValidation = true;
	}

	private void HandleValidationFinished()
	{
		_startValidation = false;
	}
}