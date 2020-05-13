using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	public static class StringExtensions
	{
		public static RenderFragment ToRenderFragment(this string content)
        {
            Contract.Requires<ArgumentNullException>(content != null, nameof(content));

            return new RenderFragment(builder =>
            {
                builder.AddContent(0, content);
            });
        }
    }
}
