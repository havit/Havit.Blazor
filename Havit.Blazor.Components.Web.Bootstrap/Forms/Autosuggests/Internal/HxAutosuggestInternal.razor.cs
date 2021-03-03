using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestInternal<TItemType, TValueType> : IAsyncDisposable
	{
		[Parameter] public TValueType Value { get; set; }
		[Parameter] public EventCallback<TValueType> ValueChanged { get; set; }

		[Parameter] public AutosuggestDataProviderDelegate<TItemType> DataProvider { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// </summary>
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		/// <summary>
		/// Text to display for null value.
		/// </summary>
		[Parameter] public string NullText { get; set; }

		/// <summary>
		/// Gets item from <see cref="Value"/>.
		/// </summary>
		[Parameter] public Func<TValueType, Task<TItemType>> ItemFromValueResolver { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int MinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int Delay { get; set; } = 300;

		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public string InputId { get; set; }

		[Parameter] public bool EnabledEffective { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private string dropdownId = "hx" + Guid.NewGuid().ToString("N");
		private System.Timers.Timer timer;
		private string userInput = String.Empty;
		private CancellationTokenSource cancellationTokenSource;
		private List<TItemType> suggestions;
		private bool userInputModified;
		private bool isDropdownOpened = false;
		private bool isBlured;
		private IJSObjectReference jsModule;
		private HxAutosuggestInput autosuggestInput;

		internal string ChipValue => userInput;

		private async Task SetValueItemWithEventCallback(TItemType selectedItem)
		{
			TValueType value = GetValueFromItem(selectedItem);

			if (!EqualityComparer<TValueType>.Default.Equals(Value, value))
			{
				Value = value;
				await ValueChanged.InvokeAsync(Value);
			}
		}

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			Contract.Requires<InvalidOperationException>(DataProvider != null, $"Property {nameof(DataProvider)} on {GetType()} must have a value.");

			if (!EqualityComparer<TValueType>.Default.Equals(Value, default))
			{
				if ((ItemFromValueResolver == null) && (typeof(TValueType) == typeof(TItemType)))
				{
					userInput = TextSelector.Invoke((TItemType)(object)Value);
				}
				else
				{
					userInput = TextSelector.Invoke(await ItemFromValueResolver(Value));
				}
			}
			else
			{
				userInput = NullText;
			}
			userInputModified = false;
		}

		private async Task HandleInputInput(string newUserInput)
		{
			// user changes an input
			userInput = newUserInput;
			userInputModified = true;

			timer?.Stop(); // if waiting for an interval, stop it
			cancellationTokenSource?.Cancel(); // if already loading data, cancel it

			// start new time interval
			if (userInput.Length >= MinimumLength)
			{
				if (timer == null)
				{
					timer = new System.Timers.Timer();
					timer.AutoReset = false; // just once
					timer.Elapsed += HandleTimerElapsed;
				}
				timer.Interval = Delay;
				timer.Start();
			}
			else
			{
				// or close a dropdown
				suggestions = null;
				await DestroyDropdownAsync();
			}
		}

		private async void HandleTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			// when a time interval reached, update suggestions
			await InvokeAsync(async () =>
			{
				await UpdateSuggestionsAsync();
			});
		}

		private async Task HandleFocus()
		{
			// when an input looses focus, close a dropdown
			await DestroyDropdownAsync();
		}

		// Kvůli updatovanání HTML a kolizi s bootstrap Dropdown nesmíme v InputBlur přerenderovat html!
		private void HandleInputBlur()
		{
			// when user clicks back button in browser this method can be called after it is disposed!
			isBlured = true;
			timer?.Stop(); // if waiting for an interval, stop it
			cancellationTokenSource?.Cancel(); // if waiting for an interval, stop it
		}

		private async Task UpdateSuggestionsAsync()
		{
			// Cancelation is performed in HandleInputInput method
			cancellationTokenSource?.Dispose();

			cancellationTokenSource = new CancellationTokenSource();
			CancellationToken cancellationToken = cancellationTokenSource.Token;

			AutosuggestDataProviderRequest request = new AutosuggestDataProviderRequest
			{
				UserInput = userInput,
				CancellationToken = cancellationToken
			};

			AutosuggestDataProviderResult<TItemType> result;
			try
			{
				result = await DataProvider.Invoke(request);
			}
			catch (OperationCanceledException operationCanceledException) when (operationCanceledException.CancellationToken == cancellationToken)
			{
				return;
			}

			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			suggestions = result.Data.ToList();

			if (suggestions?.Any() ?? false)
			{
				await OpenDropdownAsync();
			}
			else
			{
				await DestroyDropdownAsync();
			}

			StateHasChanged();
		}

		private async Task HandleItemClick(TItemType item)
		{
			// user clicked on an item in the "dropdown".
			await SetValueItemWithEventCallback(item);
			userInput = TextSelectorEffective(item);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (isBlured)
			{
				isBlured = false;
				if (userInputModified && !isDropdownOpened)
				{
					await SetValueItemWithEventCallback(default);
					userInput = String.Empty;
					userInputModified = false;
					StateHasChanged();
				}
			}
		}
		#region OpenDropdownAsync, DestroyDropdownAsync, EnsureJsModuleAsync
		private async Task OpenDropdownAsync()
		{
			if (!isDropdownOpened)
			{
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("open", autosuggestInput.InputElement);
				isDropdownOpened = true;
			}
		}

		private async Task DestroyDropdownAsync()
		{
			if (isDropdownOpened)
			{
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("destroy", autosuggestInput.InputElement);
				isDropdownOpened = false;
			}
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxautosuggest.js");
		}
		#endregion

		private TValueType GetValueFromItem(TItemType item)
		{
			if (item == null)
			{
				return default;
			}

			if (ValueSelector != null)
			{
				return ValueSelector(item);
			}

			if (typeof(TValueType) == typeof(TItemType))
			{
				return (TValueType)(object)item;
			}

			throw new InvalidOperationException("ValueSelector property not set.");
		}

		private string TextSelectorEffective(TItemType item)
		{
			return (item == null)
				? NullText
				: TextSelectorHelper.GetText(TextSelector, item);
		}

		public async ValueTask DisposeAsync()
		{
			timer?.Dispose();
			timer = null;
			cancellationTokenSource?.Dispose();
			cancellationTokenSource = null;

			if (jsModule != null)
			{
				await jsModule.DisposeAsync();
			}
		}

	}
}

