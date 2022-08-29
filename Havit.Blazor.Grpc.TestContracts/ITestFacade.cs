namespace Havit.Blazor.Grpc.TestContracts
{
	[ApiContract]
	public interface ITestFacade
	{
		Task<Dto<int>> GetIntFromIntAsync(Dto<int> input, CancellationToken cancellationToken = default);

		Task<DtoWithNestedClass> GetDtoWithNestedClassPropertyFromDtoWithNestedClassPropertyAsync(DtoWithNestedClass input, CancellationToken cancellationToken = default);
	}
}
