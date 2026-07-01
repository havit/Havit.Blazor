# HxOtpInput_Demo_Validation.razor

```razor
<EditForm Model="model" OnValidSubmit="HandleValidSubmit">
	<DataAnnotationsValidator />

	<HxOtpInput Label="Verification code" @bind-Value="model.Code" />

	<HxSubmit Color="ThemeColor.Primary" CssClass="mt-3">Verify</HxSubmit>
</EditForm>

<p class="mt-3">@message</p>

@code {
	private DemoModel model = new();
	private string message;

	private void HandleValidSubmit()
	{
		message = "Validation passed... :-)";
	}

	public class DemoModel
	{
		[Required(ErrorMessage = "Enter the verification code.")]
		[StringLength(6, MinimumLength = 6, ErrorMessage = "The verification code has 6 digits.")]
		public string Code { get; set; }
	}
}

```
