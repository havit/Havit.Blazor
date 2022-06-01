﻿using System.Linq.Expressions;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Renders a multi-selection list of <see cref="HxInputCheckbox"/> controls.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCheckboxList">https://havit.blazor.eu/components/HxCheckboxList</see>
	/// </summary>
	public class HxCheckboxList<TValue, TItem> : HxInputBase<List<TValue>> // cannot use an array: https://github.com/dotnet/aspnetcore/issues/15014
	{
		/// <summary>
		/// Items to display. 
		/// </summary>
		[Parameter] public IEnumerable<TItem> Data { get; set; }

		/// <summary>
		/// TextSelector is obsolete, please use ItemTextSelector instead.
		/// </summary>
		[Parameter]
		[Obsolete("TextSelector is obsolete, please use ItemTextSelector instead.")]
		public Func<TItem, string> TextSelector
		{
			get => ItemTextSelector;
			set => ItemTextSelector = value;
		}

		/// <summary>
		/// Selects text to display from item.
		/// When not set, ToString() is used.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemTextSelector { get; set; }

		/// <summary>
		/// ValueSelector is obsolete, please use ItemValueSelector instead.
		/// </summary>
		[Parameter]
		[Obsolete("ValueSelector is obsolete, please use ItemValueSelector instead.")]
		public Func<TItem, TValue> ValueSelector
		{
			get => ItemValueSelector;
			set => ItemValueSelector = value;
		}

		/// <summary>
		/// Selects value from item.
		/// Not required when TValue is same as TItem.
		/// </summary>
		[Parameter] public Func<TItem, TValue> ItemValueSelector { get; set; }

		/// <summary>
		/// SortKeySelector is obsolete, please use ItemSortKeySelector instead.
		/// </summary>
		[Parameter]
		[Obsolete("SortKeySelector is obsolete, please use ItemSortKeySelector instead.")]
		public Func<TItem, IComparable> SortKeySelector
		{
			get => ItemSortKeySelector;
			set => ItemSortKeySelector = value;
		}

		/// <summary>
		/// Selects value for items sorting. When not set, <see cref="TextSelector"/> property will be used.
		/// If you need complex sorting, pre-sort data manually or create a custom comparable property.
		/// </summary>
		[Parameter] public Func<TItem, IComparable> ItemSortKeySelector { get; set; }

		/// <summary>
		/// Additional CSS class(es) for underlying <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public string ItemCssClass { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemCssClassSelector { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the input element of the <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public string ItemInputCssClass { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the input element of the <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemInputCssClassSelector { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the text of the <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public string ItemTextCssClass { get; set; }

		/// <summary>
		/// Additional CSS class(es) for the text of the <see cref="HxInputCheckbox" />.
		/// </summary>
		[Parameter] public Func<TItem, string> ItemTextCssClassSelector { get; set; }

		/// <summary>
		/// When <c>true</c>, items are sorted before displaying in select.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool AutoSort { get; set; } = true;

		/// <summary>
		/// Allows grouping checkboxes on the same horizontal row by rendering them inline. Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool Inline { get; set; }

		private List<TItem> itemsToRender;

		private void RefreshState()
		{
			itemsToRender = Data?.ToList() ?? new List<TItem>();

			// AutoSort
			if (AutoSort && (itemsToRender.Count > 1))
			{
				if (ItemSortKeySelector != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.ItemSortKeySelector).ToList();
				}
				else if (ItemTextSelector != null)
				{
					itemsToRender = itemsToRender.OrderBy(this.ItemTextSelector).ToList();
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
					TValue value = SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelector, item);

					builder.OpenComponent(1, typeof(HxInputCheckbox));

					builder.AddAttribute(2, nameof(HxInputCheckbox.Label), SelectorHelpers.GetText(ItemTextSelector, item));
					builder.AddAttribute(3, nameof(HxInputCheckbox.Value), Value?.Contains(value) ?? false);
					builder.AddAttribute(4, nameof(HxInputCheckbox.ValueChanged), EventCallback.Factory.Create<bool>(this, @checked => HandleValueChanged(@checked, item)));
					builder.AddAttribute(5, nameof(HxInputCheckbox.Enabled), EnabledEffective);

					builder.AddAttribute(6, nameof(HxInputCheckbox.CssClass), CssClassHelper.Combine(ItemCssClass, ItemCssClassSelector?.Invoke(item)));
					builder.AddAttribute(7, nameof(HxInputCheckbox.InputCssClass), CssClassHelper.Combine(ItemInputCssClass, ItemInputCssClassSelector?.Invoke(item)));
					builder.AddAttribute(8, nameof(HxInputCheckbox.LabelCssClass), CssClassHelper.Combine(ItemTextCssClass, ItemTextCssClassSelector?.Invoke(item)));

					// We need ValueExpression. Ehm, HxInputCheckbox needs ValueExpression. Because it is InputBase<T> which needs ValueExpression.
					// We have nothing to give the HxInputCheckbox. So we make own class with property which we assign to the ValueExpression.
					// Impacts? Unknown. Maybe none.
					builder.AddAttribute(50, nameof(HxInputCheckbox.ValueExpression), (Expression<Func<bool>>)(() => uglyHack.HackProperty));

					builder.AddAttribute(51, nameof(HxInputCheckbox.ShowValidationMessage), false);
					builder.AddAttribute(52, nameof(HxInputCheckbox.Inline), this.Inline);

					builder.AddMultipleAttributes(100, this.AdditionalAttributes);

					builder.CloseComponent();
				}
			}
		}

		private void HandleValueChanged(bool @checked, TItem item)
		{
			var newValue = Value == null ? new List<TValue>() : new List<TValue>(Value);
			TValue value = SelectorHelpers.GetValue<TItem, TValue>(ItemValueSelector, item);
			if (@checked)
			{
				newValue.Add(value);
			}
			else
			{
				newValue.Remove(value);
			}

			CurrentValue = newValue; // setter includes ValueChanged + NotifyFieldChanged
		}

		protected override bool TryParseValueFromString(string value, out List<TValue> result, out string validationErrorMessage)
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
			throw new NotSupportedException($"{nameof(FocusAsync)} is not supported on {nameof(HxCheckboxList<TValue, TItem>)}.");
		}

	}
}