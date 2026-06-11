# HxPasswordStrength_Demo_ProgressBar.razor

```razor
<HxPasswordStrength Variant="PasswordStrengthVariant.ProgressBar">
	<HxInputText Label="Password" Type="InputType.Password" @bind-Value="password" BindEvent="BindEvent.OnInput" />
</HxPasswordStrength>

@code {
	private string password;
}

```
