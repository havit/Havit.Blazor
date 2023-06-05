if ((document.documentElement.getAttribute('data-bs-theme') == 'auto') && window.matchMedia('(prefers-color-scheme: dark)').matches) {
	document.documentElement.setAttribute('data-bs-theme', 'dark');
}