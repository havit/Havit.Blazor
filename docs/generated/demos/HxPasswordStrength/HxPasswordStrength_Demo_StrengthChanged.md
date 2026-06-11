# HxPasswordStrength_Demo_StrengthChanged.razor

```razor
<HxPasswordStrength OnStrengthChanged="HandleStrengthChanged">
	<HxInputText Label="Password" Type="InputType.Password" @bind-Value="password" BindEvent="BindEvent.OnInput" />
</HxPasswordStrength>

@if (eventArgs is not null)
{
	<p class="mt-2">
		Strength: <strong>@(eventArgs.Strength?.ToString() ?? "(empty password)")</strong><br />
		Score: <strong>@eventArgs.Score</strong>
	</p>
}

@code {
	private string password;
	private PasswordStrengthChangedEventArgs eventArgs;

	private void HandleStrengthChanged(PasswordStrengthChangedEventArgs args)
	{
		eventArgs = args;
	}
}

```
