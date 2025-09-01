namespace Havit.Blazor.TestApp.MinimalApi;

public static class FileUploadEndpoint
{
	public static WebApplication UseFileUploadEndpoint(this WebApplication app)
	{
		string tempPath = Path.GetTempPath();

		app.MapPost("/file-upload-streamed", async (IFormFile file) =>
		{
			Directory.CreateDirectory(tempPath);

			string filePath = Path.Combine(tempPath, file.FileName);

			using FileStream stream = File.Create(filePath);
			await file.CopyToAsync(stream);

			return Results.Ok(new
			{
				file.FileName,
				file.Length,
				SavedTo = filePath
			});
		});

		return app;
	}
}
