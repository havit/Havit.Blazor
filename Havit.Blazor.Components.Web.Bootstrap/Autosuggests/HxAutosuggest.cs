using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxAutosuggest<TItemType> : HxInputBase<TItemType>
	{
		[Parameter] public AutosuggestDataProviderDelegate<TItemType> DataProvider { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int MinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int Delay { get; set; } = 300;

		private protected override string CoreInputCssClass => "form-control";
		private protected override string CoreCssClass => String.Empty;

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenComponent<HxAutosuggestInternal<TItemType, TItemType>>(1);
			builder.AddAttribute(1000, nameof(HxAutosuggestInternal<TItemType, TItemType>.Value), Value);
			builder.AddAttribute(1001, nameof(HxAutosuggestInternal<TItemType, TItemType>.ValueChanged), EventCallback.Factory.Create<TItemType>(this, HandleValueChanged));
			builder.AddAttribute(1002, nameof(HxAutosuggestInternal<TItemType, TItemType>.DataProvider), DataProvider);
			builder.AddAttribute(1003, nameof(HxAutosuggestInternal<TItemType, TItemType>.TextSelector), TextSelector);
			builder.AddAttribute(1004, nameof(HxAutosuggestInternal<TItemType, TItemType>.MinimumLength), MinimumLength);
			builder.AddAttribute(1005, nameof(HxAutosuggestInternal<TItemType, TItemType>.Delay), Delay);
			builder.AddAttribute(1006, nameof(HxAutosuggestInternal<TItemType, TItemType>.InputId), InputId);
			builder.AddAttribute(1007, nameof(HxAutosuggestInternal<TItemType, TItemType>.InputCssClass), GetInputCssClassToRender()); // we may render "is-invalid" which has no sense here (there is no invalid-feedback following the element).
			builder.AddAttribute(1008, nameof(HxAutosuggestInternal<TItemType, TItemType>.EnabledEffective), EnabledEffective);
			builder.AddAttribute(1009, nameof(HxAutosuggestInternal<TItemType, TItemType>.TextFromValueSelector), TextSelector /* we have ITItemType and TValueType of the same type */);
			// No ValueSelector - /* we have ITItemType and TValueType of the same type */
			builder.CloseComponent();			
		}

		protected override void BuildRenderValidationMessage(RenderTreeBuilder builder)
		{
			if (ShowValidationMessage)
			{
				builder.OpenElement(1, "div");
				builder.AddAttribute(2, "class", IsValueValid() ? InvalidCssClass : null);
				builder.CloseElement();

				builder.OpenRegion(3);
				base.BuildRenderValidationMessage(builder);
				builder.CloseRegion();
			}
		}

		private async Task HandleValueChanged(TItemType newValue)
		{
			Value = newValue;
			await ValueChanged.InvokeAsync(Value);
		}

		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TItemType result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}
	}
}
