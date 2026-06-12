# HxPasswordStrength_Demo_TextFeedback.razor

```razor
<HxPasswordStrength ShowText="true"
					WeakText="Too weak"
					FairText="Could be better"
					GoodText="Good password"
					StrongText="Excellent!">
	<HxInputText Label="Password" Type="InputType.Password" @bind-Value="password" BindEvent="BindEvent.OnInput" />
</HxPasswordStrength>

@code {
	private string password;
}

```
