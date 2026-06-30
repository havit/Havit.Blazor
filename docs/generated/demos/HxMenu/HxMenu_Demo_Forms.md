# HxMenu_Demo_Forms.razor

```razor
<HxMenu AutoClose="MenuAutoClose.Outside" style="--bs-menu-min-width: 300px;">
	<Toggle>
		<HxMenuToggleButton Color="ThemeColor.Secondary">Sign in</HxMenuToggleButton>
	</Toggle>
	<Content>
		<EditForm Model="model" class="p-3" style="--hx-form-spacing: 0.75rem;">
			<HxInputText Label="Email address" Type="InputType.Email" Placeholder="email@example.com" @bind-Value="model.Email" />
			<HxInputText Label="Password" Type="InputType.Password" Placeholder="Password" @bind-Value="model.Password" />
			<HxCheckbox Text="Remember me" @bind-Value="model.RememberMe" />
			<div class="vstack gap-2">
				<HxSubmit Text="Sign in" Color="ThemeColor.Primary" />
				<a class="btn-text theme-secondary" href="#">New around here? Sign up</a>
				<a class="btn-text theme-secondary" href="#">Forgot password?</a>
			</div>
		</EditForm>
	</Content>
</HxMenu>

@code
{
	private FormModel model = new();

	public class FormModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}

```
