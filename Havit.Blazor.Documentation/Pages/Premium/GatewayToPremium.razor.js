export function setSkipGatewayPage(skipGatewayPage) {
	const date = new Date();
	date.setTime(date.getTime() + (24 * 60 * 60 * 1000)); // 24 hours
	document.cookie = "SkipGatewayPage=" + skipGatewayPage + "; expires = " + date.toGMTString() + "; path = /";
}

export function getSkipGatewayPage() {
	const name = "SkipGatewayPage=";
	const decodedCookie = decodeURIComponent(document.cookie);
	const ca = decodedCookie.split(';');
	for (let i = 0; i < ca.length; i++) {
		let c = ca[i];
		while (c.charAt(0) === ' ') {
			c = c.substring(1);
		}
		if (c.indexOf(name) === 0) {
			return c.substring(name.length, c.length);
		}
	}
	return "";
}
