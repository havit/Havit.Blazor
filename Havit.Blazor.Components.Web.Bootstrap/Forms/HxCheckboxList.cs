﻿using System;
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
	public class HxCheckboxList<TItem> : HxInputBase<List<TItem>> // cannot use an array: https://github.com/dotnet/aspnetcore/issues/15014
	{
		/// <summary>
		/// Items to display. 
		/// </summary>
		[Parameter] public IEnumerable<TItem> Data { get; set; }

		/// <summary>
		/// Selects text to display from item.
		/// When not set, ToString() is used.
		/// </summary>
		[Parameter] public Func<TItem, string> TextSelector { get; set; }

		/// <summary>
		/// Selects value for items sorting. When not set, <see cref="TextSelector"/> property will be used.
		/// If you need complex sorting, pre-sort data manually or create a custom comparable property.
		/// </summary>
		[Parameter] public Func<TItem, IComparable> SortKeySelector { get; set; }

		/// <summary>
		/// When true, items are sorted before displaying in select.
		/// Default value is true.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true;

		private List<TItem> itemsToRender;

		private void RefreshState()
		{
			itemsToRender = Data?.ToList() ?? new List<TItem>();

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

				builder.OpenElement(1, "ul");
				builder.AddAttribute(2, "class", "list-group");

				foreach (var item in itemsToRender)
				{
					builder.OpenElement(3, "li");
					builder.AddAttribute(4, "class", "list-group-item");

					builder.OpenComponent(5, typeof(HxInputCheckbox));
					builder.AddAttribute(6, nameof(HxInputCheckbox.Label), TextSelectorHelper.GetText(TextSelector, item));
					builder.AddAttribute(7, nameof(HxInputCheckbox.Value), Value?.Contains(item) ?? false);
					builder.AddAttribute(8, nameof(HxInputCheckbox.ValueChanged), EventCallback.Factory.Create<bool>(this, @checked => HandleValueChanged(@checked, item)));

					// We need ValueExpression. Ehm, HxInputCheckbox needs ValueExpression. Because it is InputBase<T> which needs ValueExpression.
					// We have nothing to give the HxInputCheckbox. So we make own class with property which we assign to the ValueExpression.
					// Impacts? Unknown. Maybe none.
					builder.AddAttribute(9, nameof(HxInputCheckbox.ValueExpression), (Expression<Func<bool>>)(() => uglyHack.HackProperty));

					builder.AddAttribute(10, nameof(HxInputCheckbox.ShowValidationMessage), false);
					builder.CloseComponent();

					builder.CloseElement(); // li
				}

				builder.CloseElement(); // ul
			}
		}

		private void HandleValueChanged(bool @checked, TItem item)
		{
			var newValue = Value?.ToList() ?? new List<TItem>();
			if (@checked)
			{
				newValue.Add(item);
			}
			else
			{
				newValue.Remove(item);
			}

			CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
		}

		protected override bool TryParseValueFromString(string value, out List<TItem> result, out string validationErrorMessage)
		{
			throw new NotSupportedException();
		}

		private class UglyHack
		{
			public bool HackProperty { get; set; }
		}

		/// <summary>
		/// Throws NotSupportedException - giving focus to an input element is not supported on the HxCheckboxList.
		/// </summary>
		public override ValueTask FocusAsync()
		{
			throw new NotSupportedException($"{nameof(FocusAsync)} is not supported on {nameof(HxCheckboxList<TItem>)}.");
		}

	}
}