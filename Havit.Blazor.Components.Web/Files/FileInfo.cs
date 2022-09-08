using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Havit.Blazor.Components.Web;

public class FileInfo
{
	[JsonPropertyName("index")]
	public int FileIndex { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = String.Empty;

	[JsonIgnore]
	public DateTimeOffset LastModified => DateTimeOffset.FromUnixTimeMilliseconds(LastModifiedUnixMilliseconds);

	[JsonPropertyName("lastModified")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public long LastModifiedUnixMilliseconds { get; set; }

	[JsonPropertyName("size")]
	public long Size { get; set; }

	[JsonPropertyName("type")]
	public string ContentType { get; set; } = String.Empty;
}
