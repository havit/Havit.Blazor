namespace Havit.Blazor.Storage.Exceptions;

/// <summary>
/// Thrown when the requested zero-based index is out of range of the keys present in the browser storage area
/// (<c>localStorage</c> or <c>sessionStorage</c>).
/// </summary>
public class StorageIndexOutOfRangeException : Exception
{
	public StorageIndexOutOfRangeException() { }

	public StorageIndexOutOfRangeException(string message) : base(message) { }

	public StorageIndexOutOfRangeException(string message, Exception innerException) : base(message, innerException) { }
}