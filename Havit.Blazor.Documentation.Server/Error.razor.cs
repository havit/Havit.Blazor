using System.Diagnostics;

namespace Havit.Blazor.Documentation.Server;

public partial class Error
{
	[CascadingParameter] private HttpContext HttpContext { get; set; }

	private string _requestId;

	protected override void OnInitialized()
	{
		_requestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
	}
}