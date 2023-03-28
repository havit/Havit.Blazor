namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.DemoData;

public interface IDemoDataService
{
	IEnumerable<EmployeeDto> GetAllEmployees();

	Task<IEnumerable<EmployeeDto>> GetEmployeesDataFragmentAsync(int startIndex, int? count, CancellationToken cancellationToken = default);
	Task<int> GetEmployeesCountAsync(CancellationToken cancellationToken = default);
	Task<List<EmployeeDto>> FindEmployeesByName(string query, CancellationToken cancellationToken = default);
}