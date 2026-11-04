// Text feedback colors per strength level (the same mapping as the Bootstrap Strength plugin uses).
const textColors = {
	weak: 'danger',
	fair: 'warning',
	good: 'info',
	strong: 'success'
};

export function initialize(element, hxPasswordStrengthDotnetObjectReference, options) {
	if (!element) {
		return;
	}

	element.hxPasswordStrengthDotnetObjectReference = hxPasswordStrengthDotnetObjectReference;
	element.addEventListener('strengthChange.bs.strength', handleStrengthChange);

	new bootstrap.Strength(element, options ?? {});
}

function handleStrengthChange(event) {
	// The plugin sets "--strength-color: var(--{color}-text)" on the text element, but the CSS reads --bs-strength-color
	// and the --{color}-text tokens do not exist (Bootstrap 6.0.0-alpha.1 bug). Set the property the CSS reads.
	const textElement = getTextElement(event.target);
	if (textElement) {
		if (event.strength) {
			textElement.style.setProperty('--bs-strength-color', `var(--bs-${textColors[event.strength]}-fg)`);
		} else {
			textElement.style.removeProperty('--bs-strength-color');
		}
	}

	event.target.hxPasswordStrengthDotnetObjectReference.invokeMethodAsync('HxPasswordStrength_HandleStrengthChanged', event.strength ?? null, event.score ?? 0);
}

function getTextElement(element) {
	return element.parentElement?.querySelector(':scope > .strength-text') ?? null;
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

	// Reset the UI updated by the plugin to the state rendered by Blazor (no strength),
	// so the component can render the static display or re-create the plugin with new options.
	delete element.dataset.bsStrength;
	for (const segment of element.querySelectorAll('.strength-segment')) {
		segment.classList.remove('active');
	}

	const textElement = getTextElement(element);
	if (textElement) {
		textElement.textContent = '';
		delete textElement.dataset.bsStrength;
		textElement.style.removeProperty('--bs-strength-color');
		textElement.style.removeProperty('--strength-color');
	}
}
