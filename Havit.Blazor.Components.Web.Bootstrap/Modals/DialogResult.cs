using Havit.Diagnostics.Contracts;
using System;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Result of the dialog.
	/// </summary>
	/// <typeparam name="TValue"></typeparam>
	public sealed class DialogResult<TValue>
	{
		/// <summary>
		/// True when the dialog has successful result, false then it was cancelled.
		/// </summary>
		public bool Successful { get; }

		/// <summary>
		/// The value of the sucessful result.
		/// </summary>
		/// <exception cref="InvalidOperationException">When the dialog is not successful.</exception>
		public TValue Value
		{
			get
			{
				Contract.Assert<InvalidOperationException>(Successful, $"{nameof(Value)} property can be read only when {nameof(Successful)} is True.");
				return value;
			}
		}
		private TValue value;

		/// <summary>
		/// Constructor.
		/// </summary>
		private DialogResult(bool successful, TValue value = default)
		{
			this.Successful = successful;
			this.value = value;
		}

		/// <summary>
		/// Returns new instance for cancelled dialog.
		/// </summary>
		internal static DialogResult<TValue> CreateCancelled() => new DialogResult<TValue>(false);

		/// <summary>
		/// Returns new instance for successful dialog.
		/// </summary>
		internal static DialogResult<TValue> CreateSuccessful(TValue value) => new DialogResult<TValue>(true, value);
	}
}