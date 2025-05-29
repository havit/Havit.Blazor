namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Search
{
	[Inject] private NavigationManager NavigationManager { get; set; }

	private SearchItem SelectedResult
	{
		get
		{
			return null;
		}
		set
		{
			NavigateToSelectedPage(value);
		}
	}

	private string _userInput;

	private const int DefaultsLevel = 2;
	private const int EnumsLevel = 2;
	private const int EventArgsLevel = 2;
	private const int DelegatesLevel = 2;

	private readonly List<SearchItem> _searchItems = new()
	{
		new("/premium", "Premium", "support subscription sponsorship price pricing license licensing SLA priority enterprise showcase Goran blocks elements"),


		// Components and other pages

		new("/components/Inputs", "Inputs", "form"),
		new("/components/Inputs#input-groups", "Inputs > Input groups", ""),
		new("/components/Inputs#floating-labels", "Inputs > Floating labels", ""),

		new("/components/HxAccordion", "HxAccordion", "collapse"),
		new("/components/HxAlert", "HxAlert", "message warning exclamation panel"),
		new("/components/HxAnchorFragmentNavigation", "HxAnchorFragmentNavigation", "id scroll"),
		new("/components/HxAutosuggest", "HxAutosuggest", "autocomplete search typeahead select combobox"),
		new("/components/HxBadge", "HxBadge", "chip tag"),
		new("/components/HxBreadcrumb", "HxBreadcrumb", "navigation link"),
		new("/components/HxButton", "HxButton", "click submit input tooltip"),
		new("/components/HxButtonGroup", "HxButtonGroup", "collection"),
		new("/components/HxButtonToolbar", "HxButtonToolbar", ""),
		new("/components/HxCalendar", "HxCalendar", "datepicker"),
		new("/components/HxCard", "HxCard", "panel"),
		new("/components/HxCarousel", "HxCarousel", "slider jumbotron"),
		new("/components/HxCheckbox", "HxCheckbox", "hxinputcheckbox switch"),
		new("/components/HxCheckboxList", "HxCheckboxList", "multiselect"),
		new("/components/HxChipList", "HxChipList", "tags badges"),
		new("/components/HxCloseButton", "HxCloseButton", "dismiss cross x"),
		new("/components/HxCollapse", "HxCollapse", "accordion dropdown expand"),
		new("/components/HxCollapseToggleButton", "HxCollapseToggleButton", ""),
		new("/components/HxCollapseToggleElement", "HxCollapseToggleElement", ""),
		new("/components/HxContextMenu", "HxContextMenu", "dropdown popup"),
		new("/components/HxDialogBase", "HxDialogBase", "custom dialog modal messagebox"),
		new("/components/HxDropdown", "HxDropdown", "collapse tooltip popover popup popper HxDropdownToggleElement HxDropdownMenu HxDropdownContent HxDropdownHeader HxDropdownItemNavLink HxDropdownItem HxDropdownItemText HxDropdownDivider"),
		new("/components/HxDropdownButtonGroup", "HxDropdownButtonGroup", "collapse tooltip popover popup popper HxDropdownToggleButton"),
		new("/components/HxDynamicElement", "HxDynamicElement", "dynamiccomponent html"),
		new("/components/HxEChart", "HxEChart", "graph piechart barchart map apache echarts"),
		new("/components/HxFilterForm", "HxFilterForm", "HxListLayout"),
		new("/components/HxFormState", "HxFormState", "enabled disabled"),
		new("/components/HxFormValue", "HxFormValue", "readonly"),
		new("/components/HxGoogleTagManager", "HxGoogleTagManager", "gtm ga analytics"),
		new("/components/HxGrid", "HxGrid", "data row column table datalist"),
		new("/components/HxGrid#InfiniteScroll", "HxGrid > Infinite scroll (Virtualized)", ""),
		new("/components/HxGrid#context-menu", "HxGrid > Context menu", ""),
		new("/components/HxGrid#multiselect", "HxGrid > Multiselect with checkboxes", ""),
		new("/components/HxGrid#inline-editing", "HxGrid > Inline-editing", ""),
		new("/components/HxGrid#header-filtering", "HxGrid > Filtering from header", ""),
		new("/components/HxIcon", "HxIcon", "bootstrap picture image font"),
		new("/components/HxInputDate", "HxInputDate", "datepicker"),
		new("/components/HxInputDateRange", "HxInputDateRange", "period datepicker"),
		new("/components/HxInputFile", "HxInputFile[Core]", "upload single multiple"),
		new("/components/HxInputFileDropZone", "HxInputFileDropZone", "upload single multiple"),
		new("/components/HxInputNumber", "HxInputNumber", ""),
		new("/components/HxInputPercent", "HxInputPercent", "normalization hxinputnumber"),
		new("/components/HxInputRange", "HxInputRange", "slider"),
		new("/components/HxInputTags", "HxInputTags", "keywords labels restricted suggestion"),
		new("/components/HxInputText", "HxInputText", "field password search textbox"),
		new("/components/HxInputTextArea", "HxInputTextArea", "field multiline"),
		new("/components/HxListGroup", "HxListGroup", "item"),
		new("/components/HxListLayout", "HxListLayout", "data presentation filter"),
		new("/components/HxMessageBox", "HxMessageBox", "pop-up full-screen dialog modal confirm"),
		new("/components/HxMessenger", "HxMessenger", "popup warning alert toaster"),
		new("/components/HxModal", "HxModal", "popup fullscreen dialog messagebox"),
		new("/components/HxMultiSelect", "HxMultiSelect", "dropdownlist picker checkbox multiple"),
		new("/components/HxNav", "HxNav", "navigation"),
		new("/components/HxNavLink#HxNavLink", "HxNavLink", "href redirect navigation"),
		new("/components/HxNavbar", "HxNavbar", "navigation header"),
		new("/components/HxOffcanvas", "HxOffcanvas", "drawer"),
		new("/components/HxPager", "HxPager", "list pagination"),
		new("/components/HxPlaceholder", "HxPlaceholder", "loading skeleton spinner progress"),
		new("/components/HxPopover", "HxPopover", "hover tooltip popper dropdown"),
		new("/components/HxProgress", "HxProgress", "loading bar indicator"),
		new("/components/HxProgressIndicator", "HxProgressIndicator", "loading spinner"),
		new("/components/HxRadioButtonList", "HxRadioButtonList", "multiselect"),
		new("/components/HxRedirectTo", "HxRedirectTo", "navigateto 302 301 moved navigationmanager"),
		new("/components/HxRepeater", "HxRepeater", "multi clone foreach iterator iterate"),
		new("/components/HxScrollspy", "HxScrollspy", "anchor navigation link"),
		new("/components/HxSearchBox", "HxSearchBox", "autosuggest autocomplete searchbar omnibox input"),
		new("/components/HxSelect", "HxSelect", "dropdownlist picker"),
		new("/components/HxSidebar", "HxSidebar", "navigation collapse layout responsive"),
		new("/components/HxSmartPasteButton", "HxSmartPasteButton", "clipboard ai gpt artificial intelligence copy"),
		new("/components/HxSmartTextArea", "HxSmartTextArea", "autocompletions suggest intellisense typeahead ai gpt artificial intelligence"),
		new("/components/HxSmartComboBox", "HxSmartComboBox", "autocomplete search typeahead select suggest ai artificial intelligence autosuggest"),
		new("/components/HxSpinner", "HxSpinner", "loading progress placeholder skeleton"),
		new("/components/HxSubmit#HxSubmit", "HxSubmit", "send form button"),
		new("/components/HxSwitch", "HxSwitch", "hxinputswitch hxradiobutton checkbox"),
		new("/components/HxTabPanel", "HxTabPanel", "page tabs"),
		new("/components/HxToast", "HxToast", "messenger"),
		new("/components/HxTooltip", "HxTooltip", "hover popup popover dropdown popper"),
		new("/components/HxTreeView", "HxTreeView", "hierarchy"),
		new("/components/HxValidationMessage", "HxValidationMessage", "form"),

		// Concepts
		new("/concepts/defaults-and-settings", "Defaults & Settings", "configuration themes wide preset"),
		new("/concepts/Debouncer", "Debouncer", "delay timer"),
		new("/concepts/dark-color-mode-theme", "Dark color mode", "dark color theme"),

		// Supportive types

		// Defaults (settings)

		new("/types/AutosuggestSettings", "Autosuggest Settings", "defaults", DefaultsLevel),
		new("/types/BadgeSettings", "Badge Settings", "defaults", DefaultsLevel),
		new("/types/ButtonSettings", "Button Settings", "defaults", DefaultsLevel),
		new("/types/CalendarSettings", "Calendar Settings", "defaults", DefaultsLevel),
		new("/types/CardSettings", "Card Settings", "defaults", DefaultsLevel),
		new("/types/ChipListSettings", "ChipList Settings", "defaults", DefaultsLevel),
		new("/types/CloseButtonSettings", "CloseButton Settings", "defaults", DefaultsLevel),
		new("/types/CheckboxSettings", "Checkbox Settings", "defaults", DefaultsLevel),
		new("/types/CheckboxListSettings", "CheckboxList Settings", "defaults", DefaultsLevel),
		new("/types/ContextMenuSettings", "ContextMenu Settings", "defaults", DefaultsLevel),
		new("/types/FormValueSettings", "FormValue Settings", "defaults", DefaultsLevel),
		new("/types/GridSettings", "Grid Settings", "defaults", DefaultsLevel),
		new("/types/InputsSettings", "Inputs Settings", "defaults", DefaultsLevel),
		new("/types/InputFileSettings", "InputFile Settings", "defaults", DefaultsLevel),
		new("/types/InputRangeSettings", "InputRangeSettings Settings", "defaults", DefaultsLevel),
		new("/types/InputDateRangeSettings", "InputDateRange Settings", "defaults", DefaultsLevel),
		new("/types/InputDateSettings", "InputDate Settings", "defaults", DefaultsLevel),
		new("/types/InputNumberSettings", "InputNumber Settings", "defaults", DefaultsLevel),
		new("/types/InputTextSettings", "InputText Settings", "defaults", DefaultsLevel),
		new("/types/InputTagsSettings", "InputTags Settings", "defaults", DefaultsLevel),
		new("/types/InputFileCoreSettings", "InputFileCore Settings", "defaults", DefaultsLevel),
		new("/types/ListLayoutSettings", "ListLayout Settings", "defaults", DefaultsLevel),
		new("/types/MultiSelect Settings", "MultiSelect Settings", "defaults", DefaultsLevel),
		new("/types/Modal Settings", "Modal Settings", "defaults", DefaultsLevel),
		new("/types/MessengerServiceExtensionsSettings", "MessengerServiceExtensions Settings", "defaults", DefaultsLevel),
		new("/types/MessageBoxSettings", "MessageBox Settings", "defaults", DefaultsLevel),
		new("/types/OffcanvasSettings", "Offcanvas Settings", "defaults", DefaultsLevel),
		new("/types/PagerSettings", "Pager Settings", "defaults", DefaultsLevel),
		new("/types/PlaceholderContainerSettings", "PlaceholderContainer Settings", "defaults", DefaultsLevel),
		new("/types/PlaceholderSettings", "Placeholder Settings", "defaults", DefaultsLevel),
		new("/types/ProgressIndicatorSettings", "ProgressIndicator Settings", "defaults", DefaultsLevel),
		new("/types/RadioButtonList", "RadioButtonList Settings", "defaults", DefaultsLevel),
		new("/types/SelectSettings", "Select Settings", "defaults", DefaultsLevel),
		new("/types/SearchBoxSettings", "SearchBox Settings", "defaults", DefaultsLevel),

		// Enums

		new("/types/BadgeType", "BadgeType", "enum shape", EnumsLevel),
		new("/types/BootstrapTheme", "BootstrapTheme", "enum", EnumsLevel),
		new("/types/ButtonGroupOrientation", "ButtonGroupOrientation", "enum", EnumsLevel),
		new("/types/ButtonGroupSize", "ButtonGroupSize", "enum", EnumsLevel),
		new("/types/ButtonIconPlacement", "ButtonIconPlacement", "enum", EnumsLevel),
		new("/types/ButtonSize", "ButtonSize", "enum", EnumsLevel),
		new("/types/CardImagePlacement", "CardImagePlacement", "enum", EnumsLevel),
		new("/types/CarouselRide", "CarouselRide", "enum", EnumsLevel),
		new("/types/CollapseDirection", "CollapseDirection", "enum", EnumsLevel),
		new("/types/DropdownAutoClose", "DropdownAutoClose", "enum", EnumsLevel),
		new("/types/DropdownDirection", "DropdownDirection", "enum", EnumsLevel),
		new("/types/BindEvent", "BindEvent", "enum", EnumsLevel),
		new("/types/InputSize", "InputSize", "enum", EnumsLevel),
		new("/types/InputType", "InputType", "enum", EnumsLevel),
		new("/types/LabelValueRenderOrder", "LabelValueRenderOrder", "enum", EnumsLevel),
		new("/types/LabelType", "LabelType", "enum", EnumsLevel),
		new("/types/GridContentNavigationMode", "GridContentNavigationMode", "enum", EnumsLevel),
		new("/types/ListGroupHorizontal", "ListGroupHorizontal", "enum", EnumsLevel),
		new("/types/ModalBackdrop", "ModalBackdrop", "enum static", EnumsLevel),
		new("/types/ModalFullscreen", "ModalFullscreen", "enum behavior", EnumsLevel),
		new("/types/ModalSize", "ModalSize", "enum", EnumsLevel),
		new("/types/AnchorFragmentNavigationAutomationMode", "AnchorFragmentNavigationAutomationMode", "enum", EnumsLevel),
		new("/types/NavbarExpand", "NavbarExpand", "enum responsive expand breakpoint", EnumsLevel),
		new("/types/NavOrientation", "NavOrientation", "enum", EnumsLevel),
		new("/types/NavVariant", "NavVariant", "enum", EnumsLevel),
		new("/types/OffcanvasBackdrop", "OffcanvasBackdrop", "enum static", EnumsLevel),
		new("/types/OffcanvasPlacement", "OffcanvasPlacement", "enum", EnumsLevel),
		new("/types/OffcanvasRenderMode", "OffcanvasRenderMode", "enum", EnumsLevel),
		new("/types/OffcanvasResponsiveBreakpoint", "OffcanvasResponsiveBreakpoint", "enum", EnumsLevel),
		new("/types/OffcanvasSize", "OffcanvasSize", "enum", EnumsLevel),
		new("/types/PlaceholderAnimation", "PlaceholderAnimation", "enum", EnumsLevel),
		new("/types/PlaceholderSize", "PlaceholderSize", "enum", EnumsLevel),
		new("/types/SpinnerSize", "SpinnerSize", "enum", EnumsLevel),
		new("/types/SpinnerType", "SpinnerType", "enum", EnumsLevel),
		new("/types/ThemeColor", "ThemeColor", "enum", EnumsLevel),
		new("/types/ToastContainerPosition", "ToastContainerPosition", "enum", EnumsLevel),
		new("/types/PopoverPlacement", "PopoverPlacement", "enum", EnumsLevel),
		new("/types/PopoverTrigger", "PopoverTrigger", "enum", EnumsLevel),
		new("/types/TooltipPlacement", "TooltipPlacement", "enum", EnumsLevel),
		new("/types/TooltipTrigger", "TooltipTrigger", "enum", EnumsLevel),
		new("/types/MessageBoxButtons", "MessageBoxButtons", "enum", EnumsLevel),

		// Event arguments

		new("/types/FileUploadedEventArgs", "FileUploadedEventArgs", "argument event", EventArgsLevel),
		new("/types/UploadCompletedEventArgs", "UploadCompletedEventArgs", "argument event", EventArgsLevel),
		new("/types/UploadProgressEventArgs", "UploadProgressEventArgs", "argument event", EventArgsLevel),

		// Delegates

		new("/types/CalendarDateCustomizationProviderDelegate", "CalendarDateCustomizationProviderDelegate", "delegate action", DelegatesLevel),
		new("/types/CalendarDateCustomizationResult", "CalendarDateCustomizationResult", "delegate", DelegatesLevel),
		new("/types/CalendarDateCustomizationRequest", "CalendarDateCustomizationRequest", "delegate", DelegatesLevel),

		new("/types/AutosuggestDataProviderDelegate", "AutosuggestDataProviderDelegate", "delegate action", DelegatesLevel),
		new("/types/AutosuggestDataProviderResult", "AutosuggestDataProviderResult", "delegate", DelegatesLevel),
		new("/types/AutosuggestDataProviderRequest", "AutosuggestDataProviderRequest", "delegate", DelegatesLevel),

		new("/types/GridDataProviderDelegate", "GridDataProviderDelegate", "delegate action", DelegatesLevel),
		new("/types/GridDataProviderResult", "GridDataProviderResult", "delegate", DelegatesLevel),
		new("/types/GridDataProviderRequest", "GridDataProviderRequest", "delegate", DelegatesLevel),

		new("/types/InputTagsDataProviderDelegate", "InputTagsDataProviderDelegate", "delegate action", DelegatesLevel),
		new("/types/InputTagsDataProviderResult", "InputTagsDataProviderResult", "delegate", DelegatesLevel),
		new("/types/InputTagsDataProviderRequest", "InputTagsDataProviderRequest", "delegate", DelegatesLevel)

	};

	private HxAutosuggest<SearchItem, SearchItem> _autosuggest;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender && (_autosuggest is not null))
		{
			await _autosuggest.FocusAsync();
		}
	}

	private Task<AutosuggestDataProviderResult<SearchItem>> ProvideSuggestions(AutosuggestDataProviderRequest request)
	{
		_userInput = request.UserInput.Trim();

		return Task.FromResult(new AutosuggestDataProviderResult<SearchItem>
		{
			Data = GetSearchItems()
		});
	}

	private IEnumerable<SearchItem> GetSearchItems()
	{
		return _searchItems
				.Where(si => si.GetRelevance(_userInput) > 0)
				.OrderBy(si => si.Level)
					.ThenByDescending(si => si.GetRelevance(_userInput))
					.ThenBy(si => si.Title)
				.Take(8);
	}

	public void NavigateToSelectedPage(SearchItem searchItem)
	{
		NavigationManager.NavigateTo(searchItem.Href);
	}
}
