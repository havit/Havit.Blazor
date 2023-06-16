export function setColorMode(colorMode) {
	if (colorMode === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
		document.documentElement.setAttribute('data-bs-theme', 'dark')
	} else {
		document.documentElement.setAttribute('data-bs-theme', colorMode)
	}

	var date = new Date();
	date.setTime(date.getTime() + (60 * 24 * 60 * 60 * 1000)); // 60 days
	document.cookie = "ColorMode=" + colorMode + "; expires=" + date.toGMTString() + "; path=/";
}