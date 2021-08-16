using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Grpc.Client.ServerExceptions
{
	public interface IOperationFailedExceptionPublisher
	{
		Task PublishAsync(string errorMessage);
	}
}
