export function initialize(element, hxOtpInputDotnetObjectReference, mask, value) {
	if (!element) {
		return;
	}

	// idempotent (re)initialization - e.g. when the Length parameter changes, the component is reinitialized
	dispose(element);

	element.hxOtpInputDotnetObjectReference = hxOtpInputDotnetObjectReference;
	element.addEventListener('input.bs.otp-input', handleInput);
	element.addEventListener('complete.bs.otp-input', handleComplete);
	element.addEventListener('paste', handlePaste);
	element.addEventListener('keydown', handleKeyDown);

	const otpInput = new bootstrap.OtpInput(element, { mask: mask });

	// align the input values with the .NET value (directly, without raising any events)
	const chars = [...(value ?? '')];
	for (const [index, input] of element.querySelectorAll('input').entries()) {
		input.value = chars[index] ?? '';
	}

	element.hxOtpInputLastKnownValue = otpInput.getValue();
}

export function setValue(element, value) {
	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (!otpInput) {
		return;
	}

	element.hxOtpInputLastKnownValue = value ?? '';
	otpInput.setValue(value ?? ''); // raises complete.bs.otp-input when the code is complete
	element.hxOtpInputLastKnownValue = otpInput.getValue();
}

export function focus(element) {
	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (otpInput) {
		otpInput.focus();
	}
}

function handleInput(event) {
	notifyValueChanged(event.currentTarget, event.value);
}

function handleComplete(event) {
	const element = event.currentTarget;
	element.hxOtpInputLastKnownValue = event.value;
	element.hxOtpInputDotnetObjectReference?.invokeMethodAsync('HxOtpInput_HandleJsComplete', event.value);
}

function handlePaste(event) {
	// Bootstrap distributes the pasted digits via OtpInput.setValue() which does not raise the input.bs.otp-input event
	// (the plugin's paste handler on the input runs before this one bubbles to the container)
	notifyCurrentValue(event.currentTarget);
}

function handleKeyDown(event) {
	if ((event.key === 'Backspace') || (event.key === 'Delete')) {
		// Bootstrap mutates the sibling input values directly for these keys (no input.bs.otp-input event is raised)
		const element = event.currentTarget;
		window.setTimeout(() => notifyCurrentValue(element)); // wait for the default action (a Backspace removing a digit raises input.bs.otp-input, deduplicated by the last known value)
	}
}

function notifyCurrentValue(element) {
	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (!otpInput) {
		return;
	}
	notifyValueChanged(element, otpInput.getValue());
}

function notifyValueChanged(element, value) {
	if (!element.hxOtpInputDotnetObjectReference || (value === element.hxOtpInputLastKnownValue)) {
		return;
	}

	element.hxOtpInputLastKnownValue = value;
	element.hxOtpInputDotnetObjectReference.invokeMethodAsync('HxOtpInput_HandleJsInput', value);
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('input.bs.otp-input', handleInput);
	element.removeEventListener('complete.bs.otp-input', handleComplete);
	element.removeEventListener('paste', handlePaste);
	element.removeEventListener('keydown', handleKeyDown);
	element.hxOtpInputDotnetObjectReference = null;
	element.hxOtpInputLastKnownValue = null;

	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (otpInput) {
		otpInput.dispose();
	}
}
