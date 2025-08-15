using FluentValidation;

namespace BlazorAppTest.Pages;
public partial class HxScrollToValidationMessageContainerTest
{
	private FormModel model1 = new();
	private FormModel model2 = new();

	public class FormModel
	{
		public string Arg0 { get; set; }
		public DateTime Arg1 { get; set; }
		public int Arg2 { get; set; }
		public int Arg3 { get; set; }
	}

	public class FormModelValidator : AbstractValidator<FormModel>
	{
		public FormModelValidator()
		{
			// Random validation rules for testing purposes
			RuleFor(x => x.Arg0).NotEmpty();
			RuleFor(x => x.Arg1).NotEmpty().LessThan(DateTime.Now).WithMessage("Arg1 must be in the past.");
			RuleFor(x => x.Arg2).InclusiveBetween(1, 100);
			RuleFor(x => x.Arg2).Must(x => x % 2 == 0).WithMessage("Arg2 must be even.");
			RuleFor(x => x.Arg3).GreaterThan(0).WithMessage("Arg3 must be greater than 0.");
		}
	}
}