namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Result of the dialog.
/// </summary>
public sealed class DialogResult<TValue>
{
	/// <summary>
	/// <c>True</c> when the dialog has a successful result, <c>false</c> when it was cancelled.
	/// </summary>
	public bool Successful { get; }

	/// <summary>
	/// The value of the successful result.
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when the dialog is not successful.</exception>
	public TValue Value
	{
		get
		{
			Contract.Assert<InvalidOperationException>(Successful, $"{nameof(Value)} property can only be read when {nameof(Successful)} is True.");
			return _value;
		}
	}
	private TValue _value;

	/// <summary>
	/// Constructor.
	/// </summary>
	private DialogResult(bool successful, TValue value = default)
	{
		Successful = successful;
		_value = value;
	}

	/// <summary>
	/// Returns a new instance for a cancelled dialog.
	/// </summary>
	internal static DialogResult<TValue> CreateCancelled() => new DialogResult<TValue>(false);

	/// <summary>
	/// Returns a new instance for a successful dialog.
	/// </summary>
	internal static DialogResult<TValue> CreateSuccessful(TValue value) => new DialogResult<TValue>(true, value);
}
