using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web
{

	/// <summary>
	/// Arguments for <see cref="HxInputFileCore.OnProgress"/> event.
	/// </summary>
	public class UploadProgressEventArgs
	{
		/// <summary>
		/// Index of the file uploaded.
		/// </summary>
		public int FileIndex { get; internal set; }

		/// <summary>
		/// Name of the file provided by the browser.
		/// </summary>
		public string OriginalFileName { get; set; }

		/// <summary>
		/// Bytes uploaded.
		/// </summary>
		public long UploadedBytes { get; set; }

		/// <summary>
		/// Upload request size (slightly bigger than the file itself)
		/// </summary>
		public long UploadSize { get; set; }
	}
}
