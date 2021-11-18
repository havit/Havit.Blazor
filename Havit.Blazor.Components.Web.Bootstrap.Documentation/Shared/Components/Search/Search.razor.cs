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

		private readonly List<SearchItem> searchItems = new()
		{
			new("/components/Inputs", "Inputs", "form"),
			new("/components/Inputs", "Inputs > Input groups", ""),
			new("/components/Inputs", "Inputs > Floating labels", ""),

			new("/components/HxAutosuggest", "HxAutosuggest", "autocomplete"),

			new("/components/HxCalendar", "HxCalendar", "datepicker"),

			new("/components/HxInputCheckbox", "HxInputCheckbox", ""),

			new("/components/HxInputDate", "HxInputDate", "datepicker"),
			new("/components/HxInputDate", "HxInputDate > Dropdown calendar customization", ""),

			new("/components/HxInputDateRange", "HxInputDateRange", "period datepicker"),
			new("/components/HxInputDateRange", "HxInputDateRange > Dropdown calendar customization", ""),

			new("/components/HxInputFile", "HxInputFile[Core]", "upload single multiple"),
			new("/components/HxInputFile", "HxInputFile[Core] > Limit file size and/or accepted file types", ""),
			new("/components/HxInputFile", "HxInputFile[Core] > Simple single file upload", ""),

			new("/components/HxInputFileDropZone", "HxInputFileDropZone", "upload single multiple"),

			new("/components/HxInputNumber", "HxInputNumber", ""),

			new("/components/HxInputPercent", "HxInputPercent", "normalization hxinputnumber"),
			new("/components/HxInputPercent", "HxInputPercent > InputGroupEnd", ""),

			new("/components/HxInputSwitch", "HxInputSwitch", "hxradiobutton"),

			new("/components/HxInputTags", "HxInputTags", "keywords naked restricted suggestion"),
			new("/components/HxInputTags", "HxInputTags > Static suggestions", ""),

			new("/components/HxInputText", "HxInputText", "field password search"),

			new("/components/HxInputTextArea", "HxInputTextArea", "field multiline"),

			new("/components/HxCheckboxList", "HxCheckboxList", "multiselect"),

			new("/components/HxFormState", "HxFormState", ""),

			new("/components/HxFormValue", "HxFormValue", ""),

			new("/components/HxRadioButtonList", "HxRadioButtonList", "multiselect"),

			new("/components/HxSelect", "HxSelect", "dropdownlist picker"),

			new("/components/HxFilterForm", "HxFilterForm", "HxListLayout"),

			new("/components/HxValidationMessage", "HxValidationMessage", "form"),

			new("/components/HxButton", "HxSubmit", "send form button"),

			new("/components/HxButtonGroup", "HxButtonGroup", "collection"),
			new("/components/HxButtonGroup", "HxButtonGroup > Outlined Styles", ""),
			new("/components/HxButtonGroup", "HxButtonGroup > Button toolbar", "combine"),
			new("/components/HxButtonGroup", "HxButtonGroup > Nesting", ""),
			new("/components/HxButtonGroup", "HxButtonGroup > Vertical orientation", ""),

			new("/components/HxButtonToolbar", "HxButtonToolbar", ""),

			new("/components/HxDropdown", "HxDropdown", "collapse tooltip popover popup popper"),
			new("/components/HxDropdown", "HxDropdown > Split button", ""),
			new("/components/HxDropdown", "HxDropdown > Directions", ""),
			new("/components/HxDropdown", "HxDropdown > Header, Disabled, Active", ""),
			new("/components/HxDropdown", "HxDropdown > Custom content", ""),
			new("/components/HxDropdown", "HxDropdown > Offset and reference", ""),
			new("/components/HxDropdown", "HxDropdown > Auto close behavior", ""),
			new("/components/HxDropdown", "HxDropdown > Nav with dropdown", ""),
			new("/components/HxDropdown", "HxDropdown > Dark dropdowns", ""),

			new("/components/HxBadge", "HxBadge", "chip tag"),
			new("/components/HxBadge", "HxBadge > Headings", ""),
			new("/components/HxBadge", "HxBadge > Buttons", ""),
			new("/components/HxBadge", "HxBadge > Positioned", ""),
			new("/components/HxBadge", "HxBadge > Background colors", ""),
			new("/components/HxBadge", "HxBadge > Pill badges", ""),

			new("/components/HxChipList", "HxChipList", "tags badges"),

			new("/components/HxIcon", "HxIcon", "bootstrap picture image font"),

			new("/components/HxSpinner", "HxSpinner", "loading progress placeholder skeleton"),
			new("/components/HxSpinner", "HxSpinner > Border spinner", ""),
			new("/components/HxSpinner", "HxSpinner > Colors", ""),
			new("/components/HxSpinner", "HxSpinner > Growing spinner", ""),

			new("/components/HxProgressIndicator", "HxProgressIndicator", "loading spinner"),

			new("/components/HxTooltip", "HxTooltip", "hover popup popover dropdown popper"),
			new("/components/HxTooltip", "HxTooltip > HxButton support", ""),
			new("/components/HxTooltip", "HxTooltip > Placement", ""),
			new("/components/HxTooltip", "HxTooltip > HTML content", ""),
			new("/components/HxTooltip", "HxTooltip > Programmability", ""),

			new("/components/HxPopover", "HxPopover", "hover tooltip popper dropdown"),
			new("/components/HxPopover", "HxPopover > Placement", ""),
			new("/components/HxPopover", "HxPopover > HTML content", ""),
			new("/components/HxPopover", "HxPopover > HTML sanitization", ""),
			new("/components/HxPopover", "HxPopover > Dismiss on next click", ""),
			new("/components/HxPopover", "HxPopover > Programmability", ""),

			new("/components/HxGrid", "HxGrid", "data row column table datalist"),
			new("/components/HxGrid", "HxGrid > Client-side data processing", ""),
			new("/components/HxGrid", "HxGrid > Server-side paging & sorting", ""),
			new("/components/HxGrid", "HxGrid > Infinite scroll (Virtualized)", ""),
			new("/components/HxGrid", "HxGrid > Context menu", ""),
			new("/components/HxGrid", "HxGrid > Multiselect with checkboxes", ""),

			new("/components/HxContextMenu", "HxContextMenu", "dropdown popup"),

			new("/components/HxPager", "HxPager", "list pagination"),

			new("/components/HxRepeater", "HxRepeater", "multi clone foreach iterator iterate"),

			new("/components/HxAccordion", "HxAccordion", "collapse"),
			new("/components/HxAccordion", "HxAccordion > Plain, No IDs", ""),
			new("/components/HxAccordion", "HxAccordion > Flush", ""),
			new("/components/HxAccordion", "HxAccordion > Stay open", ""),

			new("/components/HxAlert", "HxAlert", "message warning exclamation panel"),
			new("/components/HxAlert", "HxAlert > Icons", ""),
			new("/components/HxAlert", "HxAlert > Additional content", ""),
			new("/components/HxAlert", "HxAlert > Dissmissible", ""),

			new("/components/HxCard", "HxCard", "panel"),
			new("/components/HxCard", "HxCard > Content types", ""),
			new("/components/HxCard", "HxCard > Navigation", ""),
			new("/components/HxCard", "HxCard > Images", ""),
			new("/components/HxCard", "HxCard > Variants", ""),

			new("/components/HxCarousel", "HxCarousel", "slider jumbotron"),
			new("/components/HxCarousel", "HxCarousel > Controls", ""),
			new("/components/HxCarousel", "HxCarousel > Indicators", ""),
			new("/components/HxCarousel", "HxCarousel > Captions", ""),
			new("/components/HxCarousel", "HxCarousel > Crossfade", ""),
			new("/components/HxCarousel", "HxCarousel > Interval", ""),
			new("/components/HxCarousel", "HxCarousel > Automatic sliding", ""),
			new("/components/HxCarousel", "HxCarousel > Dark variant", ""),
			new("/components/HxCarousel", "HxCarousel > Events", ""),
			new("/components/HxCarousel", "HxCarousel > Touch swiping", ""),

			new("/components/HxCollapse", "HxCollapse", "accordion dropdown expand"),
			new("/components/HxCollapse", "HxCollapse > Horizontal", ""),
			new("/components/HxCollapse", "HxCollapse > Multiple targets", ""),

			new("/components/HxPlaceholder", "HxPlaceholder", "loading skeleton spinner progress"),
			new("/components/HxPlaceholder", "HxPlaceholder > Width", ""),
			new("/components/HxPlaceholder", "HxPlaceholder > Color", ""),
			new("/components/HxPlaceholder", "HxPlaceholder > Sizing", ""),
			new("/components/HxPlaceholder", "HxPlaceholder > Animation", ""),

			new("/components/HxProgress", "HxProgress", "loading bar indicator"),
			new("/components/HxProgress", "HxProgress > Labels", ""),
			new("/components/HxProgress", "HxProgress > Height", ""),
			new("/components/HxProgress", "HxProgress > Backgrounds", ""),
			new("/components/HxProgress", "HxProgress > Multiple bars", ""),
			new("/components/HxProgress", "HxProgress > Striped", ""),
			new("/components/HxProgress", "HxProgress > Animated stripes", ""),
			new("/components/HxProgress", "HxProgress > Scale", ""),
			new("/components/HxProgress", "HxProgress > Interactive", ""),

			new("/components/HxTabPanel", "HxTabPanel", "page tabs"),

			new("/components/HxListGroup", "HxListGroup", "item"),
			new("/components/HxListGroup", "HxListGroup > Active and disabled items", ""),
			new("/components/HxListGroup", "HxListGroup > Links", ""),
			new("/components/HxListGroup", "HxListGroup > Buttons", ""),
			new("/components/HxListGroup", "HxListGroup > Flush", ""),
			new("/components/HxListGroup", "HxListGroup > Numbered", ""),
			new("/components/HxListGroup", "HxListGroup > Horizontal", ""),
			new("/components/HxListGroup", "HxListGroup > Colors", ""),
			new("/components/HxListGroup", "HxListGroup > With badges", ""),
			new("/components/HxListGroup", "HxListGroup > Custom content", ""),

			new("/components/HxListLayout", "HxListLayout", "data presentation filter"),

			new("/components/HxNavbar", "HxNavbar", "navigation header"),
			new("/components/HxNavbar", "HxNavbar > Brand", "image icon"),
			new("/components/HxNavbar", "HxNavbar > Text", "image icon"),
			new("/components/HxNavbar", "HxNavbar > Color schemes", ""),

			new("/components/HxSidebar", "HxSidebar", "navigation collapse layout responsive"),

			new("/components/HxNav", "HxNav", "navigation"),
			new("/components/HxNav", "HxNav > Vertical", ""),
			new("/components/HxNav", "HxNav > Tabs", ""),
			new("/components/HxNav", "HxNav > Pills", ""),
			new("/components/HxNav", "HxNav > Dropdowns", ""),

			new("/components/HxNavLink", "HxNavLink", "href redirect navigation"),

			new("/components/HxScrollspy", "HxScrollspy", "anchor navigation link"),
			new("/components/HxScrollspy", "HxScrollspy > Static content", "HxNav list group"),
			new("/components/HxScrollspy", "HxScrollspy > Dynamic content", "async loaded"),

			new("/components/HxBreadcrumb", "HxBreadcrumb", "navigation link"),
			new("/components/HxBreadcrumb", "HxBreadcrumb > Dividers", ""),

			new("/components/HxAnchorFragmentNavigation", "HxAnchorFragmentNavigation", "id scroll"),

			new("/components/HxMessageBox", "HxMessageBox", "pop-up full-screen dialog modal confirm"),

			new("/components/HxModal", "HxModal", "popup fullscreen dialog messagebox"),
			new("/components/HxModal", "HxModal > Static backdrop", ""),
			new("/components/HxModal", "HxModal > Scrolling long content", ""),
			new("/components/HxModal", "HxModal > Vertically centered", ""),
			new("/components/HxModal", "HxModal > Optional sizes", ""),
			new("/components/HxModal", "HxModal > Fullscreen Modal", ""),

			new("/components/HxDialogBase", "HxDialogBase", "custom dialog modal messagebox"),

			new("/components/HxOffcanvas", "HxOffcanvas", "drawer"),
			new("/components/HxOffcanvas", "HxOffcanvas > Custom close-button icon", ""),
			new("/components/HxOffcanvas", "HxOffcanvas > Placement", ""),
			new("/components/HxOffcanvas", "HxOffcanvas > Backdrop", ""),
			new("/components/HxOffcanvas", "HxOffcanvas > Size", ""),

			new("/components/HxMessenger", "HxMessenger", "popup warning alert toaster"),

			new("/components/HxToast", "HxToast", "messenger"),

			new("/components/HxGoogleTagManager", "HxGoogleTagManager", "gtm ga analytics"),

			new("/components/HxButton", "HxButton", "click submit input"),
			new("/components/HxButton", "HxButton > Outline buttons", ""),
			new("/components/HxButton", "HxButton > With a badge", ""),
			new("/components/HxButton", "HxButton > With a tooltip", ""),
			new("/components/HxButton", "HxButton > Icons", ""),

			new("/components/HxNamedViewList", "HxNamedViewList", "HxListLayout")
		};

		private Task<AutosuggestDataProviderResult<SearchItem>> ProvideSuggestions(AutosuggestDataProviderRequest request)
		{
			var userInput = request.UserInput.Trim();
			StringComparison comparisonType = StringComparison.OrdinalIgnoreCase;

			return Task.FromResult(new AutosuggestDataProviderResult<SearchItem>
			{
				Data = searchItems
						.Where(si => si.Title.Contains(userInput, comparisonType) || si.Keywords.Contains(userInput, comparisonType))
						.OrderBy(si => si.Level)
							.ThenByDescending(si => si.Title.Contains(userInput, comparisonType))
							.ThenBy(si => si.Title)
						.Take(5)
			});
		}

		public void NavigateToSelectedPage(SearchItem searchItem)
		{
			NavigationManager.NavigateTo(searchItem.Href);
		}
	}
}
