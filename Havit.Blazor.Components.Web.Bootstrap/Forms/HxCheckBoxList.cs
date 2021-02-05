using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Multiple choice by checkboxes.
	/// </summary>
	public class HxCheckBoxList<TItemType> : HxInputBase<List<TItemType>> // cannot use an array: https://github.com/dotnet/aspnetcore/issues/15014
	{
		/// <summary>
		/// Items to display. 
		/// </summary>
		[Parameter] public IEnumerable<TItemType> Data { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set, ToString() is used.
		/// </summary>
		[Parameter] public Func<TItemType, string> TextSelector { get; set; }

		/// <summary>
		/// Selects value for items sorting. When not set, <see cref="TextSelector"/> property will be used.
		/// If you need complex sorting, pre-sort data manually or create a custom comparable property.
		/// </summary>
		[Parameter] public Func<TItemType, IComparable> SortKeySelector { get; set; }

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true;

		private List<TItemType> itemsToRender;

		private void RefreshState()
		{
			itemsToRender = Data?.ToList() ?? new List<TItemType>();

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
		}

		/// <inheritdoc/>
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			RefreshState();

			if (itemsToRender.Count > 0)
			{
				UglyHack uglyHack = new UglyHack(); // see comment below

				foreach (var item in itemsToRender)
				{
					builder.OpenComponent(1, typeof(HxInputCheckBox));
					builder.AddAttribute(2, nameof(HxInputCheckBox.Label), TextSelector?.Invoke(item) ?? item?.ToString() ?? String.Empty);
					builder.AddAttribute(3, nameof(HxInputCheckBox.Value), Value?.Contains(item) ?? false);
					builder.AddAttribute(4, nameof(HxInputCheckBox.ValueChanged), EventCallback.Factory.Create<bool>(this, @checked => HandleValueChanged(@checked, item)));

					// We need ValueExpression. Ehm, HxInputCheckBox needs ValueExpression. Because it is InputBase<T> which needs ValueExpression.
					// We have nothing to give the HxInputCheckBox. So we make own class with property which we assign to the ValueExpression.
					// Impacts? Unknown. Maybe none.
					builder.AddAttribute(5, nameof(HxInputCheckBox.ValueExpression), (Expression<Func<bool>>)(() => uglyHack.HackProperty)); // TODO: Je tenhle workaround průchozí???

					builder.AddAttribute(6, nameof(HxInputCheckBox.ShowValidationMessage), false);
					builder.CloseComponent();
				}
			}
		}

		private async Task HandleValueChanged(bool @checked, TItemType item)
		{
			var newValue = Value?.ToList() ?? new List<TItemType>();
			if (@checked)
			{
				newValue.Add(item);
			}
			else
			{
				newValue.Remove(item);
			}

			Value = newValue;
			await ValueChanged.InvokeAsync(Value);
			// TODO: notify changed
		}

		protected override bool TryParseValueFromString(string value, out List<TItemType> result, out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private class UglyHack
		{
			public bool HackProperty { get; set; }
		}
	}
}