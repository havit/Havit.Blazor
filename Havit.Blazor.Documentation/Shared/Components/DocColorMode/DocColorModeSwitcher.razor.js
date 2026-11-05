export function setColorMode(colorMode) {
	if (colorMode === 'auto') {
		// no data-bs-theme = Bootstrap follows the system preference (color-scheme: light dark)
		document.documentElement.removeAttribute('data-bs-theme')
	} else {
		document.documentElement.setAttribute('data-bs-theme', colorMode)
	}

	const date = new Date();
	date.setTime(date.getTime() + (60 * 24 * 60 * 60 * 1000)); // 60 days
	document.cookie = "ColorMode=" + colorMode + "; expires=" + date.toGMTString() + "; path=/";
}