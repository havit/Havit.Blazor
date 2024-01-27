namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Base class to simplify custom modal dialog implementation.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxDialogBase">https://havit.blazor.eu/components/HxDialogBase</see>
/// </summary>
public abstract class HxDialogBase<TResult> : ComponentBase
{
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
	protected internal HxModal modal; // for HxModal @ref
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter

	/// <summary>
	/// Returns the <see cref="HxModal"/> component that this component controls. Override this method when you do not want to use the modal field set using @ref.
	/// </summary>
	protected virtual HxModal Modal => modal;

	private TaskCompletionSource<DialogResult<TResult>> _resultCompletion;
	private bool _setResultOnModalClosed;
	private bool _firstRendered = false;

	/// <summary>
	/// Shows the modal and waits until it is hidden.
	/// </summary>
	public async Task<DialogResult<TResult>> ShowAsync()
	{
		Contract.Assert<InvalidOperationException>(_firstRendered, "Cannot show the dialog before it is rendered."); // otherwise, we do not have the Modal/modal value from @ref.
		_resultCompletion = new TaskCompletionSource<DialogResult<TResult>>();
		_setResultOnModalClosed = true;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Modal.OnClosed = EventCallback.Factory.Create(this, HandleModalClosed);
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.

		await Modal.ShowAsync();
		var result = await _resultCompletion.Task;

#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Modal.OnClosed = EventCallback.Empty;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
		_resultCompletion = null; // do not hold the task with a value

		return result;
	}

	/// <summary>
	/// Hides the modal and sets the value.
	/// </summary>
	protected internal async Task HideAsync(TResult value)
	{
		_setResultOnModalClosed = false; // do not fire the event in HandleModalClosed
		await Modal.HideAsync();
		_resultCompletion.SetResult(DialogResult<TResult>.CreateSuccessful(value)); // finish ShowAsync with a value
	}

	/// <summary>
	/// Hides the modal as cancelled (no value is set).
	/// </summary>
	protected internal async Task HideAsCancelledAsync()
	{
		_setResultOnModalClosed = false; // do not fire the event in HandleModalClosed
		await Modal.HideAsync();
		_resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled()); // finish ShowAsync with no value
	}

	private void HandleModalClosed()
	{
		if (_setResultOnModalClosed)
		{
			// we expect this only when the dialog is closed out of the HideAsync/HideAsCancelledAsync methods
			// i.e. escape key or close button in the top-right corner
			_resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled());
		}
	}

	protected override void OnAfterRender(bool firstRender)
	{
		base.OnAfterRender(firstRender);
		Contract.Assert<InvalidOperationException>(Modal != null, $"The dialog must contain the {nameof(HxModal)} component with @ref=\"{nameof(modal)}\".");
		_firstRendered = true;
	}
}
