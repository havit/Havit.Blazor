# HxStepper_Demo_Alignment.razor

```razor
<HxStepper Horizontal="StepperHorizontal.Always" CssClass="mb-4">
	<HxStepperItem Active="true" Text="Default stepper" />
	<HxStepperItem Active="true" Text="Confirm email" />
	<HxStepperItem Text="Update profile" />
	<HxStepperItem Text="Finish" />
</HxStepper>

<div class="text-center mb-4">
	<HxStepper Horizontal="StepperHorizontal.Always">
		<HxStepperItem Active="true" Text="Center stepper" />
		<HxStepperItem Active="true" Text="Confirm email" />
		<HxStepperItem Text="Update profile" />
		<HxStepperItem Text="Finish" />
	</HxStepper>
</div>

<div class="text-end mb-4">
	<HxStepper Horizontal="StepperHorizontal.Always">
		<HxStepperItem Active="true" Text="End stepper" />
		<HxStepperItem Active="true" Text="Confirm email" />
		<HxStepperItem Text="Update profile" />
		<HxStepperItem Text="Finish" />
	</HxStepper>
</div>

<HxStepper Horizontal="StepperHorizontal.Always" CssClass="w-100">
	<HxStepperItem Active="true" Text="Full-width stepper" />
	<HxStepperItem Active="true" Text="Confirm email" />
	<HxStepperItem Text="Update profile" />
	<HxStepperItem Text="Finish" />
</HxStepper>

```
