namespace Havit.Blazor.Storage.Exceptions;

/// <summary>
/// Thrown when the requested browser storage area (<c>localStorage</c> or <c>sessionStorage</c>) is not accessible,
/// for example, when the user disabled it in the browser or access is blocked by the browser privacy settings.
/// </summary>
public class StorageNotAvailableException : Exception
{
	public StorageNotAvailableException() { }

	public StorageNotAvailableException(string message) : base(message) { }

	public StorageNotAvailableException(string message, Exception innerException) : base(message, innerException) { }
}