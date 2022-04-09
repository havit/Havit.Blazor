using System.ComponentModel.DataAnnotations;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Base class for HxSelect and custom-implemented SELECT-pickers.
	/// </summary>
	/// <typeparam name="TValue">Type of value.</typeparam>
	/// <typeparam name="TItem">Type of items.</typeparam>
	public abstract class HxSelectBase<TValue, TItem> : HxInputBase<TValue>, IInputWithSize
	{
		/// <summary>
		/// Return <see cref="HxSelect{TValue, TItem}"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected virtual SelectSettings GetDefaults() => HxSelect.Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="HxSelect.Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public SelectSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual SelectSettings GetSettings() => this.Settings;


		/// <summary>
		/// Size of the input.
		/// </summary>
		[Parameter] public InputSize? InputSize { get; set; }
		protected InputSize InputSizeEffective => this.InputSize ?? GetSettings()?.InputSize ?? GetDefaults()?.InputSize ?? throw new InvalidOperationException(nameof(InputSize) + " default for " + nameof(HxSelect) + " has to be set.");
		InputSize IInputWithSize.InputSizeEffective => this.InputSizeEffective;

		/// <summary>
		/// Indicates when <c>null</c> is a valid value.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected bool? NullableImpl { get; set; }

		/// <summary>
		/// Indicates when <c>null</c> is a valid value.
		/// Uses (in order) to get effective value: Nullable property, RequiresAttribute on bounded property (<c>false</c>) Nullable type on bounded property (<c>true</c>), class (<c>true</c>), default (<c>false</c>).
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

				if (System.Nullable.GetUnderlyingType(typeof(TValue)) != null)
				{
					return true;
				}

				if (typeof(TValue).IsClass)
				{
					return true;
				}

				return true;
			}
		}

		/// <summary>
		/// Text to display for <c>null</c> value.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected string NullTextImpl { get; set; }

		/// <summary>
		/// Text to display when <see cref="DataImpl"/> is <c>null</c>.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected string NullDataTextImpl { get; set; }

		/// <summary>
		/// Selects value from item.
		/// Not required when TValue is same as TItem.
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
		/// When <c>true</c>, items are sorted before displaying in select.
		/// Default value is <c>true</c>.
		/// Base property for direct setup or to be re-published as <c>[Parameter] public</c>.
		/// </summary>
		protected bool AutoSortImpl { get; set; } = true;

		/// <inheritdoc cref="HxInputBase{TValue}.EnabledEffective" />
		protected override bool EnabledEffective => base.EnabledEffective && (itemsToRender != null);

		private protected override string CoreInputCssClass => "form-select";

		private IEqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;
		private List<TItem> itemsToRender;
		private int selectedItemIndex;
		private string chipValue;

		/// <inheritdoc/>
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			chipValue = null;

			RefreshState();

			bool enabledEffective = this.EnabledEffective;

			builder.OpenElement(0, "select");
			BuildRenderInput_AddCommonAttributes(builder, null);

			builder.AddAttribute(1000, "onchange", EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));
			builder.AddEventStopPropagationAttribute(1001, "onclick", true);
			builder.AddElementReferenceCapture(1002, elementReferece => InputElement = elementReferece);

			if (itemsToRender != null)
			{
				if ((NullableEffective && enabledEffective) || (selectedItemIndex == -1))
				{
					builder.OpenElement(2000, "option");
					builder.AddAttribute(2001, "value", -1);
					builder.AddAttribute(2002, "selected", selectedItemIndex == -1);
					builder.AddContent(2003, NullTextImpl);
					builder.CloseElement();
				}

				for (int i = 0; i < itemsToRender.Count; i++)
				{
					var item = itemsToRender[i];
					if (item != null)
					{
						bool selected = (i == selectedItemIndex);

						if (enabledEffective || selected) /* when not enabled only selected item is rendered */
						{
							string text = SelectorHelpers.GetText(TextSelectorImpl, item);

							builder.OpenElement(3000, "option");
							builder.SetKey(i.ToString());
							builder.AddAttribute(3001, "value", i.ToString());
							builder.AddAttribute(3002, "selected", selected);
							builder.AddContent(3003, text);
							builder.CloseElement();

							if (selected)
							{
								chipValue = text;
							}
						}
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
				selectedItemIndex = itemsToRender.FindIndex(item => comparer.Equals(Value, SelectorHelpers.GetValue<TItem, TValue>(ValueSelectorImpl, item)));

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
			int index = int.Parse(value);
			result = (index == -1)
				? default(TValue)
				: SelectorHelpers.GetValue<TItem, TValue>(ValueSelectorImpl, itemsToRender[index]);

			validationErrorMessage = null;
			return true;
		}

		protected override void RenderChipValue(RenderTreeBuilder builder)
		{
			builder.AddContent(0, chipValue);
		}

		string IInputWithSize.GetInputSizeCssClass() => this.InputSizeEffective.AsFormSelectCssClass();
	}
}
