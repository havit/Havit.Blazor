using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxMultiSelectInternal<TValue, TItem>
	{
		[Parameter] public string InputId { get; set; }

		[Parameter] public string InputCssClass { get; set; }

		[Parameter] public string InputText { get; set; }

		[Parameter] public bool EnabledEffective { get; set; }

		[Parameter] public List<TItem> ItemsToRender { get; set; }

		[Parameter] public List<int> SelectedIndexes { get; set; }

		[Parameter] public Func<TItem, string> TextSelector { get; set; }

		[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

		[Parameter] public List<TValue> Value { get; set; }

		[Parameter] public EventCallback<SelectionChangedArgs> ItemSelectionChanged { get; set; }

		private ElementReference inputElement;

		private async Task HandleItemSelectionChangedAsync(bool newChecked, TItem item)
		{
			await ItemSelectionChanged.InvokeAsync(new SelectionChangedArgs
			{
				Checked = newChecked,
				Item = item
			});
		}

		public async ValueTask FocusAsync()
		{
			if (EqualityComparer<ElementReference>.Default.Equals(inputElement, default))
			{
				throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
			}
			await inputElement.FocusAsync();
		}

		public class SelectionChangedArgs
		{
			public bool Checked { get; set; }
			public TItem Item { get; set; }
		}
	}
}
