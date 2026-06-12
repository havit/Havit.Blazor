---
name: havit-blazor-v6-migration
description: Migrate a Blazor application from Havit.Blazor v4.x (Bootstrap 5) to v6 (Bootstrap 6). Use when upgrading the Havit.Blazor.Components.Web.Bootstrap NuGet package across the v6 major version - covers CSS class renames, component API changes, and verification steps.
---

# Havit.Blazor v4.x → v6 migration

Havit.Blazor v6 adopts **Bootstrap 6** (native `<dialog>`-based Dialog/Drawer, the Menu component, a composable theme-token system, prefix-based responsive class syntax, changed breakpoint values, ESM-only JavaScript). This is a large breaking change executed as (1) mechanical sweeps you can run with search/replace plus a bundle check, and (2) a short list of API changes that need judgment.

Human-readable companion: https://havit.blazor.eu/concepts/migration-guide-v6

Work through the steps **in order**. Steps 2 and 3 reference `references/class-renames.md` (in this skill folder) — read it before starting Step 3.

## Step 1 — Preflight

1. Find the project(s) referencing the package and record the current version (expected 4.x):
   ```
   grep -rn "Havit.Blazor.Components.Web.Bootstrap" --include="*.csproj" --include="Directory.Packages.props" .
   ```
2. Require a clean working tree (`git status --porcelain` must be empty). Create a migration branch.
3. Build the baseline: `dotnet build` must succeed **before** any change. If it doesn't, stop and report.

## Step 2 — Package and asset references

1. Bump `Havit.Blazor.Components.Web.Bootstrap` (and `Havit.Blazor.Components.Web` if referenced directly) to the latest 6.x version, then `dotnet restore`.
2. Locate the compiled v6 CSS bundle — you will verify every class rename against it:
   ```
   BUNDLE=$(find ~/.nuget/packages/havit.blazor.components.web.bootstrap/6.* -name bootstrap.min.css | head -1)
   ```
3. In `index.html` / `App.razor` / `_Layout.cshtml`:
   - **CSS**: keep the existing `_content/Havit.Blazor.Components.Web.Bootstrap/bootstrap.min.css` link or `@Html.Raw(HxSetup.RenderBootstrapCssReference())` — the URL is unchanged. If the app links a Bootstrap **5** CDN stylesheet instead, replace it with the library bundle (Bootstrap 6 alpha is not on CDNs).
   - **JavaScript**: Bootstrap 6 is **ESM-only** — there is no UMD `bootstrap.bundle.min.js` on a CDN and no implicit `window.bootstrap`. If the host page uses `@Html.Raw(HxSetup.RenderBootstrapJavaScriptReference())`, no change is needed (it now renders a module script from the library's static web assets and bridges `window.bootstrap`). If the app hard-codes a Bootstrap 5 CDN `<script src=".../bootstrap.bundle.min.js">`, **delete it** and add `HxSetup.RenderBootstrapJavaScriptReference()` (Razor host pages) or its equivalent module script for plain `index.html` hosts. The library's JS initializer also establishes the bridge before Blazor starts, so the explicit reference is a performance optimization (early download), not a hard requirement.
   - **App stylesheets**: leave `css/app.css` / `{AssemblyName}.styles.css` links intact. Do not remove them.

## Step 3 — Mechanical sweeps

Read `references/class-renames.md` now — it contains the full old→new tables. Sweep `**/*.razor`, `**/*.html`, `**/*.cshtml`, `**/*.css` (incl. `*.razor.css`), and `**/*.cs` (CssClass strings) — exclude `bin/`, `obj/`, `wwwroot/lib/`.

### 3a. Responsive infix utilities → prefix syntax

Bootstrap 6 replaces the v5 infix (`d-md-flex`, `col-md-6`) with a breakpoint **prefix** (`md:d-flex`, `md:col-6`) and renames `xxl` → `2xl`. Find all candidates:

```
rg -o '\b([a-z][a-z0-9-]*)-(sm|md|lg|xl|xxl)-([a-z0-9-]+)\b' -g '!{bin,obj}' -g '!**/wwwroot/lib/**' --no-filename -N | sort -u
```

Conversion rule: `{utility}-{bp}-{value}` → `{bp}:{utility}-{value}`, with `xxl` → `2xl` (e.g. `col-xxl-4` → `2xl:col-4`, `mt-md-3` → `md:mt-3`, `justify-content-lg-between` → `lg:justify-content-between`). Also convert the breakpoint-suffix forms the regex misses: `navbar-expand-{bp}` → `{bp}:navbar-expand`, `sticky-{bp}-top` → `{bp}:sticky-top`, `container-{bp}` → `{bp}:container`, and `*-{bp}-down` forms → `{bp}-down:` prefix.

**Verify every converted class against the bundle before replacing** — the regex also matches consumer-defined class names that must NOT be touched. In the minified CSS, `:` is escaped as `\:` and the leading digit of `2xl` as `\32 ` (with a space):

```
grep -c 'md\\:d-flex' "$BUNDLE"        # md:d-flex
grep -c '\\32 xl\\:col-4' "$BUNDLE"    # 2xl:col-4
```

If the converted form is not in the bundle, leave the original and flag it in your report.

### 3b. Removed / renamed component classes

Run the searches in `references/class-renames.md` and apply the mappings: `form-select` → `form-control`; `btn-{color}` → `btn-solid theme-{color}`; `btn-outline-{color}` → `btn-outline theme-{color}`; `modal*` → `dialog*`; `offcanvas*` → `drawer*`; `dropdown*` → `menu*`; `navbar-collapse` → drawer structure (see Step 4.6). Raw-HTML usages get the class rename; `Hx*` component usages are handled in Step 4.

**btn-check structure**: v6 moves `.btn-check` to the **label** (with variant + theme classes) and nests the input inside; `id`/`for` pairs are no longer needed:

```razor
@* v4 / Bootstrap 5 *@
<InputRadio id="radio1" class="btn-check" Value="@("Radio 1")" />
<label class="btn btn-outline-primary" for="radio1">Radio 1</label>

@* v6 / Bootstrap 6 *@
<label class="btn-check btn-outline theme-primary">
    <InputRadio Value="@("Radio 1")" />
    Radio 1
</label>
```

(Or better: replace hand-written groups with `<HxRadioButtonList RenderMode="...ButtonGroup" Variant="ButtonVariant.Outline" ... />`.)

### 3c. Theme color utilities (Bootstrap 6 token system)

Apply per `references/class-renames.md`: `text-{color}` → `fg-{color}`, `text-{color}-emphasis` → `fg-emphasis-{color}`, `bg-{color}-subtle` → `bg-subtle-{color}`, `border-{color}-subtle` → `border-subtle-{color}`, `text-bg-{color}` → `theme-{color}`. The `light`/`dark` theme colors are **removed** — map `light` → `secondary` and `dark` → `inverse` (new `accent` and `inverse` colors exist). Verify each result against the bundle as in 3a.

### 3d. Breakpoint VALUE changes

Bootstrap 6 raised the upper breakpoints: `lg` 992→**1024**, `xl` 1200→**1280**, `xxl`/`2xl` 1400→**1536** (`sm` 576 and `md` 768 unchanged). Consumer media queries written to align with Bootstrap's no longer match:

```
rg -n '(992|1200|1400)px' -g '*.css' -g '*.razor' -g '!{bin,obj}' -g '!**/wwwroot/lib/**'
```

**Flag every hit** and update to 1024/1280/1536 only where the query is clearly meant to mirror a Bootstrap breakpoint; otherwise report it for human review.

## Step 4 — Component API changes (require judgment)

For each item, search for the old component/type names and rewrite. The HAVIT Blazor MCP server / https://havit.blazor.eu docs have the full v6 API if you need parameter details.

### 4.1 HxModal → HxDialog

`HxModal` is replaced by `HxDialog` rendering a native `<dialog>`. Renamed types: `ModalSettings`→`DialogSettings`, `ModalSize`→`DialogSize`, `ModalBackdrop`→`DialogBackdrop`, `ModalRenderMode`→`DialogRenderMode`, `ModalFullscreen`→`DialogFullscreen`, `ModalHidingEventArgs`→`DialogHidingEventArgs`. Parameter changes: `Animated` (bool) → `Animation` (`DialogAnimation.Fade`/`None`/`SlideDown`/`SlideUp`); `Centered` removed (native dialogs center themselves); `DialogCssClass` and `CloseButtonSettings` removed; new `NonModal`. `ShowAsync`/`HideAsync`, `Title`, templates, `OnShown`/`OnHiding`/`OnClosed` keep their names. `HxMessageBox`/`IHxMessageBoxService` are unchanged.

```razor
@* v4 *@                                          @* v6 *@
<HxModal @ref="_modal" Title="Edit"               <HxDialog @ref="_dialog" Title="Edit"
         Size="ModalSize.Large"                             Size="DialogSize.Large"
         Backdrop="ModalBackdrop.Static">                   Backdrop="DialogBackdrop.Static">
    <BodyTemplate>...</BodyTemplate>                  <BodyTemplate>...</BodyTemplate>
</HxModal>                                        </HxDialog>
```

### 4.2 HxOffcanvas → HxDrawer

`HxOffcanvas` is replaced by `HxDrawer` (native `<dialog>` based). Renamed types: `OffcanvasSettings`→`DrawerSettings`, `OffcanvasPlacement`→`DrawerPlacement`, `OffcanvasSize`→`DrawerSize`, `OffcanvasBackdrop`→`DrawerBackdrop`, `OffcanvasRenderMode`→`DrawerRenderMode`, `OffcanvasResponsiveBreakpoint`→`DrawerResponsiveBreakpoint`, `OffcanvasHidingEventArgs`→`DrawerHidingEventArgs`. Parameters/templates/events otherwise keep their names; new `Sheet` parameter.

```razor
<HxOffcanvas @ref="_oc" Title="Filter" Placement="OffcanvasPlacement.End">   @* v4 *@
<HxDrawer @ref="_drawer" Title="Filter" Placement="DrawerPlacement.End">    @* v6 *@
```

### 4.3 HxDropdown* → HxMenu family

Hard rename **with a structure change** — `HxMenu` uses `Toggle`/`Content` render fragments instead of nested wrapper components:

```razor
@* v4 *@
<HxDropdownButtonGroup>
    <HxDropdownToggleButton Color="ThemeColor.Primary">Options</HxDropdownToggleButton>
    <HxDropdownMenu>
        <HxDropdownItem OnClick="OnA">A</HxDropdownItem>
        <HxDropdownDivider />
        <HxDropdownItemNavLink Href="/b">B</HxDropdownItemNavLink>
    </HxDropdownMenu>
</HxDropdownButtonGroup>

@* v6 *@
<HxMenu>
    <Toggle>
        <HxMenuToggleButton Color="ThemeColor.Primary">Options</HxMenuToggleButton>
    </Toggle>
    <Content>
        <HxMenuItem OnClick="OnA">A</HxMenuItem>
        <HxMenuDivider />
        <HxMenuItemNavLink Href="/b">B</HxMenuItemNavLink>
    </Content>
</HxMenu>
```

Component map: `HxDropdown`/`HxDropdownButtonGroup`→`HxMenu`, `HxDropdownToggleButton`→`HxMenuToggleButton`, `HxDropdownToggleElement`→`HxMenuToggleElement`, `HxDropdownMenu`→(dissolved into `Content`), `HxDropdownItem`→`HxMenuItem`, `HxDropdownItemNavLink`→`HxMenuItemNavLink`, `HxDropdownItemText`→`HxMenuText`, `HxDropdownDivider`→`HxMenuDivider`, `HxDropdownHeader`→`HxMenuHeader`. Types: `DropdownDirection`+`DropdownMenuAlignment`→`MenuPlacement` (single placement enum on `HxMenu.Placement`, e.g. `BottomStart`), `DropdownAutoClose`→`MenuAutoClose`, `PopperStrategy`→`FloatingStrategy` (Floating UI replaces Popper). Raw HTML: `data-bs-toggle="dropdown"` → `data-bs-toggle="menu"`.

### 4.4 HxButton: Outline removed, Variant added

`HxButton.Outline` (bool) is removed. Use `Variant` (`ButtonVariant.Solid` default, `Outline`, `Subtle`, `Text`, `Link`) composed with `Color`: `<HxButton Color="ThemeColor.Primary" Outline="true">` → `<HxButton Color="ThemeColor.Primary" Variant="ButtonVariant.Outline">`. `ThemeColor.Light`/`Dark` enum members are removed — use `Secondary`/`Inverse` (new: `Accent`, `Inverse`). New `ButtonSize.ExtraSmall` (`btn-xs`) exists.

### 4.5 ThemeColor / ThemeColorExtensions in C# code

Any C# `switch` over `ThemeColor` or use of `ThemeColor.Light`/`Dark` fails to compile — map to `Secondary`/`Inverse`. `ToTextColorCss()` now emits `fg-*`, `ToTextBackgroundColorCss()` emits `theme-*`; review call sites that string-match the results.

### 4.6 HxNavbarCollapse → HxNavbarDrawer (+ HxNavbarToggler)

Bootstrap 6 removed `.navbar-collapse`; responsive navbar content is a **Drawer** that the navbar CSS flattens inline at/above the expand breakpoint. `HxNavbarCollapse` → `HxNavbarDrawer` (new optional `Title`, `Placement` — default `DrawerPlacement.End`); `HxNavbarToggler` now targets the drawer (default target id suffix changed `-collapse` → `-drawer`; only relevant if you hard-coded it).

```razor
@* v4 *@                              @* v6 *@
<HxNavbar>                            <HxNavbar>
    <HxNavbarBrand>App</HxNavbarBrand>    <HxNavbarBrand>App</HxNavbarBrand>
    <HxNavbarToggler />                   <HxNavbarToggler />
    <HxNavbarCollapse>                    <HxNavbarDrawer Title="Menu">
        <HxNav>...</HxNav>                    <HxNav>...</HxNav>
    </HxNavbarCollapse>                   </HxNavbarDrawer>
</HxNavbar>                           </HxNavbar>
```

### 4.7 HxSidebar: mobile navigation dropped — mobile-nav recipe

`HxSidebar` no longer implements mobile navigation: `HxSidebar.ResponsiveBreakpoint`, the `SidebarResponsiveBreakpoint` enum, and the built-in mobile hamburger toggler are **removed**; the sidebar's only mode is the horizontal icon-rail collapse. The application layout must provide mobile navigation itself — hide the sidebar below a breakpoint with display utilities and serve navigation from `HxNavbar` with `HxNavbarDrawer` instead. If the migrated app relied on the sidebar's built-in mobile mode, this recipe is the replacement (apply it; do not look for a renamed parameter):

```razor
@* v6 layout recipe (lg = navbar's default expand breakpoint) *@
<HxNavbar>
    <HxNavbarBrand>App</HxNavbarBrand>
    <HxNavbarToggler />
    <HxNavbarDrawer>
        <HxNav>...top-level links...</HxNav>
        @* mobile-only nav; hidden where the drawer content is flattened inline *@
        <div class="lg:d-none mt-3">
            ...sidebar navigation items for mobile...
        </div>
    </HxNavbarDrawer>
</HxNavbar>

<div class="d-flex">
    <HxSidebar CssClass="d-none lg:d-flex">...</HxSidebar>
    <main class="flex-grow-1">@Body</main>
</div>
```

If the app referenced the internal class `hx-sidebar-collapse` in CSS, it is renamed to `hx-sidebar-content`.

### 4.8 LabelType.Floating removed on composite inputs

`LabelType.Floating` now **throws `InvalidOperationException`** on `HxAutosuggest`, `HxSearchBox`, `HxInputDate`, `HxInputDateRange`, and `HxInputTags` (the Bootstrap 6 `form-adorn` wrapper cannot host a floating label). Remove `LabelType="LabelType.Floating"` (i.e. use `Regular`) on these components — including via `Defaults`/`Settings`. Plain inputs (`HxInputText`, `HxSelect`, ...) still support floating labels.

### 4.9 ChipBadgeSettings / TagBadgeSettings → Color

`HxChipList.ChipBadgeSettings` and `HxInputTags.TagBadgeSettings` are obsolete (chips are no longer badges). Replace with the new `Color` parameter (`ThemeColor?`; default `None` = native grayscale chips): `<HxChipList ChipBadgeSettings="new() { Color = ThemeColor.Primary }" ...>` → `<HxChipList Color="ThemeColor.Primary" ...>`. Same for `Defaults`/`Settings` registrations.

### 4.10 CSS variable overrides: RGB triplets → full colors

Bootstrap 6 dropped the `--bs-*-rgb` channel variables (only `--bs-link-color-rgb` survives). Consumer overrides that supplied **RGB triplets** must now supply full colors: `--hx-sidebar-item-*-background-color`, `--hx-tree-view-item-*-background`, `--hx-calendar-day-today-background` (e.g. `--hx-tree-view-item-hover-background: 13,110,253;` → `--hx-tree-view-item-hover-background: #0d6efd;`). Consumer CSS using `rgba(var(--bs-primary-rgb), .25)` → `color-mix(in srgb, var(--bs-primary) 25%, transparent)`. Search: `rg -n -- '--bs-[a-z-]*-rgb|--hx-.*(background|color):\s*[0-9]+\s*,' -g '*.css'`.

### 4.11 Consumer scoped CSS that fights utilities → `@layer custom`

Bootstrap 6 utilities no longer use `!important`; everything Bootstrap ships is in CSS cascade **layers**, and any **unlayered** consumer rule (typical `.razor.css`) beats them — e.g. a scoped `display: flex` silently defeats `d-none`. Wrap scoped/custom CSS that should cooperate with utilities in Bootstrap's designated customization layer (overrides Bootstrap component styles, still loses to utilities):

```css
@layer custom {
    .my-panel { display: flex; gap: .5rem; }
}
```

Audit `*.razor.css` files for rules setting properties that utility classes on the same elements also set (`display`, `margin`, `padding`, `position`, ...) and move them into `@layer custom`.

## Step 5 — Verification loop

1. `dotnet build` — iterate until clean. Compile errors name the removed types from Step 4; do not suppress with `#pragma`/aliases.
2. Leftover scan — all of these should return nothing (or only deliberate, flagged exceptions):
   ```
   rg -n 'HxModal|HxOffcanvas|HxDropdown|HxNavbarCollapse' -g '*.razor' -g '*.cs' -g '!{bin,obj}'
   rg -n 'form-select|text-bg-|btn-outline-[a-z]+|\bbtn-(primary|secondary|success|danger|warning|info|light|dark)\b' -g '!{bin,obj}' -g '!**/wwwroot/lib/**'
   rg -on '\b[a-z][a-z0-9-]*-(sm|md|lg|xl|xxl)-[a-z0-9-]+\b' -g '!{bin,obj}' -g '!**/wwwroot/lib/**'
   ```
3. Run the application and visually check:
   - **Forms**: labels/validation render; selects styled (no bare native select); checkboxes/radios/switches; any toggle-button groups respond to clicks.
   - **Dialogs**: open/close (button, Escape, backdrop), sizes/fullscreen, message boxes.
   - **Drawers/offcanvas replacements**: open from the correct edge, backdrop dismiss.
   - **Navbar mobile**: narrow the viewport below the expand breakpoint — toggler opens the drawer, links navigate and close it; widen — content flattens inline.
   - **Sidebar**: desktop icon-rail collapse works; below the breakpoint the sidebar hides and the navbar drawer serves navigation.
   - **Theme colors**: badges/alerts/buttons show colors (a colorless gray component usually means a stale `btn-{color}`/`text-bg-*` class); check dark color mode if used.
4. Report every spot you flagged-but-did-not-change (media queries, ambiguous classes) for human review, with file:line.
