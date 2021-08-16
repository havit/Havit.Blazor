using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Grpc.Configuration;

namespace Havit.Blazor.Grpc.Core
{
	public class ProtoBufServiceBinder : ServiceBinder
	{
		public override bool IsServiceContract(Type contractType, out string name)
		{
			name = GetDefaultName(contractType);
			return true;
		}
	}
}
