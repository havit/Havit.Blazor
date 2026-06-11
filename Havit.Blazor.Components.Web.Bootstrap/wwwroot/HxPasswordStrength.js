export function initialize(element, hxPasswordStrengthDotnetObjectReference, options) {
	if (!element) {
		return;
	}

	element.hxPasswordStrengthDotnetObjectReference = hxPasswordStrengthDotnetObjectReference;
	element.addEventListener('strengthChange.bs.strength', handleStrengthChange);

	new bootstrap.Strength(element, options ?? {});
}

function handleStrengthChange(event) {
	event.target.hxPasswordStrengthDotnetObjectReference.invokeMethodAsync('HxPasswordStrength_HandleStrengthChanged', event.strength ?? null, event.score ?? 0);
}

export function evaluate(element) {
	const strength = bootstrap.Strength.getInstance(element);
	if (strength) {
		strength.evaluate();
	}
}

export function getStrength(element) {
	const strength = bootstrap.Strength.getInstance(element);
	return strength ? strength.getStrength() : null;
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('strengthChange.bs.strength', handleStrengthChange);
	element.hxPasswordStrengthDotnetObjectReference = null;

	const strength = bootstrap.Strength.getInstance(element);
	if (strength) {
		strength.dispose();
	}
}
