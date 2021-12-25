using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Defaults for the <see cref="HxInputFileCore"/>.
	/// </summary>
	public record InputFileCoreSettings
	{
		/// <summary>
		/// The maximum files size in bytes.
		/// When exceeded, the <see cref="HxInputFileCore.OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
		/// Default is <c>long.MaxValue</c> (unlimited).
		/// </summary>
		public long MaxFileSize { get; set; } = long.MaxValue;
	}
}
