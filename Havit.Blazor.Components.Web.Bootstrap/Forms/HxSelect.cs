using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{
	// TODO: Kosmetika
	public class HxSelect<TValueType, TItemType> : HxInputBase<TValueType>
	{
		[Parameter] public string NullText { get; set; }
		[Parameter] public bool Nullable { get; set; } = false; // TODO: Přesunout logiku na RequiredAttribute vs. reference vs. nullable?
		[Parameter] public Func<TItemType, TValueType> ValueSelector { get; set; } // TODO: Pojmenování?
		[Parameter] public IEnumerable<TItemType> Items { get; set; }
		[Parameter] public Func<TItemType, string> Text { get; set; } // TODO: Pojmenování?
		[Parameter] public Func<TItemType, IComparable> Sort { get; set; } // TODO: Neumíme zřetězení výrazů pro řazení, v takovém případě buď umělou vlastnost s IComparable nebo seřadit předem.
		[Parameter] public bool AutoSort { get; set; } = true;

		protected List<TItemType> itemsToRender;

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);

			itemsToRender = Items.ToList() ?? new List<TItemType>();

			if (AutoSort && (itemsToRender.Count > 1))
			{
				if (Sort != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Sort).ToList();
				}
				else if (Text != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.Text).ToList();
				}
			}
		}

		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, "select");
			BuildRenderInput_AddCommonAttributes(builder, null);

			builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

			IEqualityComparer<TValueType> comparer = EqualityComparer<TValueType>.Default;
			TItemType selectedItem = itemsToRender.FirstOrDefault(item => comparer.Equals(Value, GetValueFromItem(item)));

			if (Nullable || (selectedItem == null))
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
					builder.AddAttribute(3002, "selected", comparer.Equals(Value, GetValueFromItem(item)));
					builder.AddContent(3003, Text?.Invoke(item) ?? item?.ToString() ?? String.Empty);
					builder.CloseElement();
				}
			}

			builder.CloseElement();
		}

		protected override bool TryParseValueFromString(string value, out TValueType result, out string validationErrorMessage)
		{
			int index = int.Parse(value);
			result = (index == -1)
				? default(TValueType)
				: GetValueFromItem(itemsToRender[index]);

			validationErrorMessage = null;
			return true;
		}

		protected virtual TValueType GetValueFromItem(TItemType item)
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
