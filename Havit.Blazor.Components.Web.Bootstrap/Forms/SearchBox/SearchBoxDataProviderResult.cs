using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider result for search box data.
/// </summary>
public class SearchBoxDataProviderResult<TItem>
{
	/// <summary>
	/// Items provided by the request.
	/// </summary>
	public IEnumerable<TItem> Data { get; set; }
}
