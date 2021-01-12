using System;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web
{
	public interface IHxMessageBoxService
	{
		Task<MessageBoxButtons> ShowAsync(MessageBoxRequest request);

		Func<MessageBoxRequest, Task<MessageBoxButtons>> ShowAsyncFunc { get; set; }
	}
}
