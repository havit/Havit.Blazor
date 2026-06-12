# HxStepper_Demo_Colors.razor

```razor
<HxStepper Horizontal="StepperHorizontal.Always" CssClass="mb-4">
	<HxStepperItem Active="true" Color="ThemeColor.Accent" Text="Create account" />
	<HxStepperItem Text="Confirm email" />
	<HxStepperItem Active="true" Color="ThemeColor.Success" Text="Update profile" />
	<HxStepperItem Active="true" Color="ThemeColor.Danger" Text="Finish" />
</HxStepper>

<HxStepper Horizontal="StepperHorizontal.Always" Color="ThemeColor.Success">
	<HxStepperItem Active="true" Text="Create account" />
	<HxStepperItem Active="true" Text="Confirm email" />
	<HxStepperItem Text="Update profile" />
	<HxStepperItem Text="Finish" />
</HxStepper>

```
