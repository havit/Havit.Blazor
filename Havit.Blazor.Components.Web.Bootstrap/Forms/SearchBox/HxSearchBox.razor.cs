using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A full-text search component.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public partial class HxSearchBox<TItem>
{
	[Parameter] public SearchBoxDataProviderDelegate<TItem> DataProvider { get; set; }

	[Parameter] public bool Enabled { get; set; } = true;

	/// <summary>
	/// Text written by the user (input text).
	/// </summary>
	[Parameter]
	public string Freetext
	{
		get
		{
			return freetext;
		}
		set
		{
			freetext = value;
			_ = HandleFreetextValueChanged();
		}
	}
	private string freetext;
	[Parameter] public EventCallback<string> FreetextChanged { get; set; }
	/// <summary>
	/// Occurs, when the enter key is pressed or when the freetext item is selected in the dropdown menu.
	/// </summary>
	[Parameter] public EventCallback<string> OnFreetextSelected { get; set; }
	/// <summary>
	/// Occurs, when any item other than freetext is selected.
	/// </summary>
	[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }

	/// <summary>
	/// Placeholder text for the search input.
	/// </summary>
	[Parameter] public string Placeholder { get; set; }

	[Parameter] public Func<TItem, string> TitleSelector { get; set; }
	[Parameter] public Func<TItem, string> SubtitleSelector { get; set; }
	[Parameter] public Func<TItem, IconBase> IconSelector { get; set; }

	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }
	[Parameter] public RenderFragment<string> FreetextItemTemplate { get; set; }

	/// <summary>
	/// Rendered when the <see cref="DataProvider" /> doesn't return any data.
	/// </summary>
	[Parameter] public RenderFragment EmptyTemplate { get; set; }
	/// <summary>
	/// Rendered when no input is present.
	/// </summary>
	[Parameter] public RenderFragment EmptyInputTemplate { get; set; }

	[Parameter] public string CssClass { get; set; }
	[Parameter] public string ItemCssClass { get; set; }

	/// <summary>
	/// Icon of the input, when no text is written.
	/// </summary>
	[Parameter] public IconBase SearchIcon { get; set; } = BootstrapIcon.Search;
	/// <summary>
	/// Icon of the input, when text is written allowing the user to clear the text.
	/// </summary>
	[Parameter] public IconBase ClearIcon { get; set; }

	/// <summary>
	/// Label of the input field.
	/// </summary>
	[Parameter] public string Label { get; set; }
	/// <summary>
	/// Label type of the input field.
	/// </summary>
	[Parameter] public LabelType LabelType { get; set; }

	/// <summary>
	/// Minimum lenght to call the data provider (display any results).
	/// </summary>
	[Parameter] public int MinimumLength { get; set; } = 1;

	/// <summary>
	/// Whether the freetext item, intended to confirm freetext selection, should be displayed.
	/// </summary>
	[Parameter] public bool ShowFreetextItem { get; set; } = true;
	/// <summary>
	/// Whether freetext should be enabled, if disabled <see cref="OnFreetextSelected"/> won't ever get called.
	/// </summary>
	[Parameter] public bool FreetextEnabled { get; set; } = true;

	private List<TItem> searchResults = new();

	public async Task ClearInput()
	{
		if (Freetext != string.Empty)
		{
			Freetext = string.Empty;
			await HandleFreetextValueChanged();
		}
	}

	public async Task UpdateSuggestions()
	{
		if (string.IsNullOrEmpty(Freetext) || Freetext.Length < MinimumLength)
		{
			return;
		}

		SearchBoxDataProviderRequest request = new()
		{
			UserInput = Freetext,
			CancellationToken = default
		};

		var result = await DataProvider.Invoke(request);
		searchResults = result.Data.ToList();

		await InvokeAsync(StateHasChanged);
	}

	protected async Task HandleFreetextValueChanged()
	{
		await FreetextChanged.InvokeAsync();
		await UpdateSuggestions();
	}

	protected async Task HandleFreetextSelected()
	{
		if (FreetextEnabled)
		{
			await OnFreetextSelected.InvokeAsync(Freetext);
		}
	}
	protected async Task HandleItemSelected(TItem item)
	{
		await OnItemSelected.InvokeAsync(item);
	}
}
