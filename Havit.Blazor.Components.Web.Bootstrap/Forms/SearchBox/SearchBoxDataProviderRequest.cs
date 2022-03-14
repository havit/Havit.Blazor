using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider request for search box data.
/// </summary>
public class SearchBoxDataProviderRequest
{
	/// <summary>
	/// Current input (freetext) of the search box.
	/// </summary>
	public string UserInput { get; init; }

	/// <summary>
	/// The <see cref="System.Threading.CancellationToken"/> used to relay cancellation of the request.
	/// </summary>
	public CancellationToken CancellationToken { get; init; }
}
