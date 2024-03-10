using Havit.Diagnostics.Contracts;
using Havit.Linq;

namespace Havit.Blazor.Documentation.DemoData;

public class DemoDataService : IDemoDataService
{
	private readonly ILogger<DemoDataService> _logger;

	private readonly List<EmployeeDto> _employees;

	public DemoDataService(ILogger<DemoDataService> logger)
	{
		_logger = logger;

		_employees = new()
		{
			new EmployeeDto()
			{
				Id = 1,
				Name = "John Smith",
				Email = "john.smith@company.demo",
				Phone = "+420 123 456 789",
				Salary = 20000M,
				Position = "Software Engineer",
				Location = "Prague",
			},
			new EmployeeDto()
			{
				Id = 2,
				Name = "Mary Johnson",
				Email = "mary.johnson@company.demo",
				Phone = "+420 234 567 890",
				Salary = 25000M,
				Position = "Product Manager",
				Location = "San Francisco",
			},
			new EmployeeDto()
			{
				Id = 3,
				Name = "David Lee",
				Email = "david.lee@company.demo",
				Phone = "+420 345 678 901",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "New York",
			},
			new EmployeeDto()
			{
				Id = 4,
				Name = "Jasmine Kim",
				Email = "jasmine.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 22000M,
				Position = "Data Analyst",
				Location = "Seoul",
			},
			new EmployeeDto()
			{
				Id = 5,
				Name = "Alexandra Brown",
				Email = "alexandra.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 28000M,
				Position = "Marketing Manager",
				Location = "London",
			},
			new EmployeeDto()
			{
				Id = 6,
				Name = "Robert Garcia",
				Email = "robert.garcia@company.demo",
				Phone = "+420 789 012 345",
				Salary = 23000M,
				Position = "Software Engineer",
				Location = "Barcelona",
			},
			new EmployeeDto()
			{
				Id = 7,
				Name = "Olivia Smith",
				Email = "olivia.smith@company.demo",
				Phone = "+420 890 123 456",
				Salary = 26000M,
				Position = "Product Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Id = 8,
				Name = "Mason Johnson",
				Email = "mason.johnson@company.demo",
				Phone = "+420 012 345 678",
				Salary = 20000M,
				Position = "Sales Representative",
				Location = "Houston",
			},
			new EmployeeDto()
			{
				Id = 9,
				Name = "Ava Lee",
				Email = "ava.lee@company.demo",
				Phone = "+420 123 456 789",
				Salary = 24000M,
				Position = "Data Analyst",
				Location = "Tokyo",
			},
			new EmployeeDto()
			{
				Id = 10,
				Name = "Jacob Kim",
				Email = "jacob.kim@company.demo",
				Phone = "+420 234 567 890",
				Salary = 27000M,
				Position = "Marketing Manager",
				Location = "Paris",
			},
			new EmployeeDto()
			{
				Id = 11,
				Name = "Samuel Adams",
				Email = "samuel.adams@company.demo",
				Phone = "+420 789 012 345",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Boston",
			},
			new EmployeeDto()
			{
				Id = 12,
				Name = "Emily Park",
				Email = "emily.park@company.demo",
				Phone = "+420 890 123 456",
				Salary = 26000M,
				Position = "Marketing Coordinator",
				Location = "Vancouver",
			},
			new EmployeeDto()
			{
				Id = 13,
				Name = "Nathan Williams",
				Email = "nathan.williams@company.demo",
				Phone = "+420 012 345 678",
				Salary = 21000M,
				Position = "Sales Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Id = 14,
				Name = "Abby Kim",
				Email = "abby.kim@company.demo",
				Phone = "+420 123 456 789",
				Salary = 24000M,
				Position = "Data Scientist",
				Location = "Los Angeles",
			},
			new EmployeeDto()
			{
				Id = 15,
				Name = "Daniel Choi",
				Email = "daniel.choi@company.demo",
				Phone = "+420 234 567 890",
				Salary = 27000M,
				Position = "Software Engineer",
				Location = "Seoul",
			},
			new EmployeeDto()
			{
				Id = 16,
				Name = "Hannah Garcia",
				Email = "hannah.garcia@company.demo",
				Phone = "+420 123 456 789",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Miami",
			},
			new EmployeeDto()
			{
				Id = 17,
				Name = "William Chen",
				Email = "william.chen@company.demo",
				Phone = "+420 234 567 890",
				Salary = 26000M,
				Position = "Business Analyst",
				Location = "Singapore",
			},
			new EmployeeDto()
			{
				Id = 18,
				Name = "Ethan Davis",
				Email = "ethan.davis@company.demo",
				Phone = "+420 345 678 901",
				Salary = 21000M,
				Position = "Sales Associate",
				Location = "Houston",
			},
			new EmployeeDto()
			{
				Id = 19,
				Name = "Isabella Kim",
				Email = "isabella.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 24000M,
				Position = "Marketing Coordinator",
				Location = "Melbourne",
			},
			new EmployeeDto()
			{
				Id = 20,
				Name = "Jackson Brown",
				Email = "jackson.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 27000M,
				Position = "Project Manager",
				Location = "Toronto",
			},
			new EmployeeDto()
			{
				Id = 21,
				Name = "Ella Davis",
				Email = "ella.davis@company.demo",
				Phone = "+420 123 456 789",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Los Angeles",
			},
			new EmployeeDto()
			{
				Id = 22,
				Name = "Ryan Nguyen",
				Email = "ryan.nguyen@company.demo",
				Phone = "+420 234 567 890",
				Salary = 26000M,
				Position = "Business Analyst",
				Location = "Ho Chi Minh City",
			},
			new EmployeeDto()
			{
				Id = 23,
				Name = "Sophie Hernandez",
				Email = "sophie.hernandez@company.demo",
				Phone = "+420 345 678 901",
				Salary = 21000M,
				Position = "Sales Associate",
				Location = "Buenos Aires",
			},
			new EmployeeDto()
			{
				Id = 24,
				Name = "Alexander Kim",
				Email = "alexander.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 24000M,
				Position = "Marketing Coordinator",
				Location = "Vancouver",
			},
			new EmployeeDto()
			{
				Id = 25,
				Name = "Benjamin Brown",
				Email = "benjamin.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 27000M,
				Position = "Project Manager",
				Location = "Berlin",
			},
			new EmployeeDto()
			{
				Id = 26,
				Name = "Robert Jackson",
				Email = "robert.jackson@company.demo",
				Phone = "+420 678 901 234",
				Salary = 30000M,
				Position = "Senior Software Engineer",
				Location = "Chicago",
			},
			new EmployeeDto()
			{
				Id = 27,
				Name = "Elizabeth Martinez",
				Email = "elizabeth.martinez@company.demo",
				Phone = "+420 789 012 345",
				Salary = 32000M,
				Position = "Senior Product Manager",
				Location = "Toronto",
			},
			new EmployeeDto()
			{
				Id = 28,
				Name = "William Davis",
				Email = "william.davis@company.demo",
				Phone = "+420 890 123 456",
				Salary = 27000M,
				Position = "Sales Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Id = 29,
				Name = "Sophia Lee",
				Email = "sophia.lee@company.demo",
				Phone = "+420 901 234 567",
				Salary = 29000M,
				Position = "Senior Data Analyst",
				Location = "Tokyo",
			},
			new EmployeeDto()
			{
				Id = 30,
				Name = "Gabriel Garcia",
				Email = "gabriel.garcia@company.demo",
				Phone = "+420 012 345 678",
				Salary = 34000M,
				Position = "Senior Marketing Manager",
				Location = "Rio de Janeiro",
			},
			new EmployeeDto()
			{
				Id = 31,
				Name = "Jane Smith",
				Email = "jane.smith@company.demo",
				Phone = "+420 123 456 789",
				Salary = 20000M,
				Position = "Software Engineer",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 32,
				Name = "Jane Doe",
				Email = "jane.doe@company.demo",
				Phone = "+420 987 654 321",
				Salary = 25000M,
				Position = "Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 33,
				Name = "Bob Johnson",
				Email = "bob.johnson@company.demo",
				Phone = "+420 555 555 555",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 34,
				Name = "Sarah Lee",
				Email = "sarah.lee@company.demo",
				Phone = "+420 111 222 333",
				Salary = 22000M,
				Position = "Marketing Coordinator",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 35,
				Name = "David Kim",
				Email = "david.kim@company.demo",
				Phone = "+420 999 888 777",
				Salary = 30000M,
				Position = "Senior Software Engineer",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 36,
				Name = "Eva Novak",
				Email = "eva.novak@company.demo",
				Phone = "+420 777 777 777",
				Salary = 22000M,
				Position = "HR Manager",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 37,
				Name = "Adam Smith",
				Email = "adam.smith@company.demo",
				Phone = "+420 333 333 333",
				Salary = 24000M,
				Position = "Marketing Manager",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 38,
				Name = "Linda Lee",
				Email = "linda.lee@company.demo",
				Phone = "+420 222 222 222",
				Salary = 28000M,
				Position = "Senior Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 39,
				Name = "Peter Brown",
				Email = "peter.brown@company.demo",
				Phone = "+420 111 111 111",
				Salary = 19000M,
				Position = "Sales Manager",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 40,
				Name = "Nina Black",
				Email = "nina.black@company.demo",
				Phone = "+420 999 999 999",
				Salary = 21000M,
				Position = "Marketing Specialist",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 41,
				Name = "Mark Davis",
				Email = "mark.davis@company.demo",
				Phone = "+420 888 888 888",
				Salary = 27000M,
				Position = "Software Architect",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 42,
				Name = "Jack Green",
				Email = "jack.green@company.demo",
				Phone = "+420 666 666 666",
				Salary = 22000M,
				Position = "Software Developer",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 43,
				Name = "Emily Jones",
				Email = "emily.jones@company.demo",
				Phone = "+420 555 444 333",
				Salary = 24000M,
				Position = "Product Owner",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 44,
				Name = "Chris Wilson",
				Email = "chris.wilson@company.demo",
				Phone = "+420 777 888 999",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 45,
				Name = "Olivia Taylor",
				Email = "olivia.taylor@company.demo",
				Phone = "+420 111 222 333",
				Salary = 20000M,
				Position = "Marketing Specialist",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 46,
				Name = "Harry Brown",
				Email = "harry.brown@company.demo",
				Phone = "+420 444 555 666",
				Salary = 26000M,
				Position = "Senior Software Engineer",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 47,
				Name = "Sophia Wilson",
				Email = "sophia.wilson@company.demo",
				Phone = "+420 333 444 555",
				Salary = 23000M,
				Position = "Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 48,
				Name = "Lucas Miller",
				Email = "lucas.miller@company.demo",
				Phone = "+420 777 666 555",
				Salary = 21000M,
				Position = "Software Engineer",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 49,
				Name = "Mia Davis",
				Email = "mia.davis@company.demo",
				Phone = "+420 888 777 666",
				Salary = 22000M,
				Position = "Marketing Coordinator",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Id = 50,
				Name = "Alexander Wilson",
				Email = "alexander.wilson@company.demo",
				Phone = "+420 222 333 444",
				Salary = 28000M,
				Position = "Product Owner",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Id = 51,
				Name = "Charlotte Harris",
				Email = "charlotte.harris@company.demo",
				Phone = "+420 111 555 999",
				Salary = 19000M,
				Position = "Sales Representative",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Id = 52,
				Name = "Dominik Johnson",
				Email = "dominik.johnson@company.demo",
				Phone = "+420 222 555 999",
				Salary = 45000M,
				Position = "CEO",
				Location = "Prague"
			}
		};
	}
	public IEnumerable<EmployeeDto> GetAllEmployees()
	{
		_logger.LogInformation("DemoDataService.GetAllEmployees() called.");
		return _employees.ToList();
	}

	public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("DemoDataService.GetAllEmployeesAsync() called.");

		await Task.Delay(150, cancellationToken); // simulate server call

		return _employees.ToList();
	}

	public IQueryable<EmployeeDto> GetEmployeesAsQueryable()
	{
		_logger.LogInformation("DemoDataService.GetEmployeesAsQueryable() called.");
		return _employees.AsQueryable();
	}

	public async Task<IEnumerable<EmployeeDto>> GetEmployeesDataFragmentAsync(int startIndex, int? count, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.GetEmployeesDataFragmentAsync(startIndex: {startIndex}, count: {count}) called.");

		await Task.Delay(80, cancellationToken); // simulate server call
		return _employees.Skip(startIndex).Take(count ?? Int32.MaxValue).ToList();
	}


	public async Task<IEnumerable<EmployeeDto>> GetEmployeesDataFragmentAsync(EmployeesFilterDto filter, int startIndex, int? count, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.GetEmployeesDataFragmentAsync(startIndex: {startIndex}, count: {count}) called.");

		await Task.Delay(80, cancellationToken); // simulate server call

		return GetFilteredEmployees(filter).Skip(startIndex).Take(count ?? Int32.MaxValue).ToList();
	}

	private IEnumerable<EmployeeDto> GetFilteredEmployees(EmployeesFilterDto filter)
	{
		return _employees
			.WhereIf(!String.IsNullOrWhiteSpace(filter.Name), e => e.Name.Contains(filter.Name, StringComparison.CurrentCultureIgnoreCase))
			.WhereIf(!String.IsNullOrWhiteSpace(filter.Phone), e => e.Phone.Contains(filter.Phone, StringComparison.CurrentCultureIgnoreCase))
			.WhereIf(filter.SalaryMin.HasValue, e => e.Salary >= filter.SalaryMin)
			.WhereIf(filter.SalaryMax.HasValue, e => e.Salary <= filter.SalaryMax)
			.WhereIf(!String.IsNullOrWhiteSpace(filter.Position), e => e.Position.Contains(filter.Position, StringComparison.CurrentCultureIgnoreCase))
			.WhereIf(!String.IsNullOrWhiteSpace(filter.Location), e => e.Location.Contains(filter.Location, StringComparison.CurrentCultureIgnoreCase));
	}

	public async Task<int> GetEmployeesCountAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("DemoDataService.GetEmployeesCountAsync(..) called.");

		try
		{
			await Task.Delay(80, cancellationToken); // simulate server call
		}
		catch (TaskCanceledException)
		{
			_logger.LogInformation("DemoDataService.GetEmployeesCountAsync(..) canceled.");
			throw;
		}

		return _employees.Count;
	}

	public async Task<int> GetEmployeesCountAsync(EmployeesFilterDto filter, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("DemoDataService.GetEmployeesCountAsync(..) called.");

		try
		{
			await Task.Delay(80, cancellationToken); // simulate server call
		}
		catch (TaskCanceledException)
		{
			_logger.LogInformation("DemoDataService.GetEmployeesCountAsync(..) canceled.");
			throw;
		}

		return GetFilteredEmployees(filter).Count();
	}

	public async Task<List<EmployeeDto>> FindEmployeesByNameAsync(string query, int? limitCount = null, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.FindEmployeesByNameAsync(\"{query}\", {limitCount}) called.");

		try
		{
			await Task.Delay(180, cancellationToken); // simulate server call
		}
		catch (TaskCanceledException)
		{
			_logger.LogInformation($"DemoDataService.FindEmployeesByNameAsync(\"{query}\") canceled.");
			throw;
		}

		return _employees
			.Where(e => e.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase))
			.OrderBy(e => e.Name)
			.Take(limitCount ?? Int32.MaxValue)
			.ToList();
	}

	public async Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.GetEmployeeByIdAsync(\"{employeeId}\") called.");

		await Task.Delay(80, cancellationToken); // simulate server call

		return _employees.FirstOrDefault(e => e.Id == employeeId);
	}

	public async Task<List<EmployeeDto>> GetPreferredEmployeesAsync(int count, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.GetPreferredEmployeesAsync({count}) called.");

		await Task.Delay(80, cancellationToken); // simulate server call

		return _employees.OrderByDescending(e => e.Id).Take(count).ToList();
	}

	public async Task DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.DeleteEmployeeAsync(\"{employeeId}\") called.");

		await Task.Delay(80, cancellationToken); // simulate server call

		_employees.RemoveAll(e => e.Id == employeeId);
	}

	public async Task UpdateEmployeeAsync(EmployeeDto employee, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation($"DemoDataService.UpdateEmployee(\"{employee.Id}\") called.");

		// simulate server call
		await Task.Delay(120, cancellationToken);

		var existingEmployee = _employees.FirstOrDefault(e => e.Id == employee.Id);
		Contract.Requires<InvalidOperationException>(existingEmployee != null, $"Employee with ID {employee.Id} not found.");

		existingEmployee.Name = employee.Name;
		existingEmployee.Email = employee.Email;
		existingEmployee.Phone = employee.Phone;
		existingEmployee.Salary = employee.Salary;
		existingEmployee.Position = employee.Position;
		existingEmployee.Location = employee.Location;
	}
}
