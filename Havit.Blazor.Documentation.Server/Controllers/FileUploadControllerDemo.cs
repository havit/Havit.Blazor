using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Havit.Blazor.Documentation.Server.Controllers;

public class FileUploadControllerDemo : ControllerBase
{
	private const int BoundaryLengthLimit = 512 * 1024;

	// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads#upload-large-files-with-streaming
	// https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/mvc/models/file-uploads/samples/
	[HttpPost("/file-upload-streamed/")]
	public async Task<IActionResult> UploadStreamedFile()
	{
		if (!IsMultipartContentType(Request.ContentType))
		{
			ModelState.AddModelError("File", $"The request couldn't be processed (Error 1).");
			return BadRequest(ModelState);
		}

		var boundary = GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), lengthLimit: BoundaryLengthLimit);
		var reader = new MultipartReader(boundary, HttpContext.Request.Body);
		var section = await reader.ReadNextSectionAsync();

		while (section != null)
		{
			var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

			if (hasContentDispositionHeader)
			{
				// THIS IS WHERE YOU PASS THE FILE STREAM TO THE FACADE, the code below is not to be directly used here!

				// Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
				var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
				var trustedFileNameForFileStorage = Path.GetRandomFileName();
				// using (var targetStream = System.IO.File.Create(Path.Combine(Path.GetTempPath(), trustedFileNameForFileStorage)))
				// {
				// 	await section.Body.CopyToAsync(targetStream);
				// }

				await section.Body.CopyToAsync(Stream.Null);

				return Ok(trustedFileNameForFileStorage);
			}

			// Drain any remaining section body that hasn't been consumed and read the headers for the next section.
			section = await reader.ReadNextSectionAsync();
		}

		return BadRequest();
	}

	// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
	// The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
	public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
	{
		var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

		if (string.IsNullOrWhiteSpace(boundary))
		{
			throw new InvalidDataException("Missing content-type boundary.");
		}

		if (boundary.Length > lengthLimit)
		{
			throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
		}

		return boundary;
	}

	public static bool IsMultipartContentType(string contentType)
	{
		return !string.IsNullOrEmpty(contentType)
			   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
	}

	public static bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
	{
		// Content-Disposition: form-data; name="key";
		return contentDisposition != null
			&& contentDisposition.DispositionType.Equals("form-data")
			&& string.IsNullOrEmpty(contentDisposition.FileName.Value)
			&& string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
	}

	public static bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
	{
		// Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
		return contentDisposition != null
			&& contentDisposition.DispositionType.Equals("form-data")
			&& (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
				|| !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
	}
}
