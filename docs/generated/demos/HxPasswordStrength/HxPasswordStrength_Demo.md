# HxPasswordStrength_Demo.razor

```razor
<HxPasswordStrength>
	<HxInputText Label="Password" Type="InputType.Password" @bind-Value="password" BindEvent="BindEvent.OnInput" />
</HxPasswordStrength>

@code {
	private string password;
}

```
