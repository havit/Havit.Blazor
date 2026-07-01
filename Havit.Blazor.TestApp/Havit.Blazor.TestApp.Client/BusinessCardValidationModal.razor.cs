namespace Havit.Blazor.TestApp.Client;

public partial class BusinessCardValidationModal : ComponentBase
{
	[Parameter] public bool StartValidation { get; set; }
	[Parameter] public EventCallback OnValidationFinished { get; set; }

	private HxModal _modalComponent;

	private bool _validationStarted;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (StartValidation && !_validationStarted)
		{
			_validationStarted = true;
			await _modalComponent.ShowAsync();
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
		await _modalComponent.HideAsync();
		await OnValidationFinished.InvokeAsync();

		_validationStarted = false;
	}
}