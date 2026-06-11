# HxOtpInput_Demo_Separator.razor

```razor
<HxOtpInput Label="Verification code" @bind-Value="code" GroupSize="3" />

<HxOtpInput Label="Connected groups" @bind-Value="code" GroupSize="3" Connected="true" />

@code {
	private string code;
}

```
