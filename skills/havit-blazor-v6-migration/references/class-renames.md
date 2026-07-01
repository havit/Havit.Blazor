# Bootstrap 5 → 6 class rename tables (Havit.Blazor v4 → v6)

Verification: every NEW class below exists in the compiled v6 bundle shipped with the NuGet package
(`~/.nuget/packages/havit.blazor.components.web.bootstrap/6.*/.../bootstrap.min.css`); none of the OLD ones do.
When grepping the minified bundle, escape `:` as `\:` and the leading digit of `2xl` as `\32 ` (with a trailing space), e.g. `.md\:d-flex`, `.\32 xl\:col-4`.

## 1. Responsive syntax: infix → prefix (all utilities + grid)

Rule: `{utility}-{bp}-{value}` → `{bp}:{utility}-{value}`; breakpoint name `xxl` → `2xl`. Values, breakpoints `sm`/`md`/`lg`/`xl` keep their names. Non-responsive classes (`d-flex`, `col-6`, `mt-3`, `btn-sm`, ...) are unchanged.

| v5 (removed) | v6 |
|---|---|
| `d-md-flex`, `d-lg-none`, ... | `md:d-flex`, `lg:d-none`, ... |
| `col-md-6`, `col-lg-auto`, `col-xl` | `md:col-6`, `lg:col-auto`, `xl:col` |
| `col-xxl-4` | `2xl:col-4` |
| `row-cols-md-3` | `md:row-cols-3` |
| `offset-md-2` | `md:offset-2` |
| `order-lg-1` | `lg:order-1` |
| `flex-md-row`, `flex-lg-grow-0` | `md:flex-row`, `lg:flex-grow-0` |
| `justify-content-md-between` | `md:justify-content-between` |
| `align-items-lg-center` | `lg:align-items-center` |
| `mt-md-3`, `px-lg-4`, `gap-md-2` (all spacing) | `md:mt-3`, `lg:px-4`, `md:gap-2` |
| `text-md-start`, `text-lg-end` | `md:text-start`, `lg:text-end` |
| `float-md-end` | `md:float-end` |
| `sticky-md-top`, `sticky-lg-bottom` | `md:sticky-top`, `lg:sticky-bottom` |
| `container-md`, `container-xxl` | `md:container`, `2xl:container` |
| `navbar-expand-md`, `navbar-expand-xxl` | `md:navbar-expand`, `2xl:navbar-expand` |
| `offcanvas-md` (responsive offcanvas) | `md:drawer` |
| `modal-fullscreen-md-down` | `md-down:dialog-fullscreen` |

## 2. Breakpoint values (flag consumer media queries)

| Breakpoint | v5 min-width | v6 min-width |
|---|---|---|
| `sm` | 576px | 576px (unchanged) |
| `md` | 768px | 768px (unchanged) |
| `lg` | 992px | **1024px** |
| `xl` | 1200px | **1280px** |
| `xxl` → `2xl` | 1400px | **1536px** |

## 3. Theme color utilities (token system)

`{color}` ∈ primary, secondary, success, danger, warning, info, accent (new), inverse (new). **`light` and `dark` are removed** — map `light` → `secondary`, `dark` → `inverse`.

| v5 (removed) | v6 |
|---|---|
| `text-{color}` | `fg-{color}` |
| `text-{color}-emphasis` | `fg-emphasis-{color}` |
| `bg-{color}-subtle` | `bg-subtle-{color}` |
| `border-{color}-subtle` | `border-subtle-{color}` |
| `text-bg-{color}` | `theme-{color}` (sets bg + contrast fg via theme tokens) |
| `bg-light` / `bg-dark` | `bg-secondary` / `bg-inverse` (or semantic `bg-body-*`) |
| `text-light` / `text-dark` | `fg-secondary` / `fg-inverse` |
| `alert-{color}`, `badge` color via `text-bg-*`, `list-group-item-{color}` | component class + `theme-{color}` |

`bg-{color}` and `border-{color}` keep their names. The `--bs-{color}-rgb` CSS variables are removed (only `--bs-link-color-rgb` remains) — replace `rgba(var(--bs-primary-rgb), .25)` with `color-mix(in srgb, var(--bs-primary) 25%, transparent)`.

## 4. Buttons

| v5 (removed) | v6 |
|---|---|
| `btn-primary`, `btn-success`, ... | `btn-solid theme-primary`, `btn-solid theme-success`, ... |
| `btn-outline-primary`, ... | `btn-outline theme-primary`, ... |
| `btn-light` / `btn-dark` | `btn-solid theme-secondary` / `btn-solid theme-inverse` |
| — | new variants: `btn-subtle`, `btn-text`; new size `btn-xs` |
| `btn-link` | `btn-link` (unchanged) |
| `.btn-check` on the `<input>` + sibling `<label class="btn btn-outline-{color}" for=...>` | `.btn-check` classes on the `<label>` (`btn-check btn-outline theme-{color}`) with the `<input>` **nested inside**; no `id`/`for` needed |

## 5. Modal → Dialog (native `<dialog>`)

| v5 (removed) | v6 |
|---|---|
| `modal` | `dialog` |
| `modal-dialog`, `modal-content` | — (gone; native `<dialog>` is the single element) |
| `modal-header` / `modal-title` / `modal-body` / `modal-footer` | `dialog-header` / `dialog-title` / `dialog-body` / `dialog-footer` |
| `modal-sm` / `modal-lg` / `modal-xl` | `dialog-sm` / `dialog-lg` / `dialog-xl` |
| `modal-fullscreen`, `modal-fullscreen-{bp}-down` | `dialog-fullscreen`, `{bp}-down:dialog-fullscreen` |
| `modal-dialog-scrollable` | `dialog-scrollable` |
| `modal-dialog-centered` | — (native dialogs are centered) |
| `data-bs-toggle="modal"`, `data-bs-dismiss="modal"` | `data-bs-toggle="dialog"`, `data-bs-dismiss="dialog"` |

## 6. Offcanvas → Drawer (native `<dialog>`)

| v5 (removed) | v6 |
|---|---|
| `offcanvas` | `drawer` |
| `offcanvas-{sm..xxl}` (responsive) | `{bp}:drawer` (`2xl:drawer` for xxl) |
| `offcanvas-header` / `offcanvas-title` / `offcanvas-body` | `drawer-header` / `drawer-title` / `drawer-body` |
| `offcanvas-start` / `-end` / `-top` / `-bottom` | `drawer-start` / `drawer-end` / `drawer-top` / `drawer-bottom` |
| `data-bs-toggle="offcanvas"`, `data-bs-dismiss="offcanvas"` | `data-bs-toggle="drawer"`, `data-bs-dismiss="drawer"` |
| — | new: `drawer-sheet` |

## 7. Dropdown → Menu

| v5 (removed) | v6 |
|---|---|
| `dropdown` (wrapper) | — (the `menu` element is the component) |
| `dropdown-menu` | `menu` |
| `dropdown-item` | `menu-item` |
| `dropdown-item-text` | `menu-text` |
| `dropdown-divider` | `menu-divider` |
| `dropdown-header` | `menu-header` |
| `dropdown-toggle` | — (no caret class; the toggle is any element with `data-bs-toggle="menu"`) |
| `dropdown-toggle-split` | `menu-toggle-split` |
| `dropup` / `dropend` / `dropstart`, `dropdown-menu-end` | placement option on the Menu plugin (`MenuPlacement` in Hx components) |
| `data-bs-toggle="dropdown"` | `data-bs-toggle="menu"` |

## 8. Forms

| v5 (removed) | v6 |
|---|---|
| `form-select`, `form-select-sm`, `form-select-lg` | `form-control`, `form-control-sm`, `form-control-lg` (on `<select>` too) |
| `form-floating` on composite inputs (autosuggest/date/tags/search wrappers) | — (not supported; Regular labels) |
| input-group glue around composite inputs | `form-adorn` / `form-ghost` wrappers (handled by Hx components internally) |

## 9. Navbar

| v5 (removed) | v6 |
|---|---|
| `navbar-collapse` | — (responsive content is a `dialog.drawer`, flattened inline at/above the expand breakpoint) |
| `navbar-expand-{bp}` | `{bp}:navbar-expand` |
| `navbar-dark` / `navbar-light` | — (toggler icon uses `mask-image` + `currentcolor`; use `data-bs-theme` / theme classes) |
