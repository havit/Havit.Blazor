namespace Havit.Blazor.TestApp.Client.HxDialogTests;

public partial class HxDialog_BusinessCardValidation_Test : ComponentBase
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