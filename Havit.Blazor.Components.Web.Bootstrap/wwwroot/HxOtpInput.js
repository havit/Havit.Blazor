﻿export function initialize(element, hxOtpInputDotnetObjectReference, options, value) {
	if (!element) {
		return;
	}

	// idempotent (re)initialization - e.g. when the Length parameter changes, the component is reinitialized
	dispose(element);

	element.hxOtpInputDotnetObjectReference = hxOtpInputDotnetObjectReference;
	element.addEventListener('input.bs.otpInput', handleInput);
	element.addEventListener('complete.bs.otpInput', handleComplete);

	// align the input value with the .NET value (directly, without raising any events)
	const input = element.querySelector('input');
	if (input) {
		input.value = value ?? '';
	}

	const otpInput = new bootstrap.OtpInput(element, options);

	element.hxOtpInputLastKnownValue = otpInput.getValue();
}

export function setValue(element, value) {
	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (!otpInput) {
		return;
	}

	element.hxOtpInputLastKnownValue = value ?? '';
	otpInput.setValue(value ?? ''); // raises complete.bs.otpInput when the code is complete
	element.hxOtpInputLastKnownValue = otpInput.getValue();
}

export function focus(element) {
	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (otpInput) {
		otpInput.focus();
	}
}

function handleInput(event) {
	// Bootstrap raises the event for every change (typing, backspace, delete, paste, autofill)
	const element = event.currentTarget;
	if (!element.hxOtpInputDotnetObjectReference || (event.value === element.hxOtpInputLastKnownValue)) {
		return;
	}

	element.hxOtpInputLastKnownValue = event.value;
	element.hxOtpInputDotnetObjectReference.invokeMethodAsync('HxOtpInput_HandleJsInput', event.value);
}

function handleComplete(event) {
	const element = event.currentTarget;
	element.hxOtpInputLastKnownValue = event.value;
	element.hxOtpInputDotnetObjectReference?.invokeMethodAsync('HxOtpInput_HandleJsComplete', event.value);
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('input.bs.otpInput', handleInput);
	element.removeEventListener('complete.bs.otpInput', handleComplete);
	element.hxOtpInputDotnetObjectReference = null;
	element.hxOtpInputLastKnownValue = null;

	const otpInput = bootstrap.OtpInput.getInstance(element);
	if (otpInput) {
		otpInput.dispose(); // removes the rendered slots
	}
}
