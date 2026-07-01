# HxDialog_Demo_Animation.razor

```razor
<HxDialog @ref="noAnimationDialog" Animation="DialogAnimation.None" Title="No animation">
	<BodyTemplate>
		This dialog opens and closes instantly, without any animation.
	</BodyTemplate>
</HxDialog>
<HxDialog @ref="slideDownDialog" Animation="DialogAnimation.SlideDown" Title="Slide down">
	<BodyTemplate>
		This dialog slides down when opening.
	</BodyTemplate>
</HxDialog>
<HxDialog @ref="slideUpDialog" Animation="DialogAnimation.SlideUp" Title="Slide up">
	<BodyTemplate>
		This dialog slides up when opening.
	</BodyTemplate>
</HxDialog>

<HxButton OnClick="() => noAnimationDialog.ShowAsync()" Color="ThemeColor.Primary" CssClass="me-1 mb-1">No animation</HxButton>
<HxButton OnClick="() => slideDownDialog.ShowAsync()" Color="ThemeColor.Primary" CssClass="me-1 mb-1">Slide down</HxButton>
<HxButton OnClick="() => slideUpDialog.ShowAsync()" Color="ThemeColor.Primary" CssClass="me-1 mb-1">Slide up</HxButton>

@code
{
	private HxDialog noAnimationDialog;
	private HxDialog slideDownDialog;
	private HxDialog slideUpDialog;
}
```
