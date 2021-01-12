using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	public class HxMessageBoxService : IHxMessageBoxService
	{
		public Func<MessageBoxRequest, Task<MessageBoxButtons>> ShowAsyncFunc { get; set; }

		public Task<MessageBoxButtons> ShowAsync(MessageBoxRequest request)
		{
			return ShowAsyncFunc.Invoke(request);
		}
	}
}
