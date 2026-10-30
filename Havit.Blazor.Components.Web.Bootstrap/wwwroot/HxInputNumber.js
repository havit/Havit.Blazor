// Registered from Havit.Blazor.Components.Web.Bootstrap.lib.module.js.
export function register() {
	// Capture phase to run before any other handler (as the original inline handlers did).
	document.addEventListener('keydown', handleKeyDown, { capture: true });
	document.addEventListener('paste', handlePaste, { capture: true });
	document.addEventListener('focusout', handleFocusOut, { capture: true });
}

// SmartKeyboard
// When the user presses '-' key, we toggle the sign of the value.
// We also set the modifiedByCode flag to allow fire change event in focusout event later.
// Selection handling:
// - When everything (but non-empty) is selected before adding -, the digit portion (without the leading -) remains selected after adding -.
//   This way the user can type a digit to replace the digits and end up with a negative number (issue #1142).
// - When selected -12 in the value -1234, 12 must remain selected after - removal.
function handleKeyDown(event) {
	const input = event.target;
	if ((event.key !== '-') || !input.hasAttribute?.('data-hx-input-number-smart-keyboard')) {
		return;
	}

	let newValue;
	let newSelectionStart;
	let newSelectionEnd;
	if (input.value.startsWith('-')) {
		newValue = input.value.substring(1);
		newSelectionStart = Math.max(0, input.selectionStart - 1);
		newSelectionEnd = Math.max(0, input.selectionEnd - 1);
	}
	else {
		const everythingSelected = (input.selectionStart == 0) && (input.selectionEnd > 0) && (input.selectionEnd == input.value.length);
		newValue = '-' + input.value;
		newSelectionStart = everythingSelected ? 1 : input.selectionStart + 1;
		newSelectionEnd = everythingSelected ? newValue.length : input.selectionEnd + 1;
	}

	if (input.value !== newValue) {
		input.value = newValue;
		input.setSelectionRange(newSelectionStart, newSelectionEnd);
		input.dispatchEvent(new Event('input', { bubbles: true }));
		input.modifiedByCode = true;
		event.preventDefault();
	}
}

// SmartPaste
// Normalization of pasted value (applied only if the pasted value differs from the normalized value)
// - If we cancel the original paste event, Blazor does not recognize the change, so we fire change event manually (in focusout).
function handlePaste(event) {
	const input = event.target;
	if (!input.hasAttribute?.('data-hx-input-number-smart-paste')) {
		return;
	}

	const clipboardValue = event.clipboardData.getData('text/plain');
	const normalizedValue = clipboardValue.replace(/[^\d.,\-eE]/g, '');
	if (+clipboardValue != +normalizedValue) {
		input.value = normalizedValue;
		input.dispatchEvent(new Event('input', { bubbles: true }));
		input.modifiedByCode = true;
		event.preventDefault();
	}
}

// When the value is modified by code, we fire the change event.
function handleFocusOut(event) {
	const input = event.target;
	if (input.modifiedByCode) {
		input.modifiedByCode = false;
		input.dispatchEvent(new Event('change', { bubbles: true }));
	}
}
