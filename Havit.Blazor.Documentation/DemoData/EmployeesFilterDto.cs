namespace Havit.Blazor.Documentation.DemoData;

public record EmployeesFilterDto
{
	public string Name { get; set; }
	public string Phone { get; set; }
	public decimal? SalaryMin { get; set; }
	public decimal? SalaryMax { get; set; }
	public string Position { get; set; }
	public string Location { get; set; }
}
