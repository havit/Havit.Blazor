using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class CodeSnippet : ComponentBase
{
	[Parameter] public string File { get; set; }
	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public string Language { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private string _code;

	protected override async Task OnParametersSetAsync()
	{
		if (!String.IsNullOrWhiteSpace(File))
		{
			var resourceName = File.Replace("~", typeof(CodeSnippet).Assembly.GetName().Name).Replace("/", ".").Replace("\\", ".");

			using (Stream stream = typeof(CodeSnippet).Assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					_code = await reader.ReadToEndAsync();
				}
			}
		}

	}

	private string GetLanguageFromFileExtension()
	{
		if (String.IsNullOrWhiteSpace(File))
		{
			return null;
		}
		return Path.GetExtension(File).ToLower() switch
		{
			".razor" => "cshtml",
			".cshtml" => "cshtml",
			".html" => "html",
			".css" => "css",
			".cs" => "csharp",
			".txt" => "none",
			".js" => "js",
			_ => null
		};
	}


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);
		await JSRuntime.InvokeVoidAsync("highlightCode");
	}

	protected override bool ShouldRender()
	{
		return false; // static content
	}

}
