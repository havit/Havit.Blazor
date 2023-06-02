namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.DemoData;

public class DemoDataService : IDemoDataService
{
	private readonly ILogger<DemoDataService> logger;

	private List<EmployeeDto> employees { get; }

	public DemoDataService(ILogger<DemoDataService> logger)
	{
		this.logger = logger;

		employees = new()
		{
			new EmployeeDto()
			{
				Name = "John Smith",
				Email = "john.smith@company.demo",
				Phone = "+420 123 456 789",
				Salary = 20000M,
				Position = "Software Engineer",
				Location = "Prague",
			},
			new EmployeeDto()
			{
				Name = "Mary Johnson",
				Email = "mary.johnson@company.demo",
				Phone = "+420 234 567 890",
				Salary = 25000M,
				Position = "Product Manager",
				Location = "San Francisco",
			},
			new EmployeeDto()
			{
				Name = "David Lee",
				Email = "david.lee@company.demo",
				Phone = "+420 345 678 901",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "New York",
			},
			new EmployeeDto()
			{
				Name = "Jasmine Kim",
				Email = "jasmine.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 22000M,
				Position = "Data Analyst",
				Location = "Seoul",
			},
			new EmployeeDto()
			{
				Name = "Alexandra Brown",
				Email = "alexandra.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 28000M,
				Position = "Marketing Manager",
				Location = "London",
			},
			new EmployeeDto()
			{
				Name = "Robert Garcia",
				Email = "robert.garcia@company.demo",
				Phone = "+420 789 012 345",
				Salary = 23000M,
				Position = "Software Engineer",
				Location = "Barcelona",
			},
			new EmployeeDto()
			{
				Name = "Olivia Smith",
				Email = "olivia.smith@company.demo",
				Phone = "+420 890 123 456",
				Salary = 26000M,
				Position = "Product Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Name = "Mason Johnson",
				Email = "mason.johnson@company.demo",
				Phone = "+420 012 345 678",
				Salary = 20000M,
				Position = "Sales Representative",
				Location = "Houston",
			},
			new EmployeeDto()
			{
				Name = "Ava Lee",
				Email = "ava.lee@company.demo",
				Phone = "+420 123 456 789",
				Salary = 24000M,
				Position = "Data Analyst",
				Location = "Tokyo",
			},
			new EmployeeDto()
			{
				Name = "Jacob Kim",
				Email = "jacob.kim@company.demo",
				Phone = "+420 234 567 890",
				Salary = 27000M,
				Position = "Marketing Manager",
				Location = "Paris",
			},
			new EmployeeDto()
			{
				Name = "Samuel Adams",
				Email = "samuel.adams@company.demo",
				Phone = "+420 789 012 345",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Boston",
			},
			new EmployeeDto()
			{
				Name = "Emily Park",
				Email = "emily.park@company.demo",
				Phone = "+420 890 123 456",
				Salary = 26000M,
				Position = "Marketing Coordinator",
				Location = "Vancouver",
			},
			new EmployeeDto()
			{
				Name = "Nathan Williams",
				Email = "nathan.williams@company.demo",
				Phone = "+420 012 345 678",
				Salary = 21000M,
				Position = "Sales Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Name = "Abby Kim",
				Email = "abby.kim@company.demo",
				Phone = "+420 123 456 789",
				Salary = 24000M,
				Position = "Data Scientist",
				Location = "Los Angeles",
			},
			new EmployeeDto()
			{
				Name = "Daniel Choi",
				Email = "daniel.choi@company.demo",
				Phone = "+420 234 567 890",
				Salary = 27000M,
				Position = "Software Engineer",
				Location = "Seoul",
			},
			new EmployeeDto()
			{
				Name = "Hannah Garcia",
				Email = "hannah.garcia@company.demo",
				Phone = "+420 123 456 789",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Miami",
			},
			new EmployeeDto()
			{
				Name = "William Chen",
				Email = "william.chen@company.demo",
				Phone = "+420 234 567 890",
				Salary = 26000M,
				Position = "Business Analyst",
				Location = "Singapore",
			},
			new EmployeeDto()
			{
				Name = "Ethan Davis",
				Email = "ethan.davis@company.demo",
				Phone = "+420 345 678 901",
				Salary = 21000M,
				Position = "Sales Associate",
				Location = "Houston",
			},
			new EmployeeDto()
			{
				Name = "Isabella Kim",
				Email = "isabella.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 24000M,
				Position = "Marketing Coordinator",
				Location = "Melbourne",
			},
			new EmployeeDto()
			{
				Name = "Jackson Brown",
				Email = "jackson.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 27000M,
				Position = "Project Manager",
				Location = "Toronto",
			},
			new EmployeeDto()
			{
				Name = "Ella Davis",
				Email = "ella.davis@company.demo",
				Phone = "+420 123 456 789",
				Salary = 23000M,
				Position = "Software Developer",
				Location = "Los Angeles",
			},
			new EmployeeDto()
			{
				Name = "Ryan Nguyen",
				Email = "ryan.nguyen@company.demo",
				Phone = "+420 234 567 890",
				Salary = 26000M,
				Position = "Business Analyst",
				Location = "Ho Chi Minh City",
			},
			new EmployeeDto()
			{
				Name = "Sophie Hernandez",
				Email = "sophie.hernandez@company.demo",
				Phone = "+420 345 678 901",
				Salary = 21000M,
				Position = "Sales Associate",
				Location = "Buenos Aires",
			},
			new EmployeeDto()
			{
				Name = "Alexander Kim",
				Email = "alexander.kim@company.demo",
				Phone = "+420 456 789 012",
				Salary = 24000M,
				Position = "Marketing Coordinator",
				Location = "Vancouver",
			},
			new EmployeeDto()
			{
				Name = "Benjamin Brown",
				Email = "benjamin.brown@company.demo",
				Phone = "+420 567 890 123",
				Salary = 27000M,
				Position = "Project Manager",
				Location = "Berlin",
			},
			new EmployeeDto()
			{
				Name = "Robert Jackson",
				Email = "robert.jackson@company.demo",
				Phone = "+420 678 901 234",
				Salary = 30000M,
				Position = "Senior Software Engineer",
				Location = "Chicago",
			},
			new EmployeeDto()
			{
				Name = "Elizabeth Martinez",
				Email = "elizabeth.martinez@company.demo",
				Phone = "+420 789 012 345",
				Salary = 32000M,
				Position = "Senior Product Manager",
				Location = "Toronto",
			},
			new EmployeeDto()
			{
				Name = "William Davis",
				Email = "william.davis@company.demo",
				Phone = "+420 890 123 456",
				Salary = 27000M,
				Position = "Sales Manager",
				Location = "Sydney",
			},
			new EmployeeDto()
			{
				Name = "Sophia Lee",
				Email = "sophia.lee@company.demo",
				Phone = "+420 901 234 567",
				Salary = 29000M,
				Position = "Senior Data Analyst",
				Location = "Tokyo",
			},
			new EmployeeDto()
			{
				Name = "Gabriel Garcia",
				Email = "gabriel.garcia@company.demo",
				Phone = "+420 012 345 678",
				Salary = 34000M,
				Position = "Senior Marketing Manager",
				Location = "Rio de Janeiro",
			},
			new EmployeeDto()
			{
				Name = "John Smith",
				Email = "john.smith@company.demo",
				Phone = "+420 123 456 789",
				Salary = 20000M,
				Position = "Software Engineer",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Jane Doe",
				Email = "jane.doe@company.demo",
				Phone = "+420 987 654 321",
				Salary = 25000M,
				Position = "Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Bob Johnson",
				Email = "bob.johnson@company.demo",
				Phone = "+420 555 555 555",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Sarah Lee",
				Email = "sarah.lee@company.demo",
				Phone = "+420 111 222 333",
				Salary = 22000M,
				Position = "Marketing Coordinator",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "David Kim",
				Email = "david.kim@company.demo",
				Phone = "+420 999 888 777",
				Salary = 30000M,
				Position = "Senior Software Engineer",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Eva Novak",
				Email = "eva.novak@company.demo",
				Phone = "+420 777 777 777",
				Salary = 22000M,
				Position = "HR Manager",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Adam Smith",
				Email = "adam.smith@company.demo",
				Phone = "+420 333 333 333",
				Salary = 24000M,
				Position = "Marketing Manager",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "Linda Lee",
				Email = "linda.lee@company.demo",
				Phone = "+420 222 222 222",
				Salary = 28000M,
				Position = "Senior Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Peter Brown",
				Email = "peter.brown@company.demo",
				Phone = "+420 111 111 111",
				Salary = 19000M,
				Position = "Sales Manager",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Nina Black",
				Email = "nina.black@company.demo",
				Phone = "+420 999 999 999",
				Salary = 21000M,
				Position = "Marketing Specialist",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "Mark Davis",
				Email = "mark.davis@company.demo",
				Phone = "+420 888 888 888",
				Salary = 27000M,
				Position = "Software Architect",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Jack Green",
				Email = "jack.green@company.demo",
				Phone = "+420 666 666 666",
				Salary = 22000M,
				Position = "Software Developer",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Emily Jones",
				Email = "emily.jones@company.demo",
				Phone = "+420 555 444 333",
				Salary = 24000M,
				Position = "Product Owner",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "Chris Wilson",
				Email = "chris.wilson@company.demo",
				Phone = "+420 777 888 999",
				Salary = 18000M,
				Position = "Sales Representative",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Olivia Taylor",
				Email = "olivia.taylor@company.demo",
				Phone = "+420 111 222 333",
				Salary = 20000M,
				Position = "Marketing Specialist",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Harry Brown",
				Email = "harry.brown@company.demo",
				Phone = "+420 444 555 666",
				Salary = 26000M,
				Position = "Senior Software Engineer",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "Sophia Wilson",
				Email = "sophia.wilson@company.demo",
				Phone = "+420 333 444 555",
				Salary = 23000M,
				Position = "Product Manager",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Lucas Miller",
				Email = "lucas.miller@company.demo",
				Phone = "+420 777 666 555",
				Salary = 21000M,
				Position = "Software Engineer",
				Location = "Brno"
			},
			new EmployeeDto()
			{
				Name = "Mia Davis",
				Email = "mia.davis@company.demo",
				Phone = "+420 888 777 666",
				Salary = 22000M,
				Position = "Marketing Coordinator",
				Location = "Ostrava"
			},
			new EmployeeDto()
			{
				Name = "Alexander Wilson",
				Email = "alexander.wilson@company.demo",
				Phone = "+420 222 333 444",
				Salary = 28000M,
				Position = "Product Owner",
				Location = "Prague"
			},
			new EmployeeDto()
			{
				Name = "Charlotte Harris",
				Email = "charlotte.harris@company.demo",
				Phone = "+420 111 555 999",
				Salary = 19000M,
				Position = "Sales Representative",
				Location = "Brno"
			}
		};
	}
	public IEnumerable<EmployeeDto> GetAllEmployees()
	{
		logger.LogInformation("DemoDataService.GetAllEmployees() called.");
		return employees.ToList();
	}

	public async Task<IEnumerable<EmployeeDto>> GetEmployeesDataFragmentAsync(int startIndex, int? count, CancellationToken cancellationToken = default)
	{
		logger.LogInformation($"DemoDataService.GetEmployeesDataFragmentAsync(startIndex: {startIndex}, count: {count}) called.");

		await Task.Delay(80, cancellationToken); // simulate server call
		return employees.Skip(startIndex).Take(count ?? Int32.MaxValue).ToList();
	}

	public async Task<int> GetEmployeesCountAsync(CancellationToken cancellationToken = default)
	{
		logger.LogInformation("DemoDataService.GetEmployeesCountAsync(..) called.");

		await Task.Delay(80, cancellationToken); // simulate server call
		return employees.Count;
	}

	public async Task<List<EmployeeDto>> FindEmployeesByName(string query, CancellationToken cancellationToken = default)
	{
		logger.LogInformation($"DemoDataService.FindEmployeesByName(\"{query}\") called.");

		await Task.Delay(180, cancellationToken); // simulate server call
		return employees.Where(e => e.Name.Contains(query, StringComparison.CurrentCultureIgnoreCase)).ToList();
	}
}
