using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.ComponentModel;

namespace Havit.Blazor.Grpc.TestContracts
{
	[ApiContract]
	public interface ITestFacade
	{
		Task<Dto<int>> GetIntFromIntAsync(Dto<int> input, CancellationToken cancellationToken = default);
	}
}
