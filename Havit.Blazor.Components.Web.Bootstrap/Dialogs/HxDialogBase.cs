namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Base class to simplify custom dialog dialog implementation.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxDialogBase">https://havit.blazor.eu/components/HxDialogBase</see>
/// </summary>
public abstract class HxDialogBase<TResult> : ComponentBase
{
#pragma warning disable SA1307 // Accessible fields must begin with upper-case letter
	protected internal HxDialog dialog; // for HxDialog @ref
#pragma warning restore SA1307 // Accessible fields must begin with upper-case letter

	/// <summary>
	/// Returns the <see cref="HxDialog"/> component that this component controls. Override this method when you do not want to use the dialog field set using @ref.
	/// </summary>
	protected virtual HxDialog Dialog => dialog;

	private TaskCompletionSource<DialogResult<TResult>> _resultCompletion;
	private bool _setResultOnDialogClosed;
	private bool _firstRendered = false;

	/// <summary>
	/// Shows the dialog and waits until it is hidden.
	/// </summary>
	public async Task<DialogResult<TResult>> ShowAsync()
	{
		Contract.Assert<InvalidOperationException>(_firstRendered, "Cannot show the dialog before it is rendered."); // otherwise, we do not have the Dialog/dialog value from @ref.
		_resultCompletion = new TaskCompletionSource<DialogResult<TResult>>();
		_setResultOnDialogClosed = true;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Dialog.OnClosed = EventCallback.Factory.Create(this, HandleDialogClosed);
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.

		await Dialog.ShowAsync();
		var result = await _resultCompletion.Task;

#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Dialog.OnClosed = EventCallback.Empty;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
		_resultCompletion = null; // do not hold the task with a value

		return result;
	}

	/// <summary>
	/// Hides the dialog and sets the value.
	/// </summary>
	protected internal async Task HideAsync(TResult value)
	{
		_setResultOnDialogClosed = false; // do not fire the event in HandleDialogClosed
		await Dialog.HideAsync();
		_resultCompletion.SetResult(DialogResult<TResult>.CreateSuccessful(value)); // finish ShowAsync with a value
	}

	/// <summary>
	/// Hides the dialog as cancelled (no value is set).
	/// </summary>
	protected internal async Task HideAsCancelledAsync()
	{
		_setResultOnDialogClosed = false; // do not fire the event in HandleDialogClosed
		await Dialog.HideAsync();
		_resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled()); // finish ShowAsync with no value
	}

	private void HandleDialogClosed()
	{
		if (_setResultOnDialogClosed)
		{
			// we expect this only when the dialog is closed out of the HideAsync/HideAsCancelledAsync methods
			// i.e. escape key or close button in the top-right corner
			_resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled());
		}
	}

	protected override void OnAfterRender(bool firstRender)
	{
		base.OnAfterRender(firstRender);
		Contract.Assert<InvalidOperationException>(Dialog != null, $"The dialog must contain the {nameof(HxDialog)} component with @ref=\"{nameof(dialog)}\".");
		_firstRendered = true;
	}
}
