﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web
{
	public partial class HxInputFileCore : InputFile, IAsyncDisposable
	{
		/// <summary>
		/// URL of the server endpoint receiving the files.
		/// </summary>
		[Parameter] public string UploadUrl { get; set; }

		/// <summary>
		/// Raised during running file upload (the frequency depends on browser implementation).
		/// </summary>
		[Parameter] public EventCallback<UploadProgressEventArgs> OnProgress { get; set; }

		/// <summary>
		/// Raised after a file is uploaded (for every single file separately).
		/// </summary>
		[Parameter] public EventCallback<FileUploadedEventArgs> OnFileUploaded { get; set; }

		/// <summary>
		/// Raised when all files are uploaded (after all <see cref="OnFileUploaded"/> events).
		/// </summary>
		[Parameter] public EventCallback<UploadCompletedEventArgs> OnUploadCompleted { get; set; }

		/// <summary>
		/// Single <c>false</c> or multiple <c>true</c> files upload.
		/// </summary>
		[Parameter] public bool Multiple { get; set; }

		/// <summary>
		/// Input element id.
		/// </summary>
		[Parameter] public string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		public int FileCount { get; private set; }

		private DotNetObjectReference<HxInputFileCore> dotnetObjectReference;
		private IJSObjectReference jsModule;
		private TaskCompletionSource<UploadCompletedEventArgs> uploadCompletedTaskCompletionSource;
		private ConcurrentBag<FileUploadedEventArgs> filesUploaded;

		public HxInputFileCore()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			// Temporary hack as base implementation of InputFile does not expose ElementReference (vNext: https://github.com/dotnet/aspnetcore/blob/main/src/Components/Web/src/Forms/InputFile.cs)
			AdditionalAttributes ??= new Dictionary<string, object>();
			AdditionalAttributes["id"] = this.Id;
			AdditionalAttributes["multiple"] = this.Multiple;
		}

		/// <summary>
		/// Starts the upload.
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		/// <remarks>
		/// We do not want to make the Havit.Blazor library dependant on WebAssembly libraries (IAccessTokenProvider and such). Therefor the accessToken here...
		/// </remarks>
		public async Task StartUploadAsync(string accessToken = null)
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(UploadUrl), $"{nameof(UploadUrl)} has to be set.");

			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web/hxinputfilecore.js");
			filesUploaded = new ConcurrentBag<FileUploadedEventArgs>();

			await jsModule.InvokeVoidAsync("upload", Id, dotnetObjectReference, this.UploadUrl, accessToken);
		}

		/// <summary>
		/// Uploads the file(s).
		/// </summary>
		/// <param name="accessToken">Authorization Bearer Token to be used for upload (i.e. use IAccessTokenProvider).</param>
		public async Task<UploadCompletedEventArgs> UploadAsync(string accessToken = null)
		{
			uploadCompletedTaskCompletionSource = new TaskCompletionSource<UploadCompletedEventArgs>();

			await StartUploadAsync(accessToken);

			return await uploadCompletedTaskCompletionSource.Task;
		}

		/// <summary>
		/// Gets list of files chosen.
		/// </summary>
		public async Task<FileInfo[]> GetFilesAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web/hxinputfilecore.js");

			return await jsModule.InvokeAsync<FileInfo[]>("getFiles", Id);
		}

		/// <summary>
		/// Clears associated input element and resets component to initial state.
		/// </summary>
		public async Task ResetAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web/hxinputfilecore.js");

			await jsModule.InvokeVoidAsync("reset", Id);
		}

		/// <summary>
		/// Receive upload progress notification from underlying javascript.
		/// </summary>
		[JSInvokable("HxInputFileCore_HandleUploadProgress")]
		public async Task HandleUploadProgress(int fileIndex, string fileName, long loaded, long total)
		{
			var uploadProgress = new UploadProgressEventArgs()
			{
				FileIndex = fileIndex,
				OriginalFileName = fileName,
				UploadedBytes = loaded,
				UploadSize = total
			};
			await OnProgress.InvokeAsync(uploadProgress);
		}

		/// <summary>
		/// Receive upload finished notification from underlying javascript.
		/// </summary>
		[JSInvokable("HxInputFileCore_HandleFileUploaded")]
		public async Task HandleFileUploaded(int fileIndex, string fileName, long fileSize, string fileType, long fileLastModified, int responseStatus, string responseText)
		{
			var fileUploaded = new FileUploadedEventArgs()
			{
				FileIndex = fileIndex,
				OriginalFileName = fileName,
				ContentType = fileType,
				Size = fileSize,
				LastModified = DateTimeOffset.FromUnixTimeMilliseconds(fileLastModified),
				ResponseStatus = (HttpStatusCode)responseStatus,
				ResponseText = responseText,
			};
			filesUploaded.Add(fileUploaded);
			await OnFileUploaded.InvokeAsync(fileUploaded);
		}

		/// <summary>
		/// Receive upload finished notification from underlying javascript.
		/// </summary>
		[JSInvokable("HxInputFileCore_HandleUploadCompleted")]
		public async Task HandleUploadCompleted(int fileCount, long totalSize)
		{
			var uploadCompleted = new UploadCompletedEventArgs()
			{
				FilesUploaded = filesUploaded,
				FileCount = fileCount,
				TotalSize = totalSize
			};
			filesUploaded = null;
			uploadCompletedTaskCompletionSource?.TrySetResult(uploadCompleted);
			await OnUploadCompleted.InvokeAsync(uploadCompleted);
		}

		public async ValueTask DisposeAsync()
		{
			((IDisposable)this).Dispose();

			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", Id);
				await jsModule.DisposeAsync();
			}

			dotnetObjectReference.Dispose();
		}
	}
}