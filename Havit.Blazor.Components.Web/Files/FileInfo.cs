using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
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
}
