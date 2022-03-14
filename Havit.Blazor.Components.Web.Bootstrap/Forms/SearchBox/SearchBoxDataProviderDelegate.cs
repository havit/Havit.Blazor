using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Data provider (delegate).
/// </summary>
public delegate Task<SearchBoxDataProviderResult<TItem>> SearchBoxDataProviderDelegate<TItem>(SearchBoxDataProviderRequest request);
