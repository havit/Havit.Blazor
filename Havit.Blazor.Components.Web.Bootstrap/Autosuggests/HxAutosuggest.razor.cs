using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxAutosuggest : IDisposable // TODO? IAsyncDisposable
	{
		[Parameter] public string Value { get; set; }
		[Parameter] public EventCallback<string> ValueChanged { get; set; }
		[Parameter] public EventCallback<SuggestionRequest> SuggestionsRequested { get; set; } // TODO? SuggestionsProvider + ála public delegate ValueTask<ItemsProviderResult<TItem>> ItemsProviderDelegate<TItem>(ItemsProviderRequest request);

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int MinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int Delay { get; set; } = 300;

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private string dropdownId;
		private System.Timers.Timer timer;
		private CancellationTokenSource cancellationTokenSource;
		private string autosuggestValue;
		private List<string> suggestions;

		public HxAutosuggest()
		{
			dropdownId = "hx" + Guid.NewGuid().ToString("N");
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (!SuggestionsRequested.HasDelegate)
			{
				throw new ArgumentException("Property not set.", nameof(SuggestionsRequested));
			}
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

			timer?.Stop();
			cancellationTokenSource?.Cancel();

			if (userInput.Length >= MinimumLength)
			{
				if (timer == null)
				{
					timer = new System.Timers.Timer();
					timer.Interval = Delay;
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
			timer?.Stop();
			cancellationTokenSource?.Cancel();
		}

		private async Task<bool> UpdateSuggestions()
		{
			cancellationTokenSource?.Dispose(); // TODO nebo Cancel?!

			cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = cancellationTokenSource.Token;
			SuggestionRequest suggestionRequest = new SuggestionRequest(autosuggestValue, cancellationToken);
			await SuggestionsRequested.InvokeAsync(suggestionRequest);

			if (cancellationToken.IsCancellationRequested)
			{
				return false;
			}

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
			// TODO JavaScript Isolation
			await JSRuntime.InvokeVoidAsync("hxBootstrapAutosuggest_openDropdown", $"#{dropdownId} input");
		}

		private async Task DestroyDropdown()
		{
			await JSRuntime.InvokeVoidAsync("hxBootstrapAutosuggest_destroyDropdown", $"#{dropdownId} input");
		}
		#endregion

		public void Dispose()
		{
			timer?.Dispose();
			cancellationTokenSource?.Dispose();
		}
	}
}

