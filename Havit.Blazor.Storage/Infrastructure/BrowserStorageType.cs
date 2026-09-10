namespace Havit.Blazor.Storage.Infrastructure;

/// <summary>
/// Identifies which Web Storage area is accessed.
/// </summary>
internal enum BrowserStorageType
{
	/// <summary>
	/// The <c>window.localStorage</c> area.
	/// </summary>
	Local = 0,

	/// <summary>
	/// The <c>window.sessionStorage</c> area.
	/// </summary>
	Session = 1
}