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
	public class HxAutosuggest<TItemType, TValueType> : HxInputBase<TValueType>, IInputWithSize
	{
		[Parameter] public AutosuggestDataProviderDelegate<TItemType> DataProvider { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// </summary>
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set <c>ToString()</c> is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		/// <summary>
		/// Text to display for null value.
		/// </summary>
		[Parameter] public string NullText { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int MinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int Delay { get; set; } = 300;

		/// <inheritdoc />
		[Parameter] public InputSize InputSize { get; set; }

		/// <summary>
		/// Returns corresponding item for (select) Value.
		/// </summary>
		[Parameter] public Func<TValueType, Task<TItemType>> ItemFromValueResolver { get; set; }

		private protected override string CoreInputCssClass => "form-control";
		private protected override string CoreCssClass => String.Empty;

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenComponent<HxAutosuggestInternal<TItemType, TValueType>>(1);
			builder.AddAttribute(1000, nameof(HxAutosuggestInternal<TItemType, TValueType>.Value), Value);
			builder.AddAttribute(1001, nameof(HxAutosuggestInternal<TItemType, TValueType>.ValueChanged), EventCallback.Factory.Create<TValueType>(this, HandleValueChanged));
			builder.AddAttribute(1002, nameof(HxAutosuggestInternal<TItemType, TValueType>.DataProvider), DataProvider);
			builder.AddAttribute(1003, nameof(HxAutosuggestInternal<TItemType, TValueType>.ValueSelector), ValueSelector);
			builder.AddAttribute(1004, nameof(HxAutosuggestInternal<TItemType, TValueType>.TextSelector), TextSelector);
			builder.AddAttribute(1005, nameof(HxAutosuggestInternal<TItemType, TValueType>.NullText), NullText);
			builder.AddAttribute(1006, nameof(HxAutosuggestInternal<TItemType, TValueType>.MinimumLength), MinimumLength);
			builder.AddAttribute(1007, nameof(HxAutosuggestInternal<TItemType, TValueType>.Delay), Delay);
			builder.AddAttribute(1008, nameof(HxAutosuggestInternal<TItemType, TValueType>.InputId), InputId);
			builder.AddAttribute(1009, nameof(HxAutosuggestInternal<TItemType, TValueType>.InputCssClass), GetInputCssClassToRender()); // we may render "is-invalid" which has no sense here (there is no invalid-feedback following the element).
			builder.AddAttribute(1010, nameof(HxAutosuggestInternal<TItemType, TValueType>.EnabledEffective), EnabledEffective);
			builder.AddAttribute(1011, nameof(HxAutosuggestInternal<TItemType, TValueType>.ItemFromValueResolver), ItemFromValueResolver);
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

		private async Task HandleValueChanged(TValueType newValue)
		{
			Value = newValue;
			await ValueChanged.InvokeAsync(Value);
		}

		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValueType result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}
	}
}
