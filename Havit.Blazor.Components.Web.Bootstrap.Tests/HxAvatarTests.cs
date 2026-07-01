namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxAvatarTests : BunitTestBase
{
	[Fact]
	public void HxAvatar_Render_DisplaysInitials()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.AddChildContent("AB")
		);

		// Assert
		Assert.Equal("AB", cut.Find("span.avatar").TextContent.Trim());
	}

	[Fact]
	public void HxAvatar_ImageSrc_RendersAvatarImg()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.ImageSrc, "avatar.png")
			.Add(p => p.ImageAlt, "John Doe")
		);

		// Assert
		var image = cut.Find("span.avatar > img.avatar-img");
		Assert.Equal("avatar.png", image.GetAttribute("src"));
		Assert.Equal("John Doe", image.GetAttribute("alt"));
	}

	[Theory]
	[InlineData(AvatarSize.ExtraSmall, "avatar-xs")]
	[InlineData(AvatarSize.Small, "avatar-sm")]
	[InlineData(AvatarSize.Large, "avatar-lg")]
	[InlineData(AvatarSize.ExtraLarge, "avatar-xl")]
	public void HxAvatar_Size_AppliesCorrectCssClass(AvatarSize size, string expectedCssClass)
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Size, size)
		);

		// Assert
		Assert.NotNull(cut.Find($"span.avatar.{expectedCssClass}"));
	}

	[Fact]
	public void HxAvatar_RegularSize_DoesNotRenderSizeCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Size, AvatarSize.Regular)
		);

		// Assert
		Assert.Equal("avatar", cut.Find("span").GetAttribute("class"));
	}

	[Theory]
	[InlineData(AvatarStatus.Online, "status-online", "Online")]
	[InlineData(AvatarStatus.Offline, "status-offline", "Offline")]
	[InlineData(AvatarStatus.Busy, "status-busy", "Busy")]
	[InlineData(AvatarStatus.Away, "status-away", "Away")]
	public void HxAvatar_Status_RendersStatusIndicator(AvatarStatus status, string expectedCssClass, string expectedLabel)
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Status, status)
		);

		// Assert
		var statusElement = cut.Find($"span.avatar > span.avatar-status.{expectedCssClass}");
		Assert.Equal("img", statusElement.GetAttribute("role"));
		Assert.Equal(expectedLabel, statusElement.GetAttribute("aria-label"));
	}

	[Fact]
	public void HxAvatar_NoStatus_DoesNotRenderStatusIndicator()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>();

		// Assert
		Assert.Empty(cut.FindAll("span.avatar-status"));
	}

	[Fact]
	public void HxAvatar_StatusLabel_OverridesAriaLabel()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Status, AvatarStatus.Online)
			.Add(p => p.StatusLabel, "Available")
		);

		// Assert
		Assert.Equal("Available", cut.Find("span.avatar-status").GetAttribute("aria-label"));
	}

	[Fact]
	public void HxAvatar_Color_AppliesThemeCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
		);

		// Assert
		Assert.NotNull(cut.Find("span.avatar.theme-primary"));
	}

	[Fact]
	public void HxAvatar_Subtle_AppliesSubtleCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatar>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Danger)
			.Add(p => p.Subtle, true)
		);

		// Assert
		Assert.NotNull(cut.Find("span.avatar.avatar-subtle.theme-danger"));
	}

	[Fact]
	public void HxAvatarStack_Render_RendersAvatars()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatarStack>(parameters => parameters
			.AddChildContent<HxAvatar>(avatar => avatar.AddChildContent("AB"))
			.AddChildContent<HxAvatar>(avatar => avatar.AddChildContent("+5"))
		);

		// Assert
		Assert.Equal(2, cut.FindAll("div.avatar-stack > span.avatar").Count);
	}

	[Theory]
	[InlineData(AvatarSize.ExtraSmall, "avatar-stack-xs")]
	[InlineData(AvatarSize.Small, "avatar-stack-sm")]
	[InlineData(AvatarSize.Large, "avatar-stack-lg")]
	[InlineData(AvatarSize.ExtraLarge, "avatar-stack-xl")]
	public void HxAvatarStack_Size_AppliesCorrectCssClass(AvatarSize size, string expectedCssClass)
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatarStack>(parameters => parameters
			.Add(p => p.Size, size)
		);

		// Assert
		Assert.NotNull(cut.Find($"div.avatar-stack.{expectedCssClass}"));
	}

	[Fact]
	public void HxAvatarStack_RegularSize_DoesNotRenderSizeCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxAvatarStack>();

		// Assert
		Assert.Equal("avatar-stack", cut.Find("div").GetAttribute("class"));
	}
}
