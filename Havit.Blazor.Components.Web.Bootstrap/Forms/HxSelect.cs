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
	/// Select.
	/// </summary>
	/// <typeparam name="TValueType">Type of value.</typeparam>
	/// <typeparam name="TItemType">Type of items.</typeparam>
	public class HxSelect<TValueType, TItemType> : HxInputBase<TValueType>
	{
		/// <summary>
		/// Indicates when null is a valid value.
		/// </summary>
		[Parameter] public bool? Nullable { get; set; }

		/// <summary>
		/// Indicates when null is a valid value.
		/// Uses (in order) to get effective value: Nullable property, RequiresAttribute on bounded property (false), Nullable type on bounded property (true), class (true), default (false).
		/// </summary>
		protected bool NullableEffective
		{
			get
			{
				if (Nullable != null)
				{
					return Nullable.Value;
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
		/// </summary>
		[Parameter] public string NullText { get; set; }

		/// <summary>
		/// Text to display when Items are null.
		/// </summary>
		[Parameter] public string NullItemsText { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValueType is same as TItemTime.
		/// </summary>
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; }

		/// <summary>
		/// Items to display. 
		/// </summary>
		[Parameter] public IEnumerable<TItemType> Items { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		/// <summary>
		/// Selects value to sort items. Uses <see cref="TextSelector"/> property when not set.
		/// When complex sorting required, sort data manually and don't let sort them by this component. Alternatively create a custom comparable property.
		/// </summary>
		[Parameter] public Func<TItemType, IComparable> SortKeySelector { get; set; }

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true;

		/// <inheritdoc />
		/// </summary>
		protected override bool EnabledEffective => base.EnabledEffective && (itemsToRender != null);

		private IEqualityComparer<TValueType> comparer = EqualityComparer<TValueType>.Default;
		private List<TItemType> itemsToRender;
		private TItemType selectedItem;
		private int selectedItemIndex;

		/// <inheritdoc/>
		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			if (Items != null)
			{ 
				itemsToRender = Items.ToList();
				
				// AutoSort
				if (AutoSort && (itemsToRender.Count > 1))
				{
					if (SortKeySelector != null)
					{
						itemsToRender = itemsToRender.OrderBy(this.SortKeySelector).ToList();
					}
					else if (TextSelector != null)
					{
						itemsToRender = itemsToRender.OrderBy(this.TextSelector).ToList();
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
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "select");
			builder.AddAttribute(1, "class", "form-select");
			BuildRenderInput_AddCommonAttributes(builder, null);

			builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1002, "onclick", true); // TODO: Chceme onclick:stopPropagation na HxSelect nastavitelné?

			if (itemsToRender != null)
			{
				if (NullableEffective || (selectedItem == null))
				{
					builder.OpenElement(2000, "option");
					builder.AddAttribute(2001, "value", -1);
					builder.AddContent(2002, NullText);
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
						builder.AddContent(3003, TextSelector?.Invoke(item) ?? item?.ToString() ?? String.Empty);
						builder.CloseElement();
					}
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(NullItemsText))
				{
					builder.OpenElement(4000, "option");
					builder.AddAttribute(4001, "selected", true);
					builder.AddContent(4002, NullItemsText);
					builder.CloseElement();
				}
			}
			builder.CloseElement();
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
	}
}
