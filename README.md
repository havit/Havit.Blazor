# HAVIT Blazor - Bootstrap 5 component bundle

[![Nuget](https://img.shields.io/nuget/v/Havit.Blazor.Components.Web.Bootstrap)](https://www.nuget.org/packages/Havit.Blazor.Components.Web.Bootstrap/)
[![Nuget](https://img.shields.io/nuget/dt/Havit.Blazor.Components.Web.Bootstrap)](https://www.nuget.org/packages/Havit.Blazor.Components.Web.Bootstrap/)
[![Build Status](https://dev.azure.com/havit/DEV/_apis/build/status/002.HFW-HavitBlazor?branchName=master)](https://dev.azure.com/havit/DEV/_build/latest?definitionId=318&branchName=master)
[![GitHub](https://img.shields.io/github/license/havit/Havit.Blazor)](https://github.com/havit/Havit.Blazor/blob/master/LICENSE)
[![GitHub](https://img.shields.io/github/stars/havit/Havit.Blazor)](https://github.com/havit/Havit.Blazor/)

* Free Bootstrap 5 components for ASP.NET Blazor
* .NET 5.0+ with Blazor WebAssembly or Blazor Server (other hosting models not tested yet, .NET 6.0 fully supported)
* [Enterprise project template](https://github.com/havit/NewProjectTemplate-Blazor) (optional) - layered architecture, EF Core, gRPC code-first, ...


# See [&gt;&gt;Interactive Documentation & Demos&lt;&lt;](https://havit.blazor.eu)

# Components

## Forms

* [`HxAutosuggest`](https://havit.blazor.eu/components/HxAutosuggest) - Component for single item selection with dynamic suggestions.
* [`HxCalendar`](https://havit.blazor.eu/components/HxCalendar) - Basic calendar building block.
* [`HxInputCheckbox`](https://havit.blazor.eu/components/HxInputCheckbox) - Checkbox input.
* [`HxInputDate`](https://havit.blazor.eu/components/HxInputDate) - Date picker. Form input component for entering a date.
* [`HxInputDateRange`](https://havit.blazor.eu/components/HxInputDateRange) - Date range picker. Form input component for entering start date and end date.
* [`HxInputFile[Core]`](https://havit.blazor.eu/components/HxInputFile[Core]) - Wraps HxInputFileCore as Bootstrap form control (incl. Label etc.).
* [`HxInputFileDropZone`](https://havit.blazor.eu/components/HxInputFileDropZone) - Ready-made UX for drag&amp;drop file upload.
* [`HxInputNumber`](https://havit.blazor.eu/components/HxInputNumber) - Numeric input.
* [`HxInputPercent`](https://havit.blazor.eu/components/HxInputPercent) - Numeric input in percentages with value normalization (0% = 0, 100% = 1.0).
* [`HxInputSwitch`](https://havit.blazor.eu/components/HxInputSwitch) - Switch input.
* [`HxInputTags`](https://havit.blazor.eu/components/HxInputTags) - Input for entering tags.
* [`HxInputText`](https://havit.blazor.eu/components/HxInputText) - Text input (also password, search, etc.)
* [`HxInputTextArea`](https://havit.blazor.eu/components/HxInputTextArea) - [Textarea](https://getbootstrap.com/docs/5.0/forms/floating-labels/#textareas).
* [`HxCheckboxList`](https://havit.blazor.eu/components/HxCheckboxList) - Multiple choice by checkboxes.
* [`HxFormState`](https://havit.blazor.eu/components/HxFormState) - Propagates form states as a cascading parementer to child components.
* [`HxFormValue`](https://havit.blazor.eu/components/HxFormValue) - Displays a read-only value in the form control visual.
* [`HxRadioButtonList`](https://havit.blazor.eu/components/HxRadioButtonList) - Select.
* [`HxSelect`](https://havit.blazor.eu/components/HxSelect) - Select - DropDownList - single-item picker.
* [`HxFilterForm`](https://havit.blazor.eu/components/HxFilterForm) - Edit form derived from HxModelEditForm with support for chip generators.
* [`HxValidationMessage`](https://havit.blazor.eu/components/HxValidationMessage) - Displays a list of validation messages for a specified field.

## Buttons & Indicators

* [`HxButtonGroup`](https://havit.blazor.eu/components/HxButtonGroup) - Bootstrap [ButtonGroups](https://getbootstrap.com/docs/5.0/components/button-group/). 
* [`HxButtonToolbar`](https://havit.blazor.eu/components/HxButtonToolbar) - Bootstrap [ButtonGroups](https://getbootstrap.com/docs/5.0/components/button-group/).
* [`HxSubmit`](https://havit.blazor.eu/components/HxSubmit) - Submit button.
* [`HxDropdown`](https://havit.blazor.eu/components/HxDropdown) - [Bootstrap 5 Dropdown](https://getbootstrap.com/docs/5.1/components/dropdowns/).
* [`HxBadge`](https://havit.blazor.eu/components/HxBadge) - [Bootstrap Badge](https://getbootstrap.com/docs/5.0/components/badge/) component.
* [`HxChipList`](https://havit.blazor.eu/components/HxChipList) - Presents a list of chips as badges.
* [`HxIcon`](https://havit.blazor.eu/components/HxIcon) - Displays an icon.
* [`HxSpinner`](https://havit.blazor.eu/components/HxSpinner) - Bootstrap <a href="https://getbootstrap.com/docs/5.0/components/spinners/">Spinner</a> (usually indicates operation in progress).
* [`HxProgressIndicator`](https://havit.blazor.eu/components/HxProgressIndicator) - Displays the content of the component as "in progress".
* [`HxTooltip`](https://havit.blazor.eu/components/HxTooltip) - <a href="https://getbootstrap.com/docs/5.0/components/tooltips/">Bootstrap Tooltip</a> component, activates on hover.
* [`HxPopover`](https://havit.blazor.eu/components/HxPopover) - <a href="https://getbootstrap.com/docs/5.0/components/popovers/">Bootstrap Popover</a> component.

## Data & Grid

* [`HxGrid`](https://havit.blazor.eu/components/HxGrid) - Grid to display tabular data from data source.
* [`HxContextMenu`](https://havit.blazor.eu/components/HxContextMenu) - Ready-made context menu.
* [`HxPager`](https://havit.blazor.eu/components/HxPager) - Pager.
* [`HxRepeater`](https://havit.blazor.eu/components/HxRepeater) - A data-bound list component.

## Layout & Typography

* [`HxAccordion`](https://havit.blazor.eu/components/HxAccordion) - <a href="https://getbootstrap.com/docs/5.1/components/accordion/">Bootstrap accordion</a> component.
* [`HxAlert`](https://havit.blazor.eu/components/HxAlert) - Bootstrap Alert.
* [`HxBadge`](https://havit.blazor.eu/components/HxBadge) - <a href="https://getbootstrap.com/docs/5.0/components/badge/">Bootstrap Badge</a> component.
* [`HxCard`](https://havit.blazor.eu/components/HxCard) - Bootstrap <a href="https://getbootstrap.com/docs/5.1/components/card/">Card</a> component.
* [`HxCarousel`](https://havit.blazor.eu/components/HxCarousel) - A slideshow component for cycling through elements—images or slides of text—like a carousel.
* [`HxCollapse`](https://havit.blazor.eu/components/HxCollapse) - <a href="https://getbootstrap.com/docs/5.1/components/collapse/">Bootstrap 5 Collapse</a> component.
* [`HxIcon`](https://havit.blazor.eu/components/HxIcon) - Displays an icon.
* [`HxSpinner`](https://havit.blazor.eu/components/HxSpinner) - Bootstrap <a href="https://getbootstrap.com/docs/5.0/components/spinners/">Spinner</a> (usually indicates operation in progress).
* [`HxPlaceholder`](https://havit.blazor.eu/components/HxPlaceholder) - <a href="https://getbootstrap.com/docs/5.1/components/placeholders/">Bootstrap 5 Placeholder</a> component, aka Skeleton.
* [`HxProgress`](https://havit.blazor.eu/components/HxProgress) - <a href="https://getbootstrap.com/docs/5.1/components/progress/">Bootstrap 5 Progress</a> component.
* [`HxProgressIndicator`](https://havit.blazor.eu/components/HxProgressIndicator) - Displays the content of the component as "in progress".
* [`HxTooltip`](https://havit.blazor.eu/components/HxTooltip) - <a href="https://getbootstrap.com/docs/5.0/components/tooltips/">Bootstrap Tooltip</a> component, activates on hover.
* [`HxTabPanel`](https://havit.blazor.eu/components/HxTabPanel) - Container for <code><a href="https://www.havit.blazor.eu/components/HxTab/">HxTab</a></code>s.
* [`HxListGroup`](https://havit.blazor.eu/components/HxListGroup) - <a href="https://getbootstrap.com/docs/5.1/components/list-group/">Bootstrap 5 List group</a> component.
* [`HxListLayout`](https://havit.blazor.eu/components/HxListLayout) - Page layout for data presentation (usualy `HxGrid` with filter in `HxOffcanvas`).
* [`HxFormValue`](https://havit.blazor.eu/components/HxFormValue) - Displays a read-only value in the form control visual (as <code>.form-control</code>, with label, border, etc.).

## Navigation

* [`HxNavbar`](https://havit.blazor.eu/components/HxNavbar) - <a href="https://getbootstrap.com/docs/5.1/components/navbar/">Bootstrap 5 Navbar</a> component - responsive navigation header.
* [`HxSidebar`](https://havit.blazor.eu/components/HxSidebar) - Sidebar component - responsive navigation sidebar.
* [`HxNav`](https://havit.blazor.eu/components/HxNav) - <a href="https://getbootstrap.com/docs/5.0/components/navs-tabs/">Bootstrap Nav</a> component.
* [`HxNavLink`](https://havit.blazor.eu/components/HxNavLink) - <a href="https://getbootstrap.com/docs/5.0/components/navs-tabs/">Bootstrap Nav</a> component.
* [`HxScrollspy`](https://havit.blazor.eu/components/HxScrollspy) - <a href="https://getbootstrap.com/docs/5.0/components/scrollspy/">Bootstrap Scrollspy</a> component.
* [`HxBreadcrumb`](https://havit.blazor.eu/components/HxBreadcrumb) - <a href="https://getbootstrap.com/docs/5.1/components/breadcrumb/">Bootstrap 5 Breadcrumb</a> component. Indicates the current page’s location within a navigational hierarchy.
* [`HxAnchorFragmentNavigation`](https://havit.blazor.eu/components/HxAnchorFragmentNavigation) - Compensates Blazor deficiency in anchor-fragments (scrolling to <code>page#id</code> URLs).
* [`HxTabPanel`](https://havit.blazor.eu/components/HxTabPanel) - Container for <code><a href="/components/HxTab/">HxTab</a></code>s for easier implementation of tabbed UI.

## Modals & Interactions

* [`HxMessageBox`](https://havit.blazor.eu/components/HxMessageBox) - Component to display message-boxes.
* [`HxModal`](https://havit.blazor.eu/components/HxModal) - Component to render modal dialog as a <a href="https://getbootstrap.com/docs/5.1/components/modal/">Bootstrap Modal</a>.
* [`HxDialogBase`](https://havit.blazor.eu/components/HxDialogBase) - Base class to simplify custom modal dialog implementation.
* [`HxOffcanvas`](https://havit.blazor.eu/components/HxOffcanvas) - <a href="https://getbootstrap.com/docs/5.0/components/offcanvas/">Bootstrap Offcanvas</a> component (aka Drawer).
* [`HxMessenger`](https://havit.blazor.eu/components/HxMessenger) - <code><a href="/components/HxToastContainer/">HxToastContainer</a></code> wrapper for displaying <code><a href="/components/HxToast/">HxToast</a></code> messages dispatched through <code><a href="/components/IHxMessengerService/">IHxMessengerService</a></code>.
* [`HxToast`](https://havit.blazor.eu/components/HxToast) - <a href="https://getbootstrap.com/docs/5.1/components/toasts/">Bootstrap Toast</a> component. Not intented to be used in user code, use <code><a href="/components/HxMessenger/">HxMessenger</a></code>.

## Special

* [`HxDynamicElement`](https://havit.blazor.eu/components/HxDynamicElement) - Renders an element with the specified name, attributes and child-content.
* [`HxGoogleTagManager`](https://havit.blazor.eu/components/HxGoogleTagManager) - Support for <a href="https://developers.google.com/tag-manager/devguide">Google Tag Manager</a> - initialization and pushing data to data-layer.