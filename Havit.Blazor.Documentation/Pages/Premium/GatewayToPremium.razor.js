export function setSkipGatewayPage(skipGatewayPage) {
	const date = new Date();
	date.setTime(date.getTime() + (60 * 24 * 60 * 60 * 1000)); // 60 days
	document.cookie = "SkipGatewayPage=" + skipGatewayPage + "; expires = " + date.toGMTString() + "; path = /";
}
