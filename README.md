
# HAVIT Blazor - Bootstrap 5 component bundle

[![GitHub Repo](https://img.shields.io/static/v1?style=flat&logo=github&label=GitHub&message=repo&cacheSeconds=604800)](https://github.com/havit/Havit.Blazor/)
[![NuGet Version](https://img.shields.io/nuget/v/Havit.Blazor.Components.Web.Bootstrap)](https://www.nuget.org/packages/Havit.Blazor.Components.Web.Bootstrap/)
[![Release Notes](https://img.shields.io/badge/GitHub-Relese%20Notes-yellow?logo=github&cacheSeconds=604800)](https://github.com/havit/Havit.Blazor/releases)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Havit.Blazor.Components.Web.Bootstrap)](https://www.nuget.org/packages/Havit.Blazor.Components.Web.Bootstrap/)
[![GitHub License](https://img.shields.io/github/license/havit/Havit.Blazor)](https://github.com/havit/Havit.Blazor/blob/master/LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/havit/Havit.Blazor)](https://github.com/havit/Havit.Blazor/)  
[![Build Status](https://dev.azure.com/havit/DEV/_apis/build/status/002.HFW-HavitBlazor?branchName=master)](https://dev.azure.com/havit/DEV/_build/latest?definitionId=318&branchName=master)
[![#StandWithUkraine](https://img.shields.io/badge/%23StandWithUkraine-Russian%20warship%2C%20go%20f%23ck%20yourself-blue)](https://www.peopleinneed.net/what-we-do/humanitarian-aid-and-development/ukraine)

* Free Bootstrap 5.3 components for ASP.NET Blazor
* .NET 6+ with Blazor WebAssembly or Blazor Server (other hosting models not tested yet, .NET 7 fully supported)
* [Enterprise project template](https://github.com/havit/NewProjectTemplate-Blazor) (optional) - layered architecture, EF Core, gRPC code-first, ...

If you enjoy using [HAVIT Blazor](https://havit.blazor.eu/), you can [become a sponsor](https://github.com/sponsors/havit). Your sponsorship will allow us to devote time to features and issues requested by the community (i.e. not required by our own projects) ❤️.


# See [&gt;&gt;Interactive Documentation & Demos&lt;&lt;](https://havit.blazor.eu)

## 🔥[Migration to v4](https://havit.blazor.eu/migrating)🔥

# Components

## Forms

* [`HxAutosuggest`](https://havit.blazor.eu/components/HxAutosuggest) - Component for single item selection with dynamic suggestions.
* [`HxCalendar`](https://havit.blazor.eu/components/HxCalendar) - Basic calendar building block.
* [`HxInputDate`](https://havit.blazor.eu/components/HxInputDate) - Date picker. Form input component for entering a date.
* [`HxInputDateRange`](https://havit.blazor.eu/components/HxInputDateRange) - Date range picker. Form input component for entering start date and end date.
* [`HxInputFile[Core]`](https://havit.blazor.eu/components/HxInputFile[Core]) - Wraps HxInputFileCore as Bootstrap form control (incl. Label etc.).
* [`HxInputFileDropZone`](https://havit.blazor.eu/components/HxInputFileDropZone) - Ready-made UX for drag&amp;drop file upload.
* [`HxInputNumber`](https://havit.blazor.eu/components/HxInputNumber) - Numeric input.
* [`HxInputPercent`](https://havit.blazor.eu/components/HxInputPercent) - Numeric input in percentages with value normalization (0% = 0, 100% = 1.0).
* [`HxInputRange`](https://havit.blazor.eu/components/HxInputRange) - Range input (slider).
* [`HxInputTags`](https://havit.blazor.eu/components/HxInputTags) - Input for entering tags.
* [`HxInputText`](https://havit.blazor.eu/components/HxInputText) - Text input (also password, search, etc.)
* [`HxInputTextArea`](https://havit.blazor.eu/components/HxInputTextArea) - [Textarea](https://getbootstrap.com/docs/5.3/forms/floating-labels/#textareas).
* [`HxCheckbox`](https://havit.blazor.eu/components/HxCheckbox) - Checkbox input.
* [`HxCheckboxList`](https://havit.blazor.eu/components/HxCheckboxList) - Multiple choice by checkboxes.
* [`HxFormState`](https://havit.blazor.eu/components/HxFormState) - Propagates form states as a cascading parementer to child components.
* [`HxFormValue`](https://havit.blazor.eu/components/HxFormValue) - Displays a read-only value in the form control visual.
* [`HxRadioButtonList`](https://havit.blazor.eu/components/HxRadioButtonList) - Select.
* [`HxSelect`](https://havit.blazor.eu/components/HxSelect) - Select - DropDownList - single-item picker.
* [`HxMultiSelect`](https://havit.blazor.eu/components/HxMultiSelect) - Unlike a normal select, multiselect allows the user to select multiple options at once.
* [`HxSearchBox`](https://havit.blazor.eu/components/HxSearchBox) - A search input component witch automatic suggestions, initial dropdown template and free-text queries support.
* [`HxSwitch`](https://havit.blazor.eu/components/HxSwitch) - Switch input.
* [`HxFilterForm`](https://havit.blazor.eu/components/HxFilterForm) - Edit form derived from HxModelEditForm with support for chip generators.
* [`HxValidationMessage`](https://havit.blazor.eu/components/HxValidationMessage) - Displays a list of validation messages for a specified field.

## Buttons & Indicators

* [`HxButton`](https://havit.blazor.eu/components/HxButton) - Bootstrap [button](https://getbootstrap.com/docs/5.3/components/buttons/).
* [`HxButtonGroup`](https://havit.blazor.eu/components/HxButtonGroup) - Bootstrap [ButtonGroups](https://getbootstrap.com/docs/5.3/components/button-group/). 
* [`HxButtonToolbar`](https://havit.blazor.eu/components/HxButtonToolbar) - Bootstrap [ButtonGroups](https://getbootstrap.com/docs/5.3/components/button-group/).
* [`HxCloseButton`](https://havit.blazor.eu/components/HxCloseButton) - Bootstrap [close-button](https://getbootstrap.com/docs/5.3/components/close-button/).
* [`HxSubmit`](https://havit.blazor.eu/components/HxSubmit) - Submit button.
* [`HxDropdown`](https://havit.blazor.eu/components/HxDropdown) - [Bootstrap 5 Dropdown](https://getbootstrap.com/docs/5.3/components/dropdowns/).
* [`HxBadge`](https://havit.blazor.eu/components/HxBadge) - [Bootstrap Badge](https://getbootstrap.com/docs/5.3/components/badge/) component.
* [`HxChipList`](https://havit.blazor.eu/components/HxChipList) - Presents a list of chips as badges.
* [`HxIcon`](https://havit.blazor.eu/components/HxIcon) - Displays an icon.
* [`HxSpinner`](https://havit.blazor.eu/components/HxSpinner) - Bootstrap [Spinner](https://getbootstrap.com/docs/5.3/components/spinners/) (usually indicates operation in progress).
* [`HxProgressIndicator`](https://havit.blazor.eu/components/HxProgressIndicator) - Displays the content of the component as "in progress".
* [`HxTooltip`](https://havit.blazor.eu/components/HxTooltip) - [Bootstrap Tooltip](https://getbootstrap.com/docs/5.3/components/tooltips/) component, activates on hover.
* [`HxPopover`](https://havit.blazor.eu/components/HxPopover) - [Bootstrap Popover](https://getbootstrap.com/docs/5.3/components/popovers/) component.

## Data & Grid

* [`HxGrid`](https://havit.blazor.eu/components/HxGrid) - Grid to display tabular data from data source.
* [`HxContextMenu`](https://havit.blazor.eu/components/HxContextMenu) - Ready-made context menu.
* [`HxPager`](https://havit.blazor.eu/components/HxPager) - Pager.
* [`HxRepeater`](https://havit.blazor.eu/components/HxRepeater) - A data-bound list component.
* [`HxTreeView`](https://havit.blazor.eu/components/HxTreeView) - Component to display hierarchy data structure.

## Layout & Typography

* [`HxAccordion`](https://havit.blazor.eu/components/HxAccordion) - [Bootstrap accordion](https://getbootstrap.com/docs/5.3/components/accordion/) component.
* [`HxAlert`](https://havit.blazor.eu/components/HxAlert) - Bootstrap Alert.
* [`HxBadge`](https://havit.blazor.eu/components/HxBadge) - [Bootstrap Badge](https://getbootstrap.com/docs/5.3/components/badge/) component.
* [`HxCard`](https://havit.blazor.eu/components/HxCard) - [Bootstrap Card](https://getbootstrap.com/docs/5.3/components/card/) component.
* [`HxCarousel`](https://havit.blazor.eu/components/HxCarousel) - A slideshow component for cycling through elements—images or slides of text—like a carousel.
* [`HxCollapse`](https://havit.blazor.eu/components/HxCollapse) - [Bootstrap 5 Collapse](https://getbootstrap.com/docs/5.3/components/collapse/) component.
* [`HxDropdown`](https://havit.blazor.eu/components/HxDropdown) - [Bootstrap 5 Dropdown](https://getbootstrap.com/docs/5.3/components/dropdowns/).
* [`HxIcon`](https://havit.blazor.eu/components/HxIcon) - Displays an icon.
* [`HxSpinner`](https://havit.blazor.eu/components/HxSpinner) - [Bootstrap Spinner](https://getbootstrap.com/docs/5.3/components/spinners/) (usually indicates operation in progress).
* [`HxPlaceholder`](https://havit.blazor.eu/components/HxPlaceholder) - [Bootstrap 5 Placeholder](https://getbootstrap.com/docs/5.3/components/placeholders/) component, aka Skeleton.
* [`HxProgress`](https://havit.blazor.eu/components/HxProgress) - [Bootstrap 5 Progress](https://getbootstrap.com/docs/5.3/components/progress/) component.
* [`HxProgressIndicator`](https://havit.blazor.eu/components/HxProgressIndicator) - Displays the content of the component as "in progress".
* [`HxTooltip`](https://havit.blazor.eu/components/HxTooltip) - [Bootstrap Tooltip](https://getbootstrap.com/docs/5.3/components/tooltips/) component, activates on hover.
* [`HxTabPanel`](https://havit.blazor.eu/components/HxTabPanel) - Container for `HxTab`.
* [`HxListGroup`](https://havit.blazor.eu/components/HxListGroup) - [Bootstrap 5 List group](https://getbootstrap.com/docs/5.3/components/list-group/) component.
* [`HxListLayout`](https://havit.blazor.eu/components/HxListLayout) - Page layout for data presentation (usualy `HxGrid` with filter in `HxOffcanvas`).
* [`HxFormValue`](https://havit.blazor.eu/components/HxFormValue) - Displays a read-only value in the form control visual (as `.form-control`, with label, border, etc.).

## Navigation

* [`HxNavbar`](https://havit.blazor.eu/components/HxNavbar) - [Bootstrap 5 Navbar](https://getbootstrap.com/docs/5.3/components/navbar/) component - responsive navigation header.
* [`HxSidebar`](https://havit.blazor.eu/components/HxSidebar) - Sidebar component - responsive navigation sidebar.
* [`HxNav`](https://havit.blazor.eu/components/HxNav) - [Bootstrap Nav](https://getbootstrap.com/docs/5.3/components/navs-tabs/) component.
* [`HxNavLink`](https://havit.blazor.eu/components/HxNavLink) - [Bootstrap Nav](https://getbootstrap.com/docs/5.3/components/navs-tabs/) component.
* [`HxScrollspy`](https://havit.blazor.eu/components/HxScrollspy) - [Bootstrap Scrollspy](https://getbootstrap.com/docs/5.3/components/scrollspy/) component.
* [`HxBreadcrumb`](https://havit.blazor.eu/components/HxBreadcrumb) - [Bootstrap 5 Breadcrumb](https://getbootstrap.com/docs/5.3/components/breadcrumb/) component. Indicates the current page’s location within a navigational hierarchy.
* [`HxAnchorFragmentNavigation`](https://havit.blazor.eu/components/HxAnchorFragmentNavigation) - Compensates Blazor deficiency in anchor-fragments (scrolling to <code>page#id</code> URLs).
* [`HxContextMenu`](https://havit.blazor.eu/components/HxContextMenu) - Ready-made context menu.
* [`HxDropdown`](https://havit.blazor.eu/components/HxDropdown) - [Bootstrap 5 Dropdown](https://getbootstrap.com/docs/5.3/components/dropdowns/).
* [`HxTabPanel`](https://havit.blazor.eu/components/HxTabPanel) - Container for `HxTab` for easier implementation of tabbed UI.
* [`HxRedirectTo`](https://havit.blazor.eu/components/HxRedirectTo) - Rendering a `HxRedirectTo` will navigate to a new location.

## Modals & Interactions

* [`HxMessageBox`](https://havit.blazor.eu/components/HxMessageBox) - Component to display message-boxes.
* [`HxModal`](https://havit.blazor.eu/components/HxModal) - Component to render modal dialog as a [Bootstrap Modal](https://getbootstrap.com/docs/5.3/components/modal/).
* [`HxDialogBase`](https://havit.blazor.eu/components/HxDialogBase) - Base class to simplify custom modal dialog implementation.
* [`HxOffcanvas`](https://havit.blazor.eu/components/HxOffcanvas) - [Bootstrap Offcanvas](https://getbootstrap.com/docs/5.3/components/offcanvas/) component (aka Drawer).
* [`HxMessenger`](https://havit.blazor.eu/components/HxMessenger) - `HxToastContainer` wrapper for displaying `HxToast` messages dispatched through `IHxMessengerService`.
* [`HxToast`](https://havit.blazor.eu/components/HxToast) - [Bootstrap Toast](https://getbootstrap.com/docs/5.3/components/toasts/) component. Not intented to be used in user code, use `HxMessenger`.

## Special

* [`HxDynamicElement`](https://havit.blazor.eu/components/HxDynamicElement) - Renders an element with the specified name, attributes and child-content.
* [`HxGoogleTagManager`](https://havit.blazor.eu/components/HxGoogleTagManager) - Support for [Google Tag Manager](https://developers.google.com/tag-manager/devguide) - initialization and pushing data to data-layer.
