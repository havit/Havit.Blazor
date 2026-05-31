namespace Havit.Blazor.Storage.Exceptions;

/// <summary>
/// Thrown when the requested key is not present in the browser storage area
/// (<c>localStorage</c> or <c>sessionStorage</c>).
/// </summary>
public class StorageKeyNotFoundException : Exception
{
	public StorageKeyNotFoundException() { }

	public StorageKeyNotFoundException(string message) : base(message) { }

	public StorageKeyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}