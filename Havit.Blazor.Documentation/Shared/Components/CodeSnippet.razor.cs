using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class CodeSnippet : ComponentBase, IDisposable
{
	[Parameter] public string File { get; set; }
	[Parameter] public RenderFragment ChildContent { get; set; }
	[Parameter] public string Language { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private string _code;
	private ElementReference _codeElement;
	private bool _copied;
	private CancellationTokenSource _revertCts;

	private async Task CopyToClipboardAsync()
	{
		if (_revertCts is not null)
		{
			await _revertCts.CancelAsync();
			_revertCts.Dispose();
		}
		_revertCts = new CancellationTokenSource();

		await JSRuntime.InvokeVoidAsync("copyTextFromElement", _codeElement);
		_copied = true;
		// Fire-and-forget the revert so the click handler returns immediately
		// (an awaited delay keeps the button in the "click in progress" state, which shows a spinner).
		_ = RevertCopiedAfterDelayAsync(_revertCts.Token);
	}

	private async Task RevertCopiedAfterDelayAsync(CancellationToken cancellationToken)
	{
		try
		{
			await Task.Delay(2000, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			return;
		}
		_copied = false;
		await InvokeAsync(StateHasChanged);
	}

	public void Dispose()
	{
		_revertCts?.Cancel();
		_revertCts?.Dispose();
	}

	private string GetEffectiveLanguage() => Language ?? GetLanguageFromFileExtension() ?? "cshtml";

	private string GetLanguageLabel()
	{
		return GetEffectiveLanguage() switch
		{
			"cshtml" => "Razor",
			"razor" => "Razor",
			"html" => "HTML",
			"css" => "CSS",
			"csharp" => "C#",
			"cs" => "C#",
			"js" => "JavaScript",
			"json" => "JSON",
			"xml" => "XML",
			"none" => null,
			var other => other?.ToUpperInvariant()
		};
	}

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
		if (firstRender)
		{
			await JSRuntime.InvokeVoidAsync("highlightCode");
		}
	}

}
