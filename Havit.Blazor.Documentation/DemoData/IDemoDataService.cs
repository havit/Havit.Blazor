namespace Havit.Blazor.Documentation.DemoData;

public interface IDemoDataService
{
	IEnumerable<EmployeeDto> GetAllEmployees();
	Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
	IQueryable<EmployeeDto> GetEmployeesAsQueryable();

	Task<IEnumerable<EmployeeDto>> GetEmployeesDataFragmentAsync(int startIndex, int? count, CancellationToken cancellationToken = default);
	Task<int> GetEmployeesCountAsync(CancellationToken cancellationToken = default);
	Task<List<EmployeeDto>> FindEmployeesByNameAsync(string query, int? limitCount = null, CancellationToken cancellationToken = default);
	Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default);
	Task<List<EmployeeDto>> GetPreferredEmployeesAsync(int count, CancellationToken cancellationToken = default);
}