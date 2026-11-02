// Loaded by the module script emitted by HxSetup.RenderBootstrapJavaScriptReference().
// An external script (not an inline one), so a strict Content Security Policy needs only 'self' for it, no nonce (#1730).
// Bootstrap 6 is ESM-only; this re-exposes window.bootstrap as a compatibility bridge for the Hx* JS modules.
// The shared window.havitBlazorBootstrapReady promise slot is claimed SYNCHRONOUSLY (??= with no preceding await)
// so this script and the library's JS initializer (beforeWebStart/beforeStart) can never both import the bundle -
// a non-atomic guard would let the other party start a second import of a differently-URLed copy,
// duplicating the Bootstrap data-API listeners (every delegated toggle would fire twice).
// The version query of this script (?v=...) is passed on to the bundle URL.
window.havitBlazorBootstrapReady ??= import('./bootstrap.bundle.min.js' + new URL(import.meta.url).search).then(m => { window.bootstrap = m; });
await window.havitBlazorBootstrapReady;
