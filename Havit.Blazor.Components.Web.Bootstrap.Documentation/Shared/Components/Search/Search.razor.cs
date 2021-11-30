using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
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

		private string userInput;

		private readonly List<SearchItem> searchItems = new()
		{
			new("/components/Inputs", "Inputs", "form"),
			new("/components/Inputs#InputGroups", "Inputs > Input groups", ""),
			new("/components/Inputs#FloatingLabels", "Inputs > Floating labels", ""),

			new("/components/HxAutosuggest", "HxAutosuggest", "autocomplete"),

			new("/components/HxCalendar", "HxCalendar", "datepicker"),

			new("/components/HxInputCheckbox", "HxInputCheckbox", ""),

			new("/components/HxInputDate", "HxInputDate", "datepicker"),
			new("/components/HxInputDate#CalendarCustomization", "HxInputDate > Dropdown calendar customization", ""),

			new("/components/HxInputDateRange", "HxInputDateRange", "period datepicker"),
			new("/components/HxInputDateRange#CalendarsCustomization", "HxInputDateRange > Dropdown calendar customization", ""),

			new("/components/HxInputFile", "HxInputFile[Core]", "upload single multiple"),
			new("/components/HxInputFile#LimitFileSizeAndTypes", "HxInputFile[Core] > Limit file size and/or accepted file types", ""),
			new("/components/HxInputFile#SingleFileUpload", "HxInputFile[Core] > Simple single file upload", ""),

			new("/components/HxInputFileDropZone", "HxInputFileDropZone", "upload single multiple"),

			new("/components/HxInputNumber", "HxInputNumber", ""),

			new("/components/HxInputPercent", "HxInputPercent", "normalization hxinputnumber"),
			new("/components/HxInputPercent#InputGroupEnd", "HxInputPercent > InputGroupEnd", ""),

			new("/components/HxInputSwitch", "HxInputSwitch", "hxradiobutton"),

			new("/components/HxInputTags", "HxInputTags", "keywords naked restricted suggestion"),
			new("/components/HxInputTags#StaticSuggestions", "HxInputTags > Static suggestions", ""),

			new("/components/HxInputText", "HxInputText", "field password search"),

			new("/components/HxInputTextArea", "HxInputTextArea", "field multiline"),

			new("/components/HxCheckboxList", "HxCheckboxList", "multiselect"),

			new("/components/HxFormState", "HxFormState", ""),

			new("/components/HxFormValue", "HxFormValue", ""),
			new("/components/HxFormValue#Sizing", "HxFormValue > Sizing", ""),
			new("/components/HxFormValue#InputGroups", "HxFormValue > Input Groups", ""),
			new("/components/HxFormValue#CustomContent", "HxFormValue > Custom Content", ""),

			new("/components/HxRadioButtonList", "HxRadioButtonList", "multiselect"),

			new("/components/HxSelect", "HxSelect", "dropdownlist picker"),

			new("/components/HxFilterForm", "HxFilterForm", "HxListLayout"),

			new("/components/HxValidationMessage", "HxValidationMessage", "form"),

			new("/components/HxSubmit#HxSubmit", "HxSubmit", "send form button"),

			new("/components/HxButtonGroup", "HxButtonGroup", "collection"),
			new("/components/HxButtonGroup#OutlinedStyles", "HxButtonGroup > Outlined Styles", ""),
			new("/components/HxButtonGroup#ButtonToolbar", "HxButtonGroup > Button toolbar", "combine"),
			new("/components/HxButtonGroup#Nesting", "HxButtonGroup > Nesting", ""),
			new("/components/HxButtonGroup#VerticalOrientation", "HxButtonGroup > Vertical orientation", ""),
			new("/components/HxButtonGroup#CheckboxGroup", "HxButtonGroup > With checkboxes", ""),

			new("/components/HxButtonToolbar", "HxButtonToolbar", ""),

			new("/components/HxDropdown", "HxDropdown", "collapse tooltip popover popup popper"),
			new("/components/HxDropdown#SplitButton", "HxDropdown > Split button", ""),
			new("/components/HxDropdown#Directions", "HxDropdown > Directions", ""),
			new("/components/HxDropdown#HeaderDisabledActive", "HxDropdown > Header, Disabled, Active", ""),
			new("/components/HxDropdown#CustomContent", "HxDropdown > Custom content", ""),
			new("/components/HxDropdown#OffsetAndreference", "HxDropdown > Offset and reference", ""),
			new("/components/HxDropdown#AutoCloseBehavior", "HxDropdown > Auto close behavior", ""),
			new("/components/HxDropdown#NavWithDropdown", "HxDropdown > Nav with dropdown", ""),
			new("/components/HxDropdown#DarkDropdowns", "HxDropdown > Dark dropdowns", ""),

			new("/components/HxBadge", "HxBadge", "chip tag"),
			new("/components/HxBadge#Headings", "HxBadge > Headings", ""),
			new("/components/HxBadge#Buttons", "HxBadge > Buttons", ""),
			new("/components/HxBadge#Positioned", "HxBadge > Positioned", ""),
			new("/components/HxBadge#BackgroundColors", "HxBadge > Background colors", ""),
			new("/components/HxBadge#PillBadges", "HxBadge > Pill badges", ""),

			new("/components/HxChipList", "HxChipList", "tags badges"),

			new("/components/HxIcon", "HxIcon", "bootstrap picture image font"),

			new("/components/HxSpinner", "HxSpinner", "loading progress placeholder skeleton"),
			new("/components/HxSpinner#BorderSpinner", "HxSpinner > Border spinner", ""),
			new("/components/HxSpinner#Colors", "HxSpinner > Colors", ""),
			new("/components/HxSpinner#GrowingSpinner", "HxSpinner > Growing spinner", ""),

			new("/components/HxProgressIndicator", "HxProgressIndicator", "loading spinner"),

			new("/components/HxTooltip", "HxTooltip", "hover popup popover dropdown popper"),
			new("/components/HxTooltip#HxButtonSupport", "HxTooltip > HxButton support", ""),
			new("/components/HxTooltip#Placement", "HxTooltip > Placement", ""),
			new("/components/HxTooltip#HTMLContent", "HxTooltip > HTML content", ""),
			new("/components/HxTooltip#Programmability", "HxTooltip > Programmability", ""),

			new("/components/HxPopover", "HxPopover", "hover tooltip popper dropdown"),
			new("/components/HxPopover#Placement", "HxPopover > Placement", ""),
			new("/components/HxPopover#HTMLContent", "HxPopover > HTML content", ""),
			new("/components/HxPopover#HTMLSanitization", "HxPopover > HTML sanitization", ""),
			new("/components/HxPopover#DismissOnNextClick", "HxPopover > Dismiss on next click", ""),
			new("/components/HxPopover#Programmability", "HxPopover > Programmability", ""),

			new("/components/HxGrid", "HxGrid", "data row column table datalist"),
			new("/components/HxGrid#ClientSideDataProcessing", "HxGrid > Client-side data processing", ""),
			new("/components/HxGrid#ServerSidePagingAndSorting", "HxGrid > Server-side paging & sorting", ""),
			new("/components/HxGrid#InfiniteScroll", "HxGrid > Infinite scroll (Virtualized)", ""),
			new("/components/HxGrid#ContextMenu", "HxGrid > Context menu", ""),
			new("/components/HxGrid#Multiselect", "HxGrid > Multiselect with checkboxes", ""),

			new("/components/HxContextMenu", "HxContextMenu", "dropdown popup"),

			new("/components/HxPager", "HxPager", "list pagination"),

			new("/components/HxRepeater", "HxRepeater", "multi clone foreach iterator iterate"),

			new("/components/HxAccordion", "HxAccordion", "collapse"),
			new("/components/HxAccordion#PlainNoIDs", "HxAccordion > Plain, No IDs", ""),
			new("/components/HxAccordion#Flush", "HxAccordion > Flush", ""),
			new("/components/HxAccordion#StayOpen", "HxAccordion > Stay open", ""),

			new("/components/HxAlert", "HxAlert", "message warning exclamation panel"),
			new("/components/HxAlert#Icons", "HxAlert > Icons", ""),
			new("/components/HxAlert#AdditionalContent", "HxAlert > Additional content", ""),
			new("/components/HxAlert#Dissmissible", "HxAlert > Dissmissible", ""),

			new("/components/HxCard", "HxCard", "panel"),
			new("/components/HxCard#ContentTypes", "HxCard > Content types", ""),
			new("/components/HxCard#Navigation", "HxCard > Navigation", ""),
			new("/components/HxCard#Images", "HxCard > Images", ""),
			new("/components/HxCard#Variants", "HxCard > Variants", ""),

			new("/components/HxCarousel", "HxCarousel", "slider jumbotron"),
			new("/components/HxCarousel#Controls", "HxCarousel > Controls", ""),
			new("/components/HxCarousel#Indicators", "HxCarousel > Indicators", ""),
			new("/components/HxCarousel#Captions", "HxCarousel > Captions", ""),
			new("/components/HxCarousel#Crossfade", "HxCarousel > Crossfade", ""),
			new("/components/HxCarousel#IndividualInterval", "HxCarousel > Interval", ""),
			new("/components/HxCarousel#AutomaticSliding", "HxCarousel > Automatic sliding", ""),
			new("/components/HxCarousel#DarkVariant", "HxCarousel > Dark variant", ""),
			new("/components/HxCarousel#Events", "HxCarousel > Events", ""),
			new("/components/HxCarousel#TouchSwiping", "HxCarousel > Touch swiping", ""),

			new("/components/HxCollapse", "HxCollapse", "accordion dropdown expand"),
			new("/components/HxCollapse#Horizontal", "HxCollapse > Horizontal", ""),
			new("/components/HxCollapse#MultipleTargets", "HxCollapse > Multiple targets", ""),

			new("/components/HxPlaceholder", "HxPlaceholder", "loading skeleton spinner progress"),
			new("/components/HxPlaceholder#Width", "HxPlaceholder > Width", ""),
			new("/components/HxPlaceholder#Color", "HxPlaceholder > Color", ""),
			new("/components/HxPlaceholder#Sizing", "HxPlaceholder > Sizing", ""),
			new("/components/HxPlaceholder#Animation", "HxPlaceholder > Animation", ""),

			new("/components/HxProgress", "HxProgress", "loading bar indicator"),
			new("/components/HxProgress#Labels", "HxProgress > Labels", ""),
			new("/components/HxProgress#Height", "HxProgress > Height", ""),
			new("/components/HxProgress#Backgrounds", "HxProgress > Backgrounds", ""),
			new("/components/HxProgress#MultipleBars", "HxProgress > Multiple bars", ""),
			new("/components/HxProgress#Striped", "HxProgress > Striped", ""),
			new("/components/HxProgress#AnimatedStripes", "HxProgress > Animated stripes", ""),
			new("/components/HxProgress#Scale", "HxProgress > Scale", ""),
			new("/components/HxProgress#Interactive", "HxProgress > Interactive", ""),

			new("/components/HxTabPanel", "HxTabPanel", "page tabs"),

			new("/components/HxListGroup", "HxListGroup", "item"),
			new("/components/HxListGroup#ActiveAndDisabledItems", "HxListGroup > Active and disabled items", ""),
			new("/components/HxListGroup#Links", "HxListGroup > Links", ""),
			new("/components/HxListGroup#Buttons", "HxListGroup > Buttons", ""),
			new("/components/HxListGroup#Flush", "HxListGroup > Flush", ""),
			new("/components/HxListGroup#Numbered", "HxListGroup > Numbered", ""),
			new("/components/HxListGroup#Horizontal", "HxListGroup > Horizontal", ""),
			new("/components/HxListGroup#Colors", "HxListGroup > Colors", ""),
			new("/components/HxListGroup#WithBadges", "HxListGroup > With badges", ""),
			new("/components/HxListGroup#CustomContent", "HxListGroup > Custom content", ""),

			new("/components/HxListLayout", "HxListLayout", "data presentation filter"),

			new("/components/HxNavbar", "HxNavbar", "navigation header"),
			new("/components/HxNavbar#Brand", "HxNavbar > Brand", "image icon"),
			new("/components/HxNavbar#Text", "HxNavbar > Text", "image icon"),
			new("/components/HxNavbar#ColorSchemes", "HxNavbar > Color schemes", ""),

			new("/components/HxSidebar", "HxSidebar", "navigation collapse layout responsive"),

			new("/components/HxNav", "HxNav", "navigation"),
			new("/components/HxNav#Vertical", "HxNav > Vertical", ""),
			new("/components/HxNav#Tabs", "HxNav > Tabs", ""),
			new("/components/HxNav#Pills", "HxNav > Pills", ""),
			new("/components/HxNav#Dropdowns", "HxNav > Dropdowns", ""),

			new("/components/HxNavLink#HxNavLink", "HxNavLink", "href redirect navigation"),

			new("/components/HxScrollspy", "HxScrollspy", "anchor navigation link"),
			new("/components/HxScrollspy#StaticContentHxNavNavigation", "HxScrollspy > Static content", "HxNav list group"),
			new("/components/HxScrollspy#DynamicContent", "HxScrollspy > Dynamic content", "async loaded"),

			new("/components/HxBreadcrumb", "HxBreadcrumb", "navigation link"),
			new("/components/HxBreadcrumb#Dividers", "HxBreadcrumb > Dividers", ""),

			new("/components/HxAnchorFragmentNavigation", "HxAnchorFragmentNavigation", "id scroll"),

			new("/components/HxMessageBox", "HxMessageBox", "pop-up full-screen dialog modal confirm"),

			new("/components/HxModal", "HxModal", "popup fullscreen dialog messagebox"),
			new("/components/HxModal#StaticBackdrop", "HxModal > Static backdrop", ""),
			new("/components/HxModal#ScrollingLongContent", "HxModal > Scrolling long content", ""),
			new("/components/HxModal#VerticallyCentered", "HxModal > Vertically centered", ""),
			new("/components/HxModal#OptionalSizes", "HxModal > Optional sizes", ""),
			new("/components/HxModal#FullscreenModal", "HxModal > Fullscreen Modal", ""),

			new("/components/HxDialogBase", "HxDialogBase", "custom dialog modal messagebox"),

			new("/components/HxOffcanvas", "HxOffcanvas", "drawer"),
			new("/components/HxOffcanvas#CustomCloseButtonIcon", "HxOffcanvas > Custom close-button icon", ""),
			new("/components/HxOffcanvas#Placement", "HxOffcanvas > Placement", ""),
			new("/components/HxOffcanvas#Backdrop", "HxOffcanvas > Backdrop", ""),
			new("/components/HxOffcanvas#Size", "HxOffcanvas > Size", ""),

			new("/components/HxMessenger", "HxMessenger", "popup warning alert toaster"),

			new("/components/HxToast", "HxToast", "messenger"),

			new("/components/HxGoogleTagManager", "HxGoogleTagManager", "gtm ga analytics"),

			new("/components/HxButton", "HxButton", "click submit input"),
			new("/components/HxButton#OutlineButtons", "HxButton > Outline buttons", ""),
			new("/components/HxButton#WithBadge", "HxButton > With a badge", ""),
			new("/components/HxButton#WithTooltip", "HxButton > With a tooltip", ""),
			new("/components/HxButton#Icons", "HxButton > Icons", ""),

			new("/components/HxNamedViewList", "HxNamedViewList", "HxListLayout")
		};

		private HxAutosuggest<SearchItem, SearchItem> autosuggest;

		private bool wasFocused = false;

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender && !wasFocused)
			{
				wasFocused = true;
				await Task.Delay(1);
				await autosuggest.FocusAsync();
			}
		}

		private Task<AutosuggestDataProviderResult<SearchItem>> ProvideSuggestions(AutosuggestDataProviderRequest request)
		{
			this.userInput = request.UserInput.Trim();

			return Task.FromResult(new AutosuggestDataProviderResult<SearchItem>
			{
				Data = GetSearchItems()
			});
		}

		private IEnumerable<SearchItem> GetSearchItems()
		{
			return searchItems
					.Where(si => si.GetRelevance(userInput) > 0)
					.OrderBy(si => si.Level)
						.ThenByDescending(si => si.GetRelevance(userInput))
						.ThenBy(si => si.Title)
					.Take(5);
		}

		private void NavigateToFirstResult()
		{
			var firstSearchResult = GetSearchItems().FirstOrDefault();

			if (firstSearchResult != null)
			{
				NavigateToSelectedPage(firstSearchResult);
			}
		}

		public void NavigateToSelectedPage(SearchItem searchItem)
		{
			NavigationManager.NavigateTo(searchItem.Href);
		}
	}
}
