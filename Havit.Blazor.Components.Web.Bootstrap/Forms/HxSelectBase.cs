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
	/// Base class for HxSelect and custom-implemented SELECT-pickers.
	/// </summary>
	/// <typeparam name="TValueType">Type of value.</typeparam>
	/// <typeparam name="TItemType">Type of items.</typeparam>
	public abstract class HxSelectBase<TValueType, TItemType> : HxInputBase<TValueType>, IInputWithSize
	{
		/// <inheritdoc />
		[Parameter] public InputSize InputSize { get; set; }

		/// <summary>
		/// Indicates when null is a valid value.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected bool? NullableImpl { get; set; }

		/// <summary>
		/// Indicates when null is a valid value.
		/// Uses (in order) to get effective value: Nullable property, RequiresAttribute on bounded property (false), Nullable type on bounded property (true), class (true), default (false).
		/// </summary>
		protected bool NullableEffective
		{
			get
			{
				if (NullableImpl != null)
				{
					return NullableImpl.Value;
				}

				if (GetValueAttribute<RequiredAttribute>() != null)
				{
					return false;
				}

				if (System.Nullable.GetUnderlyingType(typeof(TValueType)) != null)
				{
					return true;
				}

				if (typeof(TValueType).IsClass)
				{
					return true;
				}

				return true;
			}
		}

		/// <summary>
		/// Text to display for null value.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected string NullTextImpl { get; set; }

		/// <summary>
		/// Text to display when <see cref="DataImpl"/> is null.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected string NullDataTextImpl { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItemType, TValueType> ValueSelectorImpl { get; set; }

		/// <summary>
		/// Items to display. 
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected IEnumerable<TItemType> DataImpl { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItemType, string> TextSelectorImpl { get; set; }

		/// <summary>
		/// Selects value to sort items. Uses <see cref="TextSelectorImpl"/> property when not set.
		/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected Func<TItemType, IComparable> SortKeySelectorImpl { get; set; }

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected bool AutoSortImpl { get; set; } = true;

		/// <inheritdoc />
		protected override bool EnabledEffective => base.EnabledEffective && (itemsToRender != null);

		private protected override string CoreInputCssClass => "form-select";

		private IEqualityComparer<TValueType> comparer = EqualityComparer<TValueType>.Default;
		private List<TItemType> itemsToRender;
		private TItemType selectedItem;
		private int selectedItemIndex;

		/// <inheritdoc/>
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			RefreshState();

			builder.OpenElement(0, "select");
			BuildRenderInput_AddCommonAttributes(builder, null);

			builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1002, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxSelect nastavitelné?

			if (itemsToRender != null)
			{
				if (NullableEffective || (selectedItem == null))
				{
					builder.OpenElement(2000, "option");
					builder.AddAttribute(2001, "value", -1);
					builder.AddContent(2002, NullTextImpl);
					builder.CloseElement();
				}

				for (int i = 0; i < itemsToRender.Count; i++)
				{
					var item = itemsToRender[i];
					if (item != null)
					{
						builder.OpenElement(3000, "option");
						builder.AddAttribute(3001, "value", i.ToString());
						builder.AddAttribute(3002, "selected", i == selectedItemIndex);
						builder.AddContent(3003, TextSelectorImpl?.Invoke(item) ?? item?.ToString() ?? String.Empty);
						builder.CloseElement();
					}
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(NullDataTextImpl))
				{
					builder.OpenElement(4000, "option");
					builder.AddAttribute(4001, "selected", true);
					builder.AddContent(4002, NullDataTextImpl);
					builder.CloseElement();
				}
			}
			builder.CloseElement();
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
				selectedItem = itemsToRender.FirstOrDefault(item => comparer.Equals(Value, GetValueFromItem(item))); // null when not found
				selectedItemIndex = itemsToRender.IndexOf(selectedItem); // -1 when not found

				if ((Value != null) && (selectedItem == null))
				{
					throw new InvalidOperationException($"Data does not contain item for current value '{Value}'.");
				}
			}
			else
			{
				itemsToRender = null;
				selectedItem = default;
				selectedItemIndex = -1;
			}
		}

		/// <inheritdoc/>
		protected override bool TryParseValueFromString(string value, out TValueType result, out string validationErrorMessage)
		{
			int index = int.Parse(value);
			result = (index == -1)
				? default(TValueType)
				: GetValueFromItem(itemsToRender[index]);

			validationErrorMessage = null;
			return true;
		}

		private TValueType GetValueFromItem(TItemType item)
		{
			if (ValueSelectorImpl != null)
			{
				return ValueSelectorImpl(item);
			}

			if (typeof(TValueType) == typeof(TItemType))
			{
				return (TValueType)(object)item;
			}

			throw new InvalidOperationException("ValueSelector property not set.");
		}

		string IInputWithSize.GetInputSizeCssClass()
		{
			return this.InputSize switch
			{
				InputSize.Regular => null,
				InputSize.Small => "form-select-sm",
				InputSize.Large => "form-select-lg",
				_ => throw new InvalidOperationException(InputSize.ToString())
			};
		}
	}
}
