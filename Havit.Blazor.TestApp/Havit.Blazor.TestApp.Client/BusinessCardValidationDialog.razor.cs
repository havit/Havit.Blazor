namespace Havit.Blazor.TestApp.Client;

public partial class BusinessCardValidationDialog : ComponentBase
{
	[Parameter] public bool StartValidation { get; set; }
	[Parameter] public EventCallback OnValidationFinished { get; set; }

	private HxDialog _dialogComponent;

	private bool _validationStarted;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (StartValidation && !_validationStarted)
		{
			_validationStarted = true;
			await _dialogComponent.ShowAsync();
		}
	}

	private async Task ValidateBusinessCard()
	{
		//var workContext = EventCallback.Factory.Create(this, FinishValidation);

		// Do work
		await Task.Delay(500);

		// Invoke finish handler
		//await workContext.InvokeAsync();
		await FinishValidation();
	}

	private async Task FinishValidation()
	{
		await _dialogComponent.HideAsync();
		await OnValidationFinished.InvokeAsync();

		_validationStarted = false;
	}
}