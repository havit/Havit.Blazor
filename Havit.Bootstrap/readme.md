# Havit.Bootstrap

## General notes
1. Bootstrap 6 sources are consumed as an npm dependency pinned to a commit of the upstream [`v6-dev` branch](https://github.com/twbs/bootstrap/tree/v6-dev) (`6.0.0-alpha1` is not published to npm yet). The pin lives in `package.json` (`bootstrap: github:twbs/bootstrap#<commit>`).
2. Havit.Bootstrap overrides are done in the *scss* folder. Bootstrap 6 uses the Sass module system — variable overrides are passed via `@use "bootstrap/scss/bootstrap" with (...)` in `scss/bootstrap.scss`.
3. CSS custom properties are written unprefixed in the Sass sources and prefixed with `bs-` by PostCSS (`postcss-prefix-custom-properties`, see `postcss.config.mjs`). The `$prefix` Sass variable no longer exists.
4. There is no RTL build anymore — Bootstrap 6 uses CSS logical properties.
5. Minification uses `lightningcss` (`build/css-minify.mjs`, taken from upstream) because clean-css does not support `@layer`, `color-mix()`, `oklch()`, and other modern CSS used by v6. Browser targets come from `.browserslistrc` (taken from upstream).
6. NodeJS/npm is needed to compile the CSS.
7. Havit.Bootstrap has its own `.editorconfig` with different settings than HAVIT standards to comply with Bootstrap stylelint settings.
8. The former v5 override partials (`scss/_variables.scss`, `_buttons.scss`, `_dropdown.scss`, `_forms.scss`, `_tables.scss`) are kept unimported for reference and are being ported per component within the v6 milestone.

## Update instructions
1. Pick the new upstream commit (or, once released, the npm version) and update the `bootstrap` dependency in `package.json`
2. Run `npm install`
3. Compare `package.json`, `postcss.config.mjs`, `build/css-minify.mjs`, and `.browserslistrc` with upstream (`twbs/bootstrap@v6-dev`) and update dependencies and build scripts accordingly
4. Make sure the build process works by running the `css` build script

## Build instructions
There are several build scripts that take care of compiling, vendor prefixing, and minifying the Bootstrap CSS with HAVIT overrides.
1. Run `npm install` to install the dependencies
2. Build script `css` takes care of cleaning, CSS compilation, CSS custom property prefixing + vendor prefixing, and minification, and runs only once
3. Build script `watch` acts as a file-watcher and runs compilation scripts after saving changes
4. Build script `publish-to-blazor` copies files from the *dist* folder to *Havit.Blazor.Components.Web.Bootstrap/wwwroot*
