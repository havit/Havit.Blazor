using System.Text.Json;

namespace Havit.Blazor.Storage;

/// <summary>
/// Options for the browser storage services (<see cref="ILocalStorageService"/> and <see cref="ISessionStorageService"/>).
/// </summary>
public class StorageServiceOptions
{
	/// <summary>
	/// Default <see cref="JsonSerializerOptions"/> used by <c>SetValue</c>/<c>GetValue</c> (and their async variants)
	/// when no options are passed to the method call.
	/// When <c>null</c>, the default <see cref="System.Text.Json"/> behavior is used.
	/// </summary>
	public JsonSerializerOptions JsonSerializerOptions { get; set; }
}