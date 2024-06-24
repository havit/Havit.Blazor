namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Displays message boxes initiated through <see cref="IHxMessageBoxService"/>.
/// To be placed in the root application component / main layout.
/// </summary>
public partial class HxMessageBoxHost : ComponentBase
{
	[Inject] protected IHxMessageBoxService MessageBoxService { get; set; }

	private HxMessageBox _messageBox;
	private MessageBoxRequest _request;
	private TaskCompletionSource<MessageBoxButtons> _resultCompletion;

	protected override void OnInitialized()
	{
		base.OnInitialized();

		MessageBoxService.OnShowAsync = HandleShowRequest;
	}

	private async Task<MessageBoxButtons> HandleShowRequest(MessageBoxRequest request)
	{
		_request = request;
		_resultCompletion = new TaskCompletionSource<MessageBoxButtons>();

		StateHasChanged();

		await _messageBox.ShowAsync();

		return await _resultCompletion.Task;
	}

	private void HandleClosed(MessageBoxButtons button)
	{
		_ = _resultCompletion.TrySetResult(button);
	}
}
