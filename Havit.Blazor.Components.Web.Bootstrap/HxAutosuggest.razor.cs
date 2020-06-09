using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxAutosuggest : IDisposable
	{
		[Parameter]
		public string Value { get; set; }

		[Parameter]
		public EventCallback<string> ValueChanged { get; set; }

		[Parameter]
		public EventCallback<SuggestionRequest> SuggestionsRequested { get; set; }

		[Inject]
		public IJSRuntime JSRuntime { get; set; }

		private string dropdownId;
		private System.Timers.Timer timer;
		private string autosuggestValue;
		private List<string> suggestions;

		public HxAutosuggest()
		{
			dropdownId = Guid.NewGuid().ToString();
		}

		private async Task HandleValueChanged(string value)
		{
			Value = value;
			await ValueChanged.InvokeAsync(value);
			StateHasChanged();
		}

		private async Task HandleInputInput(string userInput)
		{
			autosuggestValue = userInput;

			if (timer != null)
			{
				timer.Stop();
			}

			if (userInput.Length >= 3)
			{
				if (timer == null)
				{
					timer = new System.Timers.Timer();
					timer.Interval = 300;
					timer.AutoReset = false; // just once
					timer.Elapsed += HandleTimerElapsed;
				}
				timer.Start();
			}
			else
			{
				await DestroyDropdown();
			}
		}

		private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// todo: paralelní běh?
			await InvokeAsync(async () =>
			{
				if (await UpdateSuggestions())
				{
					StateHasChanged();
					await OpenDropdown();
				}
			});
		}

		private async Task HandleFocus()
		{
			await DestroyDropdown();
		}

		// Kvůli updatovanání HTML a kolizi s bootstrap Dropdown nesmíme v InputBlur přerenderovat html!
		private void HandleInputBlur()
		{
			if (timer != null)
			{
				timer.Stop();
			}
		}

		private async Task<bool> UpdateSuggestions()
		{
			SuggestionRequest suggestionRequest = new SuggestionRequest(autosuggestValue);
			await SuggestionsRequested.InvokeAsync(suggestionRequest);
			// TOOD: Kontrola nastavení SuggestionsRequested

			suggestions = suggestionRequest.Suggestions;

			return suggestions?.Any() ?? false;
		}

		private async Task HandleItemClick(string value)
		{
			Value = value;
			await ValueChanged.InvokeAsync(Value);
			StateHasChanged();
		}

		#region OpenDropdown, CloseDropdown
		private async Task OpenDropdown()
		{
			await JSRuntime.InvokeVoidAsync("hxBootstrapAutosuggest_openDropdown", $"#{dropdownId} input");
		}

		private async Task DestroyDropdown()
		{
			await JSRuntime.InvokeVoidAsync("hxBootstrapAutosuggest_destroyDropdown", $"#{dropdownId} input");
		}
		#endregion

		public void Dispose()
		{
			if (timer != null)
			{
				timer.Dispose();
			}
		}
	}
}

