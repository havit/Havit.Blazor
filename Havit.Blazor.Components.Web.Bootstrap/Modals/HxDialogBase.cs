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
	/// Returns the <see cref="HxModal"/> component which this component control. Override when you do not want to use modal field set using @ref.
	/// </summary>
	protected virtual HxModal Modal => modal;

	private TaskCompletionSource<DialogResult<TResult>> resultCompletion;
	private bool setResultOnModalClosed;
	private bool firstRendered = false;

	/// <summary>
	/// Shows the modal and waits until it is hidden.
	/// </summary>
	public async Task<DialogResult<TResult>> ShowAsync()
	{
		Contract.Assert<InvalidOperationException>(firstRendered, "Cannot show dialog before it is rendered."); // otherwise we do not have Modal/modal value from @ref.
		resultCompletion = new TaskCompletionSource<DialogResult<TResult>>();
		setResultOnModalClosed = true;
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Modal.OnClosed = EventCallback.Factory.Create(this, HandleModalClosed);
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.

		await Modal.ShowAsync();
		var result = await this.resultCompletion.Task;

#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
		Modal.OnClosed = EventCallback.Empty;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
		this.resultCompletion = null; // do not hold task with a value

		return result;
	}

	/// <summary>
	/// Hides the modal and sets the value.
	/// </summary>
	protected internal async Task HideAsync(TResult value)
	{
		setResultOnModalClosed = false; // do not fire event in HandleModalClosed
		await Modal.HideAsync();
		resultCompletion.SetResult(DialogResult<TResult>.CreateSuccessful(value)); // finish ShowAsync with a value
	}

	/// <summary>
	/// Hides the modal as cancelled (no value is set).
	/// </summary>
	protected internal async Task HideAsCancelledAsync()
	{
		setResultOnModalClosed = false; // do not fire event in HandleModalClosed
		await Modal.HideAsync();
		resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled()); // finish ShowAsync with no value
	}

	private void HandleModalClosed()
	{
		if (setResultOnModalClosed)
		{
			// we expect this only when dialog closed out od HideAsync/HideAsCancelledAsync methods
			// ie. escape key or close but in top-right corner
			resultCompletion.SetResult(DialogResult<TResult>.CreateCancelled());
		}
	}

	protected override void OnAfterRender(bool firstRender)
	{
		base.OnAfterRender(firstRender);
		Contract.Assert<InvalidOperationException>(Modal != null, $"Dialog must contain {nameof(HxModal)} component with @ref=\"{nameof(modal)}\".");
		firstRendered = true;
	}
}
