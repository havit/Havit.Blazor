using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxInputFileDropZone
	{
		private const int FirstFileNamesMaxCount = 3; // Might be converted to parameter if needed.

		/// <summary>
		/// URL of the server endpoint receiving the files.
		/// </summary>
		[Parameter] public string UploadUrl { get; set; }

		/// <summary>
		/// Gets or sets the event callback that will be invoked when the collection of selected files changes.
		/// </summary>
		[Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

		/// <summary>
		/// Raised during running file upload (the frequency depends on browser implementation).
		/// </summary>
		[Parameter] public EventCallback<UploadProgressEventArgs> OnProgress { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<FileUploadedEventArgs> OnFileUploaded { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<UploadCompletedEventArgs> OnUploadCompleted { get; set; }

		/// <summary>
		/// Single <c>false</c> or multiple <c>true</c> files upload.
		/// </summary>
		[Parameter] public bool Multiple { get; set; }
		
		/// <summary>
		/// The maximum files size in bytes.
		/// When exceeded, the <see cref="OnFileUploaded"/> returns <c>413-RequestEntityTooLarge</c> as <see cref="FileUploadedEventArgs.ResponseStatus"/>.
		/// Default is <c>null</c> (unlimited).
		/// </summary>
		[Parameter] public long? MaxFileSize { get; set; }

		/// <summary>
		/// Takes as its value a comma-separated list of one or more file types, or unique file type specifiers, describing which file types to allow.
		/// <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Attributes/accept">MDN Web Docs - HTML attribute: accept</see>.
		/// </summary>
		[Parameter] public string Accept { get; set; }

		/// <summary>
		/// Content to render when no files are picked. Default content is displayed when not set.
		/// </summary>
		[Parameter] public RenderFragment NoFilesTemplate { get; set; }

		/// <summary>
		/// Custom CSS class to render with wrapping div.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the input element.
		/// </summary>
		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public bool? Enabled { get; set; }

		[Inject] protected IStringLocalizer<HxInputFileDropZone> Localizer { get; set; }

		private int fileCount = 0;
		private List<string> firstFileNames = new List<string>();
		private protected HxInputFileCore hxInputFileCoreComponentReference;

		/// <summary>
		/// Gets list of files chosen.
		/// </summary>
		public Task<FileInfo[]> GetFilesAsync() => hxInputFileCoreComponentReference.GetFilesAsync();

		/// <summary>
		/// Clears associated input-file element and resets component to initial state.
		/// </summary>
		public Task ResetAsync() => hxInputFileCoreComponentReference.ResetAsync();

		/// <summary>
		/// Starts the upload.
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		/// <remarks>
		/// We do not want to make the Havit.Blazor library dependant on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
		/// </remarks>
		public Task StartUploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.StartUploadAsync(accessToken);

		/// <summary>
		/// Uploads the file(s).
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		public Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null) => hxInputFileCoreComponentReference?.UploadAsync(accessToken);

		protected Task HandleOnChange(InputFileChangeEventArgs args)
		{
			fileCount = args.FileCount;
			firstFileNames = args.GetMultipleFiles(int.MaxValue).Take(FirstFileNamesMaxCount).Select(f => f.Name).ToList();
			StateHasChanged();

			return OnChange.InvokeAsync(args);
		}

		protected string GetFilesPickedText()
		{
			if (fileCount == 1)
			{
				return String.Format(Localizer["SingleFileSelected"], firstFileNames[0]);
			}
			else
			{
				var result = String.Format(Localizer["MultipleFilesSelected"], fileCount, String.Join(", ", firstFileNames));
				if (fileCount > firstFileNames.Count)
				{
					result = result + " " + String.Format(Localizer["MoreFiles"], fileCount - firstFileNames.Count);
				}
				return result;
			}
		}
	}
}
