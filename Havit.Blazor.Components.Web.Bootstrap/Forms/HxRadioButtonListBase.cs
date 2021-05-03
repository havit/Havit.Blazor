using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Base class for HxRadioList and custom-implemented pickers.
	/// </summary>
	/// <typeparam name="TValue">Type of value.</typeparam>
	/// <typeparam name="TItem">Type of items.</typeparam>
	public abstract class HxRadioButtonListBase<TValue, TItem> : HxInputBase<TValue>
	{
		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItem, TValue> ValueSelectorImpl { get; set; }

		/// <summary>
		/// Items to display. 
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected IEnumerable<TItem> DataImpl { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItem, string> TextSelectorImpl { get; set; }

		/// <summary>
		/// Selects value to sort items. Uses <see cref="TextSelectorImpl"/> property when not set.
		/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItem, IComparable> SortKeySelectorImpl { get; set; }

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected bool AutoSortImpl { get; set; } = true;

		/// <inheritdoc />
		protected override bool EnabledEffective => base.EnabledEffective && (itemsToRender != null);

		private protected override string CoreInputCssClass => throw new NotSupportedException();

		private IEqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
		private List<TItem> itemsToRender;
		private int selectedItemIndex;
		private string chipValue;

		protected string GroupName { get; private set; }

		protected HxRadioButtonListBase()
		{
			GroupName = Guid.NewGuid().ToString("N");
		}

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (LabelType == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating)
			{
				throw new InvalidOperationException($"Floating labes are not supported on {nameof(HxRadioButtonListBase<TValue, TItem>)}.");
			}
		}

		/// <inheritdoc/>
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			chipValue = null;

			RefreshState();

			if (itemsToRender != null)
			{
				for (int i = 0; i < itemsToRender.Count; i++)
				{
					builder.OpenRegion(0);
					BuildRenderInput_RenderRadioItem(builder, i);
					builder.CloseRegion();
				}
			}
		}

		protected void BuildRenderInput_RenderRadioItem(RenderTreeBuilder builder, int index)
		{
			var item = itemsToRender[index];
			if (item != null)
			{
				bool selected = (index == selectedItemIndex);
				string text = TextSelectorHelper.GetText(TextSelectorImpl, item);
				if (selected)
				{
					chipValue = text;
				}

				string inputId = GroupName + "_" + index.ToString();

				builder.OpenElement(100, "div");
				builder.AddAttribute(101, "class", "form-check");

				builder.OpenElement(200, "input");
				builder.AddAttribute(201, "class", "form-check-input"); // TODO CoreCssClass
				builder.AddAttribute(202, "type", "radio");
				builder.AddAttribute(203, "name", GroupName);
				builder.AddAttribute(204, "id", inputId);
				builder.AddAttribute(205, "value", index.ToString());
				builder.AddAttribute(206, "checked", selected);
				builder.AddAttribute(207, "disabled", !CascadeEnabledComponent.EnabledEffective(this));
				int j = index;
				builder.AddAttribute(208, "onclick", EventCallback.Factory.Create(this, () => HandleInputClick(j)));
				builder.AddEventStopPropagationAttribute(209, "onclick", true);
				builder.CloseElement(); // input

				builder.OpenElement(300, "label");
				builder.AddAttribute(301, "class", "form-check-label");
				builder.AddAttribute(302, "for", inputId);
				builder.AddContent(303, text);
				builder.CloseElement(); // label

				builder.CloseElement(); // div
			}
		}

		/// <inheritdoc />
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

		private void HandleInputClick(int index)
		{
			CurrentValue = GetValueFromItem(itemsToRender[index]);
		}

		private void RefreshState()
		{
			if (DataImpl != null)
			{
				itemsToRender = DataImpl.ToList();

				// AutoSort
				if (AutoSortImpl && (itemsToRender.Count > 1))
				{
					if (SortKeySelectorImpl != null)
					{
						itemsToRender = itemsToRender.OrderBy(this.SortKeySelectorImpl).ToList();
					}
					else if (TextSelectorImpl != null)
					{
						itemsToRender = itemsToRender.OrderBy(this.TextSelectorImpl).ToList();
					}
					else
					{
						itemsToRender = itemsToRender.OrderBy(i => i.ToString()).ToList();
					}
				}

				// set next properties for rendering
				selectedItemIndex = itemsToRender.FindIndex(item => comparer.Equals(Value, GetValueFromItem(item)));

				if ((Value != null) && (selectedItemIndex == -1))
				{
					throw new InvalidOperationException($"Data does not contain item for current value '{Value}'.");
				}
			}
			else
			{
				itemsToRender = null;
				selectedItemIndex = -1;
			}
		}

		/// <inheritdoc/>
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private TValue GetValueFromItem(TItem item)
		{
			if (ValueSelectorImpl != null)
			{
				return ValueSelectorImpl(item);
			}

			if (typeof(TValue) == typeof(TItem))
			{
				return (TValue)(object)item;
			}

			throw new InvalidOperationException("ValueSelector property not set.");
		}

		protected override void RenderChipGenerator(RenderTreeBuilder builder)
		{
			if (!String.IsNullOrEmpty(chipValue))
			{
				base.RenderChipGenerator(builder);
			}
		}

		protected override void RenderChipValue(RenderTreeBuilder builder)
		{
			builder.AddContent(0, chipValue);
		}

	}
}
