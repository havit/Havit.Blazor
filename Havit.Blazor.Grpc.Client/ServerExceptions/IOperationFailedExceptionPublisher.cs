﻿namespace Havit.Blazor.Grpc.Client.ServerExceptions;

public interface IOperationFailedExceptionGrpcClientListener
{
	Task ProcessAsync(string errorMessage);
}
