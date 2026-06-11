# HxPasswordStrength_Demo_Static.razor

```razor
<div class="vstack gap-3">
	<div>
		<div>Weak</div>
		<HxPasswordStrength Strength="PasswordStrengthLevel.Weak" />
	</div>
	<div>
		<div>Fair</div>
		<HxPasswordStrength Strength="PasswordStrengthLevel.Fair" />
	</div>
	<div>
		<div>Good</div>
		<HxPasswordStrength Strength="PasswordStrengthLevel.Good" />
	</div>
	<div>
		<div>Strong</div>
		<HxPasswordStrength Strength="PasswordStrengthLevel.Strong" ShowText="true" />
	</div>
	<div>
		<div>Progress bar</div>
		<HxPasswordStrength Strength="PasswordStrengthLevel.Good" Variant="PasswordStrengthVariant.ProgressBar" />
	</div>
</div>

```
