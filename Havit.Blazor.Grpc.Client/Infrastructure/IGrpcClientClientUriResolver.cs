using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Grpc.Client.Infrastructure;

public interface IGrpcClientClientUriResolver
{
	string GetCurrentClientUri();
}
