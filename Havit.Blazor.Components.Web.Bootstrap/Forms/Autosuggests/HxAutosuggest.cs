using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxAutosuggest<TItem, TValue> : HxInputBase<TValue>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
	{
		[Parameter] public AutosuggestDataProviderDelegate<TItem> DataProvider { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// </summary>
		[Parameter] public Func<TItem, TValue> ValueSelector { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set <c>ToString()</c> is used.
		/// </summary>
		[Parameter] public Func<TItem, string> TextSelector { get; set; }

		/// <summary>
		/// Template to display item.
		/// When not set, <see cref="TextSelector"/> is used.
		/// </summary>
		[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

		/// <summary>
		/// Minimal number of characters to start suggesting. Default is <c>2</c>.
		/// </summary>
		[Parameter] public int MinimumLength { get; set; } = 2;

		/// <summary>
		/// Debounce delay in miliseconds. Default is <c>300 ms</c>.
		/// </summary>
		[Parameter] public int Delay { get; set; } = 300;

		/// <summary>
		/// Short hint displayed in the input field before the user enters a value.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		/// <inheritdoc />
		[Parameter] public InputSize InputSize { get; set; }

		/// <inheritdoc />
		[Parameter] public LabelType? LabelType { get; set; }

		/// <summary>
		/// Returns corresponding item for (select) Value.
		/// </summary>
		[Parameter] public Func<TValue, Task<TItem>> ItemFromValueResolver { get; set; }

		protected override LabelValueRenderOrder RenderOrder => (LabelType == Bootstrap.LabelType.Floating) ? LabelValueRenderOrder.LabelValue : LabelValueRenderOrder.ValueOnly /* renderování labelu zajistí HxAutosuggestInternal */;
		private protected override string CoreInputCssClass => "form-control";
		private protected override string CoreCssClass => "hx-autosuggest position-relative";

		private HxAutosuggestInternal<TItem, TValue> hxAutosuggestInternalComponent;

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			LabelType labelTypeEffective = (this as IInputWithLabelType).LabelTypeEffective;

			builder.OpenComponent<HxAutosuggestInternal<TItem, TValue>>(1);
			builder.AddAttribute(1000, nameof(HxAutosuggestInternal<TItem, TValue>.Value), Value);
			builder.AddAttribute(1001, nameof(HxAutosuggestInternal<TItem, TValue>.ValueChanged), EventCallback.Factory.Create<TValue>(this, HandleValueChanged));
			builder.AddAttribute(1002, nameof(HxAutosuggestInternal<TItem, TValue>.DataProvider), DataProvider);
			builder.AddAttribute(1003, nameof(HxAutosuggestInternal<TItem, TValue>.ValueSelector), ValueSelector);
			builder.AddAttribute(1004, nameof(HxAutosuggestInternal<TItem, TValue>.TextSelector), TextSelector);
			builder.AddAttribute(1005, nameof(HxAutosuggestInternal<TItem, TValue>.ItemTemplate), ItemTemplate);
			builder.AddAttribute(1006, nameof(HxAutosuggestInternal<TItem, TValue>.MinimumLength), MinimumLength);
			builder.AddAttribute(1007, nameof(HxAutosuggestInternal<TItem, TValue>.Delay), Delay);
			builder.AddAttribute(1008, nameof(HxAutosuggestInternal<TItem, TValue>.InputId), InputId);
			builder.AddAttribute(1009, nameof(HxAutosuggestInternal<TItem, TValue>.InputCssClass), GetInputCssClassToRender()); // we may render "is-invalid" which has no sense here (there is no invalid-feedback following the element).
			builder.AddAttribute(1010, nameof(HxAutosuggestInternal<TItem, TValue>.EnabledEffective), EnabledEffective);
			builder.AddAttribute(1011, nameof(HxAutosuggestInternal<TItem, TValue>.ItemFromValueResolver), ItemFromValueResolver);
			builder.AddAttribute(1012, nameof(HxAutosuggestInternal<TItem, TValue>.Placeholder), (labelTypeEffective == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating) ? "placeholder" : Placeholder);
			builder.AddAttribute(1013, nameof(HxAutosuggestInternal<TItem, TValue>.LabelTypeEffective), labelTypeEffective);
			builder.AddAttribute(1014, nameof(HxAutosuggestInternal<TItem, TValue>.FormValueComponent), this);
			builder.AddComponentReferenceCapture(1015, component => hxAutosuggestInternalComponent = (HxAutosuggestInternal<TItem, TValue>)component);
			builder.CloseComponent();
		}

		private void HandleValueChanged(TValue newValue)
		{
			CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
		}

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc />
		public override async ValueTask FocusAsync()
		{
			if (hxAutosuggestInternalComponent == null)
			{
				throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
			}

			await hxAutosuggestInternalComponent.FocusAsync();
		}

		/// <inheritdoc />
		protected override void RenderChipGenerator(RenderTreeBuilder builder)
		{
			if (!String.IsNullOrEmpty(hxAutosuggestInternalComponent?.ChipValue))
			{
				base.RenderChipGenerator(builder);
			}
		}

		/// <inheritdoc />
		protected override void RenderChipValue(RenderTreeBuilder builder)
		{
			builder.AddContent(0, hxAutosuggestInternalComponent.ChipValue);
		}
	}
}
